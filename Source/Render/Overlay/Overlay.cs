using System;
using System.Diagnostics;

namespace Kiero.Net;

public static unsafe class Overlay
{
    private const uint dxgiPresentTest = 0x1;

    public static Action onDraw { get; set; }
    public static Action onInput { get; set; }

    internal static nint Dx11SwapChain => swapChain;
    internal static nint Dx12SwapChain => swapChain;
    internal static Dx11Backend Dx11Backend => backend11;
    internal static Dx12Backend Dx12Backend => backend12;
    internal static Dx9Backend Dx9Backend => backend9;

    public static bool isReady => ready;

    private static readonly object gate = new();
    private static nint context;
    private static nint swapChain;
    private static volatile bool ready;
    private static volatile bool initAttempted;
    private static volatile bool started;
    private static long lastTicks;
    private static bool queueWaitLogged;
    private static bool isD3D9Forced = false;

    [ThreadStatic] private static bool isRendering;

    private static RenderApi api = RenderApi.None;
    private static Dx9Backend backend9;
    private static Dx11Backend backend11;
    private static Dx12Backend backend12;
    private static GlBackend backendGl;

    public static void Initialize()
    {
        if (started) return;
        started = true;
        Logger.Info("Overlay", "Initialize");
    }

    internal static void SetD3D9Mode() => isD3D9Forced = true;

    internal static void OnPresentInternal(nint sc, uint flags)
    {
        if ((flags & dxgiPresentTest) != 0) return;
        if (sc == 0) return;
        if (isRendering) return;

        lock (gate)
        {
            if (isRendering) return;
            isRendering = true;
            try
            {
                if (api == RenderApi.Gl) return;

                if (ready && sc != swapChain)
                {
                    Logger.Warn("Overlay", $"swapchain changed 0x{swapChain:X} -> 0x{sc:X}, reinitializing");
                    ShutdownInternal();
                    initAttempted = false;
                }

                if (!ready && !InitImGui(sc, 0)) return;
                RenderFrame();
            }
            finally { isRendering = false; }
        }
    }

    internal static void OnSwapBuffers(nint hdc)
    {
        if (hdc == 0) return;
        if (isRendering) return;

        lock (gate)
        {
            if (isRendering) return;
            isRendering = true;
            try
            {
                if (api != RenderApi.None && api != RenderApi.Gl) return;

                if (!ready && !InitImGui(0, hdc)) return;
                if (hdc != backendGl.Hdc) return;
                RenderFrame();
            }
            finally { isRendering = false; }
        }
    }

    private static KieroRenderType DetectSwapChainApi(nint sc)
    {
        nint vtSc = *(nint*)sc;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, Guid*, nint*, int>)(*(nint*)(vtSc + 7 * IntPtr.Size));

        Guid iid12 = Guids.IID_ID3D12Device;
        nint dev = 0;
        if (getDevice(sc, &iid12, &dev) >= 0 && dev != 0) { Com.Release(dev); return KieroRenderType.D3D12; }

        Guid iid11 = Guids.IID_ID3D11Device;
        if (getDevice(sc, &iid11, &dev) >= 0 && dev != 0) { Com.Release(dev); return KieroRenderType.D3D11; }

        return KieroRenderType.None;
    }

    private static bool InitImGui(nint sc, nint hdc)
    {
        if (initAttempted) return false;

        bool isGl = hdc != 0;
        bool isDx9 = !isGl && (Kiero.GetRenderType() == KieroRenderType.D3D9 || isD3D9Forced);

        if (!isGl && !isDx9 && Kiero.GetRenderType() == KieroRenderType.D3D12 && ExecuteCommandListsHook.LastQueue == 0)
        {
            if (!queueWaitLogged)
            {
                queueWaitLogged = true;
                Logger.Info("Overlay", "DX12 queue not captured yet, defer");
            }
            return false;
        }

        initAttempted = true;

        uint dxFmt = 0;

        if (isGl)
        {
            Logger.Info("Overlay", $"InitImGui hdc=0x{hdc:X}");
        }
        else if (isDx9)
        {
            Logger.Info("Overlay", $"InitImGui dev=0x{sc:X}");
        }
        else
        {
            if (!Dxgi.TryGetDesc(sc, out uint w, out uint h, out dxFmt, out nint hwnd))
            {
                Logger.Error("Overlay", "GetDesc failed");
                initAttempted = false;
                return false;
            }

            if (w < 64 || h < 64)
            {
                Logger.Error("Overlay", $"too small {w}x{h}");
                initAttempted = false;
                return false;
            }

            Logger.Info("Overlay", $"InitImGui sc=0x{sc:X} {w}x{h} hwnd=0x{hwnd:X}");
        }

        context = ImGui.CreateContext(0);
        if (context == 0) { Logger.Error("Overlay", "no context"); initAttempted = false; return false; }

        ImGui.SetCurrentContext(context);
        ImGui.StyleColorsDark(0);

        nint io = ImGui.GetIO();
        if (io == 0)
        {
            Logger.Error("Overlay", "no io");
            ImGui.DestroyContext(context); context = 0;
            initAttempted = false;
            return false;
        }
        ImGui.IOSetIniFilename(io, null);

        if (isGl)
        {
            backendGl = new GlBackend();
            if (!backendGl.Init(hdc))
            {
                Logger.Error("Overlay", "GlBackend.Init failed");
                backendGl = null;
                ImGui.DestroyContext(context); context = 0;
                initAttempted = false;
                return false;
            }
            api = RenderApi.Gl;
        }
        else if (isDx9)
        {
            backend9 = new Dx9Backend();
            if (!backend9.Init(sc))
            {
                Logger.Error("Overlay", "Dx9Backend.Init failed");
                backend9 = null;
                ImGui.DestroyContext(context); context = 0;
                initAttempted = false;
                return false;
            }
            api = RenderApi.Dx9;
            swapChain = sc;
        }
        else
        {
            var kieroType = Kiero.GetRenderType();
            var actualApi = DetectSwapChainApi(sc);
            if (actualApi != KieroRenderType.None && actualApi != kieroType)
            {
                Logger.Warn("Overlay", $"API mismatch: Kiero says {kieroType}, swapchain says {actualApi}, using {actualApi}");
                kieroType = actualApi;
            }

            if (kieroType == KieroRenderType.D3D12)
            {
                backend12 = new Dx12Backend();
                if (!backend12.Init(sc, (int)dxFmt))
                {
                    Logger.Error("Overlay", "Dx12Backend.Init failed");
                    backend12 = null;
                    ImGui.DestroyContext(context); context = 0;
                    initAttempted = false;
                    return false;
                }
                api = RenderApi.Dx12;
            }
            else
            {
                backend11 = new Dx11Backend();
                if (!backend11.Init(sc))
                {
                    Logger.Error("Overlay", "Dx11Backend.Init failed");
                    backend11 = null;
                    ImGui.DestroyContext(context); context = 0;
                    initAttempted = false;
                    return false;
                }
                api = RenderApi.Dx11;
            }

            swapChain = sc;
        }

        lastTicks = Stopwatch.GetTimestamp();
        ready = true;
        Logger.Info("Overlay", $"ImGui ready (api={api})");
        return true;
    }

    private static void RenderFrame()
    {
        float w, h;

        if (api == RenderApi.Gl)
        {
            if (!Win32.GetClientRect(backendGl.Hwnd, out Win32.RECT rc)) return;
            w = rc.Right - rc.Left;
            h = rc.Bottom - rc.Top;
        }
        else if (api == RenderApi.Dx9)
        {
            if (!backend9.GetSize(out int w9, out int h9)) return;
            w = w9;
            h = h9;
        }
        else
        {
            if (!Dxgi.TryGetSize(swapChain, out uint wu, out uint hu)) return;
            w = wu; h = hu;
        }

        if (w < 1f || h < 1f) return;

        long now = Stopwatch.GetTimestamp();
        float dt = (float)((now - lastTicks) / (double)Stopwatch.Frequency);
        lastTicks = now;
        if (dt <= 0f || dt > 0.5f) dt = 1f / 60f;

        ImGui.SetCurrentContext(context);
        nint io = ImGui.GetIO();

        if (api == RenderApi.Dx9) backend9.NewFrame();
        else if (api == RenderApi.Dx11) backend11.NewFrame();
        else if (api == RenderApi.Dx12) backend12.NewFrame();
        else backendGl.NewFrame();

        ImGui.IOSetDisplaySize(io, w, h);
        ImGui.IOSetDeltaTime(io, dt);

        try { onInput?.Invoke(); }
        catch (Exception ex) { Logger.Error("OverlayInput", ex); }

        ImGui.NewFrame();

        bool drawSuccess = false;
        try { onDraw?.Invoke(); drawSuccess = true; }
        catch (Exception ex) { Logger.Error("OverlayDraw", ex); }

        if (!drawSuccess) { ImGui.EndFrame(); return; }
        ImGui.Render();

        nint dd = ImGui.GetDrawData();
        if (dd == 0) return;
        if (ImGui.DrawDataVtxCount(dd) <= 0) return;

        if (api == RenderApi.Dx9) backend9.RenderDrawData(dd);
        else if (api == RenderApi.Dx11) backend11.RenderDrawData(dd);
        else if (api == RenderApi.Dx12) backend12.RenderDrawData(dd);
        else backendGl.RenderDrawData(dd);
    }

    private static void ShutdownInternal()
    {
        try
        {
            if (backend9 != null) { backend9.Shutdown(); backend9 = null; }
            if (backend11 != null) { backend11.Shutdown(); backend11 = null; }
            if (backend12 != null) { backend12.Shutdown(); backend12 = null; }
            if (backendGl != null) { backendGl.Shutdown(); backendGl = null; }

            if (context != 0)
            {
                try { ImGui.SetCurrentContext(context); ImGui.DestroyContext(context); } catch { }
                context = 0;
            }
        }
        catch (Exception ex) { Logger.Error("Overlay", ex); }

        ready = false;
        swapChain = 0;
        api = RenderApi.None;
        Logger.Info("Overlay", "ShutdownInternal complete");
    }
}

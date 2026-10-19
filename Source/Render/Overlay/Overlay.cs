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

    public static bool isReady => ready;

    private static readonly object gate = new();
    private static nint context;
    private static nint swapChain;
    private static volatile bool ready;
    private static volatile bool initAttempted;
    private static volatile bool started;
    private static long lastTicks;
    private static bool queueWaitLogged;

    [ThreadStatic] private static bool isRendering;

    private static RenderApi api = RenderApi.None;
    private static Dx11Backend backend11;
    private static Dx12Backend backend12;

    public static void Initialize()
    {
        if (started) return;
        started = true;
        Logger.Info("Overlay", "Initialize");
    }

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
                if (ready && sc != swapChain)
                {
                    Logger.Warn("Overlay", $"swapchain changed 0x{swapChain:X} -> 0x{sc:X}, reinitializing");
                    ShutdownInternal();
                    initAttempted = false;
                }

                if (!ready && !InitImGui(sc)) return;
                if (sc != swapChain) return;
                RenderFrame(sc);
            }
            finally
            {
                isRendering = false;
            }
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

    private static bool InitImGui(nint sc)
    {
        if (initAttempted) return false;

        if (Kiero.GetRenderType() == KieroRenderType.D3D12 && ExecuteCommandListsHook.LastQueue == 0)
        {
            if (!queueWaitLogged)
            {
                queueWaitLogged = true;
                Logger.Info("Overlay", "DX12 queue not captured yet, defer");
            }
            return false;
        }

        initAttempted = true;

        if (!Dxgi.TryGetDesc(sc, out uint w, out uint h, out uint fmt, out nint hwnd))
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
            if (!backend12.Init(sc, (int)fmt))
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
                ImGui.DestroyContext(context); context = 0; // пункт 2
                initAttempted = false;
                return false;
            }
            api = RenderApi.Dx11;
        }

        swapChain = sc;
        lastTicks = Stopwatch.GetTimestamp();
        ready = true;
        Logger.Info("Overlay", $"ImGui ready (api={api})");
        return true;
    }

    private static void RenderFrame(nint sc)
    {
        if (!Dxgi.TryGetSize(sc, out uint wu, out uint hu)) return;
        if (wu == 0 || hu == 0) return;
        float w = wu, h = hu;

        long now = Stopwatch.GetTimestamp();
        float dt = (float)((now - lastTicks) / (double)Stopwatch.Frequency);
        lastTicks = now;
        if (dt <= 0f || dt > 0.5f) dt = 1f / 60f;

        ImGui.SetCurrentContext(context);
        nint io = ImGui.GetIO();

        if (api == RenderApi.Dx11) backend11.NewFrame();
        else backend12.NewFrame();

        ImGui.IOSetDisplaySize(io, w, h);
        ImGui.IOSetDeltaTime(io, dt);

        try { onInput?.Invoke(); }
        catch (Exception ex) { Logger.Error("OverlayInput", ex); }

        ImGui.NewFrame();

        bool drawSuccess = false;
        try
        {
            onDraw?.Invoke();
            drawSuccess = true;
        }
        catch (Exception ex)
        {
            Logger.Error("OverlayDraw", ex);
        }

        if (drawSuccess)
        {
            ImGui.Render();
        }
        else
        {
            ImGui.EndFrame();
            return;
        }

        nint dd = ImGui.GetDrawData();
        if (dd == 0) return;
        if (ImGui.DrawDataVtxCount(dd) <= 0) return;

        if (api == RenderApi.Dx11) backend11.RenderDrawData(dd);
        else backend12.RenderDrawData(dd);
    }

    private static void ShutdownInternal()
    {
        try
        {
            if (backend11 != null) { backend11.Shutdown(); backend11 = null; }
            if (backend12 != null) { backend12.Shutdown(); backend12 = null; }
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

internal enum RenderApi
{
    None = 0,
    Dx11 = 1,
    Dx12 = 2,
}
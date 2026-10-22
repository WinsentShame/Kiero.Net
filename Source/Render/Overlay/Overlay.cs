using System;
using System.Diagnostics;

namespace Kiero.Net;

internal enum PresentSource { Dx9, DxGi, Gl }

public static unsafe class Overlay
{
    private const uint dxgiPresentTest = 0x1;
    private const double SwapChainIdleSeconds = 2.0;

    public static Action onDraw { get; set; }
    public static Action onInput { get; set; }

    public static bool isReady => ready;

    private static readonly object gate = new();
    private static nint context;
    private static IBackend backend;
    private static nint swapChain;
    private static nint currentHwnd;
    private static bool win32Init;

    private static volatile bool ready;
    private static volatile bool initAttempted;
    private static volatile bool started;
    private static long lastTicks;
    private static long ourSwapChainLastTicks;

    [ThreadStatic] private static bool isRendering;

    public static void Initialize()
    {
        if (started) return;
        started = true;
        Logger.Info("Overlay", "Initialize");
    }

    public static void Stop()
    {
        lock (gate)
        {
            ShutdownInternal();
            initAttempted = false;
            started = false;
        }
    }

    internal static IBackend Backend => backend;
    internal static nint SwapChain => swapChain;

    internal static void OnPresent(nint handle, uint flags, PresentSource source)
    {
        if ((flags & dxgiPresentTest) != 0) return;
        if (handle == 0) return;
        if (isRendering) return;

        lock (gate)
        {
            if (isRendering) return;
            isRendering = true;
            try
            {
                if (backend != null)
                {
                    bool matches = source switch
                    {
                        PresentSource.Gl => backend is OpenglBackend,
                        PresentSource.Dx9 => backend is D3D9Backend,
                        PresentSource.DxGi => backend is Dx10Backend or D3D11Backend or D3D12Backend,
                        _ => false,
                    };
                    if (!matches) return;
                }

                if (source == PresentSource.Gl)
                {
                    if (backend is OpenglBackend gl && handle != gl.Hdc) return;
                    if (!ready && !InitImGui(0, handle, source)) return;
                }
                else if (source == PresentSource.Dx9)
                {
                    if (!ready && !InitImGui(handle, 0, source)) return;
                }
                else 
                {
                    if (ready && handle != swapChain)
                    {
                        long now = Stopwatch.GetTimestamp();
                        double idleSec = (now - ourSwapChainLastTicks) / (double)Stopwatch.Frequency;
                        if (idleSec < SwapChainIdleSeconds)
                            return;   
                        Logger.Info("Overlay", $"swapchain idle {idleSec:F1}s, switching 0x{swapChain:X} -> 0x{handle:X}");
                        ShutdownInternal();
                        initAttempted = false;
                    }
                    if (!ready && !InitImGui(handle, 0, source)) return;
                    ourSwapChainLastTicks = Stopwatch.GetTimestamp();
                }

                RenderFrame();
            }
            finally { isRendering = false; }
        }
    }

    private static RenderApi DetectDxgiApi(nint sc)
    {
        Guid iid12 = Guids.IID_ID3D12Device;
        nint dev = Vtbl.SwapChainGetDevice(sc, &iid12);
        if (dev != 0) { Com.Release(dev); return RenderApi.Dx12; }

        Guid iid10 = Guids.IID_ID3D10Device;
        dev = Vtbl.SwapChainGetDevice(sc, &iid10);
        if (dev != 0) { Com.Release(dev); return RenderApi.Dx10; }

        Guid iid11 = Guids.IID_ID3D11Device;
        dev = Vtbl.SwapChainGetDevice(sc, &iid11);
        if (dev != 0) { Com.Release(dev); return RenderApi.Dx11; }

        return RenderApi.None;
    }

    private static bool InitImGui(nint handle, nint hdc, PresentSource source)
    {
        if (initAttempted) return false;
        initAttempted = true;

        RenderApi apiKind;
        int rtvFmt = 0;
        nint hwnd = 0;

        if (source == PresentSource.Gl)
        {
            apiKind = RenderApi.Gl;
            hwnd = Win32.WindowFromDC(hdc);
        }
        else if (source == PresentSource.Dx9)
        {
            apiKind = RenderApi.Dx9;
        }
        else
        {
            apiKind = DetectDxgiApi(handle);
            if (apiKind == RenderApi.None)
            {
                Logger.Error("Overlay", "DetectDxgiApi failed");
                initAttempted = false;
                return false;
            }
            if (!Dxgi.TryGetDesc(handle, out uint w, out uint h, out uint fmt, out hwnd))
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
            rtvFmt = (int)fmt;
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

        if (hwnd != 0)
        {
            if (ImGui.ImplWin32Init(hwnd) != 0)
            {
                win32Init = true;
                currentHwnd = hwnd;
                Logger.Info("Overlay", $"Win32 backend initialized hwnd=0x{hwnd:X}");
            }
            else
            {
                Logger.Warn("Overlay", "ImplWin32Init failed — DPI/viewport features limited");
            }
        }

        IBackend newBackend;
        switch (apiKind)
        {
            case RenderApi.Dx9: newBackend = new D3D9Backend(); break;
            case RenderApi.Dx10: newBackend = new Dx10Backend(); break;
            case RenderApi.Dx11: newBackend = new D3D11Backend(); break;
            case RenderApi.Dx12: newBackend = new D3D12Backend(); break;
            case RenderApi.Gl: newBackend = new OpenglBackend(); break;
            default:
                Logger.Error("Overlay", "Unknown RenderApi");
                ImGui.DestroyContext(context); context = 0;
                initAttempted = false;
                return false;
        }

        if (!newBackend.Init(source == PresentSource.Gl ? hdc : handle, rtvFmt))
        {
            Logger.Error("Overlay", $"{newBackend.GetType().Name}.Init failed");
            try { newBackend.Shutdown(); } catch { }
            if (win32Init) { ImGui.ImplWin32Shutdown(); win32Init = false; }
            ImGui.DestroyContext(context); context = 0;
            initAttempted = false;
            return false;
        }

        backend = newBackend;
        swapChain = handle;
        ourSwapChainLastTicks = Stopwatch.GetTimestamp();
        lastTicks = Stopwatch.GetTimestamp();
        ready = true;
        Logger.Info("Overlay", $"ImGui ready (api={apiKind})");
        return true;
    }

    private static void RenderFrame()
    {
        if (!backend.TryGetSize(out float w, out float h)) return;
        if (w < 1f || h < 1f) return;

        long now = Stopwatch.GetTimestamp();
        float dt = (float)((now - lastTicks) / (double)Stopwatch.Frequency);
        lastTicks = now;
        if (dt <= 0f || dt > 0.5f) dt = 1f / 60f;

        ImGui.SetCurrentContext(context);
        nint io = ImGui.GetIO();

        backend.NewFrame();

        if (win32Init) ImGui.ImplWin32NewFrame();

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

        backend.Render(dd);
    }

    private static void ShutdownInternal()
    {
        try
        {
            if (context != 0) ImGui.SetCurrentContext(context);

            if (backend != null)
            {
                try { backend.Shutdown(); } catch (Exception ex) { Logger.Error("Overlay.Backend", ex); }
                backend = null;
            }

            if (win32Init)
            {
                try { ImGui.ImplWin32Shutdown(); } catch { }
                win32Init = false;
                currentHwnd = 0;
            }

            if (context != 0)
            {
                try { ImGui.DestroyContext(context); } catch { }
                context = 0;
            }
        }
        catch (Exception ex) { Logger.Error("Overlay", ex); }

        ready = false;
        swapChain = 0;
        Logger.Info("Overlay", "ShutdownInternal complete");
    }
}
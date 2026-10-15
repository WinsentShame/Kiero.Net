using System;
using System.Diagnostics;

namespace Kiero.Net;

public static unsafe class Overlay
{
    private const uint dxgiPresentTest = 0x1;

    public static Action onDraw { get; set; }
    public static Action onInput { get; set; }

    internal static nint Dx11SwapChain => swapChain;
    internal static Dx11Backend Dx11Backend => backend11;
    internal static Dx12Backend Dx12Backend => backend12;

    public static bool UseWin32Backend => useWin32Backend;
    public static bool isReady => ready;

    private static readonly object gate = new();
    private static nint context;
    private static nint swapChain;
    private static volatile bool ready;
    private static volatile bool initAttempted;
    private static volatile bool started;
    private static long lastTicks;
    private static bool useWin32Backend;
    private static bool queueWaitLogged;

    private static OverlayMode mode = OverlayMode.Auto;
    private static RenderApi api = RenderApi.None;
    private static Dx11Backend backend11;
    private static Dx12Backend backend12;

    public static void Initialize(OverlayMode overlayMode = OverlayMode.Auto)
    {
        if (started) return;
        started = true;
        mode = overlayMode;
        Logger.Info("Overlay", $"Initialize mode={overlayMode}");
    }

    internal static void OnPresentInternal(nint sc, uint flags)
    {
        if ((flags & dxgiPresentTest) != 0) return;
        if (sc == 0) return;

        lock (gate)
        {
            if (!ready && !InitImGui(sc)) return;
            if (sc != swapChain) return;
            RenderFrame(sc);
        }
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
            return false;
        }

        if (w < 64 || h < 64) { Logger.Error("Overlay", $"too small {w}x{h}"); return false; }

        bool wantWin32;
        switch (mode)
        {
            case OverlayMode.Win32: wantWin32 = true; break;
            case OverlayMode.Uwp: wantWin32 = false; break;
            default: wantWin32 = hwnd != 0; break;
        }

        Logger.Info("Overlay", $"InitImGui sc=0x{sc:X} {w}x{h} hwnd=0x{hwnd:X} mode={mode}");

        context = ImGui.CreateContext(0);
        if (context == 0) { Logger.Error("Overlay", "no context"); return false; }

        ImGui.SetCurrentContext(context);
        ImGui.StyleColorsDark(0);

        nint io = ImGui.GetIO();
        if (io == 0) { Logger.Error("Overlay", "no io"); return false; }
        ImGui.IOSetIniFilename(io, null);

        if (wantWin32)
        {
            if (ImGui.ImplWin32Init(hwnd) != 0)
            {
                useWin32Backend = true;
                Logger.Info("Overlay", "Win32 backend initialized");
            }
            else
            {
                Logger.Warn("Overlay", "Win32Init failed, falling back to UWP mode");
                useWin32Backend = false;
            }
        }
        else
        {
            useWin32Backend = false;
            Logger.Info("Overlay", "UWP mode: skipping Win32 backend");
        }

        var kieroType = Kiero.GetRenderType();
        if (kieroType == KieroRenderType.D3D12)
        {
            backend12 = new Dx12Backend();
            if (!backend12.Init(sc, (int)fmt))
            {
                Logger.Error("Overlay", "Dx12Backend.Init failed");
                backend12 = null;
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
                return false;
            }
            api = RenderApi.Dx11;
        }

        swapChain = sc;
        lastTicks = Stopwatch.GetTimestamp();
        ready = true;
        Logger.Info("Overlay", $"ImGui ready (win32={useWin32Backend} api={api})");
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

        if (useWin32Backend)
            ImGui.ImplWin32NewFrame();

        if (api == RenderApi.Dx11) backend11.NewFrame();
        else backend12.NewFrame();

        ImGui.IOSetDisplaySize(io, w, h);
        ImGui.IOSetDeltaTime(io, dt);

        try { onInput?.Invoke(); } catch { }

        ImGui.NewFrame();
        try { onDraw?.Invoke(); } catch { }
        ImGui.Render();

        nint dd = ImGui.GetDrawData();
        if (dd == 0) return;
        if (ImGui.DrawDataVtxCount(dd) <= 0) return;

        if (api == RenderApi.Dx11) backend11.RenderDrawData(dd);
        else backend12.RenderDrawData(dd);
    }
}

internal enum RenderApi
{
    None = 0,
    Dx11 = 1,
    Dx12 = 2,
}
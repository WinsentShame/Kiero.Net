using System;
using System.Diagnostics;

namespace Kiero.Net;

public static unsafe class Overlay
{
    private const uint dxgiPresentTest = 0x1;

    public static Action onDraw { get; set; }
    public static Action onInput { get; set; }
    public static bool isReady => ready;
    internal static nint Dx11SwapChain => swapChain;
    internal static Dx11Backend Dx11Backend => backend;

    private static readonly object gate = new();
    private static nint context;
    private static nint swapChain;
    private static volatile bool ready;
    private static volatile bool initAttempted;
    private static volatile bool started;
    private static long lastTicks;
    private static bool useWin32Backend;
    private static int skipLogCount;

    private static OverlayMode mode = OverlayMode.Auto;
    private static Dx11Backend backend;

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

        byte* desc = stackalloc byte[128];
        var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)((*(void***)sc)[12]);
        if (getDesc(sc, desc) < 0) return false;

        uint w = *(uint*)(desc + 0);
        uint h = *(uint*)(desc + 4);
        nint hwnd = *(nint*)(desc + 48);

        if (w < 64 || h < 64) return false;

        bool wantWin32;
        switch (mode)
        {
            case OverlayMode.Win32:
                wantWin32 = true;
                break;
            case OverlayMode.Uwp:
                wantWin32 = false;
                break;
            default:
                wantWin32 = hwnd != 0;
                break;
        }

        if (mode == OverlayMode.Win32 && hwnd == 0)
        {
            if (skipLogCount++ < 3)
                Logger.Warn("Overlay", $"mode=Win32 but hwnd=0 sc=0x{sc:X}");
            return false;
        }

        initAttempted = true;
        Logger.Info("Overlay", $"InitImGui sc=0x{sc:X} {w}x{h} hwnd=0x{hwnd:X} mode={mode}");

        try
        {
            context = ImGui.CreateContext(0);
            if (context == 0) { Logger.Error("Overlay", "no context"); initAttempted = false; return false; }

            ImGui.SetCurrentContext(context);
            ImGui.StyleColorsDark(0);

            nint io = ImGui.GetIO();
            if (io == 0) { Logger.Error("Overlay", "no io"); initAttempted = false; return false; }
            ImGui.IOSetIniFilename(io, null);

            if (wantWin32)
            {
                if (ImGui.ImplWin32Init(hwnd) != 0)
                {
                    useWin32Backend = true;
                    Logger.Info("Overlay", "Win32 backend initialized");
                }
                else if (mode == OverlayMode.Auto)
                {
                    Logger.Warn("Overlay", "Win32Init failed, falling back to UWP mode");
                    useWin32Backend = false;
                }
                else
                {
                    Logger.Error("Overlay", "Win32Init failed");
                    initAttempted = false;
                    return false;
                }
            }
            else
            {
                useWin32Backend = false;
                Logger.Info("Overlay", "UWP mode: skipping Win32 backend");
            }

            backend = new Dx11Backend();
            if (!backend.Init(sc))
            {
                Logger.Error("Overlay", "Dx11Backend.Init failed");
                initAttempted = false;
                return false;
            }

            swapChain = sc;
            lastTicks = Stopwatch.GetTimestamp();
            ready = true;
            Logger.Info("Overlay", $"ImGui ready (win32={useWin32Backend})");
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error("Overlay", ex);
            initAttempted = false;
            return false;
        }
    }



    private static void RenderFrame(nint sc)
    {
        float w, h;
        if (!backend.TryGetSize(sc, out w, out h)) return;
        if (w <= 0 || h <= 0) return;

        long now = Stopwatch.GetTimestamp();
        float dt = (float)((now - lastTicks) / (double)Stopwatch.Frequency);
        lastTicks = now;
        if (dt <= 0f || dt > 0.5f) dt = 1f / 60f;

        ImGui.SetCurrentContext(context);
        nint io = ImGui.GetIO();

        if (useWin32Backend)
            ImGui.ImplWin32NewFrame();

        backend.NewFrame();

        ImGui.IOSetDisplaySize(io, w, h);
        ImGui.IOSetDeltaTime(io, dt);

        try { onInput?.Invoke(); } catch { }

        ImGui.NewFrame();
        try { onDraw?.Invoke(); } catch { }
        ImGui.Render();

        nint dd = ImGui.GetDrawData();
        if (dd == 0) return;
        if (ImGui.DrawDataVtxCount(dd) <= 0) return;

        backend.RenderDrawData(dd);
    }
}
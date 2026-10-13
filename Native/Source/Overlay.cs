using System;
using System.Diagnostics;
using System.Threading;

namespace Kiero.Net;

public static unsafe class Overlay
{
    private const uint DxgiPresentTest = 0x1;

    public static Action OnDraw { get; set; }
    public static bool IsReady => _ready;

    private static readonly object Gate = new();
    private static Dx11Backend _backend;
    private static nint _context;
    private static nint _swapChain;
    private static volatile bool _ready;
    private static volatile bool _initAttempted;
    private static volatile bool _started;
    private static long _lastTicks;

    public static void Initialize()
    {
        if (_started) return;
        _started = true;
        Logger.Info("Overlay", "Initialize");
    }

    internal static void OnPresentInternal(nint sc, uint flags)
    {
        if ((flags & DxgiPresentTest) != 0) return;
        if (sc == 0) return;

        lock (Gate)
        {
            if (!_ready && !InitImGui(sc)) return;
            if (sc != _swapChain) return;
            RenderFrame(sc);
        }
    }

    private static bool InitImGui(nint sc)
    {
        if (_initAttempted) return false;
        _initAttempted = true;
        Logger.Info("Overlay", $"InitImGui sc=0x{sc:X}");

        try
        {
            nint hwnd = GetSwapChainHwnd(sc);
            if (hwnd == 0) { Logger.Error("Overlay", "no hwnd"); _initAttempted = false; return false; }

            _context = ImGui.CreateContext(0);
            if (_context == 0) { Logger.Error("Overlay", "no context"); _initAttempted = false; return false; }

            ImGui.SetCurrentContext(_context);
            ImGui.StyleColorsDark(0);

            nint io = ImGui.GetIO();
            if (io == 0) { Logger.Error("Overlay", "no io"); _initAttempted = false; return false; }
            ImGui.IOSetIniFilename(io, null);

            if (ImGui.ImplWin32Init(hwnd) == 0)
            {
                Logger.Error("Overlay", "ImplWin32Init failed");
                _initAttempted = false;
                return false;
            }

            _backend = new Dx11Backend();
            if (!_backend.Init(sc))
            {
                Logger.Error("Overlay", "Dx11Backend.Init failed");
                _initAttempted = false;
                return false;
            }

            _swapChain = sc;
            _lastTicks = Stopwatch.GetTimestamp();
            _ready = true;
            Logger.Info("Overlay", "ImGui ready");
            return true;
        }
        catch (Exception ex)
        {
            Logger.Error("Overlay", ex);
            _initAttempted = false;
            return false;
        }
    }

    private static void RenderFrame(nint sc)
    {
        float w, h;
        if (!GetSize(sc, out w, out h)) return;
        if (w <= 0 || h <= 0) return;

        long now = Stopwatch.GetTimestamp();
        float dt = (float)((now - _lastTicks) / (double)Stopwatch.Frequency);
        _lastTicks = now;
        if (dt <= 0f || dt > 0.5f) dt = 1f / 60f;

        ImGui.SetCurrentContext(_context);
        nint io = ImGui.GetIO();

        _backend.NewFrame();
        ImGui.IOSetDisplaySize(io, w, h);
        ImGui.IOSetDeltaTime(io, dt);

        ImGui.NewFrame();
        try { OnDraw?.Invoke(); } catch { }
        ImGui.Render();

        nint dd = ImGui.GetDrawData();
        if (dd == 0) return;
        if (ImGui.DrawDataVtxCount(dd) <= 0) return;

        _backend.RenderDrawData(dd);
    }

    private static bool GetSize(nint sc, out float w, out float h)
    {
        w = h = 0;
        byte* desc = stackalloc byte[128];
        var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)((*(void***)sc)[12]);
        if (getDesc(sc, desc) < 0) return false;
        w = *(uint*)(desc + 0);
        h = *(uint*)(desc + 4);
        return true;
    }

    private static nint GetSwapChainHwnd(nint sc)
    {
        byte* desc = stackalloc byte[128];
        var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)((*(void***)sc)[12]);
        if (getDesc(sc, desc) < 0) return 0;
        return *(nint*)(desc + 48);
    }
}
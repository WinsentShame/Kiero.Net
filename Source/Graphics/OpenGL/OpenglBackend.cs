namespace Kiero.Net;

internal sealed unsafe class OpenglBackend
{
    private nint hdc;
    private nint hwnd;
    private bool ready;
    private long frameCounter;

    public bool isReady => ready;
    public nint Hdc => hdc;
    public nint Hwnd => hwnd;

    public bool Init(nint hdc)
    {
        this.hdc = hdc;

        nint w = Win32.WindowFromDC(hdc);
        if (w == 0) { Logger.Error("Gl", "WindowFromDC returned 0"); return false; }
        hwnd = w;

        if (!Win32.GetClientRect(hwnd, out Win32.RECT rc))
        {
            Logger.Error("Gl", "GetClientRect failed");
            return false;
        }
        int width = rc.Right - rc.Left;
        int height = rc.Bottom - rc.Top;
        if (width < 64 || height < 64)
        {
            Logger.Error("Gl", $"too small {width}x{height}");
            return false;
        }

        Logger.Info("Gl", $"Init hdc=0x{hdc:X} hwnd=0x{hwnd:X} {width}x{height}");

        if (ImGui.ImplOpenGL3Init(null) == 0)
        {
            Logger.Error("Gl", "ImplOpenGL3Init failed");
            return false;
        }

        ready = true;
        Logger.Info("Gl", "ready");
        return true;
    }

    public void NewFrame() => ImGui.ImplOpenGL3NewFrame();

    public void RenderDrawData(nint drawData)
    {
        if (!ready || drawData == 0) return;

        ImGui.ImplOpenGL3RenderDrawData(drawData);

        long n = ++frameCounter;
        if (n == 1 || n % 600 == 0)
            Logger.Info("Gl", $"frame={n} hdc=0x{hdc:X}");
    }

    public void Shutdown()
    {
        if (!ready) return;
        ready = false;

        try { ImGui.ImplOpenGL3Shutdown(); } catch { }

        hdc = 0;
        hwnd = 0;
        Logger.Info("Gl", "Shutdown complete");
    }
}
namespace Kiero.Net;

internal sealed unsafe class OpenglBackend : IBackend
{
    private nint hdc;
    private nint hwnd;

    public bool isReady { get; private set; }
    public nint Hdc => hdc;

    public bool Init(nint handle, int rtvFormat)
    {
        hdc = handle;
        hwnd = Win32.WindowFromDC(hdc);
        if (hwnd == 0) { Logger.Error("Gl", "WindowFromDC=0"); return false; }
        if (!Win32.GetClientRect(hwnd, out Win32.RECT rc)) return false;
        if (rc.Right - rc.Left < 64 || rc.Bottom - rc.Top < 64) return false;

        if (ImGui.ImplOpenGL3Init(null) == 0) { Logger.Error("Gl", "ImplOpenGL3Init failed"); return false; }
        isReady = true;
        return true;
    }

    public void NewFrame() => ImGui.ImplOpenGL3NewFrame();
    public void Render(nint drawData) { if (isReady && drawData != 0) ImGui.ImplOpenGL3RenderDrawData(drawData); }
    public void OnResize() { }
    public void OnReset() { }
    public void OnResetPost() { }

    public bool TryGetSize(out float w, out float h)
    {
        w = h = 0;
        if (hwnd == 0) return false;
        if (!Win32.GetClientRect(hwnd, out Win32.RECT rc)) return false;
        w = rc.Right - rc.Left; h = rc.Bottom - rc.Top;
        return w > 0 && h > 0;
    }

    public void Shutdown()
    {
        if (!isReady) return;
        isReady = false;
        try { ImGui.ImplOpenGL3Shutdown(); } catch { }
        hdc = hwnd = 0;
    }
}
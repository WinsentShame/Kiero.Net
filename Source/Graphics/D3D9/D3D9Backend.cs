namespace Kiero.Net;

internal sealed unsafe class D3D9Backend : IBackend
{
    private nint device;

    public bool isReady { get; private set; }

    public bool Init(nint handle, int rtvFormat)
    {
        device = handle;
        Logger.Info("Dx9", $"Init dev=0x{device:X}");
        if (ImGui.ImplDX9Init(device) == 0) { Logger.Error("Dx9", "ImplDX9Init failed"); return false; }
        isReady = true;
        return true;
    }

    public void NewFrame() => ImGui.ImplDX9NewFrame();
    public void Render(nint drawData) { if (isReady && drawData != 0) ImGui.ImplDX9RenderDrawData(drawData); }
    public void OnResize() { }
    public void OnReset() { if (isReady) try { ImGui.ImplDX9InvalidateDeviceObjects(); } catch { } }
    public void OnResetPost() { if (isReady) try { ImGui.ImplDX9CreateDeviceObjects(); } catch { } }

    public bool TryGetSize(out float w, out float h)
    {
        w = h = 0;
        if (device == 0) return false;

        var getViewport = (delegate* unmanaged[Stdcall]<nint, void*, int>)
            Vtbl.VTableM(device, Vtbl.Direct3DDevice9_GetViewport);
        byte* vp = stackalloc byte[24];
        if (getViewport(device, vp) >= 0)
        {
            int ww = *(int*)(vp + 8), hh = *(int*)(vp + 12);
            if (ww > 0 && hh > 0) { w = ww; h = hh; return true; }
        }

        var getCreationParams = (delegate* unmanaged[Stdcall]<nint, void*, int>)
            Vtbl.VTableM(device, Vtbl.Direct3DDevice9_GetCreationParameters);
        byte* cp = stackalloc byte[24];
        if (getCreationParams(device, cp) >= 0)
        {
            nint hwnd = *(nint*)(cp + 8);
            if (hwnd != 0 && Win32.GetClientRect(hwnd, out Win32.RECT rc))
            {
                w = rc.Right - rc.Left; h = rc.Bottom - rc.Top;
                return w > 0 && h > 0;
            }
        }
        return false;
    }

    public void Shutdown()
    {
        if (!isReady) return;
        isReady = false;
        try { ImGui.ImplDX9Shutdown(); } catch { }
        device = 0;
    }
}
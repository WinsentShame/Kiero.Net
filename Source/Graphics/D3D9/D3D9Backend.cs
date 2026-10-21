using System;

namespace Kiero.Net;

internal sealed unsafe class D3D9Backend
{
    private nint device;
    private bool ready;
    private long frameCounter;

    public bool isReady => ready;

    public bool Init(nint dev)
    {
        device = dev;
        Logger.Info("Dx9", $"Init dev=0x{dev:X}");

        if (ImGui.ImplDX9Init(device) == 0)
        {
            Logger.Error("Dx9", "ImplDX9Init failed");
            return false;
        }

        ready = true;
        Logger.Info("Dx9", "ready");
        return true;
    }

    public void NewFrame() => ImGui.ImplDX9NewFrame();

    public void RenderDrawData(nint drawData)
    {
        if (!ready || drawData == 0) return;
        ImGui.ImplDX9RenderDrawData(drawData);

        long n = ++frameCounter;
        if (n == 1 || n % 600 == 0)
            Logger.Info("Dx9", $"frame={n}");
    }

    public bool GetSize(out int w, out int h)
    {
        w = h = 0;
        if (device == 0) return false;

        nint vt = *(nint*)device;

        var getViewport = (delegate* unmanaged[Stdcall]<nint, void*, int>)
            (*(nint*)(vt + 48 * IntPtr.Size));

        byte* vp = stackalloc byte[24];
        if (getViewport(device, vp) >= 0)
        {
            w = *(int*)(vp + 8);
            h = *(int*)(vp + 12);
            if (w > 0 && h > 0) return true;
        }

        var getCreationParams = (delegate* unmanaged[Stdcall]<nint, void*, int>)
            (*(nint*)(vt + 9 * IntPtr.Size));

        byte* cp = stackalloc byte[24]; 
        if (getCreationParams(device, cp) >= 0)
        {
            nint hwnd = *(nint*)(cp + 8); 
            if (hwnd != 0 && Win32.GetClientRect(hwnd, out Win32.RECT rc))
            {
                w = rc.Right - rc.Left;
                h = rc.Bottom - rc.Top;
                return w > 0 && h > 0;
            }
        }

        return false;
    }

    public void OnReset()
    {
        if (!ready) return;
        Logger.Info("Dx9", "Reset -> invalidate");
        try { ImGui.ImplDX9InvalidateDeviceObjects(); } catch { }
    }

    public void OnResetPost()
    {
        if (!ready) return;
        Logger.Info("Dx9", "Reset -> recreate");
        try { ImGui.ImplDX9CreateDeviceObjects(); } catch { }
    }

    public void Shutdown()
    {
        if (!ready) return;
        ready = false;

        try { ImGui.ImplDX9Shutdown(); } catch { }

        device = 0;
        Logger.Info("Dx9", "Shutdown complete");
    }
}
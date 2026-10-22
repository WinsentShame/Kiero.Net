using System;

namespace Kiero.Net;

internal sealed unsafe class Dx10Backend
{
    private nint swapChain;
    private nint device;
    private nint rtv;
    private nint rtvBackbuffer;
    private bool ready;
    private long frameCounter;

    public bool isReady => ready;

    public bool Init(nint sc)
    {
        swapChain = sc;
        Logger.Info("Dx10", $"Init sc=0x{sc:X}");

        nint bb = Dxgi.GetBackBuffer(sc, 0, Guids.IID_ID3D10Texture2D);
        if (bb == 0) { Logger.Error("Dx10", "backbuffer=0"); return false; }

        nint dev = 0;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, nint*, int>)
            Vtbl.VTableM(bb, Vtbl.D3D10Texture2D_GetDevice);
        int hr = getDevice(bb, &dev);
        Com.Release(bb);
        if (hr < 0 || dev == 0) { Logger.Error("Dx10", $"GetDevice hr=0x{hr:X}"); return false; }
        device = dev;

        if (ImGui.ImplDX10Init(device) == 0)
        {
            Logger.Error("Dx10", "ImplDX10Init failed");
            return false;
        }

        ready = true;
        Logger.Info("Dx10", $"ready dev=0x{dev:X}");
        return true;
    }

    public void NewFrame() => ImGui.ImplDX10NewFrame();

    public void ReleaseRenderTarget()
    {
        if (rtv != 0) { Com.Release(rtv); rtv = 0; }
        rtvBackbuffer = 0;
    }

    public void RenderDrawData(nint drawData)
    {
        if (!ready || drawData == 0) return;

        uint idx = 0;

        nint bb = Dxgi.GetBackBuffer(swapChain, idx, Guids.IID_ID3D10Texture2D);
        if (bb == 0) return;

        try
        {
            if (rtv == 0 || rtvBackbuffer != bb)
            {
                if (rtv != 0) { Com.Release(rtv); rtv = 0; }

                D3D10Shim.CreateRenderTargetView(device, bb, 0, out nint newRtv);
                if (newRtv == 0) return;

                rtv = newRtv;
                rtvBackbuffer = bb;
            }

            nint oldRtv = 0, oldDsv = 0;
            D3D10Shim.OMGetRenderTargets(device, 1, &oldRtv, &oldDsv);

            nint rtvLocal = rtv;
            D3D10Shim.OMSetRenderTargets(device, 1, &rtvLocal, 0);

            ImGui.ImplDX10RenderDrawData(drawData);

            if (oldRtv != 0 || oldDsv != 0)
                D3D10Shim.OMSetRenderTargets(device, oldRtv != 0 ? 1u : 0u, &oldRtv, oldDsv);

            if (oldRtv != 0) Com.Release(oldRtv);
            if (oldDsv != 0) Com.Release(oldDsv);

            long n = ++frameCounter;
            if (n == 1 || n % 600 == 0) Logger.Info("Dx10", $"frame={n}");
        }
        finally { Com.Release(bb); }
    }

    public void Shutdown()
    {
        ReleaseRenderTarget();
        if (ready) { try { ImGui.ImplDX10Shutdown(); } catch { } ready = false; }
        if (device != 0) { Com.Release(device); device = 0; }
        swapChain = 0;
    }
}
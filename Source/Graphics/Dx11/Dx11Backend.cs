using System;

namespace Kiero.Net;

internal sealed unsafe class Dx11Backend
{
    private const int VT_GetBuffer = 9;
    private const int VT_GetDevice = 3;
    private const int VT_GetImmediateContext = 40;
    private const int VT_CreateRenderTargetView = 9;
    private const int VT_OMSetRenderTargets = 33;
    private const int VT_GetCurrentBackBufferIndex = 36;

    private nint swapChain;
    private nint device;
    private nint context;
    private nint rtv;
    private nint rtvBb;
    private bool ready;
    private long frameCounter;

    public bool isReady => ready;

    public bool Init(nint sc)
    {
        swapChain = sc;
        Logger.Info("Dx11", $"Init sc=0x{sc:X}");

        nint bb = GetBackBuffer(sc, 0);
        if (bb == 0) { Logger.Error("Dx11", "backbuffer=0"); return false; }

        nint dev = 0;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, nint*, int>)((*(void***)bb)[VT_GetDevice]);
        int hr = getDevice(bb, &dev);
        Com.Release(bb);
        if (hr < 0 || dev == 0) { Logger.Error("Dx11", $"GetDevice hr=0x{hr:X}"); return false; }
        device = dev;
        Logger.Info("Dx11", $"device=0x{dev:X}");

        nint ctx = 0;
        var getCtx = (delegate* unmanaged[Stdcall]<nint, nint*, void>)((*(void***)dev)[VT_GetImmediateContext]);
        getCtx(dev, &ctx);
        if (ctx == 0) { Logger.Error("Dx11", "no context"); return false; }
        context = ctx;
        Logger.Info("Dx11", $"context=0x{ctx:X}");

        if (ImGui.ImplDX11Init(device, context) == 0)
        {
            Logger.Error("Dx11", "ImplDX11Init failed");
            return false;
        }

        ready = true;
        return true;
    }

    public void NewFrame() => ImGui.ImplDX11NewFrame();

    public void RenderDrawData(nint drawData)
    {
        if (!ready || drawData == 0) return;

        uint IDX = GetCurrentBackBufferIndex(swapChain);
        nint bb = GetBackBuffer(swapChain, IDX);
        if (bb == 0) return;

        try
        {
            if (rtv == 0 || rtvBb != bb)
            {
                if (rtv != 0) { Com.Release(rtv); rtv = 0; }

                nint newRtv = 0;
                var createRtv = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint*, int>)
                    ((*(void***)device)[VT_CreateRenderTargetView]);
                int hr = createRtv(device, bb, 0, &newRtv);
                if (hr < 0 || newRtv == 0) return;
                rtv = newRtv;
                rtvBb = bb;
            }

            nint rtvHandle = rtv;
            var omSet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint, void>)
                ((*(void***)context)[VT_OMSetRenderTargets]);
            omSet(context, 1, &rtvHandle, 0);

            ImGui.ImplDX11RenderDrawData(drawData);

            long N = ++frameCounter;
            if (N == 1 || N % 600 == 0)
                Logger.Info("Dx11", $"frame={N} idx={IDX} bb=0x{bb:X} rtv=0x{rtv:X}");
        }
        finally
        {
            Com.Release(bb);
        }
    }

    public void Shutdown()
    {
        if (rtv != 0) { Com.Release(rtv); rtv = 0; }
        rtvBb = 0;
        if (ready)
        {
            try { ImGui.ImplDX11Shutdown(); } catch { }
            ready = false;
        }
        if (context != 0) { Com.Release(context); context = 0; }
        if (device != 0) { Com.Release(device); device = 0; }
    }

    private static uint GetCurrentBackBufferIndex(nint sc)
    {
        nint vt = *(nint*)sc;
        nint addr = *(nint*)(vt + VT_GetCurrentBackBufferIndex * IntPtr.Size);
        if (addr == 0) return 0;
        var fn = (delegate* unmanaged[Stdcall]<nint, uint>)addr;
        return fn(sc);
    }

    private static nint GetBackBuffer(nint sc, uint idx)
    {
        nint bb = 0;
        var getBuffer = (delegate* unmanaged[Stdcall]<nint, uint, Guid*, nint*, int>)((*(void***)sc)[VT_GetBuffer]);
        Guid iid = Guids.IID_ID3D11Texture2D;
        return getBuffer(sc, idx, &iid, &bb) < 0 ? 0 : bb;
    }
}
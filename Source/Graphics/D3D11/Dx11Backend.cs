using System;

namespace Kiero.Net;

internal sealed unsafe class Dx11Backend
{
    private const int VT_GetDevice = 3;
    private const int VT_CreateRenderTargetView = 9;
    private const int VT_OMSetRenderTargets = 33;
    private const int VT_OMGetRenderTargets = 89;
    private const int VT_GetImmediateContext = 40;

    private const int MAX_BACKBUFFERS = 4;

    private nint swapChain;
    private nint swapChain3;
    private nint device;
    private nint context;

    private nint[] rtvs = new nint[MAX_BACKBUFFERS];
    private nint[] rtvBackbuffers = new nint[MAX_BACKBUFFERS];

    private bool ready;
    private long frameCounter;

    public bool isReady => ready;

    public bool Init(nint sc)
    {
        swapChain = sc;
        Logger.Info("Dx11", $"Init sc=0x{sc:X}");

        nint sc3 = 0;
        Guid iid3 = Guids.IID_IDXGISwapChain3;
        if (Com.QueryInterface(sc, &iid3, &sc3) >= 0 && sc3 != 0)
        {
            swapChain3 = sc3;
            Logger.Info("Dx11", $"swapChain3=0x{sc3:X} (GetCurrentBackBufferIndex OK)");
        }
        else
        {
            Logger.Warn("Dx11", "IDXGISwapChain3 not available, will render to buffer 0 only");
        }

        nint bb = Dxgi.GetBackBuffer(sc, 0, Guids.IID_ID3D11Texture2D);
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

    public void ReleaseRenderTarget()
    {
        for (int i = 0; i < MAX_BACKBUFFERS; i++)
        {
            if (rtvs[i] != 0) { Com.Release(rtvs[i]); rtvs[i] = 0; }
            rtvBackbuffers[i] = 0;
        }
    }

    public void RenderDrawData(nint drawData)
    {
        if (!ready || drawData == 0) return;

        uint idx = 0;
        if (swapChain3 != 0)
        {
            idx = Dxgi.GetCurrentBackBufferIndex(swapChain3);
            if (idx >= MAX_BACKBUFFERS) idx = 0;
        }

        nint bb = Dxgi.GetBackBuffer(swapChain, idx, Guids.IID_ID3D11Texture2D);
        if (bb == 0) return;

        nint oldRtv = 0;
        nint oldDsv = 0;
        bool haveOldTargets = false;

        try
        {
            if (rtvs[idx] == 0 || rtvBackbuffers[idx] != bb)
            {
                if (rtvs[idx] != 0) { Com.Release(rtvs[idx]); rtvs[idx] = 0; }

                nint newRtv = 0;
                var createRtv = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint*, int>)
                    ((*(void***)device)[VT_CreateRenderTargetView]);
                int hr = createRtv(device, bb, 0, &newRtv);
                if (hr < 0 || newRtv == 0) return;

                rtvs[idx] = newRtv;
                rtvBackbuffers[idx] = bb;
                Logger.Info("Dx11", $"RTV created for idx={idx} bb=0x{bb:X}");
            }

            try
            {
                var omGet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint*, void>)
                    ((*(void***)context)[VT_OMGetRenderTargets]);
                nint tmpRtv = 0;
                nint tmpDsv = 0;
                omGet(context, 1, &tmpRtv, &tmpDsv);
                oldRtv = tmpRtv;
                oldDsv = tmpDsv;
                haveOldTargets = true;
            }
            catch { haveOldTargets = false; }

            nint rtvHandle = rtvs[idx];
            var omSet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint, void>)
                ((*(void***)context)[VT_OMSetRenderTargets]);
            omSet(context, 1, &rtvHandle, 0);

            ImGui.ImplDX11RenderDrawData(drawData);

            if (haveOldTargets)
            {
                if (oldRtv != 0 || oldDsv != 0)
                {
                    nint restoreRtv = oldRtv;
                    omSet(context, oldRtv != 0 ? 1u : 0u, &restoreRtv, oldDsv);
                }
            }
            else
            {
                omSet(context, 0, null, 0);
            }

            long N = ++frameCounter;
            if (N == 1 || N % 600 == 0)
                Logger.Info("Dx11", $"frame={N} idx={idx} bb=0x{bb:X} rtv=0x{rtvs[idx]:X}");
        }
        finally
        {
            if (oldRtv != 0) Com.Release(oldRtv);
            if (oldDsv != 0) Com.Release(oldDsv);
            Com.Release(bb);
        }
    }

    public void Shutdown()
    {
        ReleaseRenderTarget();
        if (ready)
        {
            try { ImGui.ImplDX11Shutdown(); } catch { }
            ready = false;
        }
        if (context != 0) { Com.Release(context); context = 0; }
        if (device != 0) { Com.Release(device); device = 0; }
        if (swapChain3 != 0) { Com.Release(swapChain3); swapChain3 = 0; }
        swapChain = 0;
    }
}
using System;

namespace Kiero.Net;

internal sealed unsafe class D3D11Backend : IBackend
{
    private const int MAX_BACKBUFFERS = 4;

    private nint swapChain;
    private nint swapChain3;
    private nint device;
    private nint context;

    private nint[] rtvs = new nint[MAX_BACKBUFFERS];
    private nint[] rtvBackbuffers = new nint[MAX_BACKBUFFERS];

    public bool isReady { get; private set; }

    public bool Init(nint handle, int rtvFormat)  
    {
        swapChain = handle;

        nint sc3 = 0;
        Guid iid3 = Guids.IID_IDXGISwapChain3;
        if (Com.QueryInterface(swapChain, &iid3, &sc3) >= 0 && sc3 != 0) swapChain3 = sc3;

        nint bb = Dxgi.GetBackBuffer(swapChain, 0, Guids.IID_ID3D11Texture2D);
        if (bb == 0) { Logger.Error("Dx11", "backbuffer=0"); return false; }

        nint dev = 0;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, nint*, int>)
            Vtbl.VTableM(bb, Vtbl.D3D11Texture2D_GetDevice);
        int hr = getDevice(bb, &dev);
        Com.Release(bb);
        if (hr < 0 || dev == 0) { Logger.Error("Dx11", $"GetDevice hr=0x{hr:X}"); return false; }
        device = dev;

        nint ctx = 0;
        var getCtx = (delegate* unmanaged[Stdcall]<nint, nint*, void>)
            Vtbl.VTableM(device, Vtbl.D3D11Device_GetImmediateContext);
        getCtx(device, &ctx);
        if (ctx == 0) { Logger.Error("Dx11", "no context"); return false; }
        context = ctx;

        if (ImGui.ImplDX11Init(device, context) == 0) { Logger.Error("Dx11", "ImplDX11Init failed"); return false; }
        isReady = true;
        return true;
    }

    public void NewFrame() => ImGui.ImplDX11NewFrame();

    public void OnResize()
    {
        for (int i = 0; i < MAX_BACKBUFFERS; i++)
        {
            if (rtvs[i] != 0) { Com.Release(rtvs[i]); rtvs[i] = 0; }
            rtvBackbuffers[i] = 0;
        }
    }

    public void OnReset() { }
    public void OnResetPost() { }

    public void Render(nint drawData)
    {
        if (!isReady || drawData == 0) return;

        uint idx = 0;
        if (swapChain3 != 0)
        {
            idx = Dxgi.GetCurrentBackBufferIndex(swapChain3);
            if (idx >= MAX_BACKBUFFERS) idx = 0;
        }

        nint bb = Dxgi.GetBackBuffer(swapChain, idx, Guids.IID_ID3D11Texture2D);
        if (bb == 0) return;

        nint oldRtv = 0, oldDsv = 0;
        try
        {
            if (rtvs[idx] == 0 || rtvBackbuffers[idx] != bb)
            {
                if (rtvs[idx] != 0) { Com.Release(rtvs[idx]); rtvs[idx] = 0; }
                nint newRtv = 0;
                var createRtv = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint*, int>)
                    Vtbl.VTableM(device, Vtbl.D3D11Device_CreateRenderTargetView);
                int hr = createRtv(device, bb, 0, &newRtv);
                if (hr < 0 || newRtv == 0) return;
                rtvs[idx] = newRtv;
                rtvBackbuffers[idx] = bb;
            }

            var omGet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint*, void>)
                Vtbl.VTableM(context, Vtbl.D3D11Context_OMGetRenderTargets);
            omGet(context, 1, &oldRtv, &oldDsv);

            nint rtvHandle = rtvs[idx];
            var omSet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint, void>)
                Vtbl.VTableM(context, Vtbl.D3D11Context_OMSetRenderTargets);
            omSet(context, 1, &rtvHandle, 0);

            ImGui.ImplDX11RenderDrawData(drawData);

            if (oldRtv != 0 || oldDsv != 0)
            {
                nint restore = oldRtv;
                omSet(context, oldRtv != 0 ? 1u : 0u, &restore, oldDsv);
            }
        }
        finally
        {
            if (oldRtv != 0) Com.Release(oldRtv);
            if (oldDsv != 0) Com.Release(oldDsv);
            Com.Release(bb);
        }
    }

    public bool TryGetSize(out float w, out float h)
    {
        if (!Dxgi.TryGetSize(swapChain, out uint ww, out uint hh)) { w = h = 0; return false; }
        w = ww; h = hh; return true;
    }

    public void Shutdown()
    {
        if (!isReady) return;
        isReady = false;
        OnResize();
        try { ImGui.ImplDX11Shutdown(); } catch { }
        if (context != 0) { Com.Release(context); context = 0; }
        if (device != 0) { Com.Release(device); device = 0; }
        if (swapChain3 != 0) { Com.Release(swapChain3); swapChain3 = 0; }
        swapChain = 0;
    }
}
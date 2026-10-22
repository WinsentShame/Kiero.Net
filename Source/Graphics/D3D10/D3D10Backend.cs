namespace Kiero.Net;

internal sealed unsafe class Dx10Backend : IBackend
{
    private nint swapChain;
    private nint device;
    private nint rtv;
    private nint rtvBackbuffer;

    public bool isReady { get; private set; }

    public bool Init(nint handle, int rtvFormat)
    {
        swapChain = handle;
        Logger.Info("Dx10", $"Init sc=0x{swapChain:X}");

        nint bb = Dxgi.GetBackBuffer(swapChain, 0, Guids.IID_ID3D10Texture2D);
        if (bb == 0) { Logger.Error("Dx10", "backbuffer=0"); return false; }

        nint dev = 0;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, nint*, int>)
            Vtbl.VTableM(bb, Vtbl.D3D10Texture2D_GetDevice);
        int hr = getDevice(bb, &dev);
        Com.Release(bb);
        if (hr < 0 || dev == 0) { Logger.Error("Dx10", $"GetDevice hr=0x{hr:X}"); return false; }
        device = dev;

        if (ImGui.ImplDX10Init(device) == 0) { Logger.Error("Dx10", "ImplDX10Init failed"); return false; }
        isReady = true;
        return true;
    }

    public void NewFrame() => ImGui.ImplDX10NewFrame();

    public void OnResize()
    {
        if (rtv != 0) { Com.Release(rtv); rtv = 0; }
        rtvBackbuffer = 0;
    }

    public void OnReset() { }
    public void OnResetPost() { }

    public void Render(nint drawData)
    {
        if (!isReady || drawData == 0) return;

        nint bb = Dxgi.GetBackBuffer(swapChain, 0, Guids.IID_ID3D10Texture2D);
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
        }
        finally { Com.Release(bb); }
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
        try { ImGui.ImplDX10Shutdown(); } catch { }
        if (device != 0) { Com.Release(device); device = 0; }
        swapChain = 0;
    }
}
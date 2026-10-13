using System;

namespace Kiero.Net;

internal sealed unsafe class Dx11Backend
{
    private static readonly Guid IID_ID3D11Texture2D =
        new("6f15aaf2-d208-4e89-9ab4-489535d34f9c");

    private nint _swapChain;
    private nint _device;
    private nint _context;
    private nint _rtv;
    private nint _rtvBb;
    private bool _ready;

    public bool IsReady => _ready;

    public bool Init(nint sc)
    {
        _swapChain = sc;
        Logger.Info("Dx11", $"Init sc=0x{sc:X}");

        nint bb = GetBackBuffer(sc, 0);
        if (bb == 0) { Logger.Error("Dx11", "backbuffer=0"); return false; }

        nint device = 0;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, nint*, int>)((*(void***)bb)[3]);
        int hr = getDevice(bb, &device);
        Release(bb);
        if (hr < 0 || device == 0) { Logger.Error("Dx11", $"GetDevice hr=0x{hr:X}"); return false; }
        _device = device;
        Logger.Info("Dx11", $"device=0x{device:X}");

        nint ctx = 0;
        var getCtx = (delegate* unmanaged[Stdcall]<nint, nint*, void>)((*(void***)device)[40]);
        getCtx(device, &ctx);
        if (ctx == 0) { Logger.Error("Dx11", "no context"); return false; }
        _context = ctx;
        Logger.Info("Dx11", $"context=0x{ctx:X}");

        if (ImGui.ImplDX11Init(_device, _context) == 0)
        {
            Logger.Error("Dx11", "ImplDX11Init failed");
            return false;
        }

        _ready = true;
        return true;
    }

    public void NewFrame() => ImGui.ImplDX11NewFrame();

    public void RenderDrawData(nint drawData)
    {
        if (!_ready || drawData == 0) return;

        nint bb = GetBackBuffer(_swapChain, 0);
        if (bb == 0) return;

        try
        {
            if (_rtv == 0 || _rtvBb != bb)
            {
                if (_rtv != 0) { Release(_rtv); _rtv = 0; }

                nint newRtv = 0;
                var createRtv = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint*, int>)
                    ((*(void***)_device)[9]);
                int hr = createRtv(_device, bb, 0, &newRtv);
                if (hr < 0 || newRtv == 0) return;
                _rtv = newRtv;
                _rtvBb = bb;
            }

            nint rtv = _rtv;
            var omSet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint, void>)
                ((*(void***)_context)[33]);
            omSet(_context, 1, &rtv, 0);

            ImGui.ImplDX11RenderDrawData(drawData);
        }
        finally
        {
            Release(bb);
        }
    }

    public void OnResize()
    {
        if (_rtv != 0) { Release(_rtv); _rtv = 0; }
        _rtvBb = 0;
    }

    public void Shutdown()
    {
        OnResize();
        if (_ready)
        {
            try { ImGui.ImplDX11Shutdown(); } catch { }
            _ready = false;
        }
        if (_context != 0) { Release(_context); _context = 0; }
        if (_device != 0) { Release(_device); _device = 0; }
    }

    private static nint GetBackBuffer(nint sc, uint idx)
    {
        nint bb = 0;
        var getBuffer = (delegate* unmanaged[Stdcall]<nint, uint, Guid*, nint*, int>)((*(void***)sc)[9]);
        Guid iid = IID_ID3D11Texture2D;
        return getBuffer(sc, idx, &iid, &bb) < 0 ? 0 : bb;
    }

    private static void Release(nint p)
    {
        if (p == 0) return;
        var rel = (delegate* unmanaged[Stdcall]<nint, uint>)((*(void***)p)[2]);
        rel(p);
    }
}
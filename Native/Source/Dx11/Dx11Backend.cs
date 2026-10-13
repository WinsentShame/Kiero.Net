using System;
using System.Runtime.InteropServices;

namespace Kiero.Net;

/// <summary>
/// DirectX 11 рендер-бэкенд. Держит device/context, кеширует RTV на back buffer,
/// прокидывает кадры ImGui. Отвечает ТОЛЬКО за DX11 — Win32-часть зовётся из хука.
/// </summary>
internal static unsafe class Dx11Backend
{
    // IID_ID3D11Texture2D
    private static readonly Guid IID_ID3D11Texture2D =
        new("6f15aaf2-d208-4e89-9ab4-489535d34f9c");

    private static nint _swapChain;
    private static nint _device;    // refcount++ при Init
    private static nint _context;   // refcount++ при Init
    private static nint _rtv;       // закешированный RTV для текущего back buffer
    private static nint _rtvBb;     // back buffer, из которого сделан _rtv
    private static bool _ready;

    public static bool IsReady => _ready;
    public static nint Device => _device;
    public static nint Context => _context;

    // vtable индексы (см. контекст проекта)
    private const int VT_GetBuffer = 9;   // IDXGISwapChain
    private const int VT_GetDevice = 3;   // ID3D11DeviceChild
    private const int VT_GetImmediateContext = 40;  // ID3D11Device
    private const int VT_CreateRenderTargetView = 9;   // ID3D11Device
    private const int VT_OMSetRenderTargets = 33;  // ID3D11DeviceContext
    private const int VT_Release = 2;   // IUnknown

    /// <summary>
    /// Достаёт device/context из swapchain и инициализирует DX11-бэкенд ImGui.
    /// Win32-бэкенд (shim_ImplWin32_Init) должен быть вызван ДО этого — он даёт ImGui хендл окна.
    /// </summary>
    public static bool Init(nint swapChain)
    {
        _swapChain = swapChain;
        _ready = false;

        nint bb = GetBackBuffer(swapChain, 0);
        if (bb == 0) { Logger.Warn("Dx11", "backbuffer=0"); return false; }

        // ID3D11DeviceChild::GetDevice — возвращает device через QueryInterface
        nint device = 0;
        var getDevice = (delegate* unmanaged[Stdcall]<nint, nint*, int>)((*(void***)bb)[VT_GetDevice]);
        int hr = getDevice(bb, &device);
        Release(bb);
        if (hr < 0 || device == 0) { Logger.Warn("Dx11", $"GetDevice hr=0x{hr:X}"); return false; }
        _device = device;

        // ID3D11Device::GetImmediateContext — void
        nint ctx = 0;
        var getCtx = (delegate* unmanaged[Stdcall]<nint, nint*, void>)((*(void***)device)[VT_GetImmediateContext]);
        getCtx(device, &ctx);
        if (ctx == 0) { Logger.Warn("Dx11", "GetImmediateContext=0"); return false; }
        _context = ctx;

        if (ImGui.ImplDX11Init(_device, _context) == 0)
        {
            Logger.Warn("Dx11", "shim_ImplDX11_Init вернул false");
            return false;
        }

        _ready = true;
        Logger.Info("Dx11", $"init ok dev=0x{_device:X} ctx=0x{_context:X}");
        return true;
    }

    /// <summary>Вызывать каждый кадр ПЕРЕД ImGui.NewFrame(), после ImplWin32NewFrame.</summary>
    public static void NewFrame()
    {
        if (!_ready) return;
        ImGui.ImplDX11NewFrame();
    }

    /// <summary>
    /// Рендерит draw data в текущий back buffer swapchain'а.
    /// Вызывать из детура Present сразу после ImGui.Render() и до вызова оригинала.
    /// </summary>
    public static void Render(nint drawData)
    {
        if (!_ready || drawData == 0) return;

        nint bb = GetBackBuffer(_swapChain, 0);
        if (bb == 0) return;

        // Пересоздаём RTV только когда back buffer реально сменился
        // (например, после Present флип-модель чередует два буфера).
        if (_rtv == 0 || _rtvBb != bb)
        {
            if (_rtv != 0) { Release(_rtv); _rtv = 0; }

            nint newRtv = 0;
            var createRtv = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint*, int>)
                ((*(void***)_device)[VT_CreateRenderTargetView]);
            int hr = createRtv(_device, bb, 0, &newRtv);
            if (hr < 0 || newRtv == 0)
            {
                Logger.Warn("Dx11", $"CreateRenderTargetView hr=0x{hr:X}");
                Release(bb);
                return;
            }
            _rtv = newRtv;
            _rtvBb = bb;
        }

        // OM stage
        nint rtv = _rtv;
        var omSet = (delegate* unmanaged[Stdcall]<nint, uint, nint*, nint, void>)
            ((*(void***)_context)[VT_OMSetRenderTargets]);
        omSet(_context, 1, &rtv, 0);

        // Сам ImGui: рисует в текущий RTV/context
        ImGui.ImplDX11RenderDrawData(drawData);

        // back buffer получен через GetBuffer (AddRef), освобождаем нашу ссылку
        Release(bb);
    }

    /// <summary>
    /// Вызывать из хука ResizeBuffers ДО вызова оригинала — иначе RTV станет висячим.
    /// </summary>
    public static void OnResize()
    {
        if (_rtv != 0) { Release(_rtv); _rtv = 0; }
        _rtvBb = 0;
    }

    public static void Shutdown()
    {
        OnResize();
        if (_ready)
        {
            try { ImGui.ImplDX11Shutdown(); } catch { }
            _ready = false;
        }
        if (_context != 0) { Release(_context); _context = 0; }
        if (_device != 0) { Release(_device); _device = 0; }
        _swapChain = 0;
    }

    private static nint GetBackBuffer(nint sc, uint index)
    {
        nint bb = 0;
        var getBuffer = (delegate* unmanaged[Stdcall]<nint, uint, Guid*, nint*, int>)
            ((*(void***)sc)[VT_GetBuffer]);
        Guid iid = IID_ID3D11Texture2D;
        int hr = getBuffer(sc, index, &iid, &bb);
        return hr < 0 ? 0 : bb;
    }

    private static void Release(nint p)
    {
        if (p == 0) return;
        var rel = (delegate* unmanaged[Stdcall]<nint, uint>)((*(void***)p)[VT_Release]);
        rel(p);
    }
}
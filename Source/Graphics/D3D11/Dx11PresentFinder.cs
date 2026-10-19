using System;

namespace Kiero.Net;

public static unsafe class Dx11PresentFinder
{
    public static bool TryGet(out nint present, out nint present1, out nint resizeBuffers)
        => TryGet(out present, out present1, out resizeBuffers, out _);

    public static bool TryGet(out nint present, out nint present1, out nint resizeBuffers, out nint resizeBuffers1)
    {
        present = 0;
        present1 = 0;
        resizeBuffers = 0;
        resizeBuffers1 = 0;

        nint hwnd = Win32.CreateWindowExW(0, "STATIC", "kiero-dummy", 0, 0, 0, 1, 1, 0, 0, 0, 0);
        if (hwnd == 0) return false;

        try
        {
            var desc = new Dxgi.SwapChainDesc
            {
                BufferDesc = new Dxgi.ModeDesc
                {
                    Width = 1,
                    Height = 1,
                    Format = Dxgi.FormatR8G8B8A8Unorm,
                    RefreshRateNum = 60,
                    RefreshRateDen = 1,
                },
                SampleDesc = new Dxgi.SampleDesc { Count = 1 },
                BufferUsage = Dxgi.UsageRenderTargetOutput,
                BufferCount = 2,
                OutputWindow = hwnd,
                Windowed = 1,
                SwapEffect = 0,
            };
            uint[] levels = { 0xb100, 0xb000 };
            nint sc = 0, dev = 0, ctx = 0;
            int hr;
            fixed (uint* pl = levels)
                hr = D3D11.D3D11CreateDeviceAndSwapChain(0, 1, 0, 0,
                    pl, (uint)levels.Length, 7, &desc, &sc, &dev, null, &ctx);

            if (hr < 0 || sc == 0) return false;

            try
            {
                nint vt = *(nint*)sc;
                present = *(nint*)(vt + 8 * IntPtr.Size);
                resizeBuffers = *(nint*)(vt + 13 * IntPtr.Size);

                nint sc3 = 0;
                Guid iid3 = Guids.IID_IDXGISwapChain3;
                if (Com.QueryInterface(sc, &iid3, &sc3) >= 0 && sc3 != 0)
                {
                    try
                    {
                        nint vt3 = *(nint*)sc3;
                        present1 = *(nint*)(vt3 + 22 * IntPtr.Size);                   
                        resizeBuffers1 = *(nint*)(vt3 + DxgiVt.SwapChain3_ResizeBuffers1 * IntPtr.Size);
                    }
                    finally
                    {
                        Com.Release(sc3);
                    }
                }
                else
                {
                    nint sc1 = 0;
                    Guid iid1 = Guids.IID_IDXGISwapChain1;
                    if (Com.QueryInterface(sc, &iid1, &sc1) >= 0 && sc1 != 0)
                    {
                        try
                        {
                            nint vt1 = *(nint*)sc1;
                            present1 = *(nint*)(vt1 + 22 * IntPtr.Size);
                        }
                        finally
                        {
                            Com.Release(sc1);
                        }
                    }
                }

                return present != 0;
            }
            finally
            {
                Com.Release(ctx);
                Com.Release(dev);
                Com.Release(sc);
            }
        }
        finally
        {
            Win32.DestroyWindow(hwnd);
        }
    }
}
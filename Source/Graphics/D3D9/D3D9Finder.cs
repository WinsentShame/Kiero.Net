namespace Kiero.Net;

public static unsafe class D3D9Finder
{
    public static bool TryGet(out nint present, out nint reset)
    {
        present = 0;
        reset = 0;

        nint hwnd = Win32.CreateWindowExW(0, "STATIC", "kiero-dummy", 0, 0, 0, 1, 1, 0, 0, 0, 0);
        if (hwnd == 0) return false;

        nint d3d9 = D3D9.Direct3DCreate9(32);
        if (d3d9 == 0) { Win32.DestroyWindow(hwnd); return false; }

        try
        {
            var pp = new D3D9Structs.D3DPRESENT_PARAMETERS
            {
                Windowed = 1,
                SwapEffect = 1,
                BackBufferFormat = 21,
                hDeviceWindow = hwnd,
            };

            nint device = 0;
            var createDevice = (delegate* unmanaged[Stdcall]<nint, uint, uint, nint, uint, D3D9Structs.D3DPRESENT_PARAMETERS*, nint*, int>)
                Vtbl.VTableM(d3d9, Vtbl.Direct3D9_CreateDevice);

            int hr = createDevice(d3d9, 0, 1, hwnd, 32, &pp, &device);
            if (hr < 0 || device == 0) return false;

            try
            {
                present = Vtbl.VTableM(device, Vtbl.Direct3DDevice9_Present);
                reset = Vtbl.VTableM(device, Vtbl.Direct3DDevice9_Reset);

                Logger.Info("D3D9Finder", $"Present=0x{present:X} Reset=0x{reset:X}");
                return present != 0;
            }
            finally
            {
                Com.Release(device);
            }
        }
        finally
        {
            Com.Release(d3d9);
            Win32.DestroyWindow(hwnd);
        }
    }
}
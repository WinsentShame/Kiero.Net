using System;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class Dx9Finder
{
    [DllImport("d3d9.dll")]
    private static extern nint Direct3DCreate9(uint sdkVersion);

    [StructLayout(LayoutKind.Sequential)]
    private struct D3DPRESENT_PARAMETERS
    {
        public uint BackBufferWidth;
        public uint BackBufferHeight;
        public uint BackBufferFormat;
        public uint BackBufferCount;
        public uint MultiSampleType;
        public uint MultiSampleQuality;
        public uint SwapEffect;
        public nint hDeviceWindow;
        public int Windowed;
        public int EnableAutoDepthStencil;
        public uint AutoDepthStencilFormat;
        public uint Flags;
        public uint FullScreen_RefreshRateInHz;
        public uint PresentationInterval;
    }

    public static bool TryGet(out nint present, out nint reset)
    {
        present = 0;
        reset = 0;

        nint hwnd = Win32.CreateWindowExW(0, "STATIC", "kiero-dummy", 0, 0, 0, 1, 1, 0, 0, 0, 0);
        if (hwnd == 0) return false;

        nint d3d9 = Direct3DCreate9(32); 
        if (d3d9 == 0) { Win32.DestroyWindow(hwnd); return false; }

        try
        {
            var pp = new D3DPRESENT_PARAMETERS
            {
                Windowed = 1,
                SwapEffect = 1,        
                BackBufferFormat = 21, 
                hDeviceWindow = hwnd,
            };

            nint device = 0;
            nint vtD3D9 = *(nint*)d3d9;
            var createDevice = (delegate* unmanaged[Stdcall]<nint, uint, uint, nint, uint, D3DPRESENT_PARAMETERS*, nint*, int>)
                (*(nint*)(vtD3D9 + 16 * IntPtr.Size));

            int hr = createDevice(d3d9, 0, 1, hwnd, 32, &pp, &device);
            if (hr < 0 || device == 0) return false;

            try
            {
                nint vt = *(nint*)device;
                present = *(nint*)(vt + 17 * IntPtr.Size); 
                reset = *(nint*)(vt + 16 * IntPtr.Size); 

                Logger.Info("Dx9Finder", $"Present=0x{present:X} Reset=0x{reset:X}");
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
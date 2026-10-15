using System;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class D3D12
{
    public const int FEATURE_LEVEL_11_0 = 0xb000;

    [DllImport("d3d12.dll")]
    public static extern int D3D12CreateDevice(nint adapter, int minimumFeatureLevel, Guid* riid, nint* ppDevice);

    [DllImport("dxgi.dll")]
    public static extern int CreateDXGIFactory2(uint flags, Guid* riid, nint* ppFactory);
}
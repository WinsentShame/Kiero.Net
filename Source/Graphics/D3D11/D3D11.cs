using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class D3D11
{
    [DllImport("d3d11.dll")]
    public static extern int D3D11CreateDeviceAndSwapChain(
        nint adapter, uint driverType, nint software, uint flags,
        uint* featureLevels, uint featureLevelsCount, uint sdkVersion,
        Dxgi.SwapChainDesc* desc, nint* swapChain,
        nint* device, uint* featureLevel, nint* context);
}
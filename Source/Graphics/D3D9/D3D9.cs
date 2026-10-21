using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class D3D9
{
    [DllImport("d3d9.dll")]
    public static extern nint Direct3DCreate9(uint sdkVersion);
}

using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class D3D10Shim
{
    private const string Shim = "kiero_native";

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D10_CreateRenderTargetView")]
    public static extern void CreateRenderTargetView(nint dev, nint res, nint desc, out nint rtv);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D10_OMSetRenderTargets")]
    public static extern void OMSetRenderTargets(nint dev, uint n, nint* rtvs, nint dsv);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D10_OMGetRenderTargets")]
    public static extern void OMGetRenderTargets(nint dev, uint n, nint* rtvs, nint* dsv);
}
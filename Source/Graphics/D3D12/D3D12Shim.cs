using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class D3D12Shim
{
    private const string Shim = "kiero_native";

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_GetCPUDescriptorHandleForHeapStart")]
    public static extern ulong GetCpuHandle(nint heap);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_CommandQueueGetType")]
    public static extern int QueueGetType(nint queue);
}
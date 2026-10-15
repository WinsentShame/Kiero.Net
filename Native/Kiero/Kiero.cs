using System;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class Kiero
{
    private const string Lib = "kiero_native";

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_init")]
    private static extern int NativeInit(int renderType);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_shutdown")]
    private static extern void NativeShutdown();

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_get_methods_table")]
    private static extern nint NativeGetMethodsTable();

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_get_render_type")]
    private static extern int NativeGetRenderType();

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_GetCPUDescriptorHandleForHeapStart")]
    public static extern ulong GetCpuHandle(nint heap);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_GetGPUDescriptorHandleForHeapStart")]
    public static extern ulong GetGpuHandle(nint heap);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_CreateRenderTargetView")]
    public static extern void CreateRtv(nint device, nint resource, nint desc, ulong cpuHandle);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_OMSetRenderTargets")]
    public static extern void OMSetRenderTargets(nint cmdList, uint numRTVs, ulong* rtvHandles, nint dsv);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_ResourceBarrier")]
    public static extern void ResourceBarrier(nint cmdList, uint num, void* barriers);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_CommandListReset")]
    public static extern void CmdListReset(nint cmdList, nint allocator, nint pso);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_CommandListClose")]
    public static extern int CmdListClose(nint cmdList);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_CommandAllocatorReset")]
    public static extern int AllocReset(nint allocator);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_ExecuteCommandLists")]
    public static extern void ExecCmdLists(nint queue, uint num, void** lists);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_QueueSignal")]
    public static extern int QueueSignal(nint queue, nint fence, ulong value);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_FenceGetCompletedValue")]
    public static extern ulong FenceGetCompleted(nint fence);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_SetDescriptorHeaps")]
    public static extern void SetDescriptorHeaps(nint cmdList, uint numHeaps, void** heaps);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_D3D12_CommandQueueGetType")]
    public static extern int QueueGetType(nint queue);

    public static KieroStatus Init(KieroRenderType type) => (KieroStatus)NativeInit((int)type);
    public static void Shutdown() => NativeShutdown();
    public static nint GetMethodsTable() => NativeGetMethodsTable();
    public static KieroRenderType GetRenderType() => (KieroRenderType)NativeGetRenderType();

    public static nint GetMethod(int index)
    {
        nint t = GetMethodsTable();
        if (t == 0) return 0;
        return *(nint*)(t + index * IntPtr.Size);
    }
}
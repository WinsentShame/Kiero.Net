using System;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class Kiero
{
    private const string Dll = "kiero_native";

    [DllImport(Dll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_init")]
    private static extern int NativeInit(int renderType);

    [DllImport(Dll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_shutdown")]
    private static extern void NativeShutdown();

    [DllImport(Dll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_get_methods_table")]
    private static extern nint NativeGetMethodsTable();

    [DllImport(Dll, CallingConvention = CallingConvention.Cdecl, EntryPoint = "kiero_wrapper_get_render_type")]
    private static extern int NativeGetRenderType();

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
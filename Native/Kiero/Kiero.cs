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

    public static KieroStatus Init(KieroRenderType type) => (KieroStatus)NativeInit((int)type);
    public static void Shutdown() => NativeShutdown();
    public static nint GetMethodsTable() => NativeGetMethodsTable();
    public static KieroRenderType GetRenderType() => (KieroRenderType)NativeGetRenderType();
}
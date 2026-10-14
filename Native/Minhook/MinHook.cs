using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class MinHook
{
    private const string lib = "minhook";

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MH_Initialize")]
    public static extern int Initialize();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MH_Uninitialize")]
    public static extern int Uninitialize();

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MH_CreateHook")]
    public static extern int CreateHook(void* target, void* detour, void** original);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MH_EnableHook")]
    public static extern int EnableHook(void* target);

    [DllImport(lib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "MH_DisableHook")]
    public static extern int DisableHook(void* target);
}
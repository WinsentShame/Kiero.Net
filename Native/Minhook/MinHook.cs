using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class MinHook
{
    private const string Lib = "minhook";

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_Initialize();

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_Uninitialize();

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_CreateHook(void* target, void* detour, void** original);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_EnableHook(void* target);

    [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
    public static extern int MH_DisableHook(void* target);
}
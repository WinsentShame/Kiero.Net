using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class PresentHook
{
    private static nint original;
    private static long counter;

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        if (MinHook.MH_Initialize() != 0) return false;

        void* detour = (void*)(delegate* unmanaged[Stdcall]<nint, uint, uint, int>)&Detour;
        void* orig = null;
        int hr = MinHook.MH_CreateHook((void*)addr, detour, &orig);
        if (hr != 0) return false;

        original = (nint)orig;
        MinHook.MH_EnableHook((void*)addr);
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour(nint sc, uint sync, uint flags)
    {
        long n = ++counter;
        if (n == 1 || n % 300 == 0)
            Logger.Info("Present", $"#{n} sc=0x{sc:X}");

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, int>)original)(sc, sync, flags);
    }
}
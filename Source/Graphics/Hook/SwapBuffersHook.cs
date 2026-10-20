using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class SwapBuffersHook
{
    private static nint original;
    private static int installed;

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installed, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, int>)&Detour;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installed = 0; return false; }
        original = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installed = 0; return false; }
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour(nint hdc)
    {
        try { Overlay.OnSwapBuffers(hdc); }
        catch (System.Exception ex) { Logger.Error("SwapBuffers", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, int>)original)(hdc);
    }
}
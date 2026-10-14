using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class PresentHook
{
    private static nint originalPresent;
    private static nint originalPresent1;
    private static long presentCalls;
    private static long present1Calls;

    public static long presentCallCount => Interlocked.Read(ref presentCalls);
    public static long present1CallCount => Interlocked.Read(ref present1Calls);

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        return InstallOne(addr, false);
    }

    public static bool InstallPresent1(nint addr)
    {
        if (addr == 0) return false;
        return InstallOne(addr, true);
    }

    private static bool InstallOne(nint addr, bool isPresent1)
    {
        void* detour = isPresent1
            ? (delegate* unmanaged[Stdcall]<nint, uint, uint, void*, int>)&DetourPresent1
            : (delegate* unmanaged[Stdcall]<nint, uint, uint, int>)&DetourPresent;

        void* orig = null;
        int hr = MinHook.CreateHook((void*)addr, detour, &orig);
        if (hr != 0)
        {
            Logger.Warn("Present", $"CreateHook({(isPresent1 ? "Present1" : "Present")}) hr={hr}");
            return false;
        }

        if (isPresent1) originalPresent1 = (nint)orig;
        else originalPresent = (nint)orig;

        int hr2 = MinHook.EnableHook((void*)addr);
        if (hr2 != 0)
        {
            Logger.Warn("Present", $"EnableHook hr={hr2}");
            return false;
        }
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent(nint sc, uint sync, uint flags)
    {
        long n = Interlocked.Increment(ref presentCalls);
        if (n <= 3 || n % 300 == 0)
            Logger.Info("Present", $"Present #{n} sc=0x{sc:X} flags=0x{flags:X}");

        try { Overlay.OnPresentInternal(sc, flags); }
        catch (System.Exception ex) { Logger.Error("Present", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, int>)originalPresent)(sc, sync, flags);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent1(nint sc, uint sync, uint flags, void* presentParams)
    {
        long n = Interlocked.Increment(ref present1Calls);
        if (n <= 3 || n % 300 == 0)
            Logger.Info("Present", $"Present1 #{n} sc=0x{sc:X} flags=0x{flags:X}");

        try { Overlay.OnPresentInternal(sc, flags); }
        catch (System.Exception ex) { Logger.Error("Present", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, void*, int>)originalPresent1)(sc, sync, flags, presentParams);
    }
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class PresentHook
{
    private static nint _originalPresent;
    private static nint _originalPresent1;
    private static long _presentCalls;
    private static long _present1Calls;

    public static long PresentCalls => Interlocked.Read(ref _presentCalls);
    public static long Present1Calls => Interlocked.Read(ref _present1Calls);

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        if (MinHook.MH_Initialize() != 0) return false;
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
            ? (void*)(delegate* unmanaged[Stdcall]<nint, uint, void*, uint, int>)&DetourPresent1
            : (void*)(delegate* unmanaged[Stdcall]<nint, uint, uint, int>)&DetourPresent;

        void* orig = null;
        int hr = MinHook.MH_CreateHook((void*)addr, detour, &orig);
        if (hr != 0)
        {
            Logger.Warn("Present", $"MH_CreateHook({(isPresent1 ? "Present1" : "Present")}) hr={hr}");
            return false;
        }

        if (isPresent1) _originalPresent1 = (nint)orig;
        else _originalPresent = (nint)orig;

        int hr2 = MinHook.MH_EnableHook((void*)addr);
        if (hr2 != 0)
        {
            Logger.Warn("Present", $"MH_EnableHook hr={hr2}");
            return false;
        }
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent(nint sc, uint sync, uint flags)
    {
        long n = Interlocked.Increment(ref _presentCalls);
        if (n <= 3 || n % 300 == 0)
            Logger.Info("Present", $"Present #{n} sc=0x{sc:X} flags=0x{flags:X}");
        try { Overlay.OnPresentInternal(sc, flags); } catch (System.Exception ex) { Logger.Error("Present", ex); }
        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, int>)_originalPresent)(sc, sync, flags);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent1(nint sc, uint sync, void* presentParams, uint flags)
    {
        long n = Interlocked.Increment(ref _present1Calls);
        if (n <= 3 || n % 300 == 0)
            Logger.Info("Present", $"Present1 #{n} sc=0x{sc:X} flags=0x{flags:X}");
        try { Overlay.OnPresentInternal(sc, flags); } catch (System.Exception ex) { Logger.Error("Present", ex); }
        return ((delegate* unmanaged[Stdcall]< nint, uint, void*, uint, int>)_originalPresent1)(sc, sync, presentParams, flags);
    }
}
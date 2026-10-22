using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class PresentHook
{
    private static nint originalPresent;
    private static nint originalPresent1;
    private static nint originalPresentDx9;
    private static long presentCalls;
    private static long present1Calls;
    private static long presentCallsDx9;

    //public static long presentCallCount => Interlocked.Read(ref presentCalls);
    //public static long present1CallCount => Interlocked.Read(ref present1Calls);
    //public static long presentCallCountDx9 => Interlocked.Read(ref presentCallsDx9);

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

    public static bool InstallDx9(nint addr)
    {
        if (addr == 0) return false;

        void* detour = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint, nint, int>)&DetourPresentDx9;
        void* orig = null;
        int hr = MinHook.CreateHook((void*)addr, detour, &orig);
        if (hr != 0)
        {
            Logger.Warn("Present", $"CreateHook(Dx9 Present) hr={hr}");
            return false;
        }
        originalPresentDx9 = (nint)orig;

        int hr2 = MinHook.EnableHook((void*)addr);
        if (hr2 != 0)
        {
            Logger.Warn("Present", $"EnableHook(Dx9) hr={hr2}");
            return false;
        }
        return true;
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

        try { Overlay.OnPresent(sc, flags, false); }
        catch (System.Exception ex) { Logger.Error("Present", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, int>)originalPresent)(sc, sync, flags);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent1(nint sc, uint sync, uint flags, void* presentParams)
    {
        long n = Interlocked.Increment(ref present1Calls);
        if (n <= 3 || n % 300 == 0)
            Logger.Info("Present", $"Present1 #{n} sc=0x{sc:X} flags=0x{flags:X}");

        try { Overlay.OnPresent(sc, flags, false); }
        catch (System.Exception ex) { Logger.Error("Present", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, void*, int>)originalPresent1)(sc, sync, flags, presentParams);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresentDx9(nint device, nint srcRect, nint dstRect, nint hwndOverride, nint dirtyRegion)
    {
        long n = Interlocked.Increment(ref presentCallsDx9);
        if (n <= 3 || n % 300 == 0)
            Logger.Info("Present", $"PresentDx9 #{n} dev=0x{device:X}");

        try { Overlay.OnPresent(device, 0, false); }
        catch (System.Exception ex) { Logger.Error("Present", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, nint, nint, nint, nint, int>)originalPresentDx9)(device, srcRect, dstRect, hwndOverride, dirtyRegion);
    }
}
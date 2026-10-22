using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class ResizeBuffersHook
{
    private static nint original;
    private static nint original1;
    private static nint originalDx9Reset;
    private static int installed;
    private static int installed1;
    private static int installedDx9;

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installed, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, int>)&Detour;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installed = 0; return false; }
        original = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installed = 0; return false; }
        return true;
    }

    public static bool InstallResizeBuffers1(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installed1, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, nint, nint, int>)&Detour1;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installed1 = 0; return false; }
        original1 = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installed1 = 0; return false; }
        return true;
    }

    public static bool InstallDx9Reset(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installedDx9, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, nint, int>)&DetourDx9Reset;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installedDx9 = 0; return false; }
        originalDx9Reset = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installedDx9 = 0; return false; }
        return true;
    }

    private static void OnResize(nint sc, string tag)
    {
        try { Overlay.Backend?.OnResize(); }
        catch (System.Exception ex) { Logger.Error(tag, ex); }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour(nint sc, uint bufferCount, uint width, uint height, uint newFormat, uint flags)
    {
        Logger.Info("ResizeBuffers", $"sc=0x{sc:X} count={bufferCount} {width}x{height} fmt={newFormat}");
        OnResize(sc, "ResizeBuffers");
        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, int>)original)(sc, bufferCount, width, height, newFormat, flags);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour1(nint sc, uint bufferCount, uint width, uint height, uint newFormat, uint flags, nint creationNodeMask, nint presentQueue)
    {
        Logger.Info("ResizeBuffers1", $"sc=0x{sc:X} count={bufferCount} {width}x{height} fmt={newFormat}");
        OnResize(sc, "ResizeBuffers1");
        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, nint, nint, int>)original1)(sc, bufferCount, width, height, newFormat, flags, creationNodeMask, presentQueue);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourDx9Reset(nint device, nint presentParams)
    {
        Logger.Info("Reset", $"Dx9 device=0x{device:X}");

        try { Overlay.Backend?.OnReset(); }
        catch (System.Exception ex) { Logger.Error("Reset", ex); }

        int hr = ((delegate* unmanaged[Stdcall]<nint, nint, int>)originalDx9Reset)(device, presentParams);

        if (hr >= 0)
        {
            try { Overlay.Backend?.OnResetPost(); }
            catch (System.Exception ex) { Logger.Error("Reset", ex); }
        }
        return hr;
    }
}
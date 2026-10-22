using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class PresentHook
{
    private static nint originalPresent;
    private static nint originalPresent1;
    private static nint originalPresentDx9;
    private static int installedPresent;
    private static int installedPresent1;
    private static int installedDx9;

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installedPresent, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, uint, uint, int>)&DetourPresent;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installedPresent = 0; return false; }
        originalPresent = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installedPresent = 0; return false; }
        return true;
    }

    public static bool InstallPresent1(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installedPresent1, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, uint, uint, void*, int>)&DetourPresent1;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installedPresent1 = 0; return false; }
        originalPresent1 = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installedPresent1 = 0; return false; }
        return true;
    }

    public static bool InstallDx9(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installedDx9, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, nint, nint, nint, nint, int>)&DetourPresentDx9;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installedDx9 = 0; return false; }
        originalPresentDx9 = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installedDx9 = 0; return false; }
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent(nint sc, uint sync, uint flags)
    {
        try { Overlay.OnPresent(sc, flags, PresentSource.DxGi); }
        catch (System.Exception ex) { Logger.Error("Present", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, int>)originalPresent)(sc, sync, flags);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresent1(nint sc, uint sync, uint flags, void* presentParams)
    {
        try { Overlay.OnPresent(sc, flags, PresentSource.DxGi); }
        catch (System.Exception ex) { Logger.Error("Present1", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, void*, int>)originalPresent1)(sc, sync, flags, presentParams);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourPresentDx9(nint device, nint srcRect, nint dstRect, nint hwndOverride, nint dirtyRegion)
    {
        try { Overlay.OnPresent(device, 0, PresentSource.Dx9); }
        catch (System.Exception ex) { Logger.Error("PresentDx9", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, nint, nint, nint, nint, int>)originalPresentDx9)(device, srcRect, dstRect, hwndOverride, dirtyRegion);
    }
}
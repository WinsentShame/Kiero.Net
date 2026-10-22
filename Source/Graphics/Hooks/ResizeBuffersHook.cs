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
        int hr = MinHook.CreateHook((void*)addr, detour, &orig);
        if (hr != 0) { Logger.Warn("ResizeBuffers", $"CreateHook hr={hr}"); installed = 0; return false; }
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
        int hr = MinHook.CreateHook((void*)addr, detour, &orig);
        if (hr != 0) { Logger.Warn("ResizeBuffers1", $"CreateHook hr={hr}"); installed1 = 0; return false; }
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
        int hr = MinHook.CreateHook((void*)addr, detour, &orig);
        if (hr != 0) { Logger.Warn("Reset", $"CreateHook hr={hr}"); installedDx9 = 0; return false; }
        originalDx9Reset = (nint)orig;

        if (MinHook.EnableHook((void*)addr) != 0) { installedDx9 = 0; return false; }
        return true;
    }

    private static void OnResize(nint sc, uint bufferCount, uint width, uint height, uint newFormat, uint flags, string tag)
    {
        Logger.Info(tag, $"sc=0x{sc:X} count={bufferCount} {width}x{height} fmt={newFormat} flags=0x{flags:X}");

        try
        {
            if (Overlay.Dx10SwapChain == sc) Overlay.Dx10Backend?.ReleaseRenderTarget();
            if (Overlay.Dx11SwapChain == sc) Overlay.Dx11Backend?.ReleaseRenderTarget();
            if (Overlay.Dx12SwapChain == sc) Overlay.Dx12Backend?.OnResizeBuffers();
        }
        catch (System.Exception ex) { Logger.Error(tag, ex); }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour(nint sc, uint bufferCount, uint width, uint height, uint newFormat, uint flags)
    {
        OnResize(sc, bufferCount, width, height, newFormat, flags, "ResizeBuffers");
        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, int>)original)(sc, bufferCount, width, height, newFormat, flags);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour1(nint sc, uint bufferCount, uint width, uint height, uint newFormat, uint flags, nint creationNodeMask, nint presentQueue)
    {
        OnResize(sc, bufferCount, width, height, newFormat, flags, "ResizeBuffers1");
        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, nint, nint, int>)original1)(sc, bufferCount, width, height, newFormat, flags, creationNodeMask, presentQueue);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourDx9Reset(nint device, nint presentParams)
    {
        Logger.Info("Reset", $"Dx9 device=0x{device:X}");

        try { Overlay.Dx9Backend?.OnReset(); }
        catch (System.Exception ex) { Logger.Error("Reset", ex); }

        int hr = ((delegate* unmanaged[Stdcall]<nint, nint, int>)originalDx9Reset)(device, presentParams);

        if (hr >= 0)
        {
            try { Overlay.Dx9Backend?.OnResetPost(); }
            catch (System.Exception ex) { Logger.Error("Reset", ex); }
        }

        return hr;
    }
}
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class ResizeBuffersHook
{
    private static nint original;

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;

        void* detour = (delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, int>)&Detour;
        void* orig = null;
        int hr = MinHook.CreateHook((void*)addr, detour, &orig);
        if (hr != 0) { Logger.Warn("ResizeBuffers", $"CreateHook hr={hr}"); return false; }
        original = (nint)orig;

        if (MinHook.EnableHook((void*)addr) != 0) return false;
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int Detour(nint sc, uint bufferCount, uint width, uint height, uint newFormat, uint flags)
    {
        Logger.Info("ResizeBuffers", $"sc=0x{sc:X} count={bufferCount} {width}x{height} fmt={newFormat} flags=0x{flags:X}");

        try
        {
            Dx11Backend be = Overlay.Dx11Backend;
            if (be != null && Overlay.Dx11SwapChain == sc)
                be.ReleaseRenderTarget();
        }
        catch (System.Exception ex) { Logger.Error("ResizeBuffers", ex); }

        return ((delegate* unmanaged[Stdcall]<nint, uint, uint, uint, uint, uint, int>)original)(sc, bufferCount, width, height, newFormat, flags);
    }
}
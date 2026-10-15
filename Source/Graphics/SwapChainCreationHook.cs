using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class SwapChainCreationHook
{
    private static nint originalForHwnd;
    private static nint originalForComposition;
    private static nint gameSwapChain;

    public static nint GameSwapChain => gameSwapChain;

    public static bool Install(nint anyFactory)
    {
        if (anyFactory == 0) return false;
        if (originalForHwnd != 0 || originalForComposition != 0) return true;

        nint vt = *(nint*)anyFactory;

        nint addrForHwnd = *(nint*)(vt + 15 * IntPtr.Size);
        nint addrForComposition = *(nint*)(vt + 24 * IntPtr.Size);

        if (addrForHwnd != 0)
        {
            void* detour = (delegate* unmanaged[Stdcall]<nint, nint, nint, void*, nint, nint, nint*, int>)&DetourForHwnd;
            void* orig = null;
            if (MinHook.CreateHook((void*)addrForHwnd, detour, &orig) == 0)
            {
                originalForHwnd = (nint)orig;
                MinHook.EnableHook((void*)addrForHwnd);
                Logger.Info("SwapChainHook", $"CreateSwapChainForHwnd hooked at 0x{addrForHwnd:X}");
            }
        }

        if (addrForComposition != 0)
        {
            void* detour = (delegate* unmanaged[Stdcall]<nint, nint, void*, nint, nint*, int>)&DetourForComposition;
            void* orig = null;
            if (MinHook.CreateHook((void*)addrForComposition, detour, &orig) == 0)
            {
                originalForComposition = (nint)orig;
                MinHook.EnableHook((void*)addrForComposition);
                Logger.Info("SwapChainHook", $"CreateSwapChainForComposition hooked at 0x{addrForComposition:X}");
            }
        }

        return originalForHwnd != 0 || originalForComposition != 0;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourForHwnd(nint factory, nint device, nint hwnd, void* desc, nint restrict, nint output, nint* sc)
    {
        int hr = ((delegate* unmanaged[Stdcall]<nint, nint, nint, void*, nint, nint, nint*, int>)originalForHwnd)(factory, device, hwnd, desc, restrict, output, sc);

        if (hr >= 0 && sc != null && *sc != 0 && gameSwapChain == 0)
        {
            gameSwapChain = *sc;
            Logger.Info("SwapChainHook", $"captured (Hwnd) sc=0x{gameSwapChain:X}");
        }
        return hr;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourForComposition(nint factory, nint device, void* desc, nint restrict, nint* sc)
    {
        int hr = ((delegate* unmanaged[Stdcall]<nint, nint, void*, nint, nint*, int>)originalForComposition)(factory, device, desc, restrict, sc);

        if (hr >= 0 && sc != null && *sc != 0 && gameSwapChain == 0)
        {
            gameSwapChain = *sc;
            Logger.Info("SwapChainHook", $"captured (Composition) sc=0x{gameSwapChain:X}");
        }
        return hr;
    }
}
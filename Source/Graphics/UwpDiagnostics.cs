using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net.Example;

public static unsafe class UwpDiagnostics
{
    private static int installed;
    private static nint origForCoreWindow;
    private static nint origForComposition;
    private static nint origCreateCompDevice;

    public static void Install()
    {
        if (Interlocked.CompareExchange(ref installed, 1, 0) != 0) return;

        if (MinHook.Initialize() != 0)
        {
            Logger.Warn("UwpDiag", "MinHook.Initialize failed");
            return;
        }

        nint dxgi = Win32.GetModuleHandleW("dxgi.dll");
        nint dcomp = Win32.GetModuleHandleW("dcomp.dll");
        Logger.Info("UwpDiag", $"dxgi=0x{dxgi:X} dcomp=0x{dcomp:X}");

        if (dxgi != 0)
        {
            nint addrFactory = Win32.GetProcAddress(dxgi, "CreateDXGIFactory2");
            if (addrFactory != 0)
            {
                var createFactory = (delegate* unmanaged[Stdcall]<uint, Guid*, nint*, int>)addrFactory;
                nint factory = 0;
                Guid iid = new("50c83a1c-e072-4c48-87b0-3630fa36a6d0");
                if (createFactory(0, &iid, &factory) >= 0 && factory != 0)
                {
                    nint vt = *(nint*)factory;
                    nint addrCore = *(nint*)(vt + 16 * IntPtr.Size);
                    nint addrComp = *(nint*)(vt + 26 * IntPtr.Size);
                    Logger.Info("UwpDiag", $"ForCoreWindow=0x{addrCore:X} ForComposition=0x{addrComp:X}");

                    void* detCore = (void*)(delegate* unmanaged[Stdcall]<nint, nint, nint, nint, uint, nint*, int>)&DetourForCoreWindow;
                    void* detComp = (void*)(delegate* unmanaged[Stdcall]<nint, nint, nint, nint, nint*, int>)&DetourForComposition;
                    void* o;
                    o = null; if (MinHook.CreateHook((void*)addrCore, detCore, &o) == 0) { origForCoreWindow = (nint)o; MinHook.EnableHook((void*)addrCore); Logger.Info("UwpDiag", "ForCoreWindow hooked"); }
                    o = null; if (MinHook.CreateHook((void*)addrComp, detComp, &o) == 0) { origForComposition = (nint)o; MinHook.EnableHook((void*)addrComp); Logger.Info("UwpDiag", "ForComposition hooked"); }
                }
            }
        }

        if (dcomp != 0)
        {
            nint addrCompDev = Win32.GetProcAddress(dcomp, "DCompositionCreateDevice");
            Logger.Info("UwpDiag", $"DCompositionCreateDevice=0x{addrCompDev:X}");
            if (addrCompDev != 0)
            {
                void* det = (void*)(delegate* unmanaged[Stdcall]<nint, Guid*, nint*, int>)&DetourCreateCompDevice;
                void* o = null;
                if (MinHook.CreateHook((void*)addrCompDev, det, &o) == 0)
                {
                    origCreateCompDevice = (nint)o;
                    MinHook.EnableHook((void*)addrCompDev);
                    Logger.Info("UwpDiag", "DCompositionCreateDevice hooked");
                }
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourForCoreWindow(nint factory, nint device, nint coreWindow, nint desc, uint restrict, nint* sc)
    {
        Logger.Info("UwpDiag", $"CreateSwapChainForCoreWindow coreWindow=0x{coreWindow:X}");
        int hr = ((delegate* unmanaged[Stdcall]<nint, nint, nint, nint, uint, nint*, int>)origForCoreWindow)(factory, device, coreWindow, desc, restrict, sc);
        nint result = (hr >= 0 && sc != null) ? *sc : 0;
        Logger.Info("UwpDiag", $"ForCoreWindow -> hr=0x{hr:X} sc=0x{result:X}");
        return hr;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourForComposition(nint factory, nint device, nint desc, nint restrict, nint* sc)
    {
        Logger.Info("UwpDiag", "CreateSwapChainForComposition");
        int hr = ((delegate* unmanaged[Stdcall]<nint, nint, nint, nint, nint*, int>)origForComposition)(factory, device, desc, restrict, sc);
        nint result = (hr >= 0 && sc != null) ? *sc : 0;
        Logger.Info("UwpDiag", $"ForComposition -> hr=0x{hr:X} sc=0x{result:X}");
        return hr;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static int DetourCreateCompDevice(nint dxgiDevice, Guid* iid, nint* device)
    {
        Logger.Info("UwpDiag", $"DCompositionCreateDevice dxgiDevice=0x{dxgiDevice:X}");
        int hr = ((delegate* unmanaged[Stdcall]<nint, Guid*, nint*, int>)origCreateCompDevice)(dxgiDevice, iid, device);
        nint result = (hr >= 0 && device != null) ? *device : 0;
        Logger.Info("UwpDiag", $"DCompositionCreateDevice -> hr=0x{hr:X} device=0x{result:X}");
        return hr;
    }
}
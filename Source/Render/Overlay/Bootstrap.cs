using System;
using System.Threading;

namespace Kiero.Net;

public static unsafe class Bootstrap
{
    public static bool Start(TimeSpan? moduleTimeout = null)
    {
        int waited = 0;
        int limit = (int)(moduleTimeout ?? TimeSpan.FromSeconds(60)).TotalMilliseconds;

        bool has9 = false, has10 = false, has11 = false, has12 = false, hasGl = false;
        while (waited < limit)
        {
            has9 = Win32.GetModuleHandleW("d3d9.dll") != 0;
            has10 = Win32.GetModuleHandleW("d3d10.dll") != 0;
            has11 = Win32.GetModuleHandleW("d3d11.dll") != 0;
            has12 = Win32.GetModuleHandleW("d3d12.dll") != 0;
            hasGl = Win32.GetModuleHandleW("opengl32.dll") != 0;
            if (has9 || has10 || has11 || has12 || hasGl) break;
            Thread.Sleep(200);
            waited += 200;
        }

        Logger.Info("Bootstrap", $"after wait {waited}ms: d3d9={has9} d3d10={has10} d3d11={has11} d3d12={has12} opengl32={hasGl}");
        if (!has9 && !has10 && !has11 && !has12 && !hasGl) { Logger.Error("Bootstrap", "no d3d9/d3d10/d3d11/d3d12/opengl32"); return false; }

        if (MinHook.Initialize() != 0) { Logger.Error("Bootstrap", "MinHook.Initialize failed"); return false; }
        Logger.Info("Bootstrap", "MinHook initialized");

        bool anyInstalled = false;

        if (hasGl)
        {
            if (InstallGl()) anyInstalled = true;
        }

        if (has9)
        {
            var st = Kiero.Init(KieroRenderType.D3D9);
            if (st == KieroStatus.Success) { if (InstallDx9()) anyInstalled = true; }
            else Logger.Warn("Bootstrap", $"Kiero.Init(D3D9) -> {st}, skip");
        }
        else if (has10)
        {
            if (InstallDx10()) anyInstalled = true;
        }
        else if (has12)
        {
            var st = Kiero.Init(KieroRenderType.D3D12);
            if (st == KieroStatus.Success) { if (InstallDx12()) anyInstalled = true; }
            else Logger.Warn("Bootstrap", $"Kiero.Init(D3D12) -> {st}, skip");
        }
        else if (has11)
        {
            var st = Kiero.Init(KieroRenderType.D3D11);
            if (st == KieroStatus.Success) { if (InstallDx11()) anyInstalled = true; }
            else Logger.Warn("Bootstrap", $"Kiero.Init(D3D11) -> {st}, skip");
        }

        if (!anyInstalled)
        {
            Logger.Error("Bootstrap", "no hooks installed");
            return false;
        }

        return true;
    }

    private static bool InstallGl()
    {
        if (!OpenglFinder.TryGet(out nint swapBuffersAddr))
        {
            Logger.Error("Bootstrap", "OpenglFinder failed");
            return false;
        }

        Logger.Info("Bootstrap", $"SwapBuffers=0x{swapBuffersAddr:X}");

        if (!SwapBuffersHook.Install(swapBuffersAddr))
        {
            Logger.Error("Bootstrap", "SwapBuffers hook install failed");
            return false;
        }
        Logger.Info("Bootstrap", "SwapBuffers hook installed");
        return true;
    }

    private static bool InstallDx9()
    {
        nint methods = Kiero.GetMethodsTable();
        if (methods == 0)
        {
            Logger.Error("Bootstrap", "Kiero methods table is null");
            return false;
        }

        nint resetAddr = *(nint*)(methods + Vtbl.Direct3DDevice9_Reset * IntPtr.Size);
        nint presentAddr = *(nint*)(methods + Vtbl.Direct3DDevice9_Present * IntPtr.Size);

        Logger.Info("Bootstrap", $"Dx9 Present=0x{presentAddr:X} Reset=0x{resetAddr:X}");

        if (resetAddr == 0 || presentAddr == 0)
        {
            Logger.Error("Bootstrap", "Dx9 vtable methods are null");
            return false;
        }

        if (ResizeBuffersHook.InstallDx9Reset(resetAddr))
            Logger.Info("Bootstrap", "Dx9 Reset hook installed");
        else
            Logger.Warn("Bootstrap", "Dx9 Reset hook not installed");

        if (!PresentHook.InstallDx9(presentAddr))
        {
            Logger.Error("Bootstrap", "Dx9 Present hook install failed");
            return false;
        }

        Logger.Info("Bootstrap", "Dx9 Present hook installed");
        return true;
    }

    private static bool InstallDx10()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out _, out nint resizeAddr, out _))
        {
            Logger.Error("Bootstrap", "Dx10 Finder failed");
            return false;
        }

        if (resizeAddr != 0 && ResizeBuffersHook.Install(resizeAddr))
            Logger.Info("Bootstrap", "Dx10 ResizeBuffers hook installed");

        if (!PresentHook.Install(presentAddr))
        {
            Logger.Error("Bootstrap", "Dx10 Present hook install failed");
            return false;
        }
        Logger.Info("Bootstrap", "Dx10 Present hook installed");
        return true;
    }

    private static bool InstallDx11()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out nint present1Addr, out nint resizeAddr, out nint resize1Addr))
        {
            Logger.Error("Bootstrap", "D3D11Finder failed");
            return false;
        }

        Logger.Info("Bootstrap", $"Dx11 Present=0x{presentAddr:X} Present1=0x{present1Addr:X} ResizeBuffers=0x{resizeAddr:X}");

        if (resizeAddr != 0 && ResizeBuffersHook.Install(resizeAddr))
            Logger.Info("Bootstrap", "ResizeBuffers hook installed");
        else
            Logger.Warn("Bootstrap", "ResizeBuffers hook not installed");

        if (resize1Addr != 0 && ResizeBuffersHook.InstallResizeBuffers1(resize1Addr))
            Logger.Info("Bootstrap", "ResizeBuffers1 hook installed");
        else if (resize1Addr == 0)
            Logger.Info("Bootstrap", "ResizeBuffers1 not available (pre-IDXGISwapChain3)");
        else
            Logger.Warn("Bootstrap", "ResizeBuffers1 hook not installed");

        if (presentAddr == 0)
        {
            Logger.Error("Bootstrap", "Present address is null");
            return false;
        }

        if (!PresentHook.Install(presentAddr))
        {
            Logger.Error("Bootstrap", "Present hook install failed");
            return false;
        }
        Logger.Info("Bootstrap", "Present hook installed");

        if (present1Addr != 0 && present1Addr != presentAddr)
        {
            if (PresentHook.InstallPresent1(present1Addr))
                Logger.Info("Bootstrap", "Present1 hook installed");
            else
                Logger.Warn("Bootstrap", "Present1 hook not installed");
        }

        return true;
    }

    private static bool InstallDx12()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out nint present1Addr, out nint resizeAddr))
        {
            Logger.Error("Bootstrap", "D3D11Finder failed");
            return false;
        }

        Logger.Info("Bootstrap", $"Dx12 via Dx11 vtable Present=0x{presentAddr:X} Present1=0x{present1Addr:X} ResizeBuffers=0x{resizeAddr:X}");
        if (D3D12Finder.TryGet(out nint execCmdListsAddr))
        {
            Logger.Info("Bootstrap", "ExecuteCommandLists hook installed");
        }
        else
        {
            Logger.Warn("Bootstrap", "D3D12Finder failed (Hook may still be active)");
        }

        if (resizeAddr != 0 && ResizeBuffersHook.Install(resizeAddr))
            Logger.Info("Bootstrap", "ResizeBuffers hook installed");
        else
            Logger.Warn("Bootstrap", "ResizeBuffers hook not installed");

        if (presentAddr == 0)
        {
            Logger.Error("Bootstrap", "Present address is null");
            return false;
        }

        if (!PresentHook.Install(presentAddr))
        {
            Logger.Error("Bootstrap", "Present hook install failed");
            return false;
        }
        Logger.Info("Bootstrap", "Present hook installed");

        if (present1Addr != 0 && present1Addr != presentAddr)
        {
            if (PresentHook.InstallPresent1(present1Addr))
                Logger.Info("Bootstrap", "Present1 hook installed");
            else
                Logger.Warn("Bootstrap", "Present1 hook not installed");
        }

        return true;
    }
}
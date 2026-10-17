using System;
using System.Threading;

namespace Kiero.Net;

public static class Bootstrap
{
    public static bool Start(TimeSpan? moduleTimeout = null)
    {
        int waited = 0;
        int limit = (int)(moduleTimeout ?? TimeSpan.FromSeconds(60)).TotalMilliseconds;

        bool has11 = false, has12 = false;
        while (waited < limit)
        {
            has11 = Win32.GetModuleHandleW("d3d11.dll") != 0;
            has12 = Win32.GetModuleHandleW("d3d12.dll") != 0;
            if (has11 || has12) break;
            Thread.Sleep(200);
            waited += 200;
        }

        Logger.Info("Bootstrap", $"after wait {waited}ms: d3d11={has11} d3d12={has12}");
        if (!has11 && !has12) { Logger.Error("Bootstrap", "no d3d11/d3d12"); return false; }

        var type = has12 ? KieroRenderType.D3D12 : KieroRenderType.D3D11;
        var st = Kiero.Init(type);
        if (st != KieroStatus.Success) { Logger.Error("Bootstrap", $"Kiero.Init({type}) -> {st}"); return false; }

        if (MinHook.Initialize() != 0) { Logger.Error("Bootstrap", "MinHook.Initialize failed"); return false; }
        Logger.Info("Bootstrap", "MinHook initialized");

        var api = Kiero.GetRenderType();
        Logger.Info("Bootstrap", $"Kiero api={api}");
        return api == KieroRenderType.D3D12 ? InstallDx12() : InstallDx11();
    }

    private static bool InstallDx11()
    {
        if (!Dx11PresentFinder.TryGet(out nint presentAddr, out nint present1Addr, out nint resizeAddr))
        {
            Logger.Error("Bootstrap", "Dx11PresentFinder failed");
            return false;
        }

        Logger.Info("Bootstrap", $"Dx11 Present=0x{presentAddr:X} Present1=0x{present1Addr:X} ResizeBuffers=0x{resizeAddr:X}");

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

    private static bool InstallDx12()
    {
        if (!Dx11PresentFinder.TryGet(out nint presentAddr, out nint present1Addr, out nint resizeAddr))
        {
            Logger.Error("Bootstrap", "Dx11PresentFinder failed");
            return false;
        }

        Logger.Info("Bootstrap", $"Dx12 via Dx11 vtable Present=0x{presentAddr:X} Present1=0x{present1Addr:X} ResizeBuffers=0x{resizeAddr:X}");
        if (Dx12ExecuteCommandListsFinder.TryGet(out nint execCmdListsAddr))
        {
            Logger.Info("Bootstrap", "ExecuteCommandLists hook installed");
        }
        else
        {
            Logger.Warn("Bootstrap", "Dx12ExecuteCommandListsFinder failed (Hook may still be active)");
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
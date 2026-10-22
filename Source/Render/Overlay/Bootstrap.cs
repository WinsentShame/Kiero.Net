using System;
using System.Threading;

namespace Kiero.Net;

public static unsafe class Bootstrap
{
    private static int started;

    public static bool Start(TimeSpan? moduleTimeout = null)
    {
        if (Interlocked.CompareExchange(ref started, 1, 0) != 0) return true;

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
        if (!has9 && !has10 && !has11 && !has12 && !hasGl)
        { Logger.Error("Bootstrap", "no graphics DLL loaded"); return false; }

        int mh = MinHook.Initialize();
        if (mh != 0 && mh != 1) { Logger.Error("Bootstrap", $"MinHook.Initialize -> {mh}"); return false; }
        Logger.Info("Bootstrap", $"MinHook ready (hr={mh})");

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

        if (has12)
        {
            var st = Kiero.Init(KieroRenderType.D3D12);
            if (st == KieroStatus.Success && InstallDx12()) anyInstalled = true;
            else if (InstallDx11Or10()) anyInstalled = true;
        }
        else if (has11)
        {
            var st = Kiero.Init(KieroRenderType.D3D11);
            if (st == KieroStatus.Success && InstallDx11()) anyInstalled = true;
            else if (InstallDx11Or10()) anyInstalled = true;
        }
        else if (has10)
        {
            if (InstallDx10()) anyInstalled = true;
        }

        if (!anyInstalled) { Logger.Error("Bootstrap", "no hooks installed"); return false; }
        return true;
    }

    public static void StartAsync(TimeSpan? moduleTimeout = null)
    {
        new Thread(() =>
        {
            try { Start(moduleTimeout); }
            catch (Exception ex) { Logger.Error("Bootstrap", ex); }
        })
        { IsBackground = true, Name = "Kiero.Bootstrap" }.Start();
    }

    private static bool InstallDx11Or10()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out _, out nint resizeAddr, out _))
        { Logger.Error("Bootstrap", "D3D11Finder failed"); return false; }

        if (resizeAddr != 0) ResizeBuffersHook.Install(resizeAddr);
        if (!PresentHook.Install(presentAddr)) { Logger.Error("Bootstrap", "Present hook failed"); return false; }
        return true;
    }

    private static bool InstallGl()
    {
        if (!OpenglFinder.TryGet(out nint addr)) { Logger.Error("Bootstrap", "OpenglFinder failed"); return false; }
        if (!SwapBuffersHook.Install(addr)) { Logger.Error("Bootstrap", "SwapBuffers hook failed"); return false; }
        Logger.Info("Bootstrap", $"SwapBuffers hook installed @ 0x{addr:X}");
        return true;
    }

    private static bool InstallDx9()
    {
        nint methods = Kiero.GetMethodsTable();
        if (methods == 0) { Logger.Error("Bootstrap", "Kiero methods table is null"); return false; }

        nint resetAddr = *(nint*)(methods + Vtbl.Direct3DDevice9_Reset * IntPtr.Size);
        nint presentAddr = *(nint*)(methods + Vtbl.Direct3DDevice9_Present * IntPtr.Size);
        if (resetAddr == 0 || presentAddr == 0) { Logger.Error("Bootstrap", "Dx9 vtable methods null"); return false; }

        ResizeBuffersHook.InstallDx9Reset(resetAddr);
        if (!PresentHook.InstallDx9(presentAddr)) { Logger.Error("Bootstrap", "Dx9 Present hook failed"); return false; }
        Logger.Info("Bootstrap", $"Dx9 hooks installed (present=0x{presentAddr:X} reset=0x{resetAddr:X})");
        return true;
    }

    private static bool InstallDx10()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out _, out nint resizeAddr, out _))
        { Logger.Error("Bootstrap", "Dx10 Finder failed"); return false; }

        if (resizeAddr != 0) ResizeBuffersHook.Install(resizeAddr);
        if (!PresentHook.Install(presentAddr)) { Logger.Error("Bootstrap", "Dx10 Present hook failed"); return false; }
        Logger.Info("Bootstrap", "Dx10 hooks installed");
        return true;
    }

    private static bool InstallDx11()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out nint present1Addr, out nint resizeAddr, out nint resize1Addr))
        { Logger.Error("Bootstrap", "D3D11Finder failed"); return false; }

        if (resizeAddr != 0) ResizeBuffersHook.Install(resizeAddr);
        if (resize1Addr != 0) ResizeBuffersHook.InstallResizeBuffers1(resize1Addr);
        if (!PresentHook.Install(presentAddr)) { Logger.Error("Bootstrap", "Present hook failed"); return false; }
        if (present1Addr != 0 && present1Addr != presentAddr) PresentHook.InstallPresent1(present1Addr);

        Logger.Info("Bootstrap", $"Dx11 hooks installed (present=0x{presentAddr:X})");
        return true;
    }

    private static bool InstallDx12()
    {
        if (!D3D11Finder.TryGet(out nint presentAddr, out nint present1Addr, out nint resizeAddr, out _))
        { Logger.Error("Bootstrap", "D3D11Finder failed"); return false; }

        if (!D3D12Finder.TryGet(out _))
        { Logger.Warn("Bootstrap", "D3D12Finder failed — DX12 will fall back to Present hook only"); }

        if (resizeAddr != 0) ResizeBuffersHook.Install(resizeAddr);
        if (!PresentHook.Install(presentAddr)) { Logger.Error("Bootstrap", "Present hook failed"); return false; }
        if (present1Addr != 0 && present1Addr != presentAddr) PresentHook.InstallPresent1(present1Addr);

        Logger.Info("Bootstrap", $"Dx12 hooks installed (present=0x{presentAddr:X})");
        return true;
    }
}
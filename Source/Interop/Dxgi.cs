using System;
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class Dxgi
{
    public const uint FormatR8G8B8A8Unorm = 28;
    public const uint FormatB8G8R8A8Unorm = 87;
    public const uint UsageRenderTargetOutput = 0x20;

    public const int ErrorNotFound = unchecked((int)0x887A0002);

    public static bool TryGetSize(nint sc, out uint w, out uint h)
    {
        w = h = 0;
        byte* desc = stackalloc byte[128];
        var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)
            Vtbl.VTableM(sc, Vtbl.SwapChain_GetDesc);
        if (getDesc(sc, desc) < 0) return false;
        w = *(uint*)(desc + 0);
        h = *(uint*)(desc + 4);
        return true;
    }

    public static bool TryGetDesc(nint sc, out uint w, out uint h, out uint fmt, out nint hwnd)
    {
        w = h = fmt = 0;
        hwnd = 0;
        byte* desc = stackalloc byte[128];
        var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)
            Vtbl.VTableM(sc, Vtbl.SwapChain_GetDesc);
        if (getDesc(sc, desc) < 0) return false;
        w = *(uint*)(desc + 0);
        h = *(uint*)(desc + 4);
        fmt = *(uint*)(desc + 16);
        hwnd = *(nint*)(desc + 48);
        return true;
    }

    public static uint GetCurrentBackBufferIndex(nint sc)
    {
        nint addr = Vtbl.VTableM(sc, Vtbl.SwapChain3_GetCurrentBackBufferIndex);
        if (addr == 0) return 0;
        var fn = (delegate* unmanaged[Stdcall]<nint, uint>)addr;
        return fn(sc);
    }

    public static nint GetBackBuffer(nint sc, uint idx, Guid iid)
    {
        nint bb = 0;
        var getBuffer = (delegate* unmanaged[Stdcall]<nint, uint, Guid*, nint*, int>)
            Vtbl.VTableM(sc, Vtbl.SwapChain_GetBuffer);
        return getBuffer(sc, idx, &iid, &bb) < 0 ? 0 : bb;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ModeDesc
    {
        public uint Width;
        public uint Height;
        public uint RefreshRateNum;
        public uint RefreshRateDen;
        public uint Format;
        public uint ScanlineOrdering;
        public uint Scaling;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SampleDesc
    {
        public uint Count;
        public uint Quality;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SwapChainDesc
    {
        public ModeDesc BufferDesc;
        public SampleDesc SampleDesc;
        public uint BufferUsage;
        public uint BufferCount;
        public nint OutputWindow;
        public int Windowed;
        public uint SwapEffect;
        public uint Flags;
    }
}
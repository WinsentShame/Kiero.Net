using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class Dxgi
{
    public const uint FormatR8G8B8A8Unorm = 28;
    public const uint FormatB8G8R8A8Unorm = 87;
    public const uint UsageRenderTargetOutput = 0x20;

    public const int ErrorNotFound = unchecked((int)0x887A0002);

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
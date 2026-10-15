using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class D3D12Structs
{
    public const int COMMAND_LIST_TYPE_DIRECT = 0;
    public const int HEAP_TYPE_CBV_SRV_UAV = 0;
    public const int HEAP_TYPE_RTV = 2;
    public const int HEAP_FLAG_NONE = 0;
    public const int HEAP_FLAG_SHADER_VISIBLE = 1;

    public const int STATE_PRESENT = 0;
    public const int STATE_RENDER_TARGET = 4;

    public const int BARRIER_TYPE_TRANSITION = 0;
    public const uint ALL_SUBRESOURCES = 0xFFFFFFFF;

    [StructLayout(LayoutKind.Sequential)]
    public struct D3D12_COMMAND_QUEUE_DESC
    {
        public int Type;
        public int Priority;
        public int Flags;
        public uint NodeMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D3D12_DESCRIPTOR_HEAP_DESC
    {
        public int Type;
        public uint NumDescriptors;
        public int Flags;
        public uint NodeMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct D3D12_CPU_DESCRIPTOR_HANDLE { public nuint ptr; }

    [StructLayout(LayoutKind.Sequential)]
    public struct D3D12_RESOURCE_BARRIER
    {
        public int Type;
        public int Flags;
        public nint Resource;
        public uint Subresource;
        public int StateBefore;
        public int StateAfter;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DXGI_SAMPLE_DESC
    {
        public uint Count;
        public uint Quality;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DXGI_SWAP_CHAIN_DESC1
    {
        public uint Width;
        public uint Height;
        public int Format;
        public int Stereo;
        public DXGI_SAMPLE_DESC SampleDesc;
        public uint BufferUsage;
        public uint BufferCount;
        public int Scaling;
        public int SwapEffect;
        public int AlphaMode;
        public uint Flags;
    }
}
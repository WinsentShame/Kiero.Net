namespace Kiero.Net;

public static class KieroIndices
{
    // DX11: [0..17] swapChain, [18..60] device, [61..204] context
    public const int D3D11_SwapChain_Present = 8;
    public const int D3D11_SwapChain_GetBuffer = 9;
    public const int D3D11_SwapChain_ResizeBuffers = 13;

    // DX12: [0..43] device, [44..62] queue, [63..71] allocator,
    //       [72..131] commandList, [132..149] swapChain
    public const int D3D12_SwapChain_Present = 132 + 8;
    public const int D3D12_SwapChain_GetBuffer = 132 + 9;
    public const int D3D12_SwapChain_ResizeBuffers = 132 + 13;
}
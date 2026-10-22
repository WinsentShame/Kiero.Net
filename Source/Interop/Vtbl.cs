using System;

namespace Kiero.Net;

public static unsafe class Vtbl
{
    /// <summary>Returns the raw address of the method at <paramref name="index"/> in the COM object's vtable.</summary>
    public static nint VTableM(nint com, int index)
    {
        nint vt = *(nint*)com;
        return *(nint*)(vt + index * IntPtr.Size);
    }

    /// <summary>
    /// QueryInterface from an IDXGISwapChain to get its device for the given IID.
    /// Returns 0 if the swapchain is not backed by a device of that type.
    /// </summary>
    public static nint SwapChainGetDevice(nint sc, Guid* iid)
    {
        var getDevice = (delegate* unmanaged[Stdcall]<nint, Guid*, nint*, int>)
            VTableM(sc, SwapChain_GetDevice);
        nint dev = 0;
        return getDevice(sc, iid, &dev) < 0 ? 0 : dev;
    }

    // ============================================================
    // IUnknown — base for every COM object
    // ============================================================
    public const int IUnknown_QueryInterface = 0;
    public const int IUnknown_AddRef = 1;
    public const int IUnknown_Release = 2;

    // ============================================================
    // IDXGIObject (dxgi.h) — inherits IUnknown
    // ============================================================
    public const int DXGIObject_SetPrivateData = 3;
    public const int DXGIObject_SetPrivateDataInterface = 4;
    public const int DXGIObject_GetPrivateData = 5;
    public const int DXGIObject_GetParent = 6;

    // ============================================================
    // IDXGIDeviceSubObject (dxgi.h)
    // ============================================================
    public const int DXGIDeviceSubObject_GetDevice = 7;
    public const int SwapChain_GetDevice = DXGIDeviceSubObject_GetDevice;   // = 7

    // ============================================================
    // IDXGISwapChain (dxgi.h) — inherits IDXGIDeviceSubObject
    // ============================================================
    public const int SwapChain_Present = 8;
    public const int SwapChain_GetBuffer = 9;
    public const int SwapChain_SetFullscreenState = 10;
    public const int SwapChain_GetFullscreenState = 11;
    public const int SwapChain_GetDesc = 12;
    public const int SwapChain_ResizeBuffers = 13;
    public const int SwapChain_ResizeTarget = 14;
    public const int SwapChain_GetContainingOutput = 15;
    public const int SwapChain_GetFrameStatistics = 16;
    public const int SwapChain_GetLastPresentCount = 17;

    // ============================================================
    // IDXGISwapChain1 (dxgi1_2.h)
    // ============================================================
    public const int SwapChain1_GetDesc1 = 18;
    public const int SwapChain1_GetFullscreenDesc = 19;
    public const int SwapChain1_GetHwnd = 20;
    public const int SwapChain1_GetCoreWindow = 21;
    public const int SwapChain1_Present1 = 22;
    public const int SwapChain1_IsTemporaryMonoSupported = 23;
    public const int SwapChain1_GetRestrictToOutput = 24;
    public const int SwapChain1_SetBackgroundColor = 25;
    public const int SwapChain1_GetBackgroundColor = 26;
    public const int SwapChain1_SetRotation = 27;
    public const int SwapChain1_GetRotation = 28;

    // ============================================================
    // IDXGISwapChain2 (dxgi1_3.h)
    // ============================================================
    public const int SwapChain2_SetSourceSize = 29;
    public const int SwapChain2_GetSourceSize = 30;
    public const int SwapChain2_SetMaximumFrameLatency = 31;
    public const int SwapChain2_GetMaximumFrameLatency = 32;
    public const int SwapChain2_GetFrameLatencyWaitableObject = 33;
    public const int SwapChain2_SetMatrixTransform = 34;
    public const int SwapChain2_GetMatrixTransform = 35;

    // ============================================================
    // IDXGISwapChain3 (dxgi1_4.h)
    // ============================================================
    public const int SwapChain3_GetCurrentBackBufferIndex = 36;
    public const int SwapChain3_CheckColorSpaceSupport = 37;
    public const int SwapChain3_SetColorSpace1 = 38;
    public const int SwapChain3_ResizeBuffers1 = 39;

    // ============================================================
    // IDXGISwapChain4 (dxgi1_5.h)
    // ============================================================
    public const int SwapChain4_SetHDRMetaData = 40;

    // ============================================================
    // IDXGIFactory (dxgi.h) — inherits IDXGIObject
    // ============================================================
    public const int Factory_EnumAdapters = 7;
    public const int Factory_MakeWindowAssociation = 8;
    public const int Factory_GetWindowAssociation = 9;
    public const int Factory_CreateSwapChain = 10;
    public const int Factory_CreateSoftwareAdapter = 11;

    // ============================================================
    // IDXGIFactory1 (dxgi1_1.h)
    // ============================================================
    public const int Factory1_EnumAdapters1 = 12;
    public const int Factory1_IsCurrent = 13;

    // ============================================================
    // IDXGIFactory2 (dxgi1_2.h)
    // ============================================================
    public const int Factory2_IsWindowedStereoEnabled = 14;
    public const int Factory2_CreateSwapChainForHwnd = 15;
    public const int Factory2_CreateSwapChainForCoreWindow = 16;
    public const int Factory2_GetSharedResourceAdapterLuid = 17;
    public const int Factory2_RegisterStereoStatusWindow = 18;
    public const int Factory2_RegisterStereoStatusEvent = 19;
    public const int Factory2_UnregisterStereoStatus = 20;
    public const int Factory2_RegisterOcclusionStatusWindow = 21;
    public const int Factory2_RegisterOcclusionStatusEvent = 22;
    public const int Factory2_UnregisterOcclusionStatus = 23;
    public const int Factory2_CreateSwapChainForComposition = 24;

    // ============================================================
    // IDXGIFactory3 (dxgi1_3.h)
    // ============================================================
    public const int Factory3_GetCreationFlags = 25;

    // ============================================================
    // IDXGIFactory4 (dxgi1_4.h)
    // ============================================================
    public const int Factory4_EnumAdapterByLuid = 26;
    public const int Factory4_EnumWarpAdapter = 27;

    // ============================================================
    // IDXGIFactory5 (dxgi1_5.h)
    // ============================================================
    public const int Factory5_CheckFeatureSupport = 28;

    // ============================================================
    // IDXGIFactory6 (dxgi1_6.h)
    // ============================================================
    public const int Factory6_EnumAdapterByGpuPreference = 29;

    // ============================================================
    // IDXGIAdapter (dxgi.h) — inherits IDXGIObject
    // ============================================================
    public const int Adapter_EnumOutputs = 7;
    public const int Adapter_GetDesc = 8;
    public const int Adapter_CheckInterfaceSupport = 9;

    // ============================================================
    // IDXGIAdapter1 (dxgi1_1.h)
    // ============================================================
    public const int Adapter1_GetDesc1 = 10;

    // ============================================================
    // IDXGIAdapter2 (dxgi1_2.h)
    // ============================================================
    public const int Adapter2_GetDesc2 = 11;

    // ============================================================
    // IDXGIAdapter3 (dxgi1_4.h)
    // ============================================================
    public const int Adapter3_RegisterHardwareContentProtectionTeardownStatusEvent = 12;
    public const int Adapter3_UnregisterHardwareContentProtectionTeardownStatus = 13;
    public const int Adapter3_QueryVideoMemoryInfo = 14;
    public const int Adapter3_SetVideoMemoryReservation = 15;
    public const int Adapter3_RegisterVideoMemoryBudgetChangeNotificationEvent = 16;
    public const int Adapter3_UnregisterVideoMemoryBudgetChangeNotification = 17;

    // ============================================================
    // IDXGIAdapter4 (dxgi1_6.h)
    // ============================================================
    public const int Adapter4_GetDesc3 = 18;

    // ============================================================
    // ID3D10Device (d3d10.h) — FULL, verified against d3d10_1core.dll
    // ============================================================
    public const int D3D10Device_QueryInterface = 0;
    public const int D3D10Device_AddRef = 1;
    public const int D3D10Device_Release = 2;
    public const int D3D10Device_VSSetConstantBuffers = 3;
    public const int D3D10Device_PSSetShaderResources = 4;
    public const int D3D10Device_PSSetShader = 5;
    public const int D3D10Device_PSSetSamplers = 6;
    public const int D3D10Device_VSSetShader = 7;
    public const int D3D10Device_DrawIndexed = 8;
    public const int D3D10Device_Draw = 9;
    public const int D3D10Device_PSSetConstantBuffers = 10;
    public const int D3D10Device_IASetInputLayout = 11;
    public const int D3D10Device_IASetVertexBuffers = 12;
    public const int D3D10Device_IASetIndexBuffer = 13;
    public const int D3D10Device_DrawIndexedInstanced = 14;
    public const int D3D10Device_DrawInstanced = 15;
    public const int D3D10Device_GSSetConstantBuffers = 16;
    public const int D3D10Device_GSSetShader = 17;
    public const int D3D10Device_IASetPrimitiveTopology = 18;
    public const int D3D10Device_VSSetShaderResources = 19;
    public const int D3D10Device_VSSetSamplers = 20;
    public const int D3D10Device_SetPredication = 21;
    public const int D3D10Device_GSSetShaderResources = 22;
    public const int D3D10Device_GSSetSamplers = 23;
    public const int D3D10Device_OMSetRenderTargets = 24;
    public const int D3D10Device_OMSetBlendState = 25;
    public const int D3D10Device_OMSetDepthStencilState = 26;
    public const int D3D10Device_SOSetTargets = 27;
    public const int D3D10Device_DrawAuto = 28;
    public const int D3D10Device_RSSetState = 29;
    public const int D3D10Device_RSSetViewports = 30;
    public const int D3D10Device_RSSetScissorRects = 31;
    public const int D3D10Device_CopySubresourceRegion = 32;
    public const int D3D10Device_CopyResource = 33;
    public const int D3D10Device_UpdateSubresource = 34;
    public const int D3D10Device_ClearRenderTargetView = 35;
    public const int D3D10Device_ClearDepthStencilView = 36;
    public const int D3D10Device_GenerateMips = 37;
    public const int D3D10Device_ResolveSubresource = 38;
    public const int D3D10Device_VSGetConstantBuffers = 39;
    public const int D3D10Device_PSGetShaderResources = 40;
    public const int D3D10Device_PSGetShader = 41;
    public const int D3D10Device_PSGetSamplers = 42;
    public const int D3D10Device_VSGetShader = 43;
    public const int D3D10Device_PSGetConstantBuffers = 44;
    public const int D3D10Device_IAGetInputLayout = 45;
    public const int D3D10Device_IAGetVertexBuffers = 46;
    public const int D3D10Device_IAGetIndexBuffer = 47;
    public const int D3D10Device_GSGetConstantBuffers = 48;
    public const int D3D10Device_GSGetShader = 49;
    public const int D3D10Device_IAGetPrimitiveTopology = 50;
    public const int D3D10Device_VSGetShaderResources = 51;
    public const int D3D10Device_VSGetSamplers = 52;
    public const int D3D10Device_GetPredication = 53;
    public const int D3D10Device_GSGetShaderResources = 54;
    public const int D3D10Device_GSGetSamplers = 55;
    public const int D3D10Device_OMGetRenderTargets = 56;
    public const int D3D10Device_OMGetBlendState = 57;
    public const int D3D10Device_OMGetDepthStencilState = 58;
    public const int D3D10Device_SOGetTargets = 59;
    public const int D3D10Device_RSGetState = 60;
    public const int D3D10Device_RSGetViewports = 61;
    public const int D3D10Device_RSGetScissorRects = 62;
    public const int D3D10Device_GetDeviceRemovedReason = 63;
    public const int D3D10Device_SetExceptionMode = 64;
    public const int D3D10Device_GetExceptionMode = 65;
    public const int D3D10Device_GetPrivateData = 66;
    public const int D3D10Device_SetPrivateData = 67;
    public const int D3D10Device_SetPrivateDataInterface = 68;
    public const int D3D10Device_ClearState = 69;
    public const int D3D10Device_Flush = 70;
    public const int D3D10Device_CreateBuffer = 71;
    public const int D3D10Device_CreateTexture1D = 72;
    public const int D3D10Device_CreateTexture2D = 73;
    public const int D3D10Device_CreateTexture3D = 74;
    public const int D3D10Device_CreateShaderResourceView = 75;
    public const int D3D10Device_CreateRenderTargetView = 76;
    public const int D3D10Device_CreateDepthStencilView = 77;
    public const int D3D10Device_CreateInputLayout = 78;
    public const int D3D10Device_CreateVertexShader = 79;
    public const int D3D10Device_CreateGeometryShader = 80;
    public const int D3D10Device_CreateGeometryShaderWithStreamOutput = 81;
    public const int D3D10Device_CreatePixelShader = 82;
    public const int D3D10Device_CreateBlendState = 83;
    public const int D3D10Device_CreateDepthStencilState = 84;
    public const int D3D10Device_CreateRasterizerState = 85;
    public const int D3D10Device_CreateSamplerState = 86;
    public const int D3D10Device_CreateQuery = 87;
    public const int D3D10Device_CreatePredicate = 88;
    public const int D3D10Device_CreateCounter = 89;
    public const int D3D10Device_CheckFormatSupport = 90;
    public const int D3D10Device_CheckMultisampleQualityLevels = 91;
    public const int D3D10Device_CheckCounterInfo = 92;
    public const int D3D10Device_CheckCounter = 93;
    public const int D3D10Device_GetCreationFlags = 94;
    public const int D3D10Device_OpenSharedResource = 95;
    public const int D3D10Device_SetTextFilterSize = 96;
    public const int D3D10Device_GetTextFilterSize = 97;
    // ID3D10Device1 extension:
    public const int D3D10Device1_CreateShaderResourceView1 = 98;
    public const int D3D10Device1_CreateBlendState1 = 99;
    public const int D3D10Device1_GetFeatureLevel = 100;

    // ============================================================
    // ID3D10DeviceChild (d3d10.h) — base for textures, buffers, shaders
    // ============================================================
    public const int D3D10DeviceChild_GetDevice = 3;
    public const int D3D10DeviceChild_GetPrivateData = 4;
    public const int D3D10DeviceChild_SetPrivateData = 5;
    public const int D3D10DeviceChild_SetPrivateDataInterface = 6;

    // ID3D10Texture2D — inherits ID3D10DeviceChild
    public const int D3D10Texture2D_GetDevice = D3D10DeviceChild_GetDevice;   // = 3

    // ============================================================
    // ID3D11Device (d3d11.h) — inherits IUnknown
    // ============================================================
    public const int D3D11Device_CreateBuffer = 3;
    public const int D3D11Device_CreateTexture1D = 4;
    public const int D3D11Device_CreateTexture2D = 5;
    public const int D3D11Device_CreateTexture3D = 6;
    public const int D3D11Device_CreateShaderResourceView = 7;
    public const int D3D11Device_CreateUnorderedAccessView = 8;
    public const int D3D11Device_CreateRenderTargetView = 9;
    public const int D3D11Device_CreateDepthStencilView = 10;
    public const int D3D11Device_CreateInputLayout = 11;
    public const int D3D11Device_CreateVertexShader = 12;
    public const int D3D11Device_CreateGeometryShader = 13;
    public const int D3D11Device_CreateGeometryShaderWithStreamOutput = 14;
    public const int D3D11Device_CreatePixelShader = 15;
    public const int D3D11Device_CreateHullShader = 16;
    public const int D3D11Device_CreateDomainShader = 17;
    public const int D3D11Device_CreateComputeShader = 18;
    public const int D3D11Device_CreateClassLinkage = 19;
    public const int D3D11Device_CreateBlendState = 20;
    public const int D3D11Device_CreateDepthStencilState = 21;
    public const int D3D11Device_CreateRasterizerState = 22;
    public const int D3D11Device_CreateSamplerState = 23;
    public const int D3D11Device_CreateQuery = 24;
    public const int D3D11Device_CreatePredicate = 25;
    public const int D3D11Device_CreateCounter = 26;
    public const int D3D11Device_CreateDeferredContext = 27;
    public const int D3D11Device_OpenSharedResource = 28;
    public const int D3D11Device_CheckFormatSupport = 29;
    public const int D3D11Device_CheckMultisampleQualityLevels = 30;
    public const int D3D11Device_CheckCounterInfo = 31;
    public const int D3D11Device_CheckCounter = 32;
    public const int D3D11Device_CheckFeatureSupport = 33;
    public const int D3D11Device_GetPrivateData = 34;
    public const int D3D11Device_SetPrivateData = 35;
    public const int D3D11Device_SetPrivateDataInterface = 36;
    public const int D3D11Device_GetFeatureLevel = 37;
    public const int D3D11Device_GetCreationFlags = 38;
    public const int D3D11Device_GetDeviceRemovedReason = 39;
    public const int D3D11Device_GetImmediateContext = 40;

    // ============================================================
    // ID3D11DeviceChild (d3d11.h)
    // ============================================================
    public const int D3D11DeviceChild_GetDevice = 3;
    public const int D3D11DeviceChild_GetPrivateData = 4;
    public const int D3D11DeviceChild_SetPrivateData = 5;
    public const int D3D11DeviceChild_SetPrivateDataInterface = 6;

    // ID3D11Texture2D — inherits ID3D11DeviceChild
    public const int D3D11Texture2D_GetDevice = D3D11DeviceChild_GetDevice;   // = 3

    // ============================================================
    // ID3D11DeviceContext (d3d11.h) — FULL, verified against d3d11.dll
    // ============================================================
    public const int D3D11Context_GetDevice = 3;
    public const int D3D11Context_GetPrivateData = 4;
    public const int D3D11Context_SetPrivateData = 5;
    public const int D3D11Context_SetPrivateDataInterface = 6;
    public const int D3D11Context_VSSetConstantBuffers = 7;
    public const int D3D11Context_PSSetShaderResources = 8;
    public const int D3D11Context_PSSetShader = 9;
    public const int D3D11Context_PSSetSamplers = 10;
    public const int D3D11Context_VSSetShader = 11;
    public const int D3D11Context_DrawIndexed = 12;
    public const int D3D11Context_Draw = 13;
    public const int D3D11Context_Map = 14;
    public const int D3D11Context_Unmap = 15;
    public const int D3D11Context_PSSetConstantBuffers = 16;
    public const int D3D11Context_IASetInputLayout = 17;
    public const int D3D11Context_IASetVertexBuffers = 18;
    public const int D3D11Context_IASetIndexBuffer = 19;
    public const int D3D11Context_DrawIndexedInstanced = 20;
    public const int D3D11Context_DrawInstanced = 21;
    public const int D3D11Context_GSSetConstantBuffers = 22;
    public const int D3D11Context_GSSetShader = 23;
    public const int D3D11Context_IASetPrimitiveTopology = 24;
    public const int D3D11Context_VSSetShaderResources = 25;
    public const int D3D11Context_VSSetSamplers = 26;
    public const int D3D11Context_Begin = 27;
    public const int D3D11Context_End = 28;
    public const int D3D11Context_GetData = 29;
    public const int D3D11Context_SetPredication = 30;
    public const int D3D11Context_GSSetShaderResources = 31;
    public const int D3D11Context_GSSetSamplers = 32;
    public const int D3D11Context_OMSetRenderTargets = 33;
    public const int D3D11Context_OMSetRenderTargetsAndUnorderedAccessViews = 34;
    public const int D3D11Context_OMSetBlendState = 35;
    public const int D3D11Context_OMSetDepthStencilState = 36;
    public const int D3D11Context_SOSetTargets = 37;
    public const int D3D11Context_DrawAuto = 38;
    public const int D3D11Context_DrawIndexedInstancedIndirect = 39;
    public const int D3D11Context_DrawInstancedIndirect = 40;
    public const int D3D11Context_Dispatch = 41;
    public const int D3D11Context_DispatchIndirect = 42;
    public const int D3D11Context_RSSetState = 43;
    public const int D3D11Context_RSSetViewports = 44;
    public const int D3D11Context_RSSetScissorRects = 45;
    public const int D3D11Context_CopySubresourceRegion = 46;
    public const int D3D11Context_CopyResource = 47;
    public const int D3D11Context_UpdateSubresource = 48;
    public const int D3D11Context_CopyStructureCount = 49;
    public const int D3D11Context_ClearRenderTargetView = 50;
    public const int D3D11Context_ClearUnorderedAccessViewUint = 51;
    public const int D3D11Context_ClearUnorderedAccessViewFloat = 52;
    public const int D3D11Context_ClearDepthStencilView = 53;
    public const int D3D11Context_GenerateMips = 54;
    public const int D3D11Context_SetResourceMinLOD = 55;
    public const int D3D11Context_GetResourceMinLOD = 56;
    public const int D3D11Context_ResolveSubresource = 57;
    public const int D3D11Context_ExecuteCommandList = 58;
    public const int D3D11Context_HSSetShaderResources = 59;
    public const int D3D11Context_HSSetShader = 60;
    public const int D3D11Context_HSSetSamplers = 61;
    public const int D3D11Context_HSSetConstantBuffers = 62;
    public const int D3D11Context_DSSetShaderResources = 63;
    public const int D3D11Context_DSSetShader = 64;
    public const int D3D11Context_DSSetSamplers = 65;
    public const int D3D11Context_DSSetConstantBuffers = 66;
    public const int D3D11Context_CSSetShaderResources = 67;
    public const int D3D11Context_CSSetUnorderedAccessViews = 68;
    public const int D3D11Context_CSSetShader = 69;
    public const int D3D11Context_CSSetSamplers = 70;
    public const int D3D11Context_CSSetConstantBuffers = 71;
    public const int D3D11Context_VSGetConstantBuffers = 72;
    public const int D3D11Context_PSGetShaderResources = 73;
    public const int D3D11Context_PSGetShader = 74;
    public const int D3D11Context_PSGetSamplers = 75;
    public const int D3D11Context_VSGetShader = 76;
    public const int D3D11Context_PSGetConstantBuffers = 77;
    public const int D3D11Context_IAGetInputLayout = 78;
    public const int D3D11Context_IAGetVertexBuffers = 79;
    public const int D3D11Context_IAGetIndexBuffer = 80;
    public const int D3D11Context_GSGetConstantBuffers = 81;
    public const int D3D11Context_GSGetShader = 82;
    public const int D3D11Context_IAGetPrimitiveTopology = 83;
    public const int D3D11Context_VSGetShaderResources = 84;
    public const int D3D11Context_VSGetSamplers = 85;
    public const int D3D11Context_GetPredication = 86;
    public const int D3D11Context_GSGetShaderResources = 87;
    public const int D3D11Context_GSGetSamplers = 88;
    public const int D3D11Context_OMGetRenderTargets = 89;
    public const int D3D11Context_OMGetRenderTargetsAndUnorderedAccessViews = 90;
    public const int D3D11Context_OMGetBlendState = 91;
    public const int D3D11Context_OMGetDepthStencilState = 92;
    public const int D3D11Context_SOGetTargets = 93;
    public const int D3D11Context_RSGetState = 94;
    public const int D3D11Context_RSGetViewports = 95;
    public const int D3D11Context_RSGetScissorRects = 96;
    public const int D3D11Context_HSGetShaderResources = 97;
    public const int D3D11Context_HSGetShader = 98;
    public const int D3D11Context_HSGetSamplers = 99;
    public const int D3D11Context_HSGetConstantBuffers = 100;
    public const int D3D11Context_DSGetShaderResources = 101;
    public const int D3D11Context_DSGetShader = 102;
    public const int D3D11Context_DSGetSamplers = 103;
    public const int D3D11Context_DSGetConstantBuffers = 104;
    public const int D3D11Context_CSGetShaderResources = 105;
    public const int D3D11Context_CSGetUnorderedAccessViews = 106;
    public const int D3D11Context_CSGetShader = 107;
    public const int D3D11Context_CSGetSamplers = 108;
    public const int D3D11Context_CSGetConstantBuffers = 109;
    public const int D3D11Context_ClearState = 110;
    public const int D3D11Context_Flush = 111;
    public const int D3D11Context_GetType = 112;
    public const int D3D11Context_GetContextFlags = 113;
    public const int D3D11Context_FinishCommandList = 114;

    // ============================================================
    // ID3D12Object (d3d12.h)
    // ============================================================
    public const int D3D12Object_GetPrivateData = 3;
    public const int D3D12Object_SetPrivateData = 4;
    public const int D3D12Object_SetPrivateDataInterface = 5;
    public const int D3D12Object_SetName = 6;
    public const int D3D12Allocator_Reset = 8;

    // ============================================================
    // ID3D12DeviceChild (d3d12.h)
    // ============================================================
    public const int D3D12DeviceChild_GetDevice = 7;

    // ============================================================
    // ID3D12Device (d3d12.h)
    // ============================================================
    public const int D3D12Device_GetNodeCount = 7;
    public const int D3D12Device_CreateCommandQueue = 8;
    public const int D3D12Device_CreateCommandAllocator = 9;
    public const int D3D12Device_CreateGraphicsPipelineState = 10;
    public const int D3D12Device_CreateComputePipelineState = 11;
    public const int D3D12Device_CreateCommandList = 12;
    public const int D3D12Device_CheckFeatureSupport = 13;
    public const int D3D12Device_CreateDescriptorHeap = 14;
    public const int D3D12Device_GetDescriptorHandleIncrementSize = 15;
    public const int D3D12Device_CreateRootSignature = 16;
    public const int D3D12Device_CreateConstantBufferView = 17;
    public const int D3D12Device_CreateShaderResourceView = 18;
    public const int D3D12Device_CreateUnorderedAccessView = 19;
    public const int D3D12Device_CreateRenderTargetView = 20;
    public const int D3D12Device_CreateDepthStencilView = 21;
    public const int D3D12Device_CreateSampler = 22;
    public const int D3D12Device_CopyDescriptors = 23;
    public const int D3D12Device_CopyDescriptorsSimple = 24;
    public const int D3D12Device_GetResourceAllocationInfo = 25;
    public const int D3D12Device_GetCustomHeapProperties = 26;
    public const int D3D12Device_CreateCommittedResource = 27;
    public const int D3D12Device_CreateHeap = 28;
    public const int D3D12Device_CreatePlacedResource = 29;
    public const int D3D12Device_CreateReservedResource = 30;
    public const int D3D12Device_CreateSharedHandle = 31;
    public const int D3D12Device_OpenSharedHandle = 32;
    public const int D3D12Device_OpenSharedHandleByName = 33;
    public const int D3D12Device_MakeResident = 34;
    public const int D3D12Device_Evict = 35;
    public const int D3D12Device_CreateFence = 36;
    public const int D3D12Device_GetDeviceRemovedReason = 37;

    // ============================================================
    // ID3D12CommandQueue (d3d12.h)
    // ============================================================
    public const int D3D12Queue_UpdateTileMappings = 8;
    public const int D3D12Queue_CopyTileMappings = 9;
    public const int D3D12Queue_ExecuteCommandLists = 10;
    public const int D3D12Queue_SetMarker = 11;
    public const int D3D12Queue_BeginEvent = 12;
    public const int D3D12Queue_EndEvent = 13;
    public const int D3D12Queue_Signal = 14;
    public const int D3D12Queue_Wait = 15;
    public const int D3D12Queue_GetTimestampFrequency = 16;
    public const int D3D12Queue_GetClockCalibration = 17;
    public const int D3D12Queue_GetDesc = 18;

    // ============================================================
    // ID3D12CommandList (d3d12.h)
    // ============================================================
    public const int D3D12CommandList_GetType = 8;

    // ============================================================
    // ID3D12GraphicsCommandList (d3d12.h)
    // ============================================================
    public const int D3D12List_Close = 9;
    public const int D3D12List_Reset = 10;
    public const int D3D12List_ClearState = 11;
    public const int D3D12List_DrawInstanced = 12;
    public const int D3D12List_DrawIndexedInstanced = 13;
    public const int D3D12List_Dispatch = 14;
    public const int D3D12List_CopyBufferRegion = 15;
    public const int D3D12List_CopyTextureRegion = 16;
    public const int D3D12List_CopyResource = 17;
    public const int D3D12List_CopyTiles = 18;
    public const int D3D12List_ResolveSubresource = 19;
    public const int D3D12List_IASetPrimitiveTopology = 20;
    public const int D3D12List_RSSetViewports = 21;
    public const int D3D12List_RSSetScissorRects = 22;
    public const int D3D12List_OMSetBlendFactor = 23;
    public const int D3D12List_OMSetStencilRef = 24;
    public const int D3D12List_SetPipelineState = 25;
    public const int D3D12List_ResourceBarrier = 26;
    public const int D3D12List_ExecuteBundle = 27;
    public const int D3D12List_SetDescriptorHeaps = 28;
    public const int D3D12List_SetComputeRootSignature = 29;
    public const int D3D12List_SetGraphicsRootSignature = 30;
    public const int D3D12List_SetComputeRootDescriptorTable = 31;
    public const int D3D12List_SetGraphicsRootDescriptorTable = 32;
    public const int D3D12List_SetComputeRoot32BitConstant = 33;
    public const int D3D12List_SetGraphicsRoot32BitConstant = 34;
    public const int D3D12List_SetComputeRoot32BitConstants = 35;
    public const int D3D12List_SetGraphicsRoot32BitConstants = 36;
    public const int D3D12List_SetComputeRootConstantBufferView = 37;
    public const int D3D12List_SetGraphicsRootConstantBufferView = 38;
    public const int D3D12List_SetComputeRootShaderResourceView = 39;
    public const int D3D12List_SetGraphicsRootShaderResourceView = 40;
    public const int D3D12List_SetComputeRootUnorderedAccessView = 41;
    public const int D3D12List_SetGraphicsRootUnorderedAccessView = 42;
    public const int D3D12List_IASetIndexBuffer = 43;
    public const int D3D12List_IASetVertexBuffers = 44;
    public const int D3D12List_SOSetTargets = 45;
    public const int D3D12List_OMSetRenderTargets = 46;
    public const int D3D12List_ClearDepthStencilView = 47;
    public const int D3D12List_ClearRenderTargetView = 48;
    public const int D3D12List_ClearUnorderedAccessViewUint = 49;
    public const int D3D12List_ClearUnorderedAccessViewFloat = 50;
    public const int D3D12List_DiscardResource = 51;
    public const int D3D12List_BeginQuery = 52;
    public const int D3D12List_EndQuery = 53;
    public const int D3D12List_ResolveQueryData = 54;
    public const int D3D12List_SetPredication = 55;
    public const int D3D12List_ExecuteIndirect = 56;

    // ============================================================
    // IDirect3D9 (d3d9.h) — inherits IUnknown
    // ============================================================
    public const int Direct3D9_RegisterSoftwareDevice = 3;
    public const int Direct3D9_GetAdapterCount = 4;
    public const int Direct3D9_GetAdapterIdentifier = 5;
    public const int Direct3D9_GetAdapterModeCount = 6;
    public const int Direct3D9_EnumAdapterModes = 7;
    public const int Direct3D9_GetAdapterDisplayMode = 8;
    public const int Direct3D9_CheckDeviceType = 9;
    public const int Direct3D9_CheckDeviceFormat = 10;
    public const int Direct3D9_CheckDeviceMultiSampleType = 11;
    public const int Direct3D9_CheckDepthStencilMatch = 12;
    public const int Direct3D9_CheckDeviceFormatConversion = 13;
    public const int Direct3D9_GetDeviceCaps = 14;
    public const int Direct3D9_GetAdapterMonitor = 15;
    public const int Direct3D9_CreateDevice = 16;

    // ============================================================
    // IDirect3DDevice9 (d3d9.h) — inherits IUnknown
    // ============================================================
    public const int Direct3DDevice9_TestCooperativeLevel = 3;
    public const int Direct3DDevice9_GetAvailableTextureMem = 4;
    public const int Direct3DDevice9_EvictManagedResources = 5;
    public const int Direct3DDevice9_GetDirect3D = 6;
    public const int Direct3DDevice9_GetDeviceCaps = 7;
    public const int Direct3DDevice9_GetDisplayMode = 8;
    public const int Direct3DDevice9_GetCreationParameters = 9;
    public const int Direct3DDevice9_SetCursorProperties = 10;
    public const int Direct3DDevice9_SetCursorPosition = 11;
    public const int Direct3DDevice9_ShowCursor = 12;
    public const int Direct3DDevice9_CreateAdditionalSwapChain = 13;
    public const int Direct3DDevice9_GetSwapChain = 14;
    public const int Direct3DDevice9_GetNumberOfSwapChains = 15;
    public const int Direct3DDevice9_Reset = 16;
    public const int Direct3DDevice9_Present = 17;
    public const int Direct3DDevice9_GetBackBuffer = 18;
    public const int Direct3DDevice9_GetRasterStatus = 19;
    public const int Direct3DDevice9_SetDialogBoxMode = 20;
    public const int Direct3DDevice9_SetGammaRamp = 21;
    public const int Direct3DDevice9_GetGammaRamp = 22;
    public const int Direct3DDevice9_CreateTexture = 23;
    public const int Direct3DDevice9_CreateVolumeTexture = 24;
    public const int Direct3DDevice9_CreateCubeTexture = 25;
    public const int Direct3DDevice9_CreateVertexBuffer = 26;
    public const int Direct3DDevice9_CreateIndexBuffer = 27;
    public const int Direct3DDevice9_CreateRenderTarget = 28;
    public const int Direct3DDevice9_CreateDepthStencilSurface = 29;
    public const int Direct3DDevice9_UpdateSurface = 30;
    public const int Direct3DDevice9_UpdateTexture = 31;
    public const int Direct3DDevice9_GetRenderTargetData = 32;
    public const int Direct3DDevice9_GetFrontBufferData = 33;
    public const int Direct3DDevice9_StretchRect = 34;
    public const int Direct3DDevice9_ColorFill = 35;
    public const int Direct3DDevice9_CreateOffscreenPlainSurface = 36;
    public const int Direct3DDevice9_SetRenderTarget = 37;
    public const int Direct3DDevice9_GetRenderTarget = 38;
    public const int Direct3DDevice9_SetDepthStencilSurface = 39;
    public const int Direct3DDevice9_GetDepthStencilSurface = 40;
    public const int Direct3DDevice9_BeginScene = 41;
    public const int Direct3DDevice9_EndScene = 42;
    public const int Direct3DDevice9_Clear = 43;
    public const int Direct3DDevice9_SetTransform = 44;
    public const int Direct3DDevice9_GetTransform = 45;
    public const int Direct3DDevice9_MultiplyTransform = 46;
    public const int Direct3DDevice9_SetViewport = 47;
    public const int Direct3DDevice9_GetViewport = 48;
}
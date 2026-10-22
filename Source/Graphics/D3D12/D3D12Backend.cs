using System;

namespace Kiero.Net;

internal sealed unsafe class D3D12Backend : IBackend
{
    private const int FRAMES_IN_FLIGHT = 2;
    private const int SRV_HEAP_SIZE = 256;

    private nint swapChain;
    private nint device;
    private nint queue;
    private nint rtvHeap;
    private uint rtvSize;

    private nint srvHeap;
    private nint fence;

    private nuint rtvHeapCpuStart;

    private nint[] allocators = new nint[FRAMES_IN_FLIGHT];
    private nint[] lists = new nint[FRAMES_IN_FLIGHT];
    private nint[] cachedBackbuffers;
    private ulong[] fenceValues = new ulong[FRAMES_IN_FLIGHT];

    private int rtvHeapSize;

    private int frameIndex;
    private ulong fenceCounter;
    private long frameCounter;

    public bool isReady { get; private set; }

    public bool Init(nint handle, int rtvFormat)
    {
        swapChain = handle;
        Logger.Info("Dx12", $"Init sc=0x{swapChain:X} rtvFormat={rtvFormat}");

        Guid iidDev = Guids.IID_ID3D12Device;
        nint dev = Vtbl.SwapChainGetDevice(swapChain, &iidDev);
        if (dev == 0) { Logger.Error("Dx12", "GetDevice failed"); return false; }
        device = dev;
        Logger.Info("Dx12", $"device=0x{dev:X}");

        if (ExecuteCommandListsHook.LastQueue == 0)
        {
            Logger.Warn("Dx12", "queue not captured yet, defer");
            return false;
        }

        nint queueDev = ExecuteCommandListsHook.LastQueueDevice;
        if (queueDev != 0 && queueDev != device)
        {
            Logger.Error("Dx12", $"queue device mismatch queueDev=0x{queueDev:X} swapDev=0x{device:X}");
            return false;
        }

        queue = ExecuteCommandListsHook.LastQueue;
        Logger.Info("Dx12", $"queue=0x{queue:X} (from hook)");

        int bufferCount = 3;
        try
        {
            byte* desc = stackalloc byte[128];
            var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)
                Vtbl.VTableM(swapChain, Vtbl.SwapChain_GetDesc);
            if (getDesc(swapChain, desc) >= 0)
            {
                uint bc = *(uint*)(desc + 40);
                if (bc >= 1 && bc <= 8) bufferCount = (int)bc;
            }
        }
        catch { }
        rtvHeapSize = bufferCount;
        cachedBackbuffers = new nint[rtvHeapSize];
        Logger.Info("Dx12", $"bufferCount={rtvHeapSize}");

        Guid iidHeap = Guids.IID_ID3D12DescriptorHeap;
        var createHeap = (delegate* unmanaged[Stdcall]<nint, D3D12Structs.D3D12_DESCRIPTOR_HEAP_DESC*, Guid*, nint*, int>)
            Vtbl.VTableM(device, Vtbl.D3D12Device_CreateDescriptorHeap);

        var rtvDesc = new D3D12Structs.D3D12_DESCRIPTOR_HEAP_DESC
        {
            Type = D3D12Structs.HEAP_TYPE_RTV,
            NumDescriptors = (uint)rtvHeapSize,
            Flags = D3D12Structs.HEAP_FLAG_NONE,
            NodeMask = 1,
        };

        nint rtvH = 0;
        int hr = createHeap(device, &rtvDesc, &iidHeap, &rtvH);
        if (hr < 0 || rtvH == 0) { Logger.Error("Dx12", $"CreateRtvHeap hr=0x{hr:X8}"); return false; }
        rtvHeap = rtvH;
        Logger.Info("Dx12", $"rtvHeap=0x{rtvH:X}");

        var getInc = (delegate* unmanaged[Stdcall]<nint, int, uint>)
            Vtbl.VTableM(device, Vtbl.D3D12Device_GetDescriptorHandleIncrementSize);
        rtvSize = getInc(device, D3D12Structs.HEAP_TYPE_RTV);
        Logger.Info("Dx12", $"rtvSize={rtvSize}");

        rtvHeapCpuStart = (nuint)D3D12Shim.GetCpuHandle(rtvHeap);
        Logger.Info("Dx12", $"rtvHeapCpuStart=0x{rtvHeapCpuStart:X}");

        var srvDesc = new D3D12Structs.D3D12_DESCRIPTOR_HEAP_DESC
        {
            Type = D3D12Structs.HEAP_TYPE_CBV_SRV_UAV,
            NumDescriptors = SRV_HEAP_SIZE,
            Flags = D3D12Structs.HEAP_FLAG_SHADER_VISIBLE,
            NodeMask = 0,
        };

        nint srvH = 0;
        hr = createHeap(device, &srvDesc, &iidHeap, &srvH);
        if (hr < 0 || srvH == 0) { Logger.Error("Dx12", $"CreateSrvHeap hr=0x{hr:X8}"); return false; }
        srvHeap = srvH;
        Logger.Info("Dx12", $"srvHeap=0x{srvH:X}");

        Guid iidAlloc = Guids.IID_ID3D12CommandAllocator;
        Guid iidList = Guids.IID_ID3D12GraphicsCommandList;
        var createAlloc = (delegate* unmanaged[Stdcall]<nint, int, Guid*, nint*, int>)
            Vtbl.VTableM(device, Vtbl.D3D12Device_CreateCommandAllocator);
        var createList = (delegate* unmanaged[Stdcall]<nint, int, int, nint, nint, Guid*, nint*, int>)
            Vtbl.VTableM(device, Vtbl.D3D12Device_CreateCommandList);

        for (int i = 0; i < FRAMES_IN_FLIGHT; i++)
        {
            nint alloc = 0;
            hr = createAlloc(device, D3D12Structs.COMMAND_LIST_TYPE_DIRECT, &iidAlloc, &alloc);
            if (hr < 0 || alloc == 0) { Logger.Error("Dx12", $"CreateAlloc[{i}] hr=0x{hr:X8}"); return false; }
            allocators[i] = alloc;

            nint list = 0;
            hr = createList(device, 0, D3D12Structs.COMMAND_LIST_TYPE_DIRECT, alloc, 0, &iidList, &list);
            if (hr < 0 || list == 0) { Logger.Error("Dx12", $"CreateList[{i}] hr=0x{hr:X8}"); return false; }
            lists[i] = list;

            var close = (delegate* unmanaged[Stdcall]<nint, int>)
                Vtbl.VTableM(list, Vtbl.D3D12List_Close);
            close(list);
            Logger.Info("Dx12", $"alloc[{i}]=0x{alloc:X} list[{i}]=0x{list:X}");
        }

        Guid iidFence = Guids.IID_ID3D12Fence;
        var createFence = (delegate* unmanaged[Stdcall]<nint, ulong, int, Guid*, nint*, int>)
            Vtbl.VTableM(device, Vtbl.D3D12Device_CreateFence);
        nint f = 0;
        hr = createFence(device, 0, 0, &iidFence, &f);
        if (hr < 0 || f == 0) { Logger.Error("Dx12", $"CreateFence hr=0x{hr:X8}"); return false; }
        fence = f;
        Logger.Info("Dx12", $"fence=0x{f:X}");

        if (ImGui.ImplDX12Init(device, queue, FRAMES_IN_FLIGHT, rtvFormat, srvHeap, SRV_HEAP_SIZE) == 0)
        {
            Logger.Error("Dx12", "ImplDX12Init failed");
            return false;
        }
        Logger.Info("Dx12", "ImplDX12Init ok");

        isReady = true;
        Logger.Info("Dx12", "ready");
        return true;
    }

    public void NewFrame() => ImGui.ImplDX12NewFrame();

    public void OnResize()
    {
        if (cachedBackbuffers != null) Array.Clear(cachedBackbuffers);
        Logger.Info("Dx12", "RTV cache cleared (OnResize)");
    }

    public void OnReset() { }
    public void OnResetPost() { }

    public bool TryGetSize(out float w, out float h)
    {
        if (!Dxgi.TryGetSize(swapChain, out uint ww, out uint hh)) { w = h = 0; return false; }
        w = ww;
        h = hh;
        return true;
    }

    public void Render(nint drawData)
    {
        if (!isReady || drawData == 0) return;

        int fi = frameIndex;

        if (fenceValues[fi] > 0)
        {
            int spins = 0;
            while (D3D12Shim.FenceGetCompletedValue(fence) < fenceValues[fi])
            {
                System.Threading.Thread.Yield();
                if (++spins > 10_000_000)
                {
                    Logger.Error("Dx12", "Fence wait timeout — GPU hung?");
                    isReady = false;
                    return;
                }
            }
        }

        var allocReset = (delegate* unmanaged[Stdcall]<nint, int>)
            Vtbl.VTableM(allocators[fi], Vtbl.D3D12Allocator_Reset);
        allocReset(allocators[fi]);

        nint cmdList = lists[fi];
        var listReset = (delegate* unmanaged[Stdcall]<nint, nint, nint, int>)
            Vtbl.VTableM(cmdList, Vtbl.D3D12List_Reset);
        listReset(cmdList, allocators[fi], 0);

        var close = (delegate* unmanaged[Stdcall]<nint, int>)
            Vtbl.VTableM(cmdList, Vtbl.D3D12List_Close);
        bool closed = false;

        try
        {
            uint idx = Dxgi.GetCurrentBackBufferIndex(swapChain);
            if (idx >= (uint)rtvHeapSize) idx = 0;

            nint backbuffer = Dxgi.GetBackBuffer(swapChain, idx, Guids.IID_ID3D12Resource);
            if (backbuffer == 0) return;

            try
            {
                var rtvHandle = new D3D12Structs.D3D12_CPU_DESCRIPTOR_HANDLE
                {
                    ptr = rtvHeapCpuStart + (nuint)(idx * rtvSize)
                };

                if (cachedBackbuffers[idx] != backbuffer)
                {
                    var createRtv = (delegate* unmanaged[Stdcall]<nint, nint, nint, D3D12Structs.D3D12_CPU_DESCRIPTOR_HANDLE, void>)
                        Vtbl.VTableM(device, Vtbl.D3D12Device_CreateRenderTargetView);
                    createRtv(device, backbuffer, 0, rtvHandle);
                    cachedBackbuffers[idx] = backbuffer;
                    Logger.Info("Dx12", $"RTV created for idx={idx} bb=0x{backbuffer:X}");
                }

                var barrier = new D3D12Structs.D3D12_RESOURCE_BARRIER
                {
                    Type = D3D12Structs.BARRIER_TYPE_TRANSITION,
                    Flags = 0,
                    Resource = backbuffer,
                    Subresource = D3D12Structs.ALL_SUBRESOURCES,
                    StateBefore = D3D12Structs.STATE_PRESENT,
                    StateAfter = D3D12Structs.STATE_RENDER_TARGET,
                };
                var resBarrier = (delegate* unmanaged[Stdcall]<nint, uint, D3D12Structs.D3D12_RESOURCE_BARRIER*, void>)
                    Vtbl.VTableM(cmdList, Vtbl.D3D12List_ResourceBarrier);
                resBarrier(cmdList, 1, &barrier);

                var omSet = (delegate* unmanaged[Stdcall]<nint, uint, D3D12Structs.D3D12_CPU_DESCRIPTOR_HANDLE*, int, nint, void>)
                    Vtbl.VTableM(cmdList, Vtbl.D3D12List_OMSetRenderTargets);
                D3D12Structs.D3D12_CPU_DESCRIPTOR_HANDLE h = rtvHandle;
                omSet(cmdList, 1, &h, 0, 0);

                var setHeaps = (delegate* unmanaged[Stdcall]<nint, uint, nint*, void>)
                    Vtbl.VTableM(cmdList, Vtbl.D3D12List_SetDescriptorHeaps);
                nint srvHeapLocal = srvHeap;
                setHeaps(cmdList, 1, &srvHeapLocal);

                ImGui.ImplDX12RenderDrawData(drawData, cmdList);

                barrier.StateBefore = D3D12Structs.STATE_RENDER_TARGET;
                barrier.StateAfter = D3D12Structs.STATE_PRESENT;
                resBarrier(cmdList, 1, &barrier);

                close(cmdList);
                closed = true;

                var exec = (delegate* unmanaged[Stdcall]<nint, uint, nint*, void>)
                    Vtbl.VTableM(queue, Vtbl.D3D12Queue_ExecuteCommandLists);
                nint cl = cmdList;
                exec(queue, 1, &cl);

                ulong signalValue = ++fenceCounter;
                var signal = (delegate* unmanaged[Stdcall]<nint, nint, ulong, int>)
                    Vtbl.VTableM(queue, Vtbl.D3D12Queue_Signal);
                signal(queue, fence, signalValue);
                fenceValues[fi] = signalValue;

                frameIndex = (fi + 1) % FRAMES_IN_FLIGHT;

                long N = ++frameCounter;
                if (N == 1 || N % 600 == 0)
                    Logger.Info("Dx12", $"frame={N} idx={idx} bb=0x{backbuffer:X}");
            }
            finally
            {
                Com.Release(backbuffer);
            }
        }
        finally
        {
            if (!closed)
            {
                try { close(cmdList); } catch { }
            }
        }
    }

    public void Shutdown()
    {
        if (!isReady) return;
        isReady = false;

        if (fence != 0 && fenceValues[0] > 0)
        {
            int spins = 0;
            while (D3D12Shim.FenceGetCompletedValue(fence) < fenceCounter)
            {
                System.Threading.Thread.Yield();
                if (++spins > 2_000_000) { Logger.Warn("Dx12", "Shutdown fence wait timeout"); break; }
            }
        }

        try { ImGui.ImplDX12Shutdown(); } catch { }

        for (int i = 0; i < FRAMES_IN_FLIGHT; i++)
        {
            if (lists[i] != 0) { Com.Release(lists[i]); lists[i] = 0; }
            if (allocators[i] != 0) { Com.Release(allocators[i]); allocators[i] = 0; }
        }

        if (fence != 0) { Com.Release(fence); fence = 0; }
        if (rtvHeap != 0) { Com.Release(rtvHeap); rtvHeap = 0; }
        if (srvHeap != 0) { Com.Release(srvHeap); srvHeap = 0; }
        if (device != 0) { Com.Release(device); device = 0; }

        queue = 0;
        swapChain = 0;
        rtvHeapCpuStart = 0;
        rtvSize = 0;
        rtvHeapSize = 0;
        cachedBackbuffers = null;
        frameIndex = 0;
        fenceCounter = 0;
        frameCounter = 0;

        Logger.Info("Dx12", "Shutdown complete");
    }
}
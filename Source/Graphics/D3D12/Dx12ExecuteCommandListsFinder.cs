using System;

namespace Kiero.Net;

public static unsafe class Dx12ExecuteCommandListsFinder
{
    public static bool TryGet(out nint executeCommandLists)
    {
        executeCommandLists = 0;

        Guid iidFactory = Guids.IID_IDXGIFactory2;
        nint factory = 0;
        int hr = D3D12.CreateDXGIFactory2(0, &iidFactory, &factory);
        if (hr < 0 || factory == 0) { Logger.Error("Dx12Finder", $"CreateDXGIFactory2 hr=0x{hr:X8}"); return false; }

        nint adapter = 0;
        nint device = 0;
        nint queue = 0;

        try
        {
            nint vtFactory = *(nint*)factory;
            var enumAdapters = (delegate* unmanaged[Stdcall]<nint, uint, nint*, int>)(*(nint*)(vtFactory + 12 * IntPtr.Size));

            byte* desc = stackalloc byte[0x140];
            for (uint i = 0; i < 8; i++)
            {
                nint a = 0;
                if (enumAdapters(factory, i, &a) < 0 || a == 0) break;

                nint vtA = *(nint*)a;
                var getDesc = (delegate* unmanaged[Stdcall]<nint, void*, int>)(*(nint*)(vtA + 10 * IntPtr.Size));

                for (int b = 0; b < 0x140; b++) desc[b] = 0;

                if (getDesc(a, desc) >= 0)
                {
                    uint flags = *(uint*)(desc + 0x130);
                    if ((flags & 0x2) == 0) { adapter = a; break; }
                }
                Com.Release(a);
            }

            if (adapter == 0) { Logger.Error("Dx12Finder", "no hardware adapter found"); return false; }

            Guid iidDev = Guids.IID_ID3D12Device;
            hr = D3D12.D3D12CreateDevice(adapter, D3D12.FEATURE_LEVEL_11_0, &iidDev, &device);
            if (hr < 0 || device == 0) { Logger.Error("Dx12Finder", $"D3D12CreateDevice hr=0x{hr:X8}"); return false; }

            var qdesc = new D3D12Structs.D3D12_COMMAND_QUEUE_DESC { Type = D3D12Structs.COMMAND_LIST_TYPE_DIRECT };
            nint vtDev = *(nint*)device;
            var createQueue = (delegate* unmanaged[Stdcall]<nint, D3D12Structs.D3D12_COMMAND_QUEUE_DESC*, Guid*, nint*, int>)(*(nint*)(vtDev + 8 * IntPtr.Size));
            Guid iidQueue = Guids.IID_ID3D12CommandQueue;
            hr = createQueue(device, &qdesc, &iidQueue, &queue);
            if (hr < 0 || queue == 0) { Logger.Error("Dx12Finder", $"CreateCommandQueue hr=0x{hr:X8}"); return false; }

            nint vtQueue = *(nint*)queue;
            nint execCmdListsAddr = *(nint*)(vtQueue + 10 * IntPtr.Size);
            if (execCmdListsAddr != 0 && !ExecuteCommandListsHook.IsInstalled)
            {
                if (ExecuteCommandListsHook.Install(execCmdListsAddr))
                {
                    Logger.Info("Dx12", "ExecuteCommandLists hook installed (from game queue)");
                    executeCommandLists = execCmdListsAddr; 
                }
                else
                {
                    Logger.Warn("Dx12", "ExecuteCommandLists hook failed");
                }
            }

            return executeCommandLists != 0;
        }
        finally
        {
            if (queue != 0) Com.Release(queue);
            if (device != 0) Com.Release(device);
            if (adapter != 0) Com.Release(adapter);
            if (factory != 0) Com.Release(factory);
        }
    }
}
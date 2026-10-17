using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Kiero.Net;

public static unsafe class ExecuteCommandListsHook
{
    private const int COMMAND_LIST_TYPE_DIRECT = 0;

    private static nint original;
    private static nint lastQueue;
    private static int installed;

    public static nint LastQueue => lastQueue;
    public static bool IsInstalled => installed != 0;

    public static bool Install(nint addr)
    {
        if (addr == 0) return false;
        if (Interlocked.CompareExchange(ref installed, 1, 0) != 0) return true;

        void* detour = (delegate* unmanaged[Stdcall]<nint, uint, nint*, void>)&Detour;
        void* orig = null;
        if (MinHook.CreateHook((void*)addr, detour, &orig) != 0) { installed = 0; return false; }
        original = (nint)orig;
        if (MinHook.EnableHook((void*)addr) != 0) { installed = 0; return false; }
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void Detour(nint queue, uint count, nint* lists)
    {
        if (queue != 0 && lastQueue == 0)
        {
            int type = D3D12Shim.QueueGetType(queue);
            if (type == COMMAND_LIST_TYPE_DIRECT)
            {
                lastQueue = queue;
                Logger.Info("ECLHook", $"captured direct queue=0x{queue:X}");
            }
        }

        ((delegate* unmanaged[Stdcall]<nint, uint, nint*, void>)original)(queue, count, lists);
    }
}
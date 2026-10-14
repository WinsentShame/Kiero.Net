using System;

namespace Kiero.Net;

public static unsafe class Com
{
    public static int QueryInterface(nint p, Guid* iid, nint* outPtr)
    {
        var fn = (delegate* unmanaged[Stdcall]<nint, Guid*, nint*, int>)((*(void***)p)[0]);
        return fn(p, iid, outPtr);
    }

    public static uint AddRef(nint p)
    {
        var fn = (delegate* unmanaged[Stdcall]<nint, uint>)((*(void***)p)[1]);
        return fn(p);
    }

    public static uint Release(nint p)
    {
        if (p == 0) return 0;
        var fn = (delegate* unmanaged[Stdcall]<nint, uint>)((*(void***)p)[2]);
        return fn(p);
    }
}
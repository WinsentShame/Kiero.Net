using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kiero.Net;

internal static unsafe class Utf8
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte* Ptr(ReadOnlySpan<byte> u8)
        => (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(u8));
}
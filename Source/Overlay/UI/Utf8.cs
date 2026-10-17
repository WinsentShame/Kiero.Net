#nullable enable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Kiero.Net;

internal static unsafe class Utf8
{
    private const int ARENA_SIZE = 32 * 1024;
    private const int BIG_SLOTS = 4;

    [ThreadStatic] private static byte[]? ARENA;
    [ThreadStatic] private static int OFFSET;
    [ThreadStatic] private static byte[][]? BIG;
    [ThreadStatic] private static int BIG_INDEX;

    public static byte* Of(string? text)
    {
        if (text == null) return null;

        int need = Encoding.UTF8.GetByteCount(text) + 1;

        if (need <= ARENA_SIZE)
        {
            ARENA ??= GC.AllocateUninitializedArray<byte>(ARENA_SIZE, pinned: true);
            if (OFFSET + need > ARENA_SIZE) OFFSET = 0;

            int start = OFFSET;
            int n = Encoding.UTF8.GetBytes(text, 0, text.Length, ARENA, start);
            ARENA[start + n] = 0;
            OFFSET = start + need;
            return (byte*)Unsafe.AsPointer(ref ARENA[start]);
        }

        BIG ??= new byte[BIG_SLOTS][];
        byte[] big = GC.AllocateUninitializedArray<byte>(need, pinned: true);
        int m = Encoding.UTF8.GetBytes(text, 0, text.Length, big, 0);
        big[m] = 0;
        BIG[BIG_INDEX] = big;
        BIG_INDEX = (BIG_INDEX + 1) % BIG_SLOTS;
        return (byte*)Unsafe.AsPointer(ref big[0]);
    }

    public static string? FromPtr(byte* p) => p == null ? null : Marshal.PtrToStringUTF8((nint)p);

    public static void WriteZ(string? s, Span<byte> dst)
    {
        dst.Clear();
        if (string.IsNullOrEmpty(s)) return;

        int max = dst.Length - 1;
        int chars = s.Length;
        while (chars > 0 && Encoding.UTF8.GetByteCount(s.AsSpan(0, chars)) > max) chars--;
        Encoding.UTF8.GetBytes(s.AsSpan(0, chars), dst);
    }

    public static string ReadZ(ReadOnlySpan<byte> buf)
    {
        int len = buf.IndexOf((byte)0);
        if (len < 0) len = buf.Length;
        return Encoding.UTF8.GetString(buf.Slice(0, len));
    }

    public static byte B(bool v) => (byte)(v ? 1 : 0);

    public static void Need(int have, int need, string what)
    {
        if (have < need) throw new ArgumentException($"{what}: нужно минимум {need} элемента(ов), передано {have}");
    }
}
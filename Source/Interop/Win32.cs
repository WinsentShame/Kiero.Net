using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class Win32
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern nint GetModuleHandleW(string name);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
    public static extern nint GetProcAddress(nint module, string name);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern nint CreateWindowExW(uint exStyle, string className, string windowName, uint style, int x, int y, int w, int h, nint parent, nint menu, nint instance, nint param);

    [DllImport("user32.dll")]
    public static extern bool DestroyWindow(nint hwnd);
}
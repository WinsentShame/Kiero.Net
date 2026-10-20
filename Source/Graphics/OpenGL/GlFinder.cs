namespace Kiero.Net;

public static class GlFinder
{
    public static bool TryGet(out nint swapBuffersAddr)
    {
        swapBuffersAddr = 0;

        nint opengl32 = Win32.GetModuleHandleW("opengl32.dll");
        if (opengl32 != 0)
        {
            nint s = Win32.GetProcAddress(opengl32, "wglSwapBuffers");
            if (s != 0)
            {
                Logger.Info("GlFinder", $"wglSwapBuffers = 0x{s:X}");
                swapBuffersAddr = s;
                return true;
            }
        }

        nint gdi32 = Win32.GetModuleHandleW("gdi32.dll");
        if (gdi32 != 0)
        {
            nint s = Win32.GetProcAddress(gdi32, "SwapBuffers");
            if (s != 0)
            {
                Logger.Info("GlFinder", $"gdi32!SwapBuffers = 0x{s:X}");
                swapBuffersAddr = s;
                return true;
            }
        }

        Logger.Error("GlFinder", "no SwapBuffers found");
        return false;
    }
}
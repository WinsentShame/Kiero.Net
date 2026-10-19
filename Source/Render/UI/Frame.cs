#nullable enable
using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Frame
    {
        public static void NewFrame() => ImGui.NewFrame();
        public static void EndFrame() => ImGui.EndFrame();
        public static void Render() => ImGui.Render();
        public static nint GetIO() => ImGui.GetIO();
        public static nint GetStyle() => ImGui.GetStyle();
        public static nint GetDrawData() => ImGui.GetDrawData();
        public static string? GetVersion() => Utf8.FromPtr(ImGui.GetVersion());

        public static (float W, float H) DisplaySize
        {
            get { ImGui.IOGetDisplaySize(ImGui.GetIO(), out float w, out float h); return (w, h); }
        }
        public static void SetDisplaySize(float w, float h) => ImGui.IOSetDisplaySize(ImGui.GetIO(), w, h);
        public static void SetDeltaTime(float dt) => ImGui.IOSetDeltaTime(ImGui.GetIO(), dt);
        public static void SetConfigFlags(int flags) => ImGui.IOSetConfigFlags(ImGui.GetIO(), flags);

        public static void StyleColorsDark() => ImGui.StyleColorsDark(0);
        public static void StyleColorsLight() => ImGui.StyleColorsLight(0);
        public static void StyleColorsClassic() => ImGui.StyleColorsClassic(0);

        private static nint INI_PTR, LOG_PTR;

        public static void SetIniFilename(string? path)
        {
            nint old = INI_PTR;
            INI_PTR = path == null ? 0 : Marshal.StringToCoTaskMemUTF8(path);
            ImGui.IOSetIniFilename(ImGui.GetIO(), (byte*)INI_PTR);
            if (old != 0) Marshal.FreeCoTaskMem(old);
        }
        public static void DisableIni() => SetIniFilename(null);

        public static void SetLogFilename(string? path)
        {
            nint old = LOG_PTR;
            LOG_PTR = path == null ? 0 : Marshal.StringToCoTaskMemUTF8(path);
            ImGui.IOSetLogFilename(ImGui.GetIO(), (byte*)LOG_PTR);
            if (old != 0) Marshal.FreeCoTaskMem(old);
        }
    }
}
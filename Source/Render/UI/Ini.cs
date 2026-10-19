#nullable enable
using System.Runtime.InteropServices;
using System.Text;

namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Ini
    {
        public static void LoadFromDisk(string file) => ImGui.LoadIniSettingsFromDisk(Utf8.Of(file));
        public static void SaveToDisk(string file) => ImGui.SaveIniSettingsToDisk(Utf8.Of(file));
        public static void LoadFromMemory(string data)
            => ImGui.LoadIniSettingsFromMemory(Utf8.Of(data), (nuint)Encoding.UTF8.GetByteCount(data));
        public static string SaveToMemory()
        {
            byte* p = ImGui.SaveIniSettingsToMemory(out nuint size);
            return p == null || size == 0 ? string.Empty : Marshal.PtrToStringUTF8((nint)p, (int)size) ?? string.Empty;
        }
    }
}
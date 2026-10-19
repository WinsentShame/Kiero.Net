#nullable enable
namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Debug
    {
        public static void ShowDemoWindow() => ImGui.ShowDemoWindow(null);
        public static void ShowDemoWindow(ref bool open) { fixed (bool* p = &open) ImGui.ShowDemoWindow((byte*)p); }
        public static void ShowMetricsWindow() => ImGui.ShowMetricsWindow(null);
        public static void ShowMetricsWindow(ref bool open) { fixed (bool* p = &open) ImGui.ShowMetricsWindow((byte*)p); }
        public static void ShowDebugLogWindow() => ImGui.ShowDebugLogWindow(null);
        public static void ShowIDStackToolWindow() => ImGui.ShowIDStackToolWindow(null);
        public static void ShowAboutWindow() => ImGui.ShowAboutWindow(null);
        public static void ShowStyleEditor() => ImGui.ShowStyleEditor(0);
        public static void ShowUserGuide() => ImGui.ShowUserGuide();
        public static void DebugTextEncoding(string text) => ImGui.DebugTextEncoding(Utf8.Of(text));
        public static void DebugFlashStyleColor(int idx) => ImGui.DebugFlashStyleColor(idx);
        public static void DebugStartItemPicker() => ImGui.DebugStartItemPicker();
    }
}
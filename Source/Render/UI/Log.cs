#nullable enable
namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Log
    {
        public static void ToTTY(int depth = -1) => ImGui.LogToTTY(depth);
        public static void ToFile(string file, int depth = -1) => ImGui.LogToFile(depth, Utf8.Of(file));
        public static void ToClipboard(int depth = -1) => ImGui.LogToClipboard(depth);
        public static void Finish() => ImGui.LogFinish();
        public static void Buttons() => ImGui.LogButtons();
        public static void Text(string text) => ImGui.LogText(Utf8.Of(text));
    }
}
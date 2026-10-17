#nullable enable
namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Clipboard
    {
        public static string? Get() => Utf8.FromPtr(ImGui.GetClipboardText());
        public static void Set(string text) => ImGui.SetClipboardText(Utf8.Of(text));
    }
}
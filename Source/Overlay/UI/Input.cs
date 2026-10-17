#nullable enable
namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Input
    {
        public static bool WantCaptureMouse => ImGui.IOWantCaptureMouse(ImGui.GetIO()) != 0;
        public static bool WantCaptureKeyboard => ImGui.IOWantCaptureKeyboard(ImGui.GetIO()) != 0;

        public static void AddMousePos(float x, float y) => ImGui.IOAddMousePos(ImGui.GetIO(), x, y);
        public static void AddMouseButton(int btn, bool down) => ImGui.IOAddMouseButton(ImGui.GetIO(), btn, Utf8.B(down));
        public static void AddMouseWheel(float wx, float wy) => ImGui.IOAddMouseWheel(ImGui.GetIO(), wx, wy);
        public static void AddKey(int key, bool down) => ImGui.IOAddKey(ImGui.GetIO(), key, Utf8.B(down));
        public static void AddInputCharacter(uint c) => ImGui.IOAddInputCharacter(ImGui.GetIO(), c);
        public static void AddFocusEvent(bool focused) => ImGui.IOAddFocusEvent(ImGui.GetIO(), Utf8.B(focused));

        public static void SetNextFrameWantCaptureKeyboard(bool want) => ImGui.SetNextFrameWantCaptureKeyboard(Utf8.B(want));
        public static void SetNextFrameWantCaptureMouse(bool want) => ImGui.SetNextFrameWantCaptureMouse(Utf8.B(want));

        public static bool IsKeyDown(int key) => ImGui.IsKeyDown(key) != 0;
        public static bool IsKeyPressed(int key, bool repeat = true) => ImGui.IsKeyPressed(key, Utf8.B(repeat)) != 0;
        public static bool IsKeyReleased(int key) => ImGui.IsKeyReleased(key) != 0;
        public static int GetKeyPressedAmount(int key, float delay, float rate) => ImGui.GetKeyPressedAmount(key, delay, rate);
        public static string? GetKeyName(int key) => Utf8.FromPtr(ImGui.GetKeyName(key));
        public static bool IsKeyChordPressed(int chord) => ImGui.IsKeyChordPressed(chord) != 0;
        public static bool Shortcut(int chord, int flags = 0) => ImGui.Shortcut(chord, flags) != 0;

        public static bool IsMouseDown(int btn = 0) => ImGui.IsMouseDown(btn) != 0;
        public static bool IsMouseClicked(int btn = 0, bool repeat = false) => ImGui.IsMouseClicked(btn, Utf8.B(repeat)) != 0;
        public static bool IsMouseReleased(int btn = 0) => ImGui.IsMouseReleased(btn) != 0;
        public static bool IsMouseReleasedWithDelay(int btn, float delay) => ImGui.IsMouseReleasedWithDelay(btn, delay) != 0;
        public static bool IsMouseDoubleClicked(int btn = 0) => ImGui.IsMouseDoubleClicked(btn) != 0;
        public static int GetMouseClickedCount(int btn = 0) => ImGui.GetMouseClickedCount(btn);
        public static bool IsAnyMouseDown() => ImGui.IsAnyMouseDown() != 0;
        public static bool IsMousePosValid() => ImGui.IsMousePosValid(null) != 0;
        public static bool IsMouseHoveringRect(float minX, float minY, float maxX, float maxY, bool clip = true)
            => ImGui.IsMouseHoveringRect(minX, minY, maxX, maxY, Utf8.B(clip)) != 0;
        public static bool IsMouseDragging(int btn = 0, float threshold = -1f) => ImGui.IsMouseDragging(btn, threshold) != 0;
        public static (float X, float Y) GetMousePos() { ImGui.GetMousePos(out float x, out float y); return (x, y); }
        public static (float X, float Y) GetMouseDragDelta(int btn = 0, float threshold = -1f)
        { ImGui.GetMouseDragDelta(out float x, out float y, btn, threshold); return (x, y); }
        public static (float X, float Y) GetMousePosOnOpeningCurrentPopup()
        { ImGui.GetMousePosOnOpeningCurrentPopup(out float x, out float y); return (x, y); }
        public static void ResetMouseDragDelta(int btn = 0) => ImGui.ResetMouseDragDelta(btn);
        public static int GetMouseCursor() => ImGui.GetMouseCursor();
        public static void SetMouseCursor(int cursor) => ImGui.SetMouseCursor(cursor);
    }
}
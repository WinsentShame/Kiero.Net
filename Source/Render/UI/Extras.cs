#nullable enable
using System;
using System.Numerics;

namespace Kiero.Net;  

public static unsafe partial class Ui  
{
    [Flags]
    public enum Child : int
    {
        None = 0,
        Border = 1 << 0,
        AlwaysUseWindowPadding = 1 << 1,
        ResizeX = 1 << 2,
        ResizeY = 1 << 3,
        AutoResizeX = 1 << 4,
        AutoResizeY = 1 << 5,
        AlwaysAutoResize = (1 << 4) | (1 << 5),
        FrameStyle = 1 << 7,
    }

    public static void PushStyleVar(int idx, Vector2 val)
        => ImGui.PushStyleVarVec2(idx, val.X, val.Y);

    public static uint GetColorU32Vec4(float r, float g, float b, float a)
        => ImGui.GetColorU32Vec4(r, g, b, a);

    public static uint GetColorU32Vec4(Vector4 c)
        => ImGui.GetColorU32Vec4(c.X, c.Y, c.Z, c.W);

    public static bool IsMouseDown(int btn) => ImGui.IsMouseDown(btn) != 0;

    public static bool IsMouseHoveringRect(float minX, float minY, float maxX, float maxY, bool clip = true)
        => ImGui.IsMouseHoveringRect(minX, minY, maxX, maxY, (byte)(clip ? 1 : 0)) != 0;

    public static bool IsMouseHoveringRect(Vector2 min, Vector2 max, bool clip = true)
        => IsMouseHoveringRect(min.X, min.Y, max.X, max.Y, clip);

    public static void GetMousePos(out float x, out float y) => ImGui.GetMousePos(out x, out y);
    public static Vector2 GetMousePos()
    {
        ImGui.GetMousePos(out float x, out float y);
        return new Vector2(x, y);
    }

    public static void GetWindowPos(out float x, out float y) => ImGui.GetWindowPos(out x, out y);
    public static void GetWindowSize(out float x, out float y) => ImGui.GetWindowSize(out x, out y);
    public static void GetCursorScreenPos(out float x, out float y) => ImGui.GetCursorScreenPos(out x, out y);
    public static void GetContentRegionAvail(out float x, out float y) => ImGui.GetContentRegionAvail(out x, out y);

    public static void ImDrawListAddRectFilled(
        nint dl, float minX, float minY, float maxX, float maxY,
        uint col, float rounding = 0f, int flags = 0)
        => ImGui.ImDrawListAddRectFilled(dl, minX, minY, maxX, maxY, col, rounding, flags);

    public static void ImDrawListAddRectFilled(
        nint dl, Vector2 min, Vector2 max, uint col, float rounding = 0f, int flags = 0)
        => ImGui.ImDrawListAddRectFilled(dl, min.X, min.Y, max.X, max.Y, col, rounding, flags);

    public static void ImDrawListAddRectFilledMultiColor(
        nint dl, float minX, float minY, float maxX, float maxY,
        uint colUprLeft, uint colUprRight, uint colBotRight, uint colBotLeft)
        => ImGui.ImDrawListAddRectFilledMultiColor(
            dl, minX, minY, maxX, maxY,
            colUprLeft, colUprRight, colBotRight, colBotLeft);

    public static void ImDrawListAddRectFilledMultiColor(
        nint dl, Vector2 min, Vector2 max,
        uint colUprLeft, uint colUprRight, uint colBotRight, uint colBotLeft)
        => ImGui.ImDrawListAddRectFilledMultiColor(
            dl, min.X, min.Y, max.X, max.Y,
            colUprLeft, colUprRight, colBotRight, colBotLeft);
    public static bool BeginChild(string id, float w, float h, Child childFlags, int windowFlags = 0)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(id + "\0");
        fixed (byte* p = bytes)
            return ImGui.BeginChild(p, w, h, (int)childFlags, windowFlags) != 0;
    }
}
#nullable enable
using System.Numerics;

namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static unsafe class Color
    {
        public static uint Rgba(byte r, byte g, byte b, byte a = 255) => (uint)((a << 24) | (b << 16) | (g << 8) | r);

        public static uint GetColorU32(int styleCol, float alpha = 1f) => ImGui.GetColorU32(styleCol, alpha);
        public static uint GetColorU32(float r, float g, float b, float a = 1f) => ImGui.GetColorU32Vec4(r, g, b, a);
        public static uint GetColorU32(Vector4 c) => ImGui.GetColorU32Vec4(c.X, c.Y, c.Z, c.W);

        public static Vector4 GetStyleColor(int idx)
        {
            Vector4 v = default;
            ImGui.GetStyleColorVec4(idx, (float*)&v);
            return v;
        }
        public static string? GetStyleColorName(int idx) => Utf8.FromPtr(ImGui.GetStyleColorName(idx));

        public static Vector4 ConvertU32ToFloat4(uint c)
        {
            Vector4 v = default;
            ImGui.ColorConvertU32ToFloat4(c, (float*)&v);
            return v;
        }
        public static uint ConvertFloat4ToU32(Vector4 c) => ImGui.ColorConvertFloat4ToU32(c.X, c.Y, c.Z, c.W);

        public static (float H, float S, float V) RgbToHsv(float r, float g, float b)
        { ImGui.ColorConvertRGBtoHSV(r, g, b, out float h, out float s, out float v); return (h, s, v); }
        public static (float R, float G, float B) HsvToRgb(float h, float s, float v)
        { ImGui.ColorConvertHSVtoRGB(h, s, v, out float r, out float g, out float b); return (r, g, b); }
    }
}
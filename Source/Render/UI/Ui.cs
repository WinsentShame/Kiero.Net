#nullable enable
using System;
using System.Numerics;

namespace Kiero.Net;

public static unsafe partial class Ui
{
    public static class Win
    {
        public const int None = 0, NoTitleBar = 1, NoResize = 2, NoMove = 4, NoScrollbar = 8,
            NoScrollWithMouse = 16, NoCollapse = 32, AlwaysAutoResize = 64, NoBackground = 128,
            NoSavedSettings = 256, NoMouseInputs = 512, MenuBar = 1024, HorizontalScrollbar = 2048,
            NoFocusOnAppearing = 4096, NoBringToFrontOnFocus = 8192, AlwaysVerticalScrollbar = 16384,
            AlwaysHorizontalScrollbar = 32768, NoNavInputs = 65536, NoNavFocus = 131072,
            NoNav = NoNavInputs | NoNavFocus,
            NoDecoration = NoTitleBar | NoResize | NoScrollbar | NoCollapse,
            NoInputs = NoMouseInputs | NoNavInputs | NoNavFocus;
    }

    public static class StyleVar
    {
        public const int Alpha = 0, DisabledAlpha = 1, WindowPadding = 2, WindowRounding = 3,
            WindowBorderSize = 4, WindowMinSize = 5, WindowTitleAlign = 6, ChildRounding = 7,
            ChildBorderSize = 8, PopupRounding = 9, PopupBorderSize = 10, FramePadding = 11,
            FrameRounding = 12, FrameBorderSize = 13, ItemSpacing = 14, ItemInnerSpacing = 15,
            IndentSpacing = 16, CellPadding = 17, ScrollbarSize = 18, ScrollbarRounding = 19,
            ScrollbarPadding = 20, GrabMinSize = 21, GrabRounding = 22, ImageRounding = 23,
            ImageBorderSize = 24, TabRounding = 25, TabBorderSize = 26, TabMinWidthBase = 27,
            TabMinWidthShrink = 28, TabBarBorderSize = 29, TabBarOverlineSize = 30,
            TableAngledHeadersAngle = 31, TableAngledHeadersTextAlign = 32, TreeLinesSize = 33,
            TreeLinesRounding = 34, MenuItemRounding = 35, SelectableRounding = 36,
            DragDropTargetRounding = 37, ButtonTextAlign = 38, SelectableTextAlign = 39,
            SeparatorSize = 40, SeparatorTextBorderSize = 41, SeparatorTextAlign = 42,
            SeparatorTextPadding = 43, DockingSeparatorSize = 44;
    }

    public static class Cond
    {
        public const int Always = 1, Once = 2, FirstUseEver = 4, Appearing = 8;
    }

    public static class Mouse
    {
        public const int Left = 0, Right = 1, Middle = 2;
    }

    public static class ColorFlags
    {
        public const int None = 0, NoAlpha = 2, NoPicker = 4, NoOptions = 8, NoSmallPreview = 16,
            NoInputs = 32, NoTooltip = 64, NoLabel = 128, NoSidePreview = 256;
    }

    public static class Col
    {
        public const int Text = 0, TextDisabled = 1, WindowBg = 2, ChildBg = 3, PopupBg = 4, Border = 5,
            BorderShadow = 6, FrameBg = 7, FrameBgHovered = 8, FrameBgActive = 9, TitleBg = 10,
            TitleBgActive = 11, TitleBgCollapsed = 12, MenuBarBg = 13, ScrollbarBg = 14,
            ScrollbarGrab = 15, ScrollbarGrabHovered = 16, ScrollbarGrabActive = 17, CheckMark = 18,
            SliderGrab = 19, SliderGrabActive = 20, Button = 21, ButtonHovered = 22, ButtonActive = 23,
            Header = 24, HeaderHovered = 25, HeaderActive = 26, Separator = 27, SeparatorHovered = 28,
            SeparatorActive = 29, ResizeGrip = 30, ResizeGripHovered = 31, ResizeGripActive = 32;
    }

    public static bool Begin(string name, int flags = 0) => ImGui.Begin(Utf8.Of(name), null, flags) != 0;
    public static bool Begin(string name, ref bool open, int flags = 0)
    {
        fixed (bool* p = &open) return ImGui.Begin(Utf8.Of(name), (byte*)p, flags) != 0;
    }
    public static void End() => ImGui.End();
    public static bool BeginChild(string id, float w = 0f, float h = 0f, int childFlags = 0, int windowFlags = 0)
        => ImGui.BeginChild(Utf8.Of(id), w, h, childFlags, windowFlags) != 0;
    public static bool BeginChild(uint id, float w = 0f, float h = 0f, int childFlags = 0, int windowFlags = 0)
        => ImGui.BeginChildID(id, w, h, childFlags, windowFlags) != 0;
    public static void EndChild() => ImGui.EndChild();

    public static bool IsWindowAppearing() => ImGui.IsWindowAppearing() != 0;
    public static bool IsWindowCollapsed() => ImGui.IsWindowCollapsed() != 0;
    public static bool IsWindowFocused(int flags = 0) => ImGui.IsWindowFocused(flags) != 0;
    public static bool IsWindowHovered(int flags = 0) => ImGui.IsWindowHovered(flags) != 0;
    public static nint GetWindowDrawList() => ImGui.GetWindowDrawList();
    public static nint GetBackgroundDrawList() => ImGui.GetBackgroundDrawList();
    public static nint GetForegroundDrawList() => ImGui.GetForegroundDrawList();
    public static nint GetMainViewport() => ImGui.GetMainViewport();
    public static (float X, float Y) GetWindowPos() { ImGui.GetWindowPos(out float x, out float y); return (x, y); }
    public static (float W, float H) GetWindowSize() { ImGui.GetWindowSize(out float w, out float h); return (w, h); }
    public static float GetWindowWidth() => ImGui.GetWindowWidth();
    public static float GetWindowHeight() => ImGui.GetWindowHeight();
    public static float GetWindowDpiScale() => ImGui.GetWindowDpiScale();

    public static void SetNextWindowPos(float x, float y, int cond = 0, float px = 0f, float py = 0f) => ImGui.SetNextWindowPos(x, y, cond, px, py);
    public static void SetNextWindowSize(float w, float h, int cond = 0) => ImGui.SetNextWindowSize(w, h, cond);
    public static void SetNextWindowSizeConstraints(float minW, float minH, float maxW, float maxH) => ImGui.SetNextWindowSizeConstraints(minW, minH, maxW, maxH);
    public static void SetNextWindowContentSize(float w, float h) => ImGui.SetNextWindowContentSize(w, h);
    public static void SetNextWindowCollapsed(bool collapsed, int cond = 0) => ImGui.SetNextWindowCollapsed(Utf8.B(collapsed), cond);
    public static void SetNextWindowFocus() => ImGui.SetNextWindowFocus();
    public static void SetNextWindowBgAlpha(float alpha) => ImGui.SetNextWindowBgAlpha(alpha);
    public static void SetNextWindowScroll(float x, float y) => ImGui.SetNextWindowScroll(x, y);
    public static void SetWindowPos(string name, float x, float y, int cond = 0) => ImGui.SetWindowPosStr(Utf8.Of(name), x, y, cond);
    public static void SetWindowSize(string name, float w, float h, int cond = 0) => ImGui.SetWindowSizeStr(Utf8.Of(name), w, h, cond);
    public static void SetWindowCollapsed(string name, bool collapsed, int cond = 0) => ImGui.SetWindowCollapsedStr(Utf8.Of(name), Utf8.B(collapsed), cond);
    public static void SetWindowFocus(string name) => ImGui.SetWindowFocusStr(Utf8.Of(name));

    public static float GetScrollX() => ImGui.GetScrollX();
    public static float GetScrollY() => ImGui.GetScrollY();
    public static float GetScrollMaxX() => ImGui.GetScrollMaxX();
    public static float GetScrollMaxY() => ImGui.GetScrollMaxY();
    public static void SetScrollX(float x) => ImGui.SetScrollX(x);
    public static void SetScrollY(float y) => ImGui.SetScrollY(y);
    public static void SetScrollHereX(float ratio = 0.5f) => ImGui.SetScrollHereX(ratio);
    public static void SetScrollHereY(float ratio = 0.5f) => ImGui.SetScrollHereY(ratio);
    public static void SetScrollFromPosX(float x, float ratio = 0.5f) => ImGui.SetScrollFromPosX(x, ratio);
    public static void SetScrollFromPosY(float y, float ratio = 0.5f) => ImGui.SetScrollFromPosY(y, ratio);

    public static void PushFont(float size) => ImGui.PushFont(0, size);
    public static void PushFont(nint font, float size = 0f) => ImGui.PushFont(font, size);
    public static void PopFont() => ImGui.PopFont();
    public static nint GetFont() => ImGui.GetFont();
    public static float GetFontSize() => ImGui.GetFontSize();

    public static void PushStyleColorU32(int idx, uint col) => ImGui.PushStyleColorU32(idx, col);
    public static void PushStyleColor(int idx, float r, float g, float b, float a = 1f) => ImGui.PushStyleColorVec4(idx, r, g, b, a);
    public static void PushStyleColor(int idx, Vector4 c) => ImGui.PushStyleColorVec4(idx, c.X, c.Y, c.Z, c.W);
    public static void PopStyleColor(int count = 1) => ImGui.PopStyleColor(count);
    public static void PushStyleVar(int idx, float val) => ImGui.PushStyleVar(idx, val);
    public static void PushStyleVar(int idx, float x, float y) => ImGui.PushStyleVarVec2(idx, x, y);
    public static void PushStyleVarX(int idx, float x) => ImGui.PushStyleVarX(idx, x);
    public static void PushStyleVarY(int idx, float y) => ImGui.PushStyleVarY(idx, y);
    public static void PopStyleVar(int count = 1) => ImGui.PopStyleVar(count);
    public static void PushItemFlag(int option, bool enabled) => ImGui.PushItemFlag(option, Utf8.B(enabled));
    public static void PopItemFlag() => ImGui.PopItemFlag();

    public static (float U, float V) GetFontTexUvWhitePixel()
    {
        ImGui.GetFontTexUvWhitePixel(out float x, out float y);
        return (x, y);
    }

    public static void PushItemWidth(float w) => ImGui.PushItemWidth(w);
    public static void PopItemWidth() => ImGui.PopItemWidth();
    public static void SetNextItemWidth(float w) => ImGui.SetNextItemWidth(w);
    public static float CalcItemWidth() => ImGui.CalcItemWidth();
    public static void PushTextWrapPos(float x = 0f) => ImGui.PushTextWrapPos(x);
    public static void PopTextWrapPos() => ImGui.PopTextWrapPos();

    public static void Separator() => ImGui.Separator();
    public static void SeparatorText(string label) => ImGui.SeparatorText(Utf8.Of(label));
    public static void SameLine(float offset = 0f, float spacing = -1f) => ImGui.SameLine(offset, spacing);
    public static void NewLine() => ImGui.NewLine();
    public static void Spacing() => ImGui.Spacing();
    public static void Dummy(float w, float h) => ImGui.Dummy(w, h);
    public static void Indent(float w = 0f) => ImGui.Indent(w);
    public static void Unindent(float w = 0f) => ImGui.Unindent(w);
    public static void BeginGroup() => ImGui.BeginGroup();
    public static void EndGroup() => ImGui.EndGroup();
    public static void AlignTextToFramePadding() => ImGui.AlignTextToFramePadding();
    public static float GetTextLineHeight() => ImGui.GetTextLineHeight();
    public static float GetTextLineHeightWithSpacing() => ImGui.GetTextLineHeightWithSpacing();
    public static float GetFrameHeight() => ImGui.GetFrameHeight();
    public static float GetFrameHeightWithSpacing() => ImGui.GetFrameHeightWithSpacing();

    public static (float X, float Y) GetCursorPos() { ImGui.GetCursorPos(out float x, out float y); return (x, y); }
    public static float GetCursorPosX() => ImGui.GetCursorPosX();
    public static float GetCursorPosY() => ImGui.GetCursorPosY();
    public static void SetCursorPos(float x, float y) => ImGui.SetCursorPos(x, y);
    public static void SetCursorPosX(float x) => ImGui.SetCursorPosX(x);
    public static void SetCursorPosY(float y) => ImGui.SetCursorPosY(y);
    public static (float X, float Y) GetCursorStartPos() { ImGui.GetCursorStartPos(out float x, out float y); return (x, y); }
    public static (float X, float Y) GetCursorScreenPos() { ImGui.GetCursorScreenPos(out float x, out float y); return (x, y); }
    public static void SetCursorScreenPos(float x, float y) => ImGui.SetCursorScreenPos(x, y);
    public static (float W, float H) GetContentRegionAvail() { ImGui.GetContentRegionAvail(out float w, out float h); return (w, h); }

    public static void PushID(string id) => ImGui.PushIDStr(Utf8.Of(id));
    public static void PushID(int id) => ImGui.PushIDInt(id);
    public static void PushID(nint ptr) => ImGui.PushIDPtr((void*)ptr);
    public static void PopID() => ImGui.PopID();
    public static uint GetID(string id) => ImGui.GetIDStr(Utf8.Of(id));
    public static uint GetID(int id) => ImGui.GetIDInt(id);
    public static uint GetID(nint ptr) => ImGui.GetIDPtr((void*)ptr);

    public static void Text(string text) => ImGui.TextUnformatted(Utf8.Of(text), null);
    public static void TextColored(float r, float g, float b, float a, string text) => ImGui.TextColored(r, g, b, a, Utf8.Of(text));
    public static void TextColored(Vector4 c, string text) => ImGui.TextColored(c.X, c.Y, c.Z, c.W, Utf8.Of(text));
    public static void TextDisabled(string text) => ImGui.TextDisabled(Utf8.Of(text));
    public static void TextWrapped(string text) => ImGui.TextWrapped(Utf8.Of(text));
    public static void LabelText(string label, string text) => ImGui.LabelText(Utf8.Of(label), Utf8.Of(text));
    public static void BulletText(string text) => ImGui.BulletText(Utf8.Of(text));
    public static void Bullet() => ImGui.Bullet();
    public static (float W, float H) CalcTextSize(string text, bool hideAfterDoubleHash = false, float wrapWidth = -1f)
    {
        ImGui.CalcTextSize(out float w, out float h, Utf8.Of(text), null, Utf8.B(hideAfterDoubleHash), wrapWidth);
        return (w, h);
    }
    public static bool TextLink(string label) => ImGui.TextLink(Utf8.Of(label)) != 0;
    public static bool TextLinkOpenURL(string label, string url) => ImGui.TextLinkOpenURL(Utf8.Of(label), Utf8.Of(url)) != 0;

    public static void Value(string prefix, bool v) => ImGui.ValueBool(Utf8.Of(prefix), Utf8.B(v));
    public static void Value(string prefix, int v) => ImGui.ValueInt(Utf8.Of(prefix), v);
    public static void Value(string prefix, uint v) => ImGui.ValueUInt(Utf8.Of(prefix), v);
    public static void Value(string prefix, float v, string? fmt = null) => ImGui.ValueFloat(Utf8.Of(prefix), v, Utf8.Of(fmt));

    public static bool Button(string label, float w = 0f, float h = 0f) => ImGui.Button(Utf8.Of(label), w, h) != 0;
    public static bool SmallButton(string label) => ImGui.SmallButton(Utf8.Of(label)) != 0;
    public static bool InvisibleButton(string id, float w, float h, int flags = 0) => ImGui.InvisibleButton(Utf8.Of(id), w, h, flags) != 0;
    public static bool ArrowButton(string id, int dir) => ImGui.ArrowButton(Utf8.Of(id), dir) != 0;

    public static bool Checkbox(string label, ref bool value)
    {
        fixed (bool* p = &value) return ImGui.Checkbox(Utf8.Of(label), (byte*)p) != 0;
    }
    public static bool CheckboxFlags(string label, ref int flags, int flagsValue)
    {
        fixed (int* p = &flags) return ImGui.CheckboxFlagsInt(Utf8.Of(label), p, flagsValue) != 0;
    }
    public static bool CheckboxFlags(string label, ref uint flags, uint flagsValue)
    {
        fixed (uint* p = &flags) return ImGui.CheckboxFlagsUInt(Utf8.Of(label), p, flagsValue) != 0;
    }
    public static bool RadioButton(string label, bool active) => ImGui.RadioButtonBool(Utf8.Of(label), Utf8.B(active)) != 0;
    public static bool RadioButton(string label, ref int v, int vButton)
    {
        fixed (int* p = &v) return ImGui.RadioButtonInt(Utf8.Of(label), p, vButton) != 0;
    }
    public static void ProgressBar(float fraction, string? overlay = null, float w = -1f, float h = 0f)
        => ImGui.ProgressBar(fraction, w, h, Utf8.Of(overlay));

    public static void Image(ulong texId, float w, float h, float u0 = 0f, float v0 = 0f, float u1 = 1f, float v1 = 1f)
        => ImGui.Image(texId, w, h, u0, v0, u1, v1);

    public static void ImageWithBg(ulong texId, float w, float h, Vector4 bg = default, Vector4? tint = null,
                                   float u0 = 0f, float v0 = 0f, float u1 = 1f, float v1 = 1f)
    {
        var t = tint ?? Vector4.One;
        ImGui.ImageWithBg(texId, w, h, u0, v0, u1, v1, bg.X, bg.Y, bg.Z, bg.W, t.X, t.Y, t.Z, t.W);
    }
    public static bool ImageButton(string id, ulong texId, float w, float h, Vector4 bg = default, Vector4? tint = null,
                                   float u0 = 0f, float v0 = 0f, float u1 = 1f, float v1 = 1f)
    {
        var t = tint ?? Vector4.One;
        return ImGui.ImageButton(Utf8.Of(id), texId, w, h, u0, v0, u1, v1, bg.X, bg.Y, bg.Z, bg.W, t.X, t.Y, t.Z, t.W) != 0;
    }

    public static bool BeginCombo(string label, string preview, int flags = 0)
        => ImGui.BeginCombo(Utf8.Of(label), Utf8.Of(preview), flags) != 0;
    public static void EndCombo() => ImGui.EndCombo();
    public static bool Combo(string label, ref int current, string[] items, int popupMaxHeight = -1)
    {
        int total = 1;
        for (int i = 0; i < items.Length; i++) total += System.Text.Encoding.UTF8.GetByteCount(items[i]) + 1;

        Span<byte> buf = total <= 2048 ? stackalloc byte[total] : new byte[total];
        int pos = 0;
        for (int i = 0; i < items.Length; i++)
        {
            pos += System.Text.Encoding.UTF8.GetBytes(items[i], buf.Slice(pos));
            buf[pos++] = 0;
        }
        buf[pos] = 0;

        fixed (byte* pb = buf)
        fixed (int* pc = &current)
            return ImGui.ComboZeros(Utf8.Of(label), pc, pb, popupMaxHeight) != 0;
    }
    public static bool BeginListBox(string label, float w = 0f, float h = 0f) => ImGui.BeginListBox(Utf8.Of(label), w, h) != 0;
    public static void EndListBox() => ImGui.EndListBox();
    public static bool ListBox(string label, ref int current, string[] items, float w = 0f, float h = 0f)
    {
        bool changed = false;
        if (BeginListBox(label, w, h))
        {
            for (int i = 0; i < items.Length; i++)
            {
                PushID(i);
                if (Selectable(items[i], current == i)) { current = i; changed = true; }
                PopID();
            }
            EndListBox();
        }
        return changed;
    }
    public static bool Selectable(string label, bool selected = false, int flags = 0, float w = 0f, float h = 0f)
        => ImGui.Selectable(Utf8.Of(label), Utf8.B(selected), flags, w, h) != 0;
    public static bool Selectable(string label, ref bool selected, int flags = 0, float w = 0f, float h = 0f)
    {
        fixed (bool* p = &selected) return ImGui.SelectablePtr(Utf8.Of(label), (byte*)p, flags, w, h) != 0;
    }

    public static bool DragFloat(string label, ref float v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0)
    {
        fixed (float* p = &v) return ImGui.DragFloat(Utf8.Of(label), p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragFloat2(string label, ref Vector2 v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0)
    {
        fixed (Vector2* p = &v) return ImGui.DragFloat2(Utf8.Of(label), (float*)p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragFloat3(string label, ref Vector3 v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0)
    {
        fixed (Vector3* p = &v) return ImGui.DragFloat3(Utf8.Of(label), (float*)p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragFloat4(string label, ref Vector4 v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0)
    {
        fixed (Vector4* p = &v) return ImGui.DragFloat4(Utf8.Of(label), (float*)p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragInt(string label, ref int v, float speed = 1f, int min = 0, int max = 0, string? fmt = null, int flags = 0)
    {
        fixed (int* p = &v) return ImGui.DragInt(Utf8.Of(label), p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragInt2(string label, Span<int> v, float speed = 1f, int min = 0, int max = 0, string? fmt = null, int flags = 0)
    {
        Utf8.Need(v.Length, 2, nameof(DragInt2));
        fixed (int* p = v) return ImGui.DragInt2(Utf8.Of(label), p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragInt3(string label, Span<int> v, float speed = 1f, int min = 0, int max = 0, string? fmt = null, int flags = 0)
    {
        Utf8.Need(v.Length, 3, nameof(DragInt3));
        fixed (int* p = v) return ImGui.DragInt3(Utf8.Of(label), p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragInt4(string label, Span<int> v, float speed = 1f, int min = 0, int max = 0, string? fmt = null, int flags = 0)
    {
        Utf8.Need(v.Length, 4, nameof(DragInt4));
        fixed (int* p = v) return ImGui.DragInt4(Utf8.Of(label), p, speed, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool DragFloatRange2(string label, ref float curMin, ref float curMax, float speed = 1f,
                                       float min = 0f, float max = 0f, string? fmt = null, string? fmtMax = null, int flags = 0)
    {
        fixed (float* a = &curMin)
        fixed (float* b = &curMax)
            return ImGui.DragFloatRange2(Utf8.Of(label), a, b, speed, min, max, Utf8.Of(fmt), Utf8.Of(fmtMax), flags) != 0;
    }
    public static bool DragIntRange2(string label, ref int curMin, ref int curMax, float speed = 1f,
                                     int min = 0, int max = 0, string? fmt = null, string? fmtMax = null, int flags = 0)
    {
        fixed (int* a = &curMin)
        fixed (int* b = &curMax)
            return ImGui.DragIntRange2(Utf8.Of(label), a, b, speed, min, max, Utf8.Of(fmt), Utf8.Of(fmtMax), flags) != 0;
    }

    public static bool SliderFloat(string label, ref float v, float min, float max, string? fmt = null, int flags = 0)
    {
        fixed (float* p = &v) return ImGui.SliderFloat(Utf8.Of(label), p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderFloat2(string label, ref Vector2 v, float min, float max, string? fmt = null, int flags = 0)
    {
        fixed (Vector2* p = &v) return ImGui.SliderFloat2(Utf8.Of(label), (float*)p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderFloat3(string label, ref Vector3 v, float min, float max, string? fmt = null, int flags = 0)
    {
        fixed (Vector3* p = &v) return ImGui.SliderFloat3(Utf8.Of(label), (float*)p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderFloat4(string label, ref Vector4 v, float min, float max, string? fmt = null, int flags = 0)
    {
        fixed (Vector4* p = &v) return ImGui.SliderFloat4(Utf8.Of(label), (float*)p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderInt(string label, ref int v, int min, int max, string? fmt = null, int flags = 0)
    {
        fixed (int* p = &v) return ImGui.SliderInt(Utf8.Of(label), p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderInt2(string label, Span<int> v, int min, int max, string? fmt = null, int flags = 0)
    {
        Utf8.Need(v.Length, 2, nameof(SliderInt2));
        fixed (int* p = v) return ImGui.SliderInt2(Utf8.Of(label), p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderInt3(string label, Span<int> v, int min, int max, string? fmt = null, int flags = 0)
    {
        Utf8.Need(v.Length, 3, nameof(SliderInt3));
        fixed (int* p = v) return ImGui.SliderInt3(Utf8.Of(label), p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderInt4(string label, Span<int> v, int min, int max, string? fmt = null, int flags = 0)
    {
        Utf8.Need(v.Length, 4, nameof(SliderInt4));
        fixed (int* p = v) return ImGui.SliderInt4(Utf8.Of(label), p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool SliderAngle(string label, ref float rad, float minDeg = -360f, float maxDeg = 360f, string? fmt = null, int flags = 0)
    {
        fixed (float* p = &rad) return ImGui.SliderAngle(Utf8.Of(label), p, minDeg, maxDeg, Utf8.Of(fmt), flags) != 0;
    }
    public static bool VSliderFloat(string label, float w, float h, ref float v, float min, float max, string? fmt = null, int flags = 0)
    {
        fixed (float* p = &v) return ImGui.VSliderFloat(Utf8.Of(label), w, h, p, min, max, Utf8.Of(fmt), flags) != 0;
    }
    public static bool VSliderInt(string label, float w, float h, ref int v, int min, int max, string? fmt = null, int flags = 0)
    {
        fixed (int* p = &v) return ImGui.VSliderInt(Utf8.Of(label), w, h, p, min, max, Utf8.Of(fmt), flags) != 0;
    }

    public static bool InputText(string label, ref string value, int maxBytes = 256, int flags = 0)
    {
        maxBytes = Math.Max(2, maxBytes);
        Span<byte> buf = maxBytes <= 1024 ? stackalloc byte[maxBytes] : new byte[maxBytes];
        Utf8.WriteZ(value, buf);
        bool changed;
        fixed (byte* p = buf) changed = ImGui.InputText(Utf8.Of(label), p, (nuint)buf.Length, flags) != 0;
        if (changed) value = Utf8.ReadZ(buf);
        return changed;
    }
    public static bool InputTextWithHint(string label, string hint, ref string value, int maxBytes = 256, int flags = 0)
    {
        maxBytes = Math.Max(2, maxBytes);
        Span<byte> buf = maxBytes <= 1024 ? stackalloc byte[maxBytes] : new byte[maxBytes];
        Utf8.WriteZ(value, buf);
        bool changed;
        fixed (byte* p = buf) changed = ImGui.InputTextWithHint(Utf8.Of(label), Utf8.Of(hint), p, (nuint)buf.Length, flags) != 0;
        if (changed) value = Utf8.ReadZ(buf);
        return changed;
    }
    public static bool InputTextMultiline(string label, ref string value, float w = 0f, float h = 0f, int maxBytes = 4096, int flags = 0)
    {
        maxBytes = Math.Max(2, maxBytes);
        Span<byte> buf = maxBytes <= 1024 ? stackalloc byte[maxBytes] : new byte[maxBytes];
        Utf8.WriteZ(value, buf);
        bool changed;
        fixed (byte* p = buf) changed = ImGui.InputTextMultiline(Utf8.Of(label), p, (nuint)buf.Length, w, h, flags) != 0;
        if (changed) value = Utf8.ReadZ(buf);
        return changed;
    }
    public static bool InputFloat(string label, ref float v, float step = 0f, float fast = 0f, string? fmt = null, int flags = 0)
    {
        fixed (float* p = &v) return ImGui.InputFloat(Utf8.Of(label), p, step, fast, Utf8.Of(fmt), flags) != 0;
    }
    public static bool InputFloat2(string label, ref Vector2 v, string? fmt = null, int flags = 0)
    {
        fixed (Vector2* p = &v) return ImGui.InputFloat2(Utf8.Of(label), (float*)p, Utf8.Of(fmt), flags) != 0;
    }
    public static bool InputFloat3(string label, ref Vector3 v, string? fmt = null, int flags = 0)
    {
        fixed (Vector3* p = &v) return ImGui.InputFloat3(Utf8.Of(label), (float*)p, Utf8.Of(fmt), flags) != 0;
    }
    public static bool InputFloat4(string label, ref Vector4 v, string? fmt = null, int flags = 0)
    {
        fixed (Vector4* p = &v) return ImGui.InputFloat4(Utf8.Of(label), (float*)p, Utf8.Of(fmt), flags) != 0;
    }
    public static bool InputInt(string label, ref int v, int step = 1, int fast = 100, int flags = 0)
    {
        fixed (int* p = &v) return ImGui.InputInt(Utf8.Of(label), p, step, fast, flags) != 0;
    }
    public static bool InputInt2(string label, Span<int> v, int flags = 0)
    {
        Utf8.Need(v.Length, 2, nameof(InputInt2));
        fixed (int* p = v) return ImGui.InputInt2(Utf8.Of(label), p, flags) != 0;
    }
    public static bool InputInt3(string label, Span<int> v, int flags = 0)
    {
        Utf8.Need(v.Length, 3, nameof(InputInt3));
        fixed (int* p = v) return ImGui.InputInt3(Utf8.Of(label), p, flags) != 0;
    }
    public static bool InputInt4(string label, Span<int> v, int flags = 0)
    {
        Utf8.Need(v.Length, 4, nameof(InputInt4));
        fixed (int* p = v) return ImGui.InputInt4(Utf8.Of(label), p, flags) != 0;
    }
    public static bool InputDouble(string label, ref double v, double step = 0.0, double fast = 0.0, string? fmt = null, int flags = 0)
    {
        fixed (double* p = &v) return ImGui.InputDouble(Utf8.Of(label), p, step, fast, Utf8.Of(fmt), flags) != 0;
    }
    public static bool ColorEdit3(string label, ref Vector3 c, int flags = 0)
    {
        fixed (Vector3* p = &c) return ImGui.ColorEdit3(Utf8.Of(label), (float*)p, flags) != 0;
    }
    public static bool ColorEdit4(string label, ref Vector4 c, int flags = 0)
    {
        fixed (Vector4* p = &c) return ImGui.ColorEdit4(Utf8.Of(label), (float*)p, flags) != 0;
    }
    public static bool ColorPicker3(string label, ref Vector3 c, int flags = 0)
    {
        fixed (Vector3* p = &c) return ImGui.ColorPicker3(Utf8.Of(label), (float*)p, flags) != 0;
    }
    public static bool ColorPicker4(string label, ref Vector4 c, int flags = 0)
    {
        fixed (Vector4* p = &c) return ImGui.ColorPicker4(Utf8.Of(label), (float*)p, flags, null) != 0;
    }
    public static bool ColorButton(string id, Vector4 c, int flags = 0, float w = 0f, float h = 0f)
        => ImGui.ColorButton(Utf8.Of(id), c.X, c.Y, c.Z, c.W, flags, w, h) != 0;

    public static bool TreeNode(string label) => ImGui.TreeNode(Utf8.Of(label)) != 0;
    public static bool TreeNodeEx(string label, int flags) => ImGui.TreeNodeEx(Utf8.Of(label), flags) != 0;
    public static void TreePush(string id) => ImGui.TreePushStr(Utf8.Of(id));
    public static void TreePop() => ImGui.TreePop();
    public static float GetTreeNodeToLabelSpacing() => ImGui.GetTreeNodeToLabelSpacing();
    public static void SetNextItemOpen(bool open, int cond = 0) => ImGui.SetNextItemOpen(Utf8.B(open), cond);
    public static bool CollapsingHeader(string label, int flags = 0)
        => ImGui.CollapsingHeader(Utf8.Of(label), flags) != 0;
    public static bool CollapsingHeader(string label, ref bool open, int flags = 0)
    {
        fixed (bool* p = &open) return ImGui.CollapsingHeaderClosable(Utf8.Of(label), (byte*)p, flags) != 0;
    }

    public static void PlotLines(string label, ReadOnlySpan<float> values, int offset = 0, string? overlay = null,
                                 float min = float.MaxValue, float max = float.MaxValue, float w = 0f, float h = 0f)
    {
        fixed (float* p = values)
            ImGui.PlotLines(Utf8.Of(label), p, values.Length, offset, Utf8.Of(overlay), min, max, w, h, sizeof(float));
    }
    public static void PlotHistogram(string label, ReadOnlySpan<float> values, int offset = 0, string? overlay = null,
                                     float min = float.MaxValue, float max = float.MaxValue, float w = 0f, float h = 0f)
    {
        fixed (float* p = values)
            ImGui.PlotHistogram(Utf8.Of(label), p, values.Length, offset, Utf8.Of(overlay), min, max, w, h, sizeof(float));
    }

    public static bool BeginMenuBar() => ImGui.BeginMenuBar() != 0;
    public static void EndMenuBar() => ImGui.EndMenuBar();
    public static bool BeginMainMenuBar() => ImGui.BeginMainMenuBar() != 0;
    public static void EndMainMenuBar() => ImGui.EndMainMenuBar();
    public static bool BeginMenu(string label, bool enabled = true) => ImGui.BeginMenu(Utf8.Of(label), Utf8.B(enabled)) != 0;
    public static void EndMenu() => ImGui.EndMenu();
    public static bool MenuItem(string label, string? shortcut = null, bool selected = false, bool enabled = true)
        => ImGui.MenuItem(Utf8.Of(label), Utf8.Of(shortcut), Utf8.B(selected), Utf8.B(enabled)) != 0;
    public static bool MenuItem(string label, string? shortcut, ref bool selected, bool enabled = true)
    {
        fixed (bool* p = &selected) return ImGui.MenuItemPtr(Utf8.Of(label), Utf8.Of(shortcut), (byte*)p, Utf8.B(enabled)) != 0;
    }

    public static bool BeginTooltip() => ImGui.BeginTooltip() != 0;
    public static void EndTooltip() => ImGui.EndTooltip();
    public static void SetTooltip(string text) => ImGui.SetTooltip(Utf8.Of(text));
    public static bool BeginItemTooltip() => ImGui.BeginItemTooltip() != 0;
    public static void SetItemTooltip(string text) => ImGui.SetItemTooltip(Utf8.Of(text));

    public static bool BeginPopup(string id, int flags = 0) => ImGui.BeginPopup(Utf8.Of(id), flags) != 0;
    public static bool BeginPopupModal(string name, int flags = 0)
        => ImGui.BeginPopupModal(Utf8.Of(name), null, flags) != 0;
    public static bool BeginPopupModal(string name, ref bool open, int flags = 0)
    {
        fixed (bool* p = &open) return ImGui.BeginPopupModal(Utf8.Of(name), (byte*)p, flags) != 0;
    }
    public static void EndPopup() => ImGui.EndPopup();
    public static void OpenPopup(string id, int flags = 0) => ImGui.OpenPopup(Utf8.Of(id), flags);
    public static void OpenPopupOnItemClick(string? id = null, int flags = 1) => ImGui.OpenPopupOnItemClick(Utf8.Of(id), flags);
    public static void CloseCurrentPopup() => ImGui.CloseCurrentPopup();
    public static bool BeginPopupContextItem(string? id = null, int flags = 1) => ImGui.BeginPopupContextItem(Utf8.Of(id), flags) != 0;
    public static bool BeginPopupContextWindow(string? id = null, int flags = 1) => ImGui.BeginPopupContextWindow(Utf8.Of(id), flags) != 0;
    public static bool BeginPopupContextVoid(string? id = null, int flags = 1) => ImGui.BeginPopupContextVoid(Utf8.Of(id), flags) != 0;
    public static bool IsPopupOpen(string id, int flags = 0) => ImGui.IsPopupOpen(Utf8.Of(id), flags) != 0;

    public static bool BeginTable(string id, int columns, int flags = 0, float w = 0f, float h = 0f, float innerWidth = 0f)
        => ImGui.BeginTable(Utf8.Of(id), columns, flags, w, h, innerWidth) != 0;
    public static void EndTable() => ImGui.EndTable();
    public static void TableNextRow(int flags = 0, float minHeight = 0f) => ImGui.TableNextRow(flags, minHeight);
    public static bool TableNextColumn() => ImGui.TableNextColumn() != 0;
    public static bool TableSetColumnIndex(int n) => ImGui.TableSetColumnIndex(n) != 0;
    public static void TableSetupColumn(string label, int flags = 0, float initWidth = 0f, uint userId = 0)
        => ImGui.TableSetupColumn(Utf8.Of(label), flags, initWidth, userId);
    public static void TableSetupScrollFreeze(int cols, int rows) => ImGui.TableSetupScrollFreeze(cols, rows);
    public static void TableHeader(string label) => ImGui.TableHeader(Utf8.Of(label));
    public static void TableHeadersRow() => ImGui.TableHeadersRow();
    public static void TableAngledHeadersRow() => ImGui.TableAngledHeadersRow();
    public static int TableGetColumnCount() => ImGui.TableGetColumnCount();
    public static int TableGetColumnIndex() => ImGui.TableGetColumnIndex();
    public static int TableGetRowIndex() => ImGui.TableGetRowIndex();
    public static string? TableGetColumnName(int n = -1) => Utf8.FromPtr(ImGui.TableGetColumnName(n));
    public static int TableGetColumnFlags(int n = -1) => ImGui.TableGetColumnFlags(n);
    public static void TableSetColumnEnabled(int n, bool enabled) => ImGui.TableSetColumnEnabled(n, Utf8.B(enabled));
    public static int TableGetHoveredColumn() => ImGui.TableGetHoveredColumn();
    public static void TableSetBgColor(int target, uint color, int column = -1) => ImGui.TableSetBgColor(target, color, column);

    public static void Columns(int count = 1, string? id = null, bool borders = true) => ImGui.Columns(count, Utf8.Of(id), Utf8.B(borders));
    public static void NextColumn() => ImGui.NextColumn();
    public static int GetColumnIndex() => ImGui.GetColumnIndex();
    public static int GetColumnsCount() => ImGui.GetColumnsCount();
    public static float GetColumnWidth(int index = -1) => ImGui.GetColumnWidth(index);
    public static void SetColumnWidth(int index, float width) => ImGui.SetColumnWidth(index, width);
    public static float GetColumnOffset(int index = -1) => ImGui.GetColumnOffset(index);
    public static void SetColumnOffset(int index, float offsetX) => ImGui.SetColumnOffset(index, offsetX);

    public static bool BeginTabBar(string id, int flags = 0) => ImGui.BeginTabBar(Utf8.Of(id), flags) != 0;
    public static void EndTabBar() => ImGui.EndTabBar();
    public static bool BeginTabItem(string label, int flags = 0)
        => ImGui.BeginTabItem(Utf8.Of(label), null, flags) != 0;
    public static bool BeginTabItem(string label, ref bool open, int flags = 0)
    {
        fixed (bool* p = &open) return ImGui.BeginTabItem(Utf8.Of(label), (byte*)p, flags) != 0;
    }
    public static void EndTabItem() => ImGui.EndTabItem();
    public static bool TabItemButton(string label, int flags = 0) => ImGui.TabItemButton(Utf8.Of(label), flags) != 0;
    public static void SetTabItemClosed(string label) => ImGui.SetTabItemClosed(Utf8.Of(label));

    public static bool BeginDragDropSource(int flags = 0) => ImGui.BeginDragDropSource(flags) != 0;
    public static void EndDragDropSource() => ImGui.EndDragDropSource();
    public static bool BeginDragDropTarget() => ImGui.BeginDragDropTarget() != 0;
    public static void EndDragDropTarget() => ImGui.EndDragDropTarget();
    public static bool SetDragDropPayload<T>(string type, ref T data, int cond = 0) where T : unmanaged
    {
        fixed (T* p = &data) return ImGui.SetDragDropPayload(Utf8.Of(type), p, (nuint)sizeof(T), cond) != 0;
    }
    public static bool AcceptDragDropPayload<T>(string type, out T value, int flags = 0) where T : unmanaged
    {
        value = default;
        nint payload = ImGui.AcceptDragDropPayload(Utf8.Of(type), flags);
        if (payload == 0) return false;
        void* data = *(void**)payload;
        int size = *(int*)((byte*)payload + sizeof(void*));
        if (data == null || size < sizeof(T)) return false;
        value = *(T*)data;
        return true;
    }

    public static void BeginDisabled(bool disabled = true) => ImGui.BeginDisabled(Utf8.B(disabled));
    public static void EndDisabled() => ImGui.EndDisabled();
    public static void PushClipRect(float minX, float minY, float maxX, float maxY, bool intersect = true)
        => ImGui.PushClipRect(minX, minY, maxX, maxY, Utf8.B(intersect));
    public static void PopClipRect() => ImGui.PopClipRect();

    public static void SetItemDefaultFocus() => ImGui.SetItemDefaultFocus();
    public static void SetKeyboardFocusHere(int offset = 0) => ImGui.SetKeyboardFocusHere(offset);
    public static void SetNavCursorVisible(bool visible) => ImGui.SetNavCursorVisible(Utf8.B(visible));
    public static void SetNextItemAllowOverlap() => ImGui.SetNextItemAllowOverlap();
    public static void SetNextItemShortcut(int chord, int flags = 0) => ImGui.SetNextItemShortcut(chord, flags);
    public static bool SetItemKeyOwner(int key) => ImGui.SetItemKeyOwner(key) != 0;

    public static bool IsItemHovered(int flags = 0) => ImGui.IsItemHovered(flags) != 0;
    public static bool IsItemActive() => ImGui.IsItemActive() != 0;
    public static bool IsItemFocused() => ImGui.IsItemFocused() != 0;
    public static bool IsItemClicked(int btn = 0) => ImGui.IsItemClicked(btn) != 0;
    public static bool IsItemVisible() => ImGui.IsItemVisible() != 0;
    public static bool IsItemEdited() => ImGui.IsItemEdited() != 0;
    public static bool IsItemActivated() => ImGui.IsItemActivated() != 0;
    public static bool IsItemDeactivated() => ImGui.IsItemDeactivated() != 0;
    public static bool IsItemDeactivatedAfterEdit() => ImGui.IsItemDeactivatedAfterEdit() != 0;
    public static bool IsItemToggledOpen() => ImGui.IsItemToggledOpen() != 0;
    public static bool IsAnyItemHovered() => ImGui.IsAnyItemHovered() != 0;
    public static bool IsAnyItemActive() => ImGui.IsAnyItemActive() != 0;
    public static bool IsAnyItemFocused() => ImGui.IsAnyItemFocused() != 0;
    public static uint GetItemID() => ImGui.GetItemID();
    public static int GetItemFlags() => ImGui.GetItemFlags();
    public static (float X, float Y) GetItemRectMin() { ImGui.GetItemRectMin(out float x, out float y); return (x, y); }
    public static (float X, float Y) GetItemRectMax() { ImGui.GetItemRectMax(out float x, out float y); return (x, y); }
    public static (float W, float H) GetItemRectSize() { ImGui.GetItemRectSize(out float w, out float h); return (w, h); }

    public static bool IsRectVisible(float w, float h) => ImGui.IsRectVisible(w, h) != 0;
    public static double GetTime() => ImGui.GetTime();
    public static int GetFrameCount() => ImGui.GetFrameCount();
}
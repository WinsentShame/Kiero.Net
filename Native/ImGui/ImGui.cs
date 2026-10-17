using System.Runtime.InteropServices;

namespace Kiero.Net;

public static unsafe class ImGui
{
    private const string Shim = "kiero_native";

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CreateContext")]
    public static extern nint CreateContext(nint sharedAtlas);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DestroyContext")]
    public static extern void DestroyContext(nint ctx);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetCurrentContext")]
    public static extern nint GetCurrentContext();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetCurrentContext")]
    public static extern void SetCurrentContext(nint ctx);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetIO")]
    public static extern nint GetIO();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetPlatformIO")]
    public static extern nint GetPlatformIO();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetStyle")]
    public static extern nint GetStyle();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_SetDisplaySize")]
    public static extern void IOSetDisplaySize(nint io, float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_GetDisplaySize")]
    public static extern void IOGetDisplaySize(nint io, out float w, out float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_SetDeltaTime")]
    public static extern void IOSetDeltaTime(nint io, float dt);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_AddMousePos")]
    public static extern void IOAddMousePos(nint io, float x, float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_AddMouseButton")]
    public static extern void IOAddMouseButton(nint io, int btn, byte down);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_AddMouseWheel")]
    public static extern void IOAddMouseWheel(nint io, float wx, float wy);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_AddKey")]
    public static extern void IOAddKey(nint io, int key, byte down);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_AddInputCharacter")]
    public static extern void IOAddInputCharacter(nint io, uint c);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_AddFocusEvent")]
    public static extern void IOAddFocusEvent(nint io, byte focused);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_WantCaptureMouse")]
    public static extern byte IOWantCaptureMouse(nint io);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_WantCaptureKeyboard")]
    public static extern byte IOWantCaptureKeyboard(nint io);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_SetConfigFlags")]
    public static extern void IOSetConfigFlags(nint io, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_SetIniFilename")]
    public static extern void IOSetIniFilename(nint io, byte* name);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IO_SetLogFilename")]
    public static extern void IOSetLogFilename(nint io, byte* name);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_NewFrame")]
    public static extern void NewFrame();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndFrame")]
    public static extern void EndFrame();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Render")]
    public static extern void Render();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetDrawData")]
    public static extern nint GetDrawData();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DrawData_GetVtxCount")]
    public static extern int DrawDataVtxCount(nint d);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DrawData_GetIdxCount")]
    public static extern int DrawDataIdxCount(nint d);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DrawData_GetCmdListsCount")]
    public static extern int DrawDataCmdListsCount(nint d);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowDemoWindow")]
    public static extern void ShowDemoWindow(byte* pOpen);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowMetricsWindow")]
    public static extern void ShowMetricsWindow(byte* pOpen);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowDebugLogWindow")]
    public static extern void ShowDebugLogWindow(byte* pOpen);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowIDStackToolWindow")]
    public static extern void ShowIDStackToolWindow(byte* pOpen);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowAboutWindow")]
    public static extern void ShowAboutWindow(byte* pOpen);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowStyleEditor")]
    public static extern void ShowStyleEditor(nint styleRef);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowStyleSelector")]
    public static extern byte ShowStyleSelector(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowFontSelector")]
    public static extern void ShowFontSelector(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ShowUserGuide")]
    public static extern void ShowUserGuide();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetVersion")]
    public static extern byte* GetVersion();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_StyleColorsDark")]
    public static extern void StyleColorsDark(nint dst);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_StyleColorsLight")]
    public static extern void StyleColorsLight(nint dst);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_StyleColorsClassic")]
    public static extern void StyleColorsClassic(nint dst);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Begin")]
    public static extern byte Begin(byte* name, byte* pOpen, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_End")]
    public static extern void End();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginChild")]
    public static extern byte BeginChild(byte* strId, float w, float h, int childFlags, int windowFlags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginChildID")]
    public static extern byte BeginChildID(uint id, float w, float h, int childFlags, int windowFlags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndChild")]
    public static extern void EndChild();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsWindowAppearing")]
    public static extern byte IsWindowAppearing();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsWindowCollapsed")]
    public static extern byte IsWindowCollapsed();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsWindowFocused")]
    public static extern byte IsWindowFocused(int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsWindowHovered")]
    public static extern byte IsWindowHovered(int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowDrawList")]
    public static extern nint GetWindowDrawList();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowPos")]
    public static extern void GetWindowPos(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowSize")]
    public static extern void GetWindowSize(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowWidth")]
    public static extern float GetWindowWidth();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowHeight")]
    public static extern float GetWindowHeight();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowPos")]
    public static extern void SetNextWindowPos(float x, float y, int cond, float px, float py);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowSize")]
    public static extern void SetNextWindowSize(float w, float h, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowSizeConstraints")]
    public static extern void SetNextWindowSizeConstraints(float minX, float minY, float maxX, float maxY);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowContentSize")]
    public static extern void SetNextWindowContentSize(float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowCollapsed")]
    public static extern void SetNextWindowCollapsed(byte collapsed, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowFocus")]
    public static extern void SetNextWindowFocus();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowBgAlpha")]
    public static extern void SetNextWindowBgAlpha(float alpha);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetScrollX")]
    public static extern float GetScrollX();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetScrollY")]
    public static extern float GetScrollY();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetScrollX")]
    public static extern void SetScrollX(float x);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetScrollY")]
    public static extern void SetScrollY(float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetScrollMaxX")]
    public static extern float GetScrollMaxX();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetScrollMaxY")]
    public static extern float GetScrollMaxY();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetScrollHereX")]
    public static extern void SetScrollHereX(float r);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetScrollHereY")]
    public static extern void SetScrollHereY(float r);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetScrollFromPosX")]
    public static extern void SetScrollFromPosX(float x, float r);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetScrollFromPosY")]
    public static extern void SetScrollFromPosY(float y, float r);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushFont")]
    public static extern void PushFont(nint font, float size);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopFont")]
    public static extern void PopFont();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFont")]
    public static extern nint GetFont();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFontSize")]
    public static extern float GetFontSize();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushStyleColorU32")]
    public static extern void PushStyleColorU32(int idx, uint col);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushStyleColorVec4")]
    public static extern void PushStyleColorVec4(int idx, float r, float g, float b, float a);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopStyleColor")]
    public static extern void PopStyleColor(int count);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushStyleVar")]
    public static extern void PushStyleVar(int idx, float val);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushStyleVarVec2")]
    public static extern void PushStyleVarVec2(int idx, float x, float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopStyleVar")]
    public static extern void PopStyleVar(int count);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetColorU32")]
    public static extern uint GetColorU32(int idx, float alpha);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushItemWidth")]
    public static extern void PushItemWidth(float w);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopItemWidth")]
    public static extern void PopItemWidth();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextItemWidth")]
    public static extern void SetNextItemWidth(float w);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CalcItemWidth")]
    public static extern float CalcItemWidth();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushTextWrapPos")]
    public static extern void PushTextWrapPos(float x);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopTextWrapPos")]
    public static extern void PopTextWrapPos();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Separator")]
    public static extern void Separator();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SeparatorText")]
    public static extern void SeparatorText(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SameLine")]
    public static extern void SameLine(float offset, float spacing);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_NewLine")]
    public static extern void NewLine();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Spacing")]
    public static extern void Spacing();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Dummy")]
    public static extern void Dummy(float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Indent")]
    public static extern void Indent(float w);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Unindent")]
    public static extern void Unindent(float w);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginGroup")]
    public static extern void BeginGroup();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndGroup")]
    public static extern void EndGroup();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_AlignTextToFramePadding")]
    public static extern void AlignTextToFramePadding();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetTextLineHeight")]
    public static extern float GetTextLineHeight();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetTextLineHeightWithSpacing")]
    public static extern float GetTextLineHeightWithSpacing();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFrameHeight")]
    public static extern float GetFrameHeight();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFrameHeightWithSpacing")]
    public static extern float GetFrameHeightWithSpacing();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetCursorPos")]
    public static extern void GetCursorPos(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetCursorPosX")]
    public static extern float GetCursorPosX();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetCursorPosY")]
    public static extern float GetCursorPosY();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetCursorPos")]
    public static extern void SetCursorPos(float x, float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetCursorPosX")]
    public static extern void SetCursorPosX(float x);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetCursorPosY")]
    public static extern void SetCursorPosY(float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetCursorStartPos")]
    public static extern void GetCursorStartPos(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetCursorScreenPos")]
    public static extern void GetCursorScreenPos(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetCursorScreenPos")]
    public static extern void SetCursorScreenPos(float x, float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetContentRegionAvail")]
    public static extern void GetContentRegionAvail(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushIDStr")]
    public static extern void PushIDStr(byte* id);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushIDInt")]
    public static extern void PushIDInt(int id);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushIDPtr")]
    public static extern void PushIDPtr(void* p);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopID")]
    public static extern void PopID();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetIDStr")]
    public static extern uint GetIDStr(byte* id);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetIDInt")]
    public static extern uint GetIDInt(int id);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TextUnformatted")]
    public static extern void TextUnformatted(byte* text, byte* textEnd);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TextColored")]
    public static extern void TextColored(float r, float g, float b, float a, byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TextDisabled")]
    public static extern void TextDisabled(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TextWrapped")]
    public static extern void TextWrapped(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LabelText")]
    public static extern void LabelText(byte* label, byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BulletText")]
    public static extern void BulletText(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Bullet")]
    public static extern void Bullet();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Button")]
    public static extern byte Button(byte* label, float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SmallButton")]
    public static extern byte SmallButton(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InvisibleButton")]
    public static extern byte InvisibleButton(byte* id, float w, float h, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ArrowButton")]
    public static extern byte ArrowButton(byte* id, int dir);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Checkbox")]
    public static extern byte Checkbox(byte* label, byte* v);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_RadioButtonInt")]
    public static extern byte RadioButtonInt(byte* label, int* v, int vButton);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ProgressBar")]
    public static extern void ProgressBar(float fraction, float w, float h, byte* overlay);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TextLink")]
    public static extern byte TextLink(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TextLinkOpenURL")]
    public static extern byte TextLinkOpenURL(byte* label, byte* url);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginCombo")]
    public static extern byte BeginCombo(byte* label, byte* preview, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndCombo")]
    public static extern void EndCombo();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragFloat")]
    public static extern byte DragFloat(byte* label, float* v, float speed, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragFloat2")]
    public static extern byte DragFloat2(byte* label, float* v, float speed, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragFloat3")]
    public static extern byte DragFloat3(byte* label, float* v, float speed, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragFloat4")]
    public static extern byte DragFloat4(byte* label, float* v, float speed, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragInt")]
    public static extern byte DragInt(byte* label, int* v, float speed, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragInt2")]
    public static extern byte DragInt2(byte* label, int* v, float speed, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragInt3")]
    public static extern byte DragInt3(byte* label, int* v, float speed, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragInt4")]
    public static extern byte DragInt4(byte* label, int* v, float speed, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderFloat")]
    public static extern byte SliderFloat(byte* label, float* v, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderFloat2")]
    public static extern byte SliderFloat2(byte* label, float* v, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderFloat3")]
    public static extern byte SliderFloat3(byte* label, float* v, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderFloat4")]
    public static extern byte SliderFloat4(byte* label, float* v, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderInt")]
    public static extern byte SliderInt(byte* label, int* v, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderInt2")]
    public static extern byte SliderInt2(byte* label, int* v, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderInt3")]
    public static extern byte SliderInt3(byte* label, int* v, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderInt4")]
    public static extern byte SliderInt4(byte* label, int* v, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderAngle")]
    public static extern byte SliderAngle(byte* label, float* vRad, float minDeg, float maxDeg, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_VSliderFloat")]
    public static extern byte VSliderFloat(byte* label, float w, float h, float* v, float min, float max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_VSliderInt")]
    public static extern byte VSliderInt(byte* label, float w, float h, int* v, int min, int max, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputText")]
    public static extern byte InputText(byte* label, byte* buf, nuint size, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputTextMultiline")]
    public static extern byte InputTextMultiline(byte* label, byte* buf, nuint size, float w, float h, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputFloat")]
    public static extern byte InputFloat(byte* label, float* v, float step, float fast, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputInt")]
    public static extern byte InputInt(byte* label, int* v, int step, int fast, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputDouble")]
    public static extern byte InputDouble(byte* label, double* v, double step, double fast, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorEdit3")]
    public static extern byte ColorEdit3(byte* label, float* col, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorEdit4")]
    public static extern byte ColorEdit4(byte* label, float* col, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorPicker3")]
    public static extern byte ColorPicker3(byte* label, float* col, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorPicker4")]
    public static extern byte ColorPicker4(byte* label, float* col, int flags, float* refCol);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorButton")]
    public static extern byte ColorButton(byte* id, float r, float g, float b, float a, int flags, float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNode")]
    public static extern byte TreeNode(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNodeEx")]
    public static extern byte TreeNodeEx(byte* label, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreePop")]
    public static extern void TreePop();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetTreeNodeToLabelSpacing")]
    public static extern float GetTreeNodeToLabelSpacing();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CollapsingHeader")]
    public static extern byte CollapsingHeader(byte* label, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CollapsingHeaderClosable")]
    public static extern byte CollapsingHeaderClosable(byte* label, byte* pOpen, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextItemOpen")]
    public static extern void SetNextItemOpen(byte isOpen, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Selectable")]
    public static extern byte Selectable(byte* label, byte selected, int flags, float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SelectablePtr")]
    public static extern byte SelectablePtr(byte* label, byte* pSelected, int flags, float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginListBox")]
    public static extern byte BeginListBox(byte* label, float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndListBox")]
    public static extern void EndListBox();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PlotLines")]
    public static extern void PlotLines(byte* label, float* values, int count, int offset, byte* overlay, float min, float max, float w, float h, int stride);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PlotHistogram")]
    public static extern void PlotHistogram(byte* label, float* values, int count, int offset, byte* overlay, float min, float max, float w, float h, int stride);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ValueBool")]
    public static extern void ValueBool(byte* prefix, byte b);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ValueInt")]
    public static extern void ValueInt(byte* prefix, int v);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ValueUInt")]
    public static extern void ValueUInt(byte* prefix, uint v);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ValueFloat")]
    public static extern void ValueFloat(byte* prefix, float v, byte* fmt);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginMenuBar")]
    public static extern byte BeginMenuBar();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndMenuBar")]
    public static extern void EndMenuBar();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginMainMenuBar")]
    public static extern byte BeginMainMenuBar();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndMainMenuBar")]
    public static extern void EndMainMenuBar();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginMenu")]
    public static extern byte BeginMenu(byte* label, byte enabled);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndMenu")]
    public static extern void EndMenu();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_MenuItem")]
    public static extern byte MenuItem(byte* label, byte* shortcut, byte selected, byte enabled);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_MenuItemPtr")]
    public static extern byte MenuItemPtr(byte* label, byte* shortcut, byte* pSelected, byte enabled);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginTooltip")]
    public static extern byte BeginTooltip();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndTooltip")]
    public static extern void EndTooltip();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetTooltip")]
    public static extern void SetTooltip(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginItemTooltip")]
    public static extern byte BeginItemTooltip();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetItemTooltip")]
    public static extern void SetItemTooltip(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginPopup")]
    public static extern byte BeginPopup(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginPopupModal")]
    public static extern byte BeginPopupModal(byte* name, byte* pOpen, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndPopup")]
    public static extern void EndPopup();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_OpenPopup")]
    public static extern byte OpenPopup(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_OpenPopupOnItemClick")]
    public static extern byte OpenPopupOnItemClick(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CloseCurrentPopup")]
    public static extern void CloseCurrentPopup();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginPopupContextItem")]
    public static extern byte BeginPopupContextItem(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginPopupContextWindow")]
    public static extern byte BeginPopupContextWindow(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginPopupContextVoid")]
    public static extern byte BeginPopupContextVoid(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsPopupOpen")]
    public static extern byte IsPopupOpen(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginTable")]
    public static extern byte BeginTable(byte* id, int cols, int flags, float w, float h, float innerW);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndTable")]
    public static extern void EndTable();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableNextRow")]
    public static extern void TableNextRow(int flags, float minH);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableNextColumn")]
    public static extern byte TableNextColumn();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableSetColumnIndex")]
    public static extern byte TableSetColumnIndex(int n);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableSetupColumn")]
    public static extern void TableSetupColumn(byte* label, int flags, float initW, uint userData);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableSetupScrollFreeze")]
    public static extern void TableSetupScrollFreeze(int cols, int rows);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableHeader")]
    public static extern void TableHeader(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableHeadersRow")]
    public static extern void TableHeadersRow();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableAngledHeadersRow")]
    public static extern void TableAngledHeadersRow();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetColumnCount")]
    public static extern int TableGetColumnCount();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetColumnIndex")]
    public static extern int TableGetColumnIndex();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetRowIndex")]
    public static extern int TableGetRowIndex();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetColumnName")]
    public static extern byte* TableGetColumnName(int n);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableSetBgColor")]
    public static extern void TableSetBgColor(int target, uint color, int col);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginTabBar")]
    public static extern byte BeginTabBar(byte* id, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndTabBar")]
    public static extern void EndTabBar();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginTabItem")]
    public static extern byte BeginTabItem(byte* label, byte* pOpen, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndTabItem")]
    public static extern void EndTabItem();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TabItemButton")]
    public static extern byte TabItemButton(byte* label, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetTabItemClosed")]
    public static extern void SetTabItemClosed(byte* label);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowDpiScale")]
    public static extern float GetWindowDpiScale();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowViewport")]
    public static extern nint GetWindowViewport();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowScroll")]
    public static extern void SetNextWindowScroll(float x, float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowViewport")]
    public static extern void SetNextWindowViewport(uint viewportId);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetWindowPosStr")]
    public static extern void SetWindowPosStr(byte* name, float x, float y, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetWindowSizeStr")]
    public static extern void SetWindowSizeStr(byte* name, float w, float h, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetWindowCollapsedStr")]
    public static extern void SetWindowCollapsedStr(byte* name, byte collapsed, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetWindowFocusStr")]
    public static extern void SetWindowFocusStr(byte* name);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFontBaked")]
    public static extern nint GetFontBaked();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushStyleVarX")]
    public static extern void PushStyleVarX(int idx, float valX);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushStyleVarY")]
    public static extern void PushStyleVarY(int idx, float valY);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushItemFlag")]
    public static extern void PushItemFlag(int option, byte enabled);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopItemFlag")]
    public static extern void PopItemFlag();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFontTexUvWhitePixel")]
    public static extern void GetFontTexUvWhitePixel(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetStyleColorVec4")]
    public static extern void GetStyleColorVec4(int idx, float* rgba);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetColorU32Vec4")]
    public static extern uint GetColorU32Vec4(float r, float g, float b, float a);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushIDStrRange")]
    public static extern void PushIDStrRange(byte* begin, byte* end);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetIDStrRange")]
    public static extern uint GetIDStrRange(byte* begin, byte* end);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetIDPtr")]
    public static extern uint GetIDPtr(void* p);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Text")]
    public static extern void Text(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CheckboxFlagsInt")]
    public static extern byte CheckboxFlagsInt(byte* label, int* flags, int flagsValue);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CheckboxFlagsUInt")]
    public static extern byte CheckboxFlagsUInt(byte* label, uint* flags, uint flagsValue);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_RadioButtonBool")]
    public static extern byte RadioButtonBool(byte* label, byte active);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Image")]
    public static extern void Image(ulong texId, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImageWithBg")]
    public static extern void ImageWithBg(ulong texId, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y,
                                           float bgr, float bgg, float bgb, float bga, float tr, float tg, float tb, float ta);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImageButton")]
    public static extern byte ImageButton(byte* strId, ulong texId, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y,
                                           float bgr, float bgg, float bgb, float bga, float tr, float tg, float tb, float ta);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ComboZeros")]
    public static extern byte ComboZeros(byte* label, int* currentItem, byte* itemsSeparatedByZeros, int popupMaxHeight);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragFloatRange2")]
    public static extern byte DragFloatRange2(byte* label, float* curMin, float* curMax, float speed, float vMin, float vMax, byte* fmt, byte* fmtMax, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragIntRange2")]
    public static extern byte DragIntRange2(byte* label, int* curMin, int* curMax, float speed, int vMin, int vMax, byte* fmt, byte* fmtMax, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragScalar")]
    public static extern byte DragScalar(byte* label, int dataType, void* pData, float speed, void* pMin, void* pMax, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DragScalarN")]
    public static extern byte DragScalarN(byte* label, int dataType, void* pData, int components, float speed, void* pMin, void* pMax, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderScalar")]
    public static extern byte SliderScalar(byte* label, int dataType, void* pData, void* pMin, void* pMax, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SliderScalarN")]
    public static extern byte SliderScalarN(byte* label, int dataType, void* pData, int components, void* pMin, void* pMax, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_VSliderScalar")]
    public static extern byte VSliderScalar(byte* label, float w, float h, int dataType, void* pData, void* pMin, void* pMax, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputTextWithHint")]
    public static extern byte InputTextWithHint(byte* label, byte* hint, byte* buf, nuint size, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputFloat2")]
    public static extern byte InputFloat2(byte* label, float* v, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputFloat3")]
    public static extern byte InputFloat3(byte* label, float* v, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputFloat4")]
    public static extern byte InputFloat4(byte* label, float* v, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputInt2")]
    public static extern byte InputInt2(byte* label, int* v, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputInt3")]
    public static extern byte InputInt3(byte* label, int* v, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputInt4")]
    public static extern byte InputInt4(byte* label, int* v, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputScalar")]
    public static extern byte InputScalar(byte* label, int dataType, void* pData, void* pStep, void* pStepFast, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_InputScalarN")]
    public static extern byte InputScalarN(byte* label, int dataType, void* pData, int components, void* pStep, void* pStepFast, byte* fmt, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNodeStrId")]
    public static extern byte TreeNodeStrId(byte* strId, byte* fmt);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNodePtrId")]
    public static extern byte TreeNodePtrId(void* ptrId, byte* fmt);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNodeExStrId")]
    public static extern byte TreeNodeExStrId(byte* strId, int flags, byte* fmt);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNodeExPtrId")]
    public static extern byte TreeNodeExPtrId(void* ptrId, int flags, byte* fmt);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreePushStr")]
    public static extern void TreePushStr(byte* strId);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreePushPtr")]
    public static extern void TreePushPtr(void* ptrId);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextItemStorageID")]
    public static extern void SetNextItemStorageID(uint storageId);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TreeNodeGetOpen")]
    public static extern byte TreeNodeGetOpen(uint storageId);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginMultiSelect")]
    public static extern nint BeginMultiSelect(int flags, int selectionSize, int itemsCount);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndMultiSelect")]
    public static extern nint EndMultiSelect();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextItemSelectionUserData")]
    public static extern void SetNextItemSelectionUserData(long selectionUserData);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemToggledSelection")]
    public static extern byte IsItemToggledSelection();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ListBoxArray")]
    public static extern byte ListBoxArray(byte* label, int* currentItem, byte** items, int itemsCount, int heightInItems);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetSortSpecs")]
    public static extern nint TableGetSortSpecs();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetColumnFlags")]
    public static extern int TableGetColumnFlags(int columnN);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableSetColumnEnabled")]
    public static extern void TableSetColumnEnabled(int columnN, byte v);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_TableGetHoveredColumn")]
    public static extern int TableGetHoveredColumn();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Columns")]
    public static extern void Columns(int count, byte* id, byte borders);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_NextColumn")]
    public static extern void NextColumn();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetColumnIndex")]
    public static extern int GetColumnIndex();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetColumnWidth")]
    public static extern float GetColumnWidth(int columnIndex);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetColumnWidth")]
    public static extern void SetColumnWidth(int columnIndex, float width);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetColumnOffset")]
    public static extern float GetColumnOffset(int columnIndex);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetColumnOffset")]
    public static extern void SetColumnOffset(int columnIndex, float offsetX);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetColumnsCount")]
    public static extern int GetColumnsCount();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DockSpace")]
    public static extern uint DockSpace(uint dockspaceId, float w, float h, int flags, void* windowClass);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DockSpaceOverViewport")]
    public static extern uint DockSpaceOverViewport(uint dockspaceId, void* viewport, int flags, void* windowClass);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowDockID")]
    public static extern void SetNextWindowDockID(uint dockId, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextWindowClass")]
    public static extern void SetNextWindowClass(void* windowClass);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetWindowDockID")]
    public static extern uint GetWindowDockID();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsWindowDocked")]
    public static extern byte IsWindowDocked();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetItemFlags")]
    public static extern int GetItemFlags();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetItemClickedCountWithSingleClickDelay")]
    public static extern int GetItemClickedCountWithSingleClickDelay(int btn, float delay);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetDrawListSharedData")]
    public static extern nint GetDrawListSharedData();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetStateStorage")]
    public static extern void SetStateStorage(void* storage);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetStateStorage")]
    public static extern nint GetStateStorage();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorConvertU32ToFloat4")]
    public static extern void ColorConvertU32ToFloat4(uint @in, float* outRgba);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorConvertFloat4ToU32")]
    public static extern uint ColorConvertFloat4ToU32(float r, float g, float b, float a);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorConvertRGBtoHSV")]
    public static extern void ColorConvertRGBtoHSV(float r, float g, float b, out float h, out float s, out float v);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ColorConvertHSVtoRGB")]
    public static extern void ColorConvertHSVtoRGB(float h, float s, float v, out float r, out float g, out float b);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsKeyChordPressed")]
    public static extern byte IsKeyChordPressed(int keyChord);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextFrameWantCaptureKeyboard")]
    public static extern void SetNextFrameWantCaptureKeyboard(byte want);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_Shortcut")]
    public static extern byte Shortcut(int keyChord, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextItemShortcut")]
    public static extern void SetNextItemShortcut(int keyChord, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetItemKeyOwner")]
    public static extern byte SetItemKeyOwner(int key);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseReleasedWithDelay")]
    public static extern byte IsMouseReleasedWithDelay(int btn, float delay);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetMouseClickedCount")]
    public static extern int GetMouseClickedCount(int btn);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMousePosValid")]
    public static extern byte IsMousePosValid(float* pos);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsAnyMouseDown")]
    public static extern byte IsAnyMouseDown();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetMousePosOnOpeningCurrentPopup")]
    public static extern void GetMousePosOnOpeningCurrentPopup(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextFrameWantCaptureMouse")]
    public static extern void SetNextFrameWantCaptureMouse(byte want);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DebugTextEncoding")]
    public static extern void DebugTextEncoding(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DebugFlashStyleColor")]
    public static extern void DebugFlashStyleColor(int idx);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DebugStartItemPicker")]
    public static extern void DebugStartItemPicker();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DebugCheckVersionAndDataLayout")]
    public static extern byte DebugCheckVersionAndDataLayout(byte* version, nuint szIo, nuint szStyle, nuint szVec2, nuint szVec4, nuint szVert, nuint szIdx);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_UpdatePlatformWindows")]
    public static extern void UpdatePlatformWindows();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_RenderPlatformWindowsDefault")]
    public static extern void RenderPlatformWindowsDefault();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_DestroyPlatformWindows")]
    public static extern void DestroyPlatformWindows();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_FindViewportByID")]
    public static extern nint FindViewportByID(uint viewportId);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_FindViewportByPlatformHandle")]
    public static extern nint FindViewportByPlatformHandle(void* handle);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LogToTTY")]
    public static extern void LogToTTY(int depth);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LogToFile")]
    public static extern void LogToFile(int depth, byte* file);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LogToClipboard")]
    public static extern void LogToClipboard(int depth);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LogFinish")]
    public static extern void LogFinish();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LogButtons")]
    public static extern void LogButtons();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LogText")]
    public static extern void LogText(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginDragDropSource")]
    public static extern byte BeginDragDropSource(int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetDragDropPayload")]
    public static extern byte SetDragDropPayload(byte* type, void* data, nuint sz, int cond);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndDragDropSource")]
    public static extern void EndDragDropSource();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginDragDropTarget")]
    public static extern byte BeginDragDropTarget();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_AcceptDragDropPayload")]
    public static extern nint AcceptDragDropPayload(byte* type, int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndDragDropTarget")]
    public static extern void EndDragDropTarget();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetDragDropPayload")]
    public static extern nint GetDragDropPayload();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_BeginDisabled")]
    public static extern void BeginDisabled(byte disabled);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_EndDisabled")]
    public static extern void EndDisabled();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PushClipRect")]
    public static extern void PushClipRect(float minX, float minY, float maxX, float maxY, byte intersect);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_PopClipRect")]
    public static extern void PopClipRect();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetItemDefaultFocus")]
    public static extern void SetItemDefaultFocus();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetKeyboardFocusHere")]
    public static extern void SetKeyboardFocusHere(int offset);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNavCursorVisible")]
    public static extern void SetNavCursorVisible(byte visible);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetNextItemAllowOverlap")]
    public static extern void SetNextItemAllowOverlap();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemHovered")]
    public static extern byte IsItemHovered(int flags);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemActive")]
    public static extern byte IsItemActive();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemFocused")]
    public static extern byte IsItemFocused();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemClicked")]
    public static extern byte IsItemClicked(int btn);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemVisible")]
    public static extern byte IsItemVisible();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemEdited")]
    public static extern byte IsItemEdited();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemActivated")]
    public static extern byte IsItemActivated();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemDeactivated")]
    public static extern byte IsItemDeactivated();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemDeactivatedAfterEdit")]
    public static extern byte IsItemDeactivatedAfterEdit();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsItemToggledOpen")]
    public static extern byte IsItemToggledOpen();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsAnyItemHovered")]
    public static extern byte IsAnyItemHovered();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsAnyItemActive")]
    public static extern byte IsAnyItemActive();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsAnyItemFocused")]
    public static extern byte IsAnyItemFocused();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetItemID")]
    public static extern uint GetItemID();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetItemRectMin")]
    public static extern void GetItemRectMin(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetItemRectMax")]
    public static extern void GetItemRectMax(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetItemRectSize")]
    public static extern void GetItemRectSize(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetMainViewport")]
    public static extern nint GetMainViewport();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetBackgroundDrawList")]
    public static extern nint GetBackgroundDrawList();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetForegroundDrawList")]
    public static extern nint GetForegroundDrawList();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsRectVisible")]
    public static extern byte IsRectVisible(float w, float h);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetTime")]
    public static extern double GetTime();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetFrameCount")]
    public static extern int GetFrameCount();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetStyleColorName")]
    public static extern byte* GetStyleColorName(int idx);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_CalcTextSize")]
    public static extern void CalcTextSize(out float ox, out float oy, byte* text, byte* end, byte hideHash, float wrapW);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsKeyDown")]
    public static extern byte IsKeyDown(int key);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsKeyPressed")]
    public static extern byte IsKeyPressed(int key, byte repeat);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsKeyReleased")]
    public static extern byte IsKeyReleased(int key);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetKeyPressedAmount")]
    public static extern int GetKeyPressedAmount(int key, float delay, float rate);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetKeyName")]
    public static extern byte* GetKeyName(int key);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseDown")]
    public static extern byte IsMouseDown(int btn);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseClicked")]
    public static extern byte IsMouseClicked(int btn, byte repeat);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseReleased")]
    public static extern byte IsMouseReleased(int btn);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseDoubleClicked")]
    public static extern byte IsMouseDoubleClicked(int btn);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseHoveringRect")]
    public static extern byte IsMouseHoveringRect(float minX, float minY, float maxX, float maxY, byte clip);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_IsMouseDragging")]
    public static extern byte IsMouseDragging(int btn, float threshold);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetMousePos")]
    public static extern void GetMousePos(out float x, out float y);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetMouseDragDelta")]
    public static extern void GetMouseDragDelta(out float x, out float y, int btn, float threshold);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ResetMouseDragDelta")]
    public static extern void ResetMouseDragDelta(int btn);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetMouseCursor")]
    public static extern int GetMouseCursor();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetMouseCursor")]
    public static extern void SetMouseCursor(int cursor);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_GetClipboardText")]
    public static extern byte* GetClipboardText();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SetClipboardText")]
    public static extern void SetClipboardText(byte* text);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LoadIniSettingsFromDisk")]
    public static extern void LoadIniSettingsFromDisk(byte* file);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_LoadIniSettingsFromMemory")]
    public static extern void LoadIniSettingsFromMemory(byte* data, nuint size);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SaveIniSettingsToDisk")]
    public static extern void SaveIniSettingsToDisk(byte* file);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_SaveIniSettingsToMemory")]
    public static extern byte* SaveIniSettingsToMemory(out nuint outSize);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_MemAlloc")]
    public static extern void* MemAlloc(nuint size);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_MemFree")]
    public static extern void MemFree(void* ptr);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX11_Init")]
    public static extern byte ImplDX11Init(nint device, nint context);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX11_Shutdown")]
    public static extern void ImplDX11Shutdown();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX11_NewFrame")]
    public static extern void ImplDX11NewFrame();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX11_RenderDrawData")]
    public static extern void ImplDX11RenderDrawData(nint drawData);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplWin32_Init")]
    public static extern byte ImplWin32Init(nint hwnd);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplWin32_Shutdown")]
    public static extern void ImplWin32Shutdown();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplWin32_NewFrame")]
    public static extern void ImplWin32NewFrame();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX12_Init")]
    public static extern byte ImplDX12Init(nint device, nint queue, int framesInFlight, int rtvFormat, nint srvHeap, uint srvHeapSize);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX12_Shutdown")]
    public static extern void ImplDX12Shutdown();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX12_NewFrame")]
    public static extern void ImplDX12NewFrame();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX12_RenderDrawData")]
    public static extern void ImplDX12RenderDrawData(nint drawData, nint commandList);

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX12_CreateDeviceObjects")]
    public static extern void ImplDX12CreateDeviceObjects();

    [DllImport(Shim, CallingConvention = CallingConvention.Cdecl, EntryPoint = "shim_ImplDX12_InvalidateDeviceObjects")]
    public static extern void ImplDX12InvalidateDeviceObjects();
}
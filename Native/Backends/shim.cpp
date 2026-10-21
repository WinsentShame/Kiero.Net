#include <windows.h>
#include <d3d11.h>
#include <d3d12.h>
#include <cstdint>
#include <cstdio>

#include "shim.h"
#include "imgui.h"
#include "imgui_impl_dx9.h"
#include "imgui_impl_dx10.h"
#include "imgui_impl_dx11.h"
#include "imgui_impl_dx12.h"
#include "imgui_impl_win32.h"
#include "imgui_impl_opengl3.h"


	// ============================================================
	// CONTEXT
	// ============================================================
	ImGuiContext* shim_CreateContext(ImFontAtlas* shared) { return ImGui::CreateContext(shared); }
	void shim_DestroyContext(ImGuiContext* ctx) { ImGui::DestroyContext(ctx); }
	ImGuiContext* shim_GetCurrentContext() { return ImGui::GetCurrentContext(); }
	void shim_SetCurrentContext(ImGuiContext* ctx) { ImGui::SetCurrentContext(ctx); }

	// ============================================================
	// IO
	// ============================================================
	ImGuiIO* shim_GetIO() { return &ImGui::GetIO(); }
	ImGuiPlatformIO* shim_GetPlatformIO() { return &ImGui::GetPlatformIO(); }
	ImGuiStyle* shim_GetStyle() { return &ImGui::GetStyle(); }

	void shim_IO_SetDisplaySize(ImGuiIO* io, float w, float h) { io->DisplaySize = ImVec2(w, h); io->DisplayFramebufferScale = ImVec2(1, 1); }
	void shim_IO_GetDisplaySize(ImGuiIO* io, float* w, float* h) { *w = io->DisplaySize.x; *h = io->DisplaySize.y; }
	void shim_IO_SetDeltaTime(ImGuiIO* io, float dt) { io->DeltaTime = dt; }
	void shim_IO_AddMousePos(ImGuiIO* io, float x, float y) { io->AddMousePosEvent(x, y); }
	void shim_IO_AddMouseButton(ImGuiIO* io, int btn, bool down) { io->AddMouseButtonEvent(btn, down); }
	void shim_IO_AddMouseWheel(ImGuiIO* io, float wx, float wy) { io->AddMouseWheelEvent(wx, wy); }
	void shim_IO_AddKey(ImGuiIO* io, int key, bool down) { io->AddKeyEvent((ImGuiKey)key, down); }
	void shim_IO_AddInputCharacter(ImGuiIO* io, unsigned int c) { io->AddInputCharacter(c); }
	void shim_IO_AddFocusEvent(ImGuiIO* io, bool focused) { io->AddFocusEvent(focused); }
	bool shim_IO_WantCaptureMouse(ImGuiIO* io) { return io->WantCaptureMouse; }
	bool shim_IO_WantCaptureKeyboard(ImGuiIO* io) { return io->WantCaptureKeyboard; }
	void shim_IO_SetConfigFlags(ImGuiIO* io, int flags) { io->ConfigFlags |= flags; }
	void shim_IO_SetIniFilename(ImGuiIO* io, const char* name) { io->IniFilename = name; }
	void shim_IO_SetLogFilename(ImGuiIO* io, const char* name) { io->LogFilename = name; }

	// ============================================================
	// FRAME
	// ============================================================
	void shim_NewFrame() { ImGui::NewFrame(); }
	void shim_EndFrame() { ImGui::EndFrame(); }
	void shim_Render() { ImGui::Render(); }
	ImDrawData* shim_GetDrawData() { return ImGui::GetDrawData(); }

	int shim_DrawData_GetVtxCount(ImDrawData* d) { return d ? d->TotalVtxCount : -1; }
	int shim_DrawData_GetIdxCount(ImDrawData* d) { return d ? d->TotalIdxCount : -1; }
	int shim_DrawData_GetCmdListsCount(ImDrawData* d) { return d ? d->CmdLists.Size : -1; }

	// ============================================================
	// DEMO / DEBUG
	// ============================================================
	void shim_ShowDemoWindow(bool* p_open) { ImGui::ShowDemoWindow(p_open); }
	void shim_ShowMetricsWindow(bool* p_open) { ImGui::ShowMetricsWindow(p_open); }
	void shim_ShowDebugLogWindow(bool* p_open) { ImGui::ShowDebugLogWindow(p_open); }
	void shim_ShowIDStackToolWindow(bool* p_open) { ImGui::ShowIDStackToolWindow(p_open); }
	void shim_ShowAboutWindow(bool* p_open) { ImGui::ShowAboutWindow(p_open); }
	void shim_ShowStyleEditor(ImGuiStyle* ref) { ImGui::ShowStyleEditor(ref); }
	bool shim_ShowStyleSelector(const char* label) { return ImGui::ShowStyleSelector(label); }
	void shim_ShowFontSelector(const char* label) { ImGui::ShowFontSelector(label); }
	void shim_ShowUserGuide() { ImGui::ShowUserGuide(); }
	const char* shim_GetVersion() { return ImGui::GetVersion(); }
	void shim_DebugTextEncoding(const char* text) { ImGui::DebugTextEncoding(text); }
	void shim_DebugFlashStyleColor(int idx) { ImGui::DebugFlashStyleColor((ImGuiCol)idx); }
	void shim_DebugStartItemPicker() { ImGui::DebugStartItemPicker(); }
	unsigned char shim_DebugCheckVersionAndDataLayout(const char* v, unsigned long long a, unsigned long long b, unsigned long long c, unsigned long long d, unsigned long long e, unsigned long long f)
	{
		return ImGui::DebugCheckVersionAndDataLayout(v, (size_t)a, (size_t)b, (size_t)c, (size_t)d, (size_t)e, (size_t)f);
	}

	// =====================================================================
	// MULTI-VIEWPORT
	// =====================================================================
	void  shim_UpdatePlatformWindows() { ImGui::UpdatePlatformWindows(); }
	void  shim_RenderPlatformWindowsDefault() { ImGui::RenderPlatformWindowsDefault(); }
	void  shim_DestroyPlatformWindows() { ImGui::DestroyPlatformWindows(); }
	void* shim_FindViewportByID(unsigned int id) { return ImGui::FindViewportByID(id); }
	void* shim_FindViewportByPlatformHandle(void* h) { return ImGui::FindViewportByPlatformHandle(h); }

	// ============================================================
	// STYLES
	// ============================================================
	void shim_StyleColorsDark(ImGuiStyle* dst) { ImGui::StyleColorsDark(dst); }
	void shim_StyleColorsLight(ImGuiStyle* dst) { ImGui::StyleColorsLight(dst); }
	void shim_StyleColorsClassic(ImGuiStyle* dst) { ImGui::StyleColorsClassic(dst); }

	// ============================================================
	// WINDOWS
	// ============================================================
	bool shim_Begin(const char* name, bool* p_open, int flags) { return ImGui::Begin(name, p_open, flags); }
	void shim_End() { ImGui::End(); }

	bool shim_BeginChild(const char* str_id, float w, float h, int child_flags, int window_flags)
	{
		return ImGui::BeginChild(str_id, ImVec2(w, h), child_flags, window_flags);
	}
	bool shim_BeginChildID(unsigned int id, float w, float h, int child_flags, int window_flags)
	{
		return ImGui::BeginChild((ImGuiID)id, ImVec2(w, h), child_flags, window_flags);
	}
	void shim_EndChild() { ImGui::EndChild(); }

	bool shim_IsWindowAppearing() { return ImGui::IsWindowAppearing(); }
	bool shim_IsWindowCollapsed() { return ImGui::IsWindowCollapsed(); }
	bool shim_IsWindowFocused(int flags) { return ImGui::IsWindowFocused(flags); }
	bool shim_IsWindowHovered(int flags) { return ImGui::IsWindowHovered(flags); }
	ImDrawList* shim_GetWindowDrawList() { return ImGui::GetWindowDrawList(); }
	void shim_GetWindowPos(float* x, float* y) { ImVec2 v = ImGui::GetWindowPos(); *x = v.x; *y = v.y; }
	void shim_GetWindowSize(float* x, float* y) { ImVec2 v = ImGui::GetWindowSize(); *x = v.x; *y = v.y; }
	float shim_GetWindowWidth() { return ImGui::GetWindowWidth(); }
	float shim_GetWindowHeight() { return ImGui::GetWindowHeight(); }
	float shim_GetWindowDpiScale() { return ImGui::GetWindowDpiScale(); }
	void* shim_GetWindowViewport() { return ImGui::GetWindowViewport(); }
	void  shim_SetNextWindowScroll(float x, float y) { ImGui::SetNextWindowScroll(ImVec2(x, y)); }
	void  shim_SetNextWindowViewport(unsigned int id) { ImGui::SetNextWindowViewport(id); }
	void  shim_SetWindowPosStr(const char* name, float x, float y, int cond) { ImGui::SetWindowPos(name, ImVec2(x, y), cond); }
	void  shim_SetWindowSizeStr(const char* name, float w, float h, int cond) { ImGui::SetWindowSize(name, ImVec2(w, h), cond); }
	void  shim_SetWindowCollapsedStr(const char* name, unsigned char c, int cond) { ImGui::SetWindowCollapsed(name, c != 0, cond); }
	void  shim_SetWindowFocusStr(const char* name) { ImGui::SetWindowFocus(name); }

	// =====================================================================
	// PARAMS STACKS
	// =====================================================================
	void* shim_GetFontBaked() { return ImGui::GetFontBaked(); }
	void  shim_PushStyleVarX(int idx, float v) { ImGui::PushStyleVarX((ImGuiStyleVar)idx, v); }
	void  shim_PushStyleVarY(int idx, float v) { ImGui::PushStyleVarY((ImGuiStyleVar)idx, v); }
	void  shim_PushItemFlag(int option, unsigned char enabled) { ImGui::PushItemFlag((ImGuiItemFlags)option, enabled != 0); }
	void  shim_PopItemFlag() { ImGui::PopItemFlag(); }

	// =====================================================================
	// STYLE READ
	// =====================================================================
	void shim_GetFontTexUvWhitePixel(float* x, float* y) { ImVec2 p = ImGui::GetFontTexUvWhitePixel(); *x = p.x; *y = p.y; }
	void shim_GetStyleColorVec4(int idx, float* rgba) { const ImVec4& c = ImGui::GetStyleColorVec4((ImGuiCol)idx); rgba[0] = c.x; rgba[1] = c.y; rgba[2] = c.z; rgba[3] = c.w; }
	unsigned int shim_GetColorU32Vec4(float r, float g, float b, float a) { return ImGui::GetColorU32(ImVec4(r, g, b, a)); }

	// ============================================================
	// WINDOW MANIPULATION
	// ============================================================
	void shim_SetNextWindowPos(float x, float y, int cond, float px, float py) { ImGui::SetNextWindowPos(ImVec2(x, y), cond, ImVec2(px, py)); }
	void shim_SetNextWindowSize(float w, float h, int cond) { ImGui::SetNextWindowSize(ImVec2(w, h), cond); }
	void shim_SetNextWindowSizeConstraints(float minx, float miny, float maxx, float maxy) { ImGui::SetNextWindowSizeConstraints(ImVec2(minx, miny), ImVec2(maxx, maxy)); }
	void shim_SetNextWindowContentSize(float w, float h) { ImGui::SetNextWindowContentSize(ImVec2(w, h)); }
	void shim_SetNextWindowCollapsed(bool collapsed, int cond) { ImGui::SetNextWindowCollapsed(collapsed, cond); }
	void shim_SetNextWindowFocus() { ImGui::SetNextWindowFocus(); }
	void shim_SetNextWindowBgAlpha(float alpha) { ImGui::SetNextWindowBgAlpha(alpha); }

	// ============================================================
	// SCROLLING
	// ============================================================
	float shim_GetScrollX() { return ImGui::GetScrollX(); }
	float shim_GetScrollY() { return ImGui::GetScrollY(); }
	void shim_SetScrollX(float x) { ImGui::SetScrollX(x); }
	void shim_SetScrollY(float y) { ImGui::SetScrollY(y); }
	float shim_GetScrollMaxX() { return ImGui::GetScrollMaxX(); }
	float shim_GetScrollMaxY() { return ImGui::GetScrollMaxY(); }
	void shim_SetScrollHereX(float r) { ImGui::SetScrollHereX(r); }
	void shim_SetScrollHereY(float r) { ImGui::SetScrollHereY(r); }
	void shim_SetScrollFromPosX(float x, float r) { ImGui::SetScrollFromPosX(x, r); }
	void shim_SetScrollFromPosY(float y, float r) { ImGui::SetScrollFromPosY(y, r); }

	// ============================================================
	// FONT / STYLE STACKS
	// ============================================================
	void shim_PushFont(ImFont* font, float size) { ImGui::PushFont(font, size); }
	void shim_PopFont() { ImGui::PopFont(); }
	ImFont* shim_GetFont() { return ImGui::GetFont(); }
	float shim_GetFontSize() { return ImGui::GetFontSize(); }

	void shim_PushStyleColorU32(int idx, unsigned int col) { ImGui::PushStyleColor(idx, col); }
	void shim_PushStyleColorVec4(int idx, float r, float g, float b, float a) { ImGui::PushStyleColor(idx, ImVec4(r, g, b, a)); }
	void shim_PopStyleColor(int count) { ImGui::PopStyleColor(count); }
	void shim_PushStyleVar(int idx, float val) { ImGui::PushStyleVar(idx, val); }
	void shim_PushStyleVarVec2(int idx, float x, float y) { ImGui::PushStyleVar(idx, ImVec2(x, y)); }
	void shim_PopStyleVar(int count) { ImGui::PopStyleVar(count); }
	unsigned int shim_GetColorU32(int idx, float alpha) { return ImGui::GetColorU32(idx, alpha); }

	// ============================================================
	// ITEM WIDTH / TEXT WRAP
	// ============================================================
	void shim_PushItemWidth(float w) { ImGui::PushItemWidth(w); }
	void shim_PopItemWidth() { ImGui::PopItemWidth(); }
	void shim_SetNextItemWidth(float w) { ImGui::SetNextItemWidth(w); }
	float shim_CalcItemWidth() { return ImGui::CalcItemWidth(); }
	void shim_PushTextWrapPos(float x) { ImGui::PushTextWrapPos(x); }
	void shim_PopTextWrapPos() { ImGui::PopTextWrapPos(); }

	// ============================================================
	// CURSOR / LAYOUT
	// ============================================================
	void shim_Separator() { ImGui::Separator(); }
	void shim_SeparatorText(const char* label) { ImGui::SeparatorText(label); }
	void shim_SameLine(float offset, float spacing) { ImGui::SameLine(offset, spacing); }
	void shim_NewLine() { ImGui::NewLine(); }
	void shim_Spacing() { ImGui::Spacing(); }
	void shim_Dummy(float w, float h) { ImGui::Dummy(ImVec2(w, h)); }
	void shim_Indent(float w) { ImGui::Indent(w); }
	void shim_Unindent(float w) { ImGui::Unindent(w); }
	void shim_BeginGroup() { ImGui::BeginGroup(); }
	void shim_EndGroup() { ImGui::EndGroup(); }
	void shim_AlignTextToFramePadding() { ImGui::AlignTextToFramePadding(); }
	float shim_GetTextLineHeight() { return ImGui::GetTextLineHeight(); }
	float shim_GetTextLineHeightWithSpacing() { return ImGui::GetTextLineHeightWithSpacing(); }
	float shim_GetFrameHeight() { return ImGui::GetFrameHeight(); }
	float shim_GetFrameHeightWithSpacing() { return ImGui::GetFrameHeightWithSpacing(); }

	void shim_GetCursorPos(float* x, float* y) { ImVec2 v = ImGui::GetCursorPos(); *x = v.x; *y = v.y; }
	float shim_GetCursorPosX() { return ImGui::GetCursorPosX(); }
	float shim_GetCursorPosY() { return ImGui::GetCursorPosY(); }
	void shim_SetCursorPos(float x, float y) { ImGui::SetCursorPos(ImVec2(x, y)); }
	void shim_SetCursorPosX(float x) { ImGui::SetCursorPosX(x); }
	void shim_SetCursorPosY(float y) { ImGui::SetCursorPosY(y); }
	void shim_GetCursorStartPos(float* x, float* y) { ImVec2 v = ImGui::GetCursorStartPos(); *x = v.x; *y = v.y; }
	void shim_GetCursorScreenPos(float* x, float* y) { ImVec2 v = ImGui::GetCursorScreenPos(); *x = v.x; *y = v.y; }
	void shim_SetCursorScreenPos(float x, float y) { ImGui::SetCursorScreenPos(ImVec2(x, y)); }
	void shim_GetContentRegionAvail(float* x, float* y) { ImVec2 v = ImGui::GetContentRegionAvail(); *x = v.x; *y = v.y; }

	// ============================================================
	// ID STACK
	// ============================================================
	void shim_PushIDStr(const char* id) { ImGui::PushID(id); }
	void shim_PushIDInt(int id) { ImGui::PushID(id); }
	void shim_PushIDPtr(const void* p) { ImGui::PushID(p); }
	void shim_PopID() { ImGui::PopID(); }
	void shim_PushIDStrRange(const char* begin, const char* end) { ImGui::PushID(begin, end); }
	unsigned int shim_GetIDStrRange(const char* begin, const char* end) { return ImGui::GetID(begin, end); }
	unsigned int shim_GetIDPtr(void* p) { return ImGui::GetID(p); }
	unsigned int shim_GetIDStr(const char* id) { return ImGui::GetID(id); }
	unsigned int shim_GetIDInt(int id)
	{
		char buf[16];
		snprintf(buf, sizeof(buf), "%d", id);
		return ImGui::GetID(buf);
	}

	// ============================================================
	// TEXT
	// ============================================================
	void shim_TextUnformatted(const char* text, const char* end) { ImGui::TextUnformatted(text, end); }
	void shim_TextColored(float r, float g, float b, float a, const char* text) { ImGui::TextColored(ImVec4(r, g, b, a), "%s", text); }
	void shim_TextDisabled(const char* text) { ImGui::TextDisabled("%s", text); }
	void shim_TextWrapped(const char* text) { ImGui::TextWrapped("%s", text); }
	void shim_LabelText(const char* label, const char* text) { ImGui::LabelText(label, "%s", text); }
	void shim_BulletText(const char* text) { ImGui::BulletText("%s", text); }
	void shim_Bullet() { ImGui::Bullet(); }

	// ============================================================
	// MAIN WIDGETS
	// ============================================================
	bool shim_Button(const char* label, float w, float h) { return ImGui::Button(label, ImVec2(w, h)); }
	bool shim_SmallButton(const char* label) { return ImGui::SmallButton(label); }
	bool shim_InvisibleButton(const char* id, float w, float h, int flags) { return ImGui::InvisibleButton(id, ImVec2(w, h), flags); }
	bool shim_ArrowButton(const char* id, int dir) { return ImGui::ArrowButton(id, (ImGuiDir)dir); }
	bool shim_Checkbox(const char* label, bool* v) { return ImGui::Checkbox(label, v); }
	bool shim_RadioButtonBool(const char* label, bool active) { return ImGui::RadioButton(label, active); }
	bool shim_RadioButtonInt(const char* label, int* v, int v_button) { return ImGui::RadioButton(label, v, v_button); }
	void shim_ProgressBar(float fraction, float w, float h, const char* overlay) { ImGui::ProgressBar(fraction, ImVec2(w, h), overlay); }
	bool shim_TextLink(const char* label) { return ImGui::TextLink(label); }
	bool shim_TextLinkOpenURL(const char* label, const char* url) { return ImGui::TextLinkOpenURL(label, url); }
	void shim_Text(const char* text) { ImGui::TextUnformatted(text); }
	unsigned char shim_CheckboxFlagsInt(const char* label, int* flags, int flags_value) { return ImGui::CheckboxFlags(label, flags, flags_value); }
	unsigned char shim_CheckboxFlagsUInt(const char* label, unsigned int* flags, unsigned int flags_value) { return ImGui::CheckboxFlags(label, flags, flags_value); }
	void shim_Image(unsigned long long tex_id, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y)
	{
		ImGui::Image((ImTextureID)tex_id, ImVec2(w, h), ImVec2(uv0x, uv0y), ImVec2(uv1x, uv1y));
	}

	void shim_ImageWithBg(unsigned long long tex_id, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y,
		float bgr, float bgg, float bgb, float bga, float tr, float tg, float tb, float ta)
	{
		ImGui::ImageWithBg((ImTextureID)tex_id, ImVec2(w, h), ImVec2(uv0x, uv0y), ImVec2(uv1x, uv1y), ImVec4(bgr, bgg, bgb, bga), ImVec4(tr, tg, tb, ta));
	}

	unsigned char shim_ImageButton(const char* str_id, unsigned long long tex_id, float w, float h,
		float uv0x, float uv0y, float uv1x, float uv1y,
		float bgr, float bgg, float bgb, float bga, float tr, float tg, float tb, float ta)
	{
		return ImGui::ImageButton(str_id, (ImTextureID)tex_id, ImVec2(w, h), ImVec2(uv0x, uv0y), ImVec2(uv1x, uv1y), ImVec4(bgr, bgg, bgb, bga), ImVec4(tr, tg, tb, ta));
	}
	unsigned char shim_ComboCallback(const char* label, int* current_item, const char* (*getter)(void*, int), void* user_data, int items_count, int popup_max_height)
	{
		return ImGui::Combo(label, current_item, getter, user_data, items_count, popup_max_height);
	}

	unsigned char shim_ComboZeros(const char* label, int* current_item, const char* items_separated_by_zeros, int popup_max_height)
	{
		return ImGui::Combo(label, current_item, items_separated_by_zeros, popup_max_height);
	}
	unsigned char shim_DragFloatRange2(const char* label, float* a, float* b, float speed, float vmin, float vmax, const char* fmt, const char* fmt_max, int flags)
	{
		return ImGui::DragFloatRange2(label, a, b, speed, vmin, vmax, fmt, fmt_max, (ImGuiSliderFlags)flags);
	}

	unsigned char shim_DragIntRange2(const char* label, int* a, int* b, float speed, int vmin, int vmax, const char* fmt, const char* fmt_max, int flags)
	{
		return ImGui::DragIntRange2(label, a, b, speed, vmin, vmax, fmt, fmt_max, (ImGuiSliderFlags)flags);
	}

	unsigned char shim_DragScalar(const char* label, int dt, void* p, float speed, const void* pmin, const void* pmax, const char* fmt, int flags)
	{
		return ImGui::DragScalar(label, (ImGuiDataType)dt, p, speed, pmin, pmax, fmt, (ImGuiSliderFlags)flags);
	}

	unsigned char shim_DragScalarN(const char* label, int dt, void* p, int n, float speed, const void* pmin, const void* pmax, const char* fmt, int flags)
	{
		return ImGui::DragScalarN(label, (ImGuiDataType)dt, p, n, speed, pmin, pmax, fmt, (ImGuiSliderFlags)flags);
	}

	unsigned char shim_SliderScalar(const char* label, int dt, void* p, const void* pmin, const void* pmax, const char* fmt, int flags)
	{
		return ImGui::SliderScalar(label, (ImGuiDataType)dt, p, pmin, pmax, fmt, (ImGuiSliderFlags)flags);
	}

	unsigned char shim_SliderScalarN(const char* label, int dt, void* p, int n, const void* pmin, const void* pmax, const char* fmt, int flags)
	{
		return ImGui::SliderScalarN(label, (ImGuiDataType)dt, p, n, pmin, pmax, fmt, (ImGuiSliderFlags)flags);
	}

	unsigned char shim_VSliderScalar(const char* label, float w, float h, int dt, void* p, const void* pmin, const void* pmax, const char* fmt, int flags)
	{
		return ImGui::VSliderScalar(label, ImVec2(w, h), (ImGuiDataType)dt, p, pmin, pmax, fmt, (ImGuiSliderFlags)flags);
	}
	unsigned char shim_InputTextWithHint(const char* label, const char* hint, char* buf, unsigned long long size, int flags)
	{
		return ImGui::InputTextWithHint(label, hint, buf, (size_t)size, (ImGuiInputTextFlags)flags);
	}

	unsigned char shim_InputFloat2(const char* label, float* v, const char* fmt, int flags) { return ImGui::InputFloat2(label, v, fmt, (ImGuiInputTextFlags)flags); }
	unsigned char shim_InputFloat3(const char* label, float* v, const char* fmt, int flags) { return ImGui::InputFloat3(label, v, fmt, (ImGuiInputTextFlags)flags); }
	unsigned char shim_InputFloat4(const char* label, float* v, const char* fmt, int flags) { return ImGui::InputFloat4(label, v, fmt, (ImGuiInputTextFlags)flags); }
	unsigned char shim_InputInt2(const char* label, int* v, int flags) { return ImGui::InputInt2(label, v, (ImGuiInputTextFlags)flags); }
	unsigned char shim_InputInt3(const char* label, int* v, int flags) { return ImGui::InputInt3(label, v, (ImGuiInputTextFlags)flags); }
	unsigned char shim_InputInt4(const char* label, int* v, int flags) { return ImGui::InputInt4(label, v, (ImGuiInputTextFlags)flags); }

	unsigned char shim_InputScalar(const char* label, int dt, void* p, const void* pstep, const void* pfast, const char* fmt, int flags)
	{
		return ImGui::InputScalar(label, (ImGuiDataType)dt, p, pstep, pfast, fmt, (ImGuiInputTextFlags)flags);
	}

	unsigned char shim_InputScalarN(const char* label, int dt, void* p, int n, const void* pstep, const void* pfast, const char* fmt, int flags)
	{
		return ImGui::InputScalarN(label, (ImGuiDataType)dt, p, n, pstep, pfast, fmt, (ImGuiInputTextFlags)flags);
	}
	unsigned char shim_TreeNodeStrId(const char* str_id, const char* fmt) { return ImGui::TreeNode(str_id, "%s", fmt); }
	unsigned char shim_TreeNodePtrId(const void* ptr_id, const char* fmt) { return ImGui::TreeNode(ptr_id, "%s", fmt); }
	unsigned char shim_TreeNodeExStrId(const char* str_id, int flags, const char* fmt) { return ImGui::TreeNodeEx(str_id, (ImGuiTreeNodeFlags)flags, "%s", fmt); }
	unsigned char shim_TreeNodeExPtrId(const void* ptr_id, int flags, const char* fmt) { return ImGui::TreeNodeEx(ptr_id, (ImGuiTreeNodeFlags)flags, "%s", fmt); }
	void shim_TreePushStr(const char* str_id) { ImGui::TreePush(str_id); }
	void shim_TreePushPtr(const void* ptr_id) { ImGui::TreePush(ptr_id); }
	void shim_SetNextItemStorageID(unsigned int id) { ImGui::SetNextItemStorageID(id); }
	unsigned char shim_TreeNodeGetOpen(unsigned int id) { return ImGui::TreeNodeGetOpen(id); }
	void* shim_BeginMultiSelect(int flags, int sel_size, int items_count) { return ImGui::BeginMultiSelect((ImGuiMultiSelectFlags)flags, sel_size, items_count); }
	void* shim_EndMultiSelect() { return ImGui::EndMultiSelect(); }
	void  shim_SetNextItemSelectionUserData(long long ud) { ImGui::SetNextItemSelectionUserData(ud); }
	unsigned char shim_IsItemToggledSelection() { return ImGui::IsItemToggledSelection(); }
	unsigned char shim_ListBoxArray(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items)
	{
		return ImGui::ListBox(label, current_item, items, items_count, height_in_items);
	}

	unsigned char shim_ListBoxCallback(const char* label, int* current_item, const char* (*getter)(void*, int), void* user_data, int items_count, int height_in_items)
	{
		return ImGui::ListBox(label, current_item, getter, user_data, items_count, height_in_items);
	}

	// ============================================================
	// COMBO
	// ============================================================
	bool shim_BeginCombo(const char* label, const char* preview, int flags) { return ImGui::BeginCombo(label, preview, flags); }
	void shim_EndCombo() { ImGui::EndCombo(); }

	// ============================================================
	// DRAG
	// ============================================================
	bool shim_DragFloat(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags) { return ImGui::DragFloat(label, v, speed, min, max, fmt, flags); }
	bool shim_DragFloat2(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags) { return ImGui::DragFloat2(label, v, speed, min, max, fmt, flags); }
	bool shim_DragFloat3(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags) { return ImGui::DragFloat3(label, v, speed, min, max, fmt, flags); }
	bool shim_DragFloat4(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags) { return ImGui::DragFloat4(label, v, speed, min, max, fmt, flags); }
	bool shim_DragInt(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags) { return ImGui::DragInt(label, v, speed, min, max, fmt, flags); }
	bool shim_DragInt2(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags) { return ImGui::DragInt2(label, v, speed, min, max, fmt, flags); }
	bool shim_DragInt3(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags) { return ImGui::DragInt3(label, v, speed, min, max, fmt, flags); }
	bool shim_DragInt4(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags) { return ImGui::DragInt4(label, v, speed, min, max, fmt, flags); }

	// ============================================================
	// SLIDER
	// ============================================================
	bool shim_SliderFloat(const char* label, float* v, float min, float max, const char* fmt, int flags) { return ImGui::SliderFloat(label, v, min, max, fmt, flags); }
	bool shim_SliderFloat2(const char* label, float* v, float min, float max, const char* fmt, int flags) { return ImGui::SliderFloat2(label, v, min, max, fmt, flags); }
	bool shim_SliderFloat3(const char* label, float* v, float min, float max, const char* fmt, int flags) { return ImGui::SliderFloat3(label, v, min, max, fmt, flags); }
	bool shim_SliderFloat4(const char* label, float* v, float min, float max, const char* fmt, int flags) { return ImGui::SliderFloat4(label, v, min, max, fmt, flags); }
	bool shim_SliderInt(const char* label, int* v, int min, int max, const char* fmt, int flags) { return ImGui::SliderInt(label, v, min, max, fmt, flags); }
	bool shim_SliderInt2(const char* label, int* v, int min, int max, const char* fmt, int flags) { return ImGui::SliderInt2(label, v, min, max, fmt, flags); }
	bool shim_SliderInt3(const char* label, int* v, int min, int max, const char* fmt, int flags) { return ImGui::SliderInt3(label, v, min, max, fmt, flags); }
	bool shim_SliderInt4(const char* label, int* v, int min, int max, const char* fmt, int flags) { return ImGui::SliderInt4(label, v, min, max, fmt, flags); }
	bool shim_SliderAngle(const char* label, float* v_rad, float min_deg, float max_deg, const char* fmt, int flags) { return ImGui::SliderAngle(label, v_rad, min_deg, max_deg, fmt, flags); }
	bool shim_VSliderFloat(const char* label, float w, float h, float* v, float min, float max, const char* fmt, int flags) { return ImGui::VSliderFloat(label, ImVec2(w, h), v, min, max, fmt, flags); }
	bool shim_VSliderInt(const char* label, float w, float h, int* v, int min, int max, const char* fmt, int flags) { return ImGui::VSliderInt(label, ImVec2(w, h), v, min, max, fmt, flags); }

	// ============================================================
	// INPUT
	// ============================================================
	bool shim_InputText(const char* label, char* buf, size_t size, int flags) { return ImGui::InputText(label, buf, size, flags); }
	bool shim_InputTextMultiline(const char* label, char* buf, size_t size, float w, float h, int flags) { return ImGui::InputTextMultiline(label, buf, size, ImVec2(w, h), flags); }
	bool shim_InputFloat(const char* label, float* v, float step, float fast, const char* fmt, int flags) { return ImGui::InputFloat(label, v, step, fast, fmt, flags); }
	bool shim_InputInt(const char* label, int* v, int step, int fast, int flags) { return ImGui::InputInt(label, v, step, fast, flags); }
	bool shim_InputDouble(const char* label, double* v, double step, double fast, const char* fmt, int flags) { return ImGui::InputDouble(label, v, step, fast, fmt, flags); }

	// ============================================================
	// COLOR
	// ============================================================
	bool			shim_ColorEdit3(const char* label, float* col, int flags) { return ImGui::ColorEdit3(label, col, flags); }
	bool			shim_ColorEdit4(const char* label, float* col, int flags) { return ImGui::ColorEdit4(label, col, flags); }
	bool			shim_ColorPicker3(const char* label, float* col, int flags) { return ImGui::ColorPicker3(label, col, flags); }
	bool			shim_ColorPicker4(const char* label, float* col, int flags, const float* ref_col) { return ImGui::ColorPicker4(label, col, flags, ref_col); }
	bool			shim_ColorButton(const char* id, float r, float g, float b, float a, int flags, float w, float h) { return ImGui::ColorButton(id, ImVec4(r, g, b, a), flags, ImVec2(w, h)); }
	void			shim_ColorConvertU32ToFloat4(unsigned int in, float* o) { ImVec4 c = ImGui::ColorConvertU32ToFloat4(in); o[0] = c.x; o[1] = c.y; o[2] = c.z; o[3] = c.w; }
	unsigned int	shim_ColorConvertFloat4ToU32(float r, float g, float b, float a) { return ImGui::ColorConvertFloat4ToU32(ImVec4(r, g, b, a)); }
	void			shim_ColorConvertRGBtoHSV(float r, float g, float b, float* h, float* s, float* v) { ImGui::ColorConvertRGBtoHSV(r, g, b, *h, *s, *v); }
	void			shim_ColorConvertHSVtoRGB(float h, float s, float v, float* r, float* g, float* b) { ImGui::ColorConvertHSVtoRGB(h, s, v, *r, *g, *b); }

	// ============================================================
	// TREE
	// ============================================================
	bool shim_TreeNode(const char* label) { return ImGui::TreeNode(label); }
	bool shim_TreeNodeEx(const char* label, int flags) { return ImGui::TreeNodeEx(label, flags); }
	void shim_TreePop() { ImGui::TreePop(); }
	float shim_GetTreeNodeToLabelSpacing() { return ImGui::GetTreeNodeToLabelSpacing(); }
	bool shim_CollapsingHeader(const char* label, int flags) { return ImGui::CollapsingHeader(label, flags); }
	bool shim_CollapsingHeaderClosable(const char* label, bool* p_open, int flags) { return ImGui::CollapsingHeader(label, p_open, flags); }
	void shim_SetNextItemOpen(bool is_open, int cond) { ImGui::SetNextItemOpen(is_open, cond); }

	// ============================================================
	// SELECTABLE
	// ============================================================
	bool shim_Selectable(const char* label, bool selected, int flags, float w, float h) { return ImGui::Selectable(label, selected, flags, ImVec2(w, h)); }
	bool shim_SelectablePtr(const char* label, bool* p_selected, int flags, float w, float h) { return ImGui::Selectable(label, p_selected, flags, ImVec2(w, h)); }

	// ============================================================
	// LISTBOX
	// ============================================================
	bool shim_BeginListBox(const char* label, float w, float h) { return ImGui::BeginListBox(label, ImVec2(w, h)); }
	void shim_EndListBox() { ImGui::EndListBox(); }

	// ============================================================
	// PLOTS
	// ============================================================
	void shim_PlotLines(const char* label, const float* values, int count, int offset, const char* overlay, float min, float max, float w, float h, int stride) { ImGui::PlotLines(label, values, count, offset, overlay, min, max, ImVec2(w, h), stride); }
	void shim_PlotHistogram(const char* label, const float* values, int count, int offset, const char* overlay, float min, float max, float w, float h, int stride) { ImGui::PlotHistogram(label, values, count, offset, overlay, min, max, ImVec2(w, h), stride); }

	// ============================================================
	// VALUE
	// ============================================================
	void shim_ValueBool(const char* prefix, bool b) { ImGui::Value(prefix, b); }
	void shim_ValueInt(const char* prefix, int v) { ImGui::Value(prefix, v); }
	void shim_ValueUInt(const char* prefix, unsigned int v) { ImGui::Value(prefix, v); }
	void shim_ValueFloat(const char* prefix, float v, const char* fmt) { ImGui::Value(prefix, v, fmt); }

	// ============================================================
	// MENU
	// ============================================================
	bool shim_BeginMenuBar() { return ImGui::BeginMenuBar(); }
	void shim_EndMenuBar() { ImGui::EndMenuBar(); }
	bool shim_BeginMainMenuBar() { return ImGui::BeginMainMenuBar(); }
	void shim_EndMainMenuBar() { ImGui::EndMainMenuBar(); }
	bool shim_BeginMenu(const char* label, bool enabled) { return ImGui::BeginMenu(label, enabled); }
	void shim_EndMenu() { ImGui::EndMenu(); }
	bool shim_MenuItem(const char* label, const char* shortcut, bool selected, bool enabled) { return ImGui::MenuItem(label, shortcut, selected, enabled); }
	bool shim_MenuItemPtr(const char* label, const char* shortcut, bool* p_selected, bool enabled) { return ImGui::MenuItem(label, shortcut, p_selected, enabled); }

	// ============================================================
	// TOOLTIP
	// ============================================================
	bool shim_BeginTooltip() { return ImGui::BeginTooltip(); }
	void shim_EndTooltip() { ImGui::EndTooltip(); }
	void shim_SetTooltip(const char* text) { ImGui::SetTooltip("%s", text); }
	bool shim_BeginItemTooltip() { return ImGui::BeginItemTooltip(); }
	void shim_SetItemTooltip(const char* text) { ImGui::SetItemTooltip("%s", text); }

	// ============================================================
	// POPUP
	// ============================================================
	bool shim_BeginPopup(const char* id, int flags) { return ImGui::BeginPopup(id, flags); }
	bool shim_BeginPopupModal(const char* name, bool* p_open, int flags) { return ImGui::BeginPopupModal(name, p_open, flags); }
	void shim_EndPopup() { ImGui::EndPopup(); }
	bool shim_OpenPopup(const char* id, int flags) { return ImGui::OpenPopup(id, flags); }
	bool shim_OpenPopupOnItemClick(const char* id, int flags) { return ImGui::OpenPopupOnItemClick(id, flags); }
	void shim_CloseCurrentPopup() { ImGui::CloseCurrentPopup(); }
	bool shim_BeginPopupContextItem(const char* id, int flags) { return ImGui::BeginPopupContextItem(id, flags); }
	bool shim_BeginPopupContextWindow(const char* id, int flags) { return ImGui::BeginPopupContextWindow(id, flags); }
	bool shim_BeginPopupContextVoid(const char* id, int flags) { return ImGui::BeginPopupContextVoid(id, flags); }
	bool shim_IsPopupOpen(const char* id, int flags) { return ImGui::IsPopupOpen(id, flags); }

	// ============================================================
	// TABLES
	// ============================================================
	bool shim_BeginTable(const char* id, int cols, int flags, float w, float h, float inner_w) { return ImGui::BeginTable(id, cols, flags, ImVec2(w, h), inner_w); }
	void shim_EndTable() { ImGui::EndTable(); }
	void shim_TableNextRow(int flags, float min_h) { ImGui::TableNextRow(flags, min_h); }
	bool shim_TableNextColumn() { return ImGui::TableNextColumn(); }
	bool shim_TableSetColumnIndex(int n) { return ImGui::TableSetColumnIndex(n); }
	void shim_TableSetupColumn(const char* label, int flags, float init_w, unsigned int user_data) { ImGui::TableSetupColumn(label, flags, init_w, user_data); }
	void shim_TableSetupScrollFreeze(int cols, int rows) { ImGui::TableSetupScrollFreeze(cols, rows); }
	void shim_TableHeader(const char* label) { ImGui::TableHeader(label); }
	void shim_TableHeadersRow() { ImGui::TableHeadersRow(); }
	void shim_TableAngledHeadersRow() { ImGui::TableAngledHeadersRow(); }
	int shim_TableGetColumnCount() { return ImGui::TableGetColumnCount(); }
	int shim_TableGetColumnIndex() { return ImGui::TableGetColumnIndex(); }
	int shim_TableGetRowIndex() { return ImGui::TableGetRowIndex(); }
	const char* shim_TableGetColumnName(int n) { return ImGui::TableGetColumnName(n); }
	void shim_TableSetBgColor(int target, unsigned int color, int col) { ImGui::TableSetBgColor(target, color, col); }
	void* shim_TableGetSortSpecs() { return ImGui::TableGetSortSpecs(); }
	int   shim_TableGetColumnFlags(int n) { return (int)ImGui::TableGetColumnFlags(n); }
	void  shim_TableSetColumnEnabled(int n, unsigned char v) { ImGui::TableSetColumnEnabled(n, v != 0); }
	int   shim_TableGetHoveredColumn() { return ImGui::TableGetHoveredColumn(); }

	// =====================================================================
	// DOCKING
	// =====================================================================
	unsigned int shim_DockSpace(unsigned int id, float w, float h, int flags, void* wc)
	{
		return ImGui::DockSpace(id, ImVec2(w, h), (ImGuiDockNodeFlags)flags, (const ImGuiWindowClass*)wc);
	}

	unsigned int shim_DockSpaceOverViewport(unsigned int id, void* vp, int flags, void* wc)
	{
		return ImGui::DockSpaceOverViewport(id, (const ImGuiViewport*)vp, (ImGuiDockNodeFlags)flags, (const ImGuiWindowClass*)wc);
	}

	void shim_SetNextWindowDockID(unsigned int id, int cond) { ImGui::SetNextWindowDockID(id, (ImGuiCond)cond); }
	void shim_SetNextWindowClass(void* wc) { ImGui::SetNextWindowClass((const ImGuiWindowClass*)wc); }
	unsigned int shim_GetWindowDockID() { return ImGui::GetWindowDockID(); }
	unsigned char shim_IsWindowDocked() { return ImGui::IsWindowDocked(); }

	// =====================================================================
	// ITEM UTILITIES
	// =====================================================================
	int shim_GetItemFlags() { return (int)ImGui::GetItemFlags(); }
	int shim_GetItemClickedCountWithSingleClickDelay(int btn, float delay) { return ImGui::GetItemClickedCountWithSingleClickDelay((ImGuiMouseButton)btn, delay); }

	// =====================================================================
	// LEGACY COLUMNS
	// =====================================================================
	void  shim_Columns(int count, const char* id, unsigned char borders) { ImGui::Columns(count, id, borders != 0); }
	void  shim_NextColumn() { ImGui::NextColumn(); }
	int   shim_GetColumnIndex() { return ImGui::GetColumnIndex(); }
	float shim_GetColumnWidth(int n) { return ImGui::GetColumnWidth(n); }
	void  shim_SetColumnWidth(int n, float w) { ImGui::SetColumnWidth(n, w); }
	float shim_GetColumnOffset(int n) { return ImGui::GetColumnOffset(n); }
	void  shim_SetColumnOffset(int n, float o) { ImGui::SetColumnOffset(n, o); }
	int   shim_GetColumnsCount() { return ImGui::GetColumnsCount(); }

	// ============================================================
	// TAB BAR
	// ============================================================
	bool shim_BeginTabBar(const char* id, int flags) { return ImGui::BeginTabBar(id, flags); }
	void shim_EndTabBar() { ImGui::EndTabBar(); }
	bool shim_BeginTabItem(const char* label, bool* p_open, int flags) { return ImGui::BeginTabItem(label, p_open, flags); }
	void shim_EndTabItem() { ImGui::EndTabItem(); }
	bool shim_TabItemButton(const char* label, int flags) { return ImGui::TabItemButton(label, flags); }
	void shim_SetTabItemClosed(const char* label) { ImGui::SetTabItemClosed(label); }

	// ============================================================
	// LOGGING
	// ============================================================
	void shim_LogToTTY(int depth) { ImGui::LogToTTY(depth); }
	void shim_LogToFile(int depth, const char* file) { ImGui::LogToFile(depth, file); }
	void shim_LogToClipboard(int depth) { ImGui::LogToClipboard(depth); }
	void shim_LogFinish() { ImGui::LogFinish(); }
	void shim_LogButtons() { ImGui::LogButtons(); }
	void shim_LogText(const char* text) { ImGui::LogText("%s", text); }

	// ============================================================
	// DRAG DROP
	// ============================================================
	bool shim_BeginDragDropSource(int flags) { return ImGui::BeginDragDropSource(flags); }
	bool shim_SetDragDropPayload(const char* type, const void* data, size_t sz, int cond) { return ImGui::SetDragDropPayload(type, data, sz, cond); }
	void shim_EndDragDropSource() { ImGui::EndDragDropSource(); }
	bool shim_BeginDragDropTarget() { return ImGui::BeginDragDropTarget(); }
	const ImGuiPayload* shim_AcceptDragDropPayload(const char* type, int flags) { return ImGui::AcceptDragDropPayload(type, flags); }
	void shim_EndDragDropTarget() { ImGui::EndDragDropTarget(); }
	const ImGuiPayload* shim_GetDragDropPayload() { return ImGui::GetDragDropPayload(); }

	// ============================================================
	// DISABLING
	// ============================================================
	void shim_BeginDisabled(bool disabled) { ImGui::BeginDisabled(disabled); }
	void shim_EndDisabled() { ImGui::EndDisabled(); }

	// ============================================================
	// CLIPPING
	// ============================================================
	void shim_PushClipRect(float minx, float miny, float maxx, float maxy, bool intersect) { ImGui::PushClipRect(ImVec2(minx, miny), ImVec2(maxx, maxy), intersect); }
	void shim_PopClipRect() { ImGui::PopClipRect(); }

	// ============================================================
	// FOCUS / NAV
	// ============================================================
	void shim_SetItemDefaultFocus() { ImGui::SetItemDefaultFocus(); }
	void shim_SetKeyboardFocusHere(int offset) { ImGui::SetKeyboardFocusHere(offset); }
	void shim_SetNavCursorVisible(bool visible) { ImGui::SetNavCursorVisible(visible); }
	void shim_SetNextItemAllowOverlap() { ImGui::SetNextItemAllowOverlap(); }

	// ============================================================
	// ITEM QUERIES
	// ============================================================
	bool shim_IsItemHovered(int flags) { return ImGui::IsItemHovered(flags); }
	bool shim_IsItemActive() { return ImGui::IsItemActive(); }
	bool shim_IsItemFocused() { return ImGui::IsItemFocused(); }
	bool shim_IsItemClicked(int btn) { return ImGui::IsItemClicked(btn); }
	bool shim_IsItemVisible() { return ImGui::IsItemVisible(); }
	bool shim_IsItemEdited() { return ImGui::IsItemEdited(); }
	bool shim_IsItemActivated() { return ImGui::IsItemActivated(); }
	bool shim_IsItemDeactivated() { return ImGui::IsItemDeactivated(); }
	bool shim_IsItemDeactivatedAfterEdit() { return ImGui::IsItemDeactivatedAfterEdit(); }
	bool shim_IsItemToggledOpen() { return ImGui::IsItemToggledOpen(); }
	bool shim_IsAnyItemHovered() { return ImGui::IsAnyItemHovered(); }
	bool shim_IsAnyItemActive() { return ImGui::IsAnyItemActive(); }
	bool shim_IsAnyItemFocused() { return ImGui::IsAnyItemFocused(); }
	unsigned int shim_GetItemID() { return ImGui::GetItemID(); }
	void shim_GetItemRectMin(float* x, float* y) { ImVec2 v = ImGui::GetItemRectMin(); *x = v.x; *y = v.y; }
	void shim_GetItemRectMax(float* x, float* y) { ImVec2 v = ImGui::GetItemRectMax(); *x = v.x; *y = v.y; }
	void shim_GetItemRectSize(float* x, float* y) { ImVec2 v = ImGui::GetItemRectSize(); *x = v.x; *y = v.y; }

	// ============================================================
	// VIEWPORT / DRAWLISTS
	// ============================================================
	ImGuiViewport* shim_GetMainViewport() { return ImGui::GetMainViewport(); }
	ImDrawList* shim_GetBackgroundDrawList() { return ImGui::GetBackgroundDrawList(); }
	ImDrawList* shim_GetForegroundDrawList() { return ImGui::GetForegroundDrawList(); }
	void shim_ImDrawList_AddCircleFilled(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments)
	{
		draw_list->AddCircleFilled(ImVec2(center_x, center_y), radius, col, num_segments);
	}

	void shim_ImDrawList_AddText(ImDrawList* draw_list, float pos_x, float pos_y, unsigned int col, const char* text_begin, const char* text_end)
	{
		draw_list->AddText(ImVec2(pos_x, pos_y), col, text_begin, text_end);
	}

	void shim_ImDrawList_AddLine(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, unsigned int col, float thickness)
	{
		draw_list->AddLine(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), col, thickness);
	}

	void shim_ImDrawList_AddRect(ImDrawList* draw_list, float min_x, float min_y, float max_x, float max_y, unsigned int col, float rounding, int flags, float thickness)
	{
		draw_list->AddRect(ImVec2(min_x, min_y), ImVec2(max_x, max_y), col, rounding, thickness, (ImDrawFlags)flags);
	}

	void shim_ImDrawList_AddRectFilled(ImDrawList* draw_list, float min_x, float min_y, float max_x, float max_y, unsigned int col, float rounding, int flags)
	{
		draw_list->AddRectFilled(ImVec2(min_x, min_y), ImVec2(max_x, max_y), col, rounding, (ImDrawFlags)flags);
	}

	void shim_ImDrawList_AddRectFilledMultiColor(ImDrawList* draw_list, float min_x, float min_y, float max_x, float max_y, unsigned int col_upr_left, unsigned int col_upr_right, unsigned int col_bot_right, unsigned int col_bot_left)
	{
		draw_list->AddRectFilledMultiColor(ImVec2(min_x, min_y), ImVec2(max_x, max_y), col_upr_left, col_upr_right, col_bot_right, col_bot_left);
	}

	void shim_ImDrawList_AddTriangle(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, unsigned int col, float thickness)
	{
		draw_list->AddTriangle(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), ImVec2(p3_x, p3_y), col, thickness);
	}

	void shim_ImDrawList_AddTriangleFilled(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, unsigned int col)
	{
		draw_list->AddTriangleFilled(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), ImVec2(p3_x, p3_y), col);
	}

	void shim_ImDrawList_AddQuad(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, float p4_x, float p4_y, unsigned int col, float thickness)
	{
		draw_list->AddQuad(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), ImVec2(p3_x, p3_y), ImVec2(p4_x, p4_y), col, thickness);
	}

	void shim_ImDrawList_AddQuadFilled(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, float p4_x, float p4_y, unsigned int col)
	{
		draw_list->AddQuadFilled(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), ImVec2(p3_x, p3_y), ImVec2(p4_x, p4_y), col);
	}

	void shim_ImDrawList_AddCircle(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments, float thickness)
	{
		draw_list->AddCircle(ImVec2(center_x, center_y), radius, col, num_segments, thickness);
	}

	void shim_ImDrawList_AddNgon(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments, float thickness)
	{
		draw_list->AddNgon(ImVec2(center_x, center_y), radius, col, num_segments, thickness);
	}

	void shim_ImDrawList_AddNgonFilled(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments)
	{
		draw_list->AddNgonFilled(ImVec2(center_x, center_y), radius, col, num_segments);
	}

	void shim_ImDrawList_AddBezierCubic(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, float p4_x, float p4_y, unsigned int col, float thickness, int num_segments)
	{
		draw_list->AddBezierCubic(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), ImVec2(p3_x, p3_y), ImVec2(p4_x, p4_y), col, thickness, num_segments);
	}

	void shim_ImDrawList_AddBezierQuadratic(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, unsigned int col, float thickness, int num_segments)
	{
		draw_list->AddBezierQuadratic(ImVec2(p1_x, p1_y), ImVec2(p2_x, p2_y), ImVec2(p3_x, p3_y), col, thickness, num_segments);
	}

	void shim_ImDrawList_AddImage(ImDrawList* draw_list, unsigned long long user_texture_id, float p_min_x, float p_min_y, float p_max_x, float p_max_y)
	{
		draw_list->AddImage((ImTextureID)user_texture_id, ImVec2(p_min_x, p_min_y), ImVec2(p_max_x, p_max_y));
	}

	void shim_ImDrawList_AddImageRounded(ImDrawList* draw_list, unsigned long long user_texture_id, float p_min_x, float p_min_y, float p_max_x, float p_max_y, float uv_min_x, float uv_min_y, float uv_max_x, float uv_max_y, unsigned int col, float rounding, int flags)
	{
		draw_list->AddImageRounded((ImTextureID)user_texture_id, ImVec2(p_min_x, p_min_y), ImVec2(p_max_x, p_max_y), ImVec2(uv_min_x, uv_min_y), ImVec2(uv_max_x, uv_max_y), col, rounding, (ImDrawFlags)flags);
	}


	// ============================================================
	// MISC
	// ============================================================
	bool shim_IsRectVisible(float w, float h) { return ImGui::IsRectVisible(ImVec2(w, h)); }
	double shim_GetTime() { return ImGui::GetTime(); }
	int shim_GetFrameCount() { return ImGui::GetFrameCount(); }
	const char* shim_GetStyleColorName(int idx) { return ImGui::GetStyleColorName(idx); }
	void shim_CalcTextSize(float* ox, float* oy, const char* text, const char* end, bool hide_hash, float wrap_w)
	{
		ImVec2 v = ImGui::CalcTextSize(text, end, hide_hash, wrap_w); *ox = v.x; *oy = v.y;
	}
	void* shim_GetDrawListSharedData() { return ImGui::GetDrawListSharedData(); }
	void  shim_SetStateStorage(void* s) { ImGui::SetStateStorage((ImGuiStorage*)s); }
	void* shim_GetStateStorage() { return ImGui::GetStateStorage(); }

	// ============================================================
	// INPUT QUERIES (keyboard/mouse)
	// ============================================================
	bool shim_IsKeyDown(int key) { return ImGui::IsKeyDown((ImGuiKey)key); }
	bool shim_IsKeyPressed(int key, bool repeat) { return ImGui::IsKeyPressed((ImGuiKey)key, repeat); }
	bool shim_IsKeyReleased(int key) { return ImGui::IsKeyReleased((ImGuiKey)key); }
	int shim_GetKeyPressedAmount(int key, float delay, float rate) { return ImGui::GetKeyPressedAmount((ImGuiKey)key, delay, rate); }
	const char* shim_GetKeyName(int key) { return ImGui::GetKeyName((ImGuiKey)key); }
	bool shim_IsMouseDown(int btn) { return ImGui::IsMouseDown(btn); }
	bool shim_IsMouseClicked(int btn, bool repeat) { return ImGui::IsMouseClicked(btn, repeat); }
	bool shim_IsMouseReleased(int btn) { return ImGui::IsMouseReleased(btn); }
	bool shim_IsMouseDoubleClicked(int btn) { return ImGui::IsMouseDoubleClicked(btn); }
	bool shim_IsMouseHoveringRect(float minx, float miny, float maxx, float maxy, bool clip) { return ImGui::IsMouseHoveringRect(ImVec2(minx, miny), ImVec2(maxx, maxy), clip); }
	bool shim_IsMouseDragging(int btn, float threshold) { return ImGui::IsMouseDragging(btn, threshold); }
	void shim_GetMousePos(float* x, float* y) { ImVec2 v = ImGui::GetMousePos(); *x = v.x; *y = v.y; }
	void shim_GetMouseDragDelta(float* x, float* y, int btn, float threshold) { ImVec2 v = ImGui::GetMouseDragDelta(btn, threshold); *x = v.x; *y = v.y; }
	void shim_ResetMouseDragDelta(int btn) { ImGui::ResetMouseDragDelta(btn); }
	int shim_GetMouseCursor() { return ImGui::GetMouseCursor(); }
	void shim_SetMouseCursor(int cursor) { ImGui::SetMouseCursor(cursor); }
	unsigned char shim_IsKeyChordPressed(int kc) { return ImGui::IsKeyChordPressed((ImGuiKeyChord)kc); }
	void  shim_SetNextFrameWantCaptureKeyboard(unsigned char w) { ImGui::SetNextFrameWantCaptureKeyboard(w != 0); }
	unsigned char shim_Shortcut(int kc, int flags) { return ImGui::Shortcut((ImGuiKeyChord)kc, (ImGuiInputFlags)flags); }
	void  shim_SetNextItemShortcut(int kc, int flags) { ImGui::SetNextItemShortcut((ImGuiKeyChord)kc, (ImGuiInputFlags)flags); }
	unsigned char shim_SetItemKeyOwner(int k) { return ImGui::SetItemKeyOwner((ImGuiKey)k); }
	unsigned char shim_IsMouseReleasedWithDelay(int btn, float d) { return ImGui::IsMouseReleasedWithDelay((ImGuiMouseButton)btn, d); }
	int   shim_GetMouseClickedCount(int btn) { return ImGui::GetMouseClickedCount((ImGuiMouseButton)btn); }
	unsigned char shim_IsMousePosValid(float* pos)
	{
		if (!pos) return ImGui::IsMousePosValid();
		ImVec2 p(pos[0], pos[1]);
		return ImGui::IsMousePosValid(&p);
	}
	unsigned char shim_IsAnyMouseDown() { return ImGui::IsAnyMouseDown(); }
	void shim_GetMousePosOnOpeningCurrentPopup(float* x, float* y) { ImVec2 p = ImGui::GetMousePosOnOpeningCurrentPopup(); *x = p.x; *y = p.y; }
	void shim_SetNextFrameWantCaptureMouse(unsigned char w) { ImGui::SetNextFrameWantCaptureMouse(w != 0); }

	// ============================================================
	// CLIPBOARD
	// ============================================================
	const char* shim_GetClipboardText() { return ImGui::GetClipboardText(); }
	void shim_SetClipboardText(const char* text) { ImGui::SetClipboardText(text); }

	// ============================================================
	// INI SETTINGS
	// ============================================================
	void shim_LoadIniSettingsFromDisk(const char* file) { ImGui::LoadIniSettingsFromDisk(file); }
	void shim_LoadIniSettingsFromMemory(const char* data, size_t size) { ImGui::LoadIniSettingsFromMemory(data, size); }
	void shim_SaveIniSettingsToDisk(const char* file) { ImGui::SaveIniSettingsToDisk(file); }
	const char* shim_SaveIniSettingsToMemory(size_t* out_size) { return ImGui::SaveIniSettingsToMemory(out_size); }

	// ============================================================
	// MEMORY
	// ============================================================
#ifndef IMGUI_DISABLE_OBSOLETE_FUNCTIONS
	void* shim_MemAlloc(size_t size) { return ImGui::MemAlloc(size); }
	void shim_MemFree(void* ptr) { ImGui::MemFree(ptr); }
#endif

	// ============================================================
	// DX9 BACKEND
	// ============================================================
	bool shim_ImplDX9_Init(void* device) { return ImGui_ImplDX9_Init((IDirect3DDevice9*)device); }
	void shim_ImplDX9_Shutdown() { ImGui_ImplDX9_Shutdown(); }
	void shim_ImplDX9_NewFrame() { ImGui_ImplDX9_NewFrame(); }
	void shim_ImplDX9_RenderDrawData(ImDrawData* draw_data) { ImGui_ImplDX9_RenderDrawData(draw_data); }
	bool shim_ImplDX9_CreateDeviceObjects() { return ImGui_ImplDX9_CreateDeviceObjects(); }
	void shim_ImplDX9_InvalidateDeviceObjects() { ImGui_ImplDX9_InvalidateDeviceObjects(); }

	// ============================================================
	// DX11 BACKEND
	// ============================================================
	bool shim_ImplDX11_Init(ID3D11Device* dev, ID3D11DeviceContext* ctx) { return ImGui_ImplDX11_Init(dev, ctx); }
	void shim_ImplDX11_Shutdown() { ImGui_ImplDX11_Shutdown(); }
	void shim_ImplDX11_NewFrame() { ImGui_ImplDX11_NewFrame(); }
	void shim_ImplDX11_RenderDrawData(ImDrawData* draw_data) { ImGui_ImplDX11_RenderDrawData(draw_data); }
	bool shim_ImplDX11_CreateDeviceObjects() { return ImGui_ImplDX11_CreateDeviceObjects(); }
	void shim_ImplDX11_InvalidateDeviceObjects() { ImGui_ImplDX11_InvalidateDeviceObjects(); }

	// ============================================================
	// DX12 BACKEND
	// ============================================================
	static ID3D12DescriptorHeap* g_SrvHeap = nullptr;
	static UINT g_SrvDescSize = 0;
	static UINT g_SrvHeapSize = 0;
	static UINT g_SrvNextSlot = 0;

	static void SrvAlloc(ImGui_ImplDX12_InitInfo* info,
		D3D12_CPU_DESCRIPTOR_HANDLE* out_cpu,
		D3D12_GPU_DESCRIPTOR_HANDLE* out_gpu)
	{
		if (g_SrvNextSlot >= g_SrvHeapSize)
		{
			out_cpu->ptr = 0;
			out_gpu->ptr = 0;
			return;
		}

		D3D12_CPU_DESCRIPTOR_HANDLE base_cpu = g_SrvHeap->GetCPUDescriptorHandleForHeapStart();
		D3D12_GPU_DESCRIPTOR_HANDLE base_gpu = g_SrvHeap->GetGPUDescriptorHandleForHeapStart();
		UINT slot = g_SrvNextSlot++;
		out_cpu->ptr = base_cpu.ptr + slot * g_SrvDescSize;
		out_gpu->ptr = base_gpu.ptr + slot * g_SrvDescSize;
	}

	static void SrvFree(ImGui_ImplDX12_InitInfo* info,
		D3D12_CPU_DESCRIPTOR_HANDLE cpu,
		D3D12_GPU_DESCRIPTOR_HANDLE gpu)
	{
		// TODO: bump allocator без reclaim.
	}

	bool shim_ImplDX12_Init(
		ID3D12Device* device,
		ID3D12CommandQueue* queue,
		int framesInFlight,
		int rtvFormat,
		ID3D12DescriptorHeap* srvHeap,
		unsigned int srvHeapSize)
	{
		g_SrvHeap = srvHeap;
		g_SrvHeapSize = srvHeapSize;
		g_SrvNextSlot = 0;
		g_SrvDescSize = device->GetDescriptorHandleIncrementSize(
			D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV);

		ImGui_ImplDX12_InitInfo info;
		info.Device = device;
		info.CommandQueue = queue;
		info.NumFramesInFlight = framesInFlight;
		info.RTVFormat = (DXGI_FORMAT)rtvFormat;
		info.DSVFormat = DXGI_FORMAT_UNKNOWN;
		info.SrvDescriptorHeap = srvHeap;
		info.SrvDescriptorAllocFn = &SrvAlloc;
		info.SrvDescriptorFreeFn = &SrvFree;

		return ImGui_ImplDX12_Init(&info);
	}

	void shim_ImplDX12_Shutdown(void) { ImGui_ImplDX12_Shutdown(); }
	void shim_ImplDX12_NewFrame(void) { ImGui_ImplDX12_NewFrame(); }

	void shim_ImplDX12_RenderDrawData(
		ImDrawData* drawData,
		ID3D12GraphicsCommandList* commandList)
	{
		ImGui_ImplDX12_RenderDrawData(drawData, commandList);
	}

	bool shim_ImplDX12_CreateDeviceObjects(void)
	{
		return ImGui_ImplDX12_CreateDeviceObjects();
	}

	void shim_ImplDX12_InvalidateDeviceObjects(void)
	{
		ImGui_ImplDX12_InvalidateDeviceObjects();
	}

	// ============================================================
	// OPENGL3 BACKEND
	// ============================================================
	bool shim_ImplOpenGL3_Init(const char* glsl_version) { return ImGui_ImplOpenGL3_Init(glsl_version); }
	void shim_ImplOpenGL3_Shutdown() { ImGui_ImplOpenGL3_Shutdown(); }
	void shim_ImplOpenGL3_NewFrame() { ImGui_ImplOpenGL3_NewFrame(); }
	void shim_ImplOpenGL3_RenderDrawData(ImDrawData* draw_data) { ImGui_ImplOpenGL3_RenderDrawData(draw_data); }


	// ============================================================
	// D3D12 HELPERS
	// ============================================================
	unsigned long long shim_D3D12_GetCPUDescriptorHandleForHeapStart(void* heap)
	{
		return (unsigned long long)((ID3D12DescriptorHeap*)heap)->GetCPUDescriptorHandleForHeapStart().ptr;
	}

	unsigned long long shim_D3D12_GetGPUDescriptorHandleForHeapStart(void* heap)
	{
		return (unsigned long long)((ID3D12DescriptorHeap*)heap)->GetGPUDescriptorHandleForHeapStart().ptr;
	}

	void shim_D3D12_CreateRenderTargetView(void* device, void* resource, void* desc, unsigned long long cpuHandle)
	{
		D3D12_CPU_DESCRIPTOR_HANDLE h;
		h.ptr = (SIZE_T)cpuHandle;
		((ID3D12Device*)device)->CreateRenderTargetView((ID3D12Resource*)resource, (const D3D12_RENDER_TARGET_VIEW_DESC*)desc, h);
	}

	void shim_D3D12_OMSetRenderTargets(void* cmdList, unsigned int numRTVs, unsigned long long* rtvHandles, void* dsv)
	{
		D3D12_CPU_DESCRIPTOR_HANDLE handles[8];
		unsigned int n = numRTVs > 8 ? 8 : numRTVs;
		for (unsigned int i = 0; i < n; i++) handles[i].ptr = (SIZE_T)rtvHandles[i];
		D3D12_CPU_DESCRIPTOR_HANDLE* dsvHandle = dsv ? (D3D12_CPU_DESCRIPTOR_HANDLE*)dsv : nullptr;
		((ID3D12GraphicsCommandList*)cmdList)->OMSetRenderTargets(n, handles, FALSE, dsvHandle);
	}

	void shim_D3D12_ResourceBarrier(void* cmdList, unsigned int num, void* barriers)
	{
		((ID3D12GraphicsCommandList*)cmdList)->ResourceBarrier(num, (const D3D12_RESOURCE_BARRIER*)barriers);
	}

	void shim_D3D12_CommandListReset(void* cmdList, void* allocator, void* pso)
	{
		((ID3D12GraphicsCommandList*)cmdList)->Reset((ID3D12CommandAllocator*)allocator, (ID3D12PipelineState*)pso);
	}

	int shim_D3D12_CommandListClose(void* cmdList)
	{
		return (int)((ID3D12GraphicsCommandList*)cmdList)->Close();
	}

	int shim_D3D12_CommandAllocatorReset(void* allocator)
	{
		return (int)((ID3D12CommandAllocator*)allocator)->Reset();
	}

	void shim_D3D12_ExecuteCommandLists(void* queue, unsigned int numLists, void** lists)
	{
		((ID3D12CommandQueue*)queue)->ExecuteCommandLists(numLists, (ID3D12CommandList* const*)lists);
	}

	int shim_D3D12_QueueSignal(void* queue, void* fence, unsigned long long value)
	{
		return (int)((ID3D12CommandQueue*)queue)->Signal((ID3D12Fence*)fence, value);
	}

	unsigned long long shim_D3D12_FenceGetCompletedValue(void* fence)
	{
		return ((ID3D12Fence*)fence)->GetCompletedValue();
	}

	void shim_D3D12_SetDescriptorHeaps(void* cmdList, unsigned int numHeaps, void** heaps)
	{
		((ID3D12GraphicsCommandList*)cmdList)->SetDescriptorHeaps(numHeaps, (ID3D12DescriptorHeap* const*)heaps);
	}

	int shim_D3D12_CommandQueueGetType(void* queue)
	{
		D3D12_COMMAND_QUEUE_DESC d = ((ID3D12CommandQueue*)queue)->GetDesc();
		return (int)d.Type;
	}

	// ============================================================
	// WIN32 BACKEND
	// ============================================================
	bool shim_ImplWin32_Init(void* hwnd) { return ImGui_ImplWin32_Init(hwnd); }
	void shim_ImplWin32_Shutdown() { ImGui_ImplWin32_Shutdown(); }
	void shim_ImplWin32_NewFrame() { ImGui_ImplWin32_NewFrame(); }
	//extern IMGUI_IMPL_API LRESULT ImGui_ImplWin32_WndProcHandler(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam);


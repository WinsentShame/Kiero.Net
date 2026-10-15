#include <windows.h>
#include <d3d11.h>
#include <d3d12.h>
#include <cstdint>
#include <cstdio>

#include "shim.h"
#include "imgui.h"
#include "imgui_impl_dx11.h"
#include "imgui_impl_dx12.h"
#include "imgui_impl_win32.h"


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

	// ============================================================
	// WINDOW MANIPULATION
	// ============================================================
	void shim_SetNextWindowPos(float x, float y, int cond, float px, float py) { ImGui::SetNextWindowPos(ImVec2(x, y), cond, ImVec2(px, py)); }
	void shim_SetNextWindowSize(float w, float h, int cond) { ImGui::SetNextWindowSize(ImVec2(w, h), cond); }
	void shim_SetNextWindowSizeConstraints(float minx, float miny, float maxx, float maxy) { ImGui::SetNextWindowSizeConstraints(ImVec2(minx, miny), ImVec2(maxx, maxy)); }
	void shim_SetNextWindowContentSize(float w, float h) { ImGui::SetNextWindowContentSize(ImVec2(w, h)); }
	void shim_SetNextWindowCollapsed(bool collapsed, int cond) { ImGui::SetNextWindowCollapsed(collapsed, cond); }
	void shim_SetNextWindowFocus() { ImGui::SetNextWindowFocus(); }
	void shim_SetNextWindowScroll(float sx, float sy) { ImGui::SetNextWindowScroll(ImVec2(sx, sy)); }
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
	bool shim_ColorEdit3(const char* label, float* col, int flags) { return ImGui::ColorEdit3(label, col, flags); }
	bool shim_ColorEdit4(const char* label, float* col, int flags) { return ImGui::ColorEdit4(label, col, flags); }
	bool shim_ColorPicker3(const char* label, float* col, int flags) { return ImGui::ColorPicker3(label, col, flags); }
	bool shim_ColorPicker4(const char* label, float* col, int flags, const float* ref_col) { return ImGui::ColorPicker4(label, col, flags, ref_col); }
	bool shim_ColorButton(const char* id, float r, float g, float b, float a, int flags, float w, float h) { return ImGui::ColorButton(id, ImVec4(r, g, b, a), flags, ImVec2(w, h)); }

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
			__debugbreak();
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
		// Bump allocator: no reclaim.
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


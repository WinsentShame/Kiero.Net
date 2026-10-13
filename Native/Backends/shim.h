#pragma once
#include <cstdint>
#include <cstddef>

struct ImGuiContext;
struct ImFontAtlas;
struct ImFont;
struct ImGuiIO;
struct ImGuiPlatformIO;
struct ImGuiStyle;
struct ImDrawData;
struct ImDrawList;
struct ImGuiViewport;
struct ImGuiPayload;

struct ID3D11Device;
struct ID3D11DeviceContext;
struct ID3D12Device;
struct ID3D12CommandQueue;
struct ID3D12DescriptorHeap;
struct ID3D12GraphicsCommandList;

#ifdef __cplusplus
extern "C" {
#endif

	// ============================================================
	// CONTEXT
	// ============================================================
	ImGuiContext* shim_CreateContext(ImFontAtlas* shared);
	void          shim_DestroyContext(ImGuiContext* ctx);
	ImGuiContext* shim_GetCurrentContext();
	void          shim_SetCurrentContext(ImGuiContext* ctx);

	// ============================================================
	// IO
	// ============================================================
	ImGuiIO* shim_GetIO();
	ImGuiPlatformIO* shim_GetPlatformIO();
	ImGuiStyle* shim_GetStyle();

	void shim_IO_SetDisplaySize(ImGuiIO* io, float w, float h);
	void shim_IO_GetDisplaySize(ImGuiIO* io, float* w, float* h);
	void shim_IO_SetDeltaTime(ImGuiIO* io, float dt);
	void shim_IO_AddMousePos(ImGuiIO* io, float x, float y);
	void shim_IO_AddMouseButton(ImGuiIO* io, int btn, bool down);
	void shim_IO_AddMouseWheel(ImGuiIO* io, float wx, float wy);
	void shim_IO_AddKey(ImGuiIO* io, int key, bool down);
	void shim_IO_AddInputCharacter(ImGuiIO* io, unsigned int c);
	void shim_IO_AddFocusEvent(ImGuiIO* io, bool focused);
	bool shim_IO_WantCaptureMouse(ImGuiIO* io);
	bool shim_IO_WantCaptureKeyboard(ImGuiIO* io);
	void shim_IO_SetConfigFlags(ImGuiIO* io, int flags);
	void shim_IO_SetIniFilename(ImGuiIO* io, const char* name);
	void shim_IO_SetLogFilename(ImGuiIO* io, const char* name);

	// ============================================================
	// FRAME
	// ============================================================
	void         shim_NewFrame();
	void         shim_EndFrame();
	void         shim_Render();
	ImDrawData* shim_GetDrawData();

	int shim_DrawData_GetVtxCount(ImDrawData* d);
	int shim_DrawData_GetIdxCount(ImDrawData* d);
	int shim_DrawData_GetCmdListsCount(ImDrawData* d);

	// ============================================================
	// DEMO / DEBUG
	// ============================================================
	void        shim_ShowDemoWindow(bool* p_open);
	void        shim_ShowMetricsWindow(bool* p_open);
	void        shim_ShowDebugLogWindow(bool* p_open);
	void        shim_ShowIDStackToolWindow(bool* p_open);
	void        shim_ShowAboutWindow(bool* p_open);
	void        shim_ShowStyleEditor(ImGuiStyle* ref);
	bool        shim_ShowStyleSelector(const char* label);
	void        shim_ShowFontSelector(const char* label);
	void        shim_ShowUserGuide();
	const char* shim_GetVersion();

	// ============================================================
	// STYLES
	// ============================================================
	void shim_StyleColorsDark(ImGuiStyle* dst);
	void shim_StyleColorsLight(ImGuiStyle* dst);
	void shim_StyleColorsClassic(ImGuiStyle* dst);

	// ============================================================
	// WINDOWS
	// ============================================================
	bool shim_Begin(const char* name, bool* p_open, int flags);
	void shim_End();
	bool shim_BeginChild(const char* str_id, float w, float h, int child_flags, int window_flags);
	bool shim_BeginChildID(unsigned int id, float w, float h, int child_flags, int window_flags);
	void shim_EndChild();

	bool        shim_IsWindowAppearing();
	bool        shim_IsWindowCollapsed();
	bool        shim_IsWindowFocused(int flags);
	bool        shim_IsWindowHovered(int flags);
	ImDrawList* shim_GetWindowDrawList();
	void        shim_GetWindowPos(float* x, float* y);
	void        shim_GetWindowSize(float* x, float* y);
	float       shim_GetWindowWidth();
	float       shim_GetWindowHeight();

	// ============================================================
	// WINDOW MANIPULATION
	// ============================================================
	void shim_SetNextWindowPos(float x, float y, int cond, float px, float py);
	void shim_SetNextWindowSize(float w, float h, int cond);
	void shim_SetNextWindowSizeConstraints(float minx, float miny, float maxx, float maxy);
	void shim_SetNextWindowContentSize(float w, float h);
	void shim_SetNextWindowCollapsed(bool collapsed, int cond);
	void shim_SetNextWindowFocus();
	void shim_SetNextWindowScroll(float sx, float sy);
	void shim_SetNextWindowBgAlpha(float alpha);

	// ============================================================
	// SCROLLING
	// ============================================================
	float shim_GetScrollX();
	float shim_GetScrollY();
	void  shim_SetScrollX(float x);
	void  shim_SetScrollY(float y);
	float shim_GetScrollMaxX();
	float shim_GetScrollMaxY();
	void  shim_SetScrollHereX(float r);
	void  shim_SetScrollHereY(float r);
	void  shim_SetScrollFromPosX(float x, float r);
	void  shim_SetScrollFromPosY(float y, float r);

	// ============================================================
	// FONT / STYLE STACKS
	// ============================================================
	void   shim_PushFont(ImFont* font, float size);
	void   shim_PopFont();
	ImFont* shim_GetFont();
	float  shim_GetFontSize();

	void         shim_PushStyleColorU32(int idx, unsigned int col);
	void         shim_PushStyleColorVec4(int idx, float r, float g, float b, float a);
	void         shim_PopStyleColor(int count);
	void         shim_PushStyleVar(int idx, float val);
	void         shim_PushStyleVarVec2(int idx, float x, float y);
	void         shim_PopStyleVar(int count);
	unsigned int shim_GetColorU32(int idx, float alpha);

	// ============================================================
	// ITEM WIDTH / TEXT WRAP
	// ============================================================
	void  shim_PushItemWidth(float w);
	void  shim_PopItemWidth();
	void  shim_SetNextItemWidth(float w);
	float shim_CalcItemWidth();
	void  shim_PushTextWrapPos(float x);
	void  shim_PopTextWrapPos();

	// ============================================================
	// CURSOR / LAYOUT
	// ============================================================
	void  shim_Separator();
	void  shim_SeparatorText(const char* label);
	void  shim_SameLine(float offset, float spacing);
	void  shim_NewLine();
	void  shim_Spacing();
	void  shim_Dummy(float w, float h);
	void  shim_Indent(float w);
	void  shim_Unindent(float w);
	void  shim_BeginGroup();
	void  shim_EndGroup();
	void  shim_AlignTextToFramePadding();
	float shim_GetTextLineHeight();
	float shim_GetTextLineHeightWithSpacing();
	float shim_GetFrameHeight();
	float shim_GetFrameHeightWithSpacing();

	void  shim_GetCursorPos(float* x, float* y);
	float shim_GetCursorPosX();
	float shim_GetCursorPosY();
	void  shim_SetCursorPos(float x, float y);
	void  shim_SetCursorPosX(float x);
	void  shim_SetCursorPosY(float y);
	void  shim_GetCursorStartPos(float* x, float* y);
	void  shim_GetCursorScreenPos(float* x, float* y);
	void  shim_SetCursorScreenPos(float x, float y);
	void  shim_GetContentRegionAvail(float* x, float* y);

	// ============================================================
	// ID STACK
	// ============================================================
	void         shim_PushIDStr(const char* id);
	void         shim_PushIDInt(int id);
	void         shim_PushIDPtr(const void* p);
	void         shim_PopID();
	unsigned int shim_GetIDStr(const char* id);
	unsigned int shim_GetIDInt(int id);

	// ============================================================
	// TEXT
	// ============================================================
	void shim_TextUnformatted(const char* text, const char* end);
	void shim_TextColored(float r, float g, float b, float a, const char* text);
	void shim_TextDisabled(const char* text);
	void shim_TextWrapped(const char* text);
	void shim_LabelText(const char* label, const char* text);
	void shim_BulletText(const char* text);
	void shim_Bullet();

	// ============================================================
	// MAIN WIDGETS
	// ============================================================
	bool shim_Button(const char* label, float w, float h);
	bool shim_SmallButton(const char* label);
	bool shim_InvisibleButton(const char* id, float w, float h, int flags);
	bool shim_ArrowButton(const char* id, int dir);
	bool shim_Checkbox(const char* label, bool* v);
	bool shim_RadioButtonBool(const char* label, bool active);
	bool shim_RadioButtonInt(const char* label, int* v, int v_button);
	void shim_ProgressBar(float fraction, float w, float h, const char* overlay);
	bool shim_TextLink(const char* label);
	bool shim_TextLinkOpenURL(const char* label, const char* url);

	// ============================================================
	// COMBO
	// ============================================================
	bool shim_BeginCombo(const char* label, const char* preview, int flags);
	void shim_EndCombo();

	// ============================================================
	// DRAG
	// ============================================================
	bool shim_DragFloat(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags);
	bool shim_DragFloat2(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags);
	bool shim_DragFloat3(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags);
	bool shim_DragFloat4(const char* label, float* v, float speed, float min, float max, const char* fmt, int flags);
	bool shim_DragInt(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags);
	bool shim_DragInt2(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags);
	bool shim_DragInt3(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags);
	bool shim_DragInt4(const char* label, int* v, float speed, int min, int max, const char* fmt, int flags);

	// ============================================================
	// SLIDER
	// ============================================================
	bool shim_SliderFloat(const char* label, float* v, float min, float max, const char* fmt, int flags);
	bool shim_SliderFloat2(const char* label, float* v, float min, float max, const char* fmt, int flags);
	bool shim_SliderFloat3(const char* label, float* v, float min, float max, const char* fmt, int flags);
	bool shim_SliderFloat4(const char* label, float* v, float min, float max, const char* fmt, int flags);
	bool shim_SliderInt(const char* label, int* v, int min, int max, const char* fmt, int flags);
	bool shim_SliderInt2(const char* label, int* v, int min, int max, const char* fmt, int flags);
	bool shim_SliderInt3(const char* label, int* v, int min, int max, const char* fmt, int flags);
	bool shim_SliderInt4(const char* label, int* v, int min, int max, const char* fmt, int flags);
	bool shim_SliderAngle(const char* label, float* v_rad, float min_deg, float max_deg, const char* fmt, int flags);
	bool shim_VSliderFloat(const char* label, float w, float h, float* v, float min, float max, const char* fmt, int flags);
	bool shim_VSliderInt(const char* label, float w, float h, int* v, int min, int max, const char* fmt, int flags);

	// ============================================================
	// INPUT
	// ============================================================
	bool shim_InputText(const char* label, char* buf, size_t size, int flags);
	bool shim_InputTextMultiline(const char* label, char* buf, size_t size, float w, float h, int flags);
	bool shim_InputFloat(const char* label, float* v, float step, float fast, const char* fmt, int flags);
	bool shim_InputInt(const char* label, int* v, int step, int fast, int flags);
	bool shim_InputDouble(const char* label, double* v, double step, double fast, const char* fmt, int flags);

	// ============================================================
	// COLOR
	// ============================================================
	bool shim_ColorEdit3(const char* label, float* col, int flags);
	bool shim_ColorEdit4(const char* label, float* col, int flags);
	bool shim_ColorPicker3(const char* label, float* col, int flags);
	bool shim_ColorPicker4(const char* label, float* col, int flags, const float* ref_col);
	bool shim_ColorButton(const char* id, float r, float g, float b, float a, int flags, float w, float h);

	// ============================================================
	// TREE
	// ============================================================
	bool  shim_TreeNode(const char* label);
	bool  shim_TreeNodeEx(const char* label, int flags);
	void  shim_TreePop();
	float shim_GetTreeNodeToLabelSpacing();
	bool  shim_CollapsingHeader(const char* label, int flags);
	bool  shim_CollapsingHeaderClosable(const char* label, bool* p_open, int flags);
	void  shim_SetNextItemOpen(bool is_open, int cond);

	// ============================================================
	// SELECTABLE
	// ============================================================
	bool shim_Selectable(const char* label, bool selected, int flags, float w, float h);
	bool shim_SelectablePtr(const char* label, bool* p_selected, int flags, float w, float h);

	// ============================================================
	// LISTBOX
	// ============================================================
	bool shim_BeginListBox(const char* label, float w, float h);
	void shim_EndListBox();

	// ============================================================
	// PLOTS
	// ============================================================
	void shim_PlotLines(const char* label, const float* values, int count, int offset, const char* overlay, float min, float max, float w, float h, int stride);
	void shim_PlotHistogram(const char* label, const float* values, int count, int offset, const char* overlay, float min, float max, float w, float h, int stride);

	// ============================================================
	// VALUE
	// ============================================================
	void shim_ValueBool(const char* prefix, bool b);
	void shim_ValueInt(const char* prefix, int v);
	void shim_ValueUInt(const char* prefix, unsigned int v);
	void shim_ValueFloat(const char* prefix, float v, const char* fmt);

	// ============================================================
	// MENU
	// ============================================================
	bool shim_BeginMenuBar();
	void shim_EndMenuBar();
	bool shim_BeginMainMenuBar();
	void shim_EndMainMenuBar();
	bool shim_BeginMenu(const char* label, bool enabled);
	void shim_EndMenu();
	bool shim_MenuItem(const char* label, const char* shortcut, bool selected, bool enabled);
	bool shim_MenuItemPtr(const char* label, const char* shortcut, bool* p_selected, bool enabled);

	// ============================================================
	// TOOLTIP
	// ============================================================
	bool shim_BeginTooltip();
	void shim_EndTooltip();
	void shim_SetTooltip(const char* text);
	bool shim_BeginItemTooltip();
	void shim_SetItemTooltip(const char* text);

	// ============================================================
	// POPUP
	// ============================================================
	bool shim_BeginPopup(const char* id, int flags);
	bool shim_BeginPopupModal(const char* name, bool* p_open, int flags);
	void shim_EndPopup();
	bool shim_OpenPopup(const char* id, int flags);
	bool shim_OpenPopupOnItemClick(const char* id, int flags);
	void shim_CloseCurrentPopup();
	bool shim_BeginPopupContextItem(const char* id, int flags);
	bool shim_BeginPopupContextWindow(const char* id, int flags);
	bool shim_BeginPopupContextVoid(const char* id, int flags);
	bool shim_IsPopupOpen(const char* id, int flags);

	// ============================================================
	// TABLES
	// ============================================================
	bool        shim_BeginTable(const char* id, int cols, int flags, float w, float h, float inner_w);
	void        shim_EndTable();
	void        shim_TableNextRow(int flags, float min_h);
	bool        shim_TableNextColumn();
	bool        shim_TableSetColumnIndex(int n);
	void        shim_TableSetupColumn(const char* label, int flags, float init_w, unsigned int user_data);
	void        shim_TableSetupScrollFreeze(int cols, int rows);
	void        shim_TableHeader(const char* label);
	void        shim_TableHeadersRow();
	void        shim_TableAngledHeadersRow();
	int         shim_TableGetColumnCount();
	int         shim_TableGetColumnIndex();
	int         shim_TableGetRowIndex();
	const char* shim_TableGetColumnName(int n);
	void        shim_TableSetBgColor(int target, unsigned int color, int col);

	// ============================================================
	// TAB BAR
	// ============================================================
	bool shim_BeginTabBar(const char* id, int flags);
	void shim_EndTabBar();
	bool shim_BeginTabItem(const char* label, bool* p_open, int flags);
	void shim_EndTabItem();
	bool shim_TabItemButton(const char* label, int flags);
	void shim_SetTabItemClosed(const char* label);

	// ============================================================
	// LOGGING
	// ============================================================
	void shim_LogToTTY(int depth);
	void shim_LogToFile(int depth, const char* file);
	void shim_LogToClipboard(int depth);
	void shim_LogFinish();
	void shim_LogButtons();
	void shim_LogText(const char* text);

	// ============================================================
	// DRAG DROP
	// ============================================================
	bool              shim_BeginDragDropSource(int flags);
	bool              shim_SetDragDropPayload(const char* type, const void* data, size_t sz, int cond);
	void              shim_EndDragDropSource();
	bool              shim_BeginDragDropTarget();
	const ImGuiPayload* shim_AcceptDragDropPayload(const char* type, int flags);
	void              shim_EndDragDropTarget();
	const ImGuiPayload* shim_GetDragDropPayload();

	// ============================================================
	// DISABLING
	// ============================================================
	void shim_BeginDisabled(bool disabled);
	void shim_EndDisabled();

	// ============================================================
	// CLIPPING
	// ============================================================
	void shim_PushClipRect(float minx, float miny, float maxx, float maxy, bool intersect);
	void shim_PopClipRect();

	// ============================================================
	// FOCUS / NAV
	// ============================================================
	void shim_SetItemDefaultFocus();
	void shim_SetKeyboardFocusHere(int offset);
	void shim_SetNavCursorVisible(bool visible);
	void shim_SetNextItemAllowOverlap();

	// ============================================================
	// ITEM QUERIES
	// ============================================================
	bool         shim_IsItemHovered(int flags);
	bool         shim_IsItemActive();
	bool         shim_IsItemFocused();
	bool         shim_IsItemClicked(int btn);
	bool         shim_IsItemVisible();
	bool         shim_IsItemEdited();
	bool         shim_IsItemActivated();
	bool         shim_IsItemDeactivated();
	bool         shim_IsItemDeactivatedAfterEdit();
	bool         shim_IsItemToggledOpen();
	bool         shim_IsAnyItemHovered();
	bool         shim_IsAnyItemActive();
	bool         shim_IsAnyItemFocused();
	unsigned int shim_GetItemID();
	void         shim_GetItemRectMin(float* x, float* y);
	void         shim_GetItemRectMax(float* x, float* y);
	void         shim_GetItemRectSize(float* x, float* y);

	// ============================================================
	// VIEWPORT / DRAWLISTS
	// ============================================================
	ImGuiViewport* shim_GetMainViewport();
	ImDrawList* shim_GetBackgroundDrawList();
	ImDrawList* shim_GetForegroundDrawList();

	// ============================================================
	// MISC
	// ============================================================
	bool        shim_IsRectVisible(float w, float h);
	double      shim_GetTime();
	int         shim_GetFrameCount();
	const char* shim_GetStyleColorName(int idx);
	void        shim_CalcTextSize(float* ox, float* oy, const char* text, const char* end, bool hide_hash, float wrap_w);

	// ============================================================
	// INPUT QUERIES
	// ============================================================
	bool        shim_IsKeyDown(int key);
	bool        shim_IsKeyPressed(int key, bool repeat);
	bool        shim_IsKeyReleased(int key);
	int         shim_GetKeyPressedAmount(int key, float delay, float rate);
	const char* shim_GetKeyName(int key);
	bool        shim_IsMouseDown(int btn);
	bool        shim_IsMouseClicked(int btn, bool repeat);
	bool        shim_IsMouseReleased(int btn);
	bool        shim_IsMouseDoubleClicked(int btn);
	bool        shim_IsMouseHoveringRect(float minx, float miny, float maxx, float maxy, bool clip);
	bool        shim_IsMouseDragging(int btn, float threshold);
	void        shim_GetMousePos(float* x, float* y);
	void        shim_GetMouseDragDelta(float* x, float* y, int btn, float threshold);
	void        shim_ResetMouseDragDelta(int btn);
	int         shim_GetMouseCursor();
	void        shim_SetMouseCursor(int cursor);

	// ============================================================
	// CLIPBOARD
	// ============================================================
	const char* shim_GetClipboardText();
	void        shim_SetClipboardText(const char* text);

	// ============================================================
	// INI SETTINGS
	// ============================================================
	void        shim_LoadIniSettingsFromDisk(const char* file);
	void        shim_LoadIniSettingsFromMemory(const char* data, size_t size);
	void        shim_SaveIniSettingsToDisk(const char* file);
	const char* shim_SaveIniSettingsToMemory(size_t* out_size);

	// ============================================================
	// MEMORY
	// ============================================================
#ifndef IMGUI_DISABLE_OBSOLETE_FUNCTIONS
	void* shim_MemAlloc(size_t size);
	void  shim_MemFree(void* ptr);
#endif

	// ============================================================
	// DX11 BACKEND
	// ============================================================
	bool shim_ImplDX11_Init(ID3D11Device* dev, ID3D11DeviceContext* ctx);
	void shim_ImplDX11_Shutdown();
	void shim_ImplDX11_NewFrame();
	void shim_ImplDX11_RenderDrawData(ImDrawData* draw_data);
	bool shim_ImplDX11_CreateDeviceObjects();
	void shim_ImplDX11_InvalidateDeviceObjects();

	// ============================================================
	// DX12 BACKEND
	// ============================================================
	bool shim_ImplDX12_Init(
		ID3D12Device* device,
		ID3D12CommandQueue* queue,
		int framesInFlight,
		int rtvFormat,
		ID3D12DescriptorHeap* srvHeap,
		unsigned int srvHeapSize);

	void shim_ImplDX12_Shutdown();
	void shim_ImplDX12_NewFrame();
	void shim_ImplDX12_RenderDrawData(ImDrawData* drawData, ID3D12GraphicsCommandList* commandList);
	bool shim_ImplDX12_CreateDeviceObjects();
	void shim_ImplDX12_InvalidateDeviceObjects();

	// ============================================================
	// WIN32 BACKEND
	// ============================================================
	bool     shim_ImplWin32_Init(void* hwnd);
	void     shim_ImplWin32_Shutdown();
	void     shim_ImplWin32_NewFrame();

#ifdef __cplusplus
}
#endif

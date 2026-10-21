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
	void			shim_ShowDemoWindow(bool* p_open);
	void			shim_ShowMetricsWindow(bool* p_open);
	void			shim_ShowDebugLogWindow(bool* p_open);
	void			shim_ShowIDStackToolWindow(bool* p_open);
	void			shim_ShowAboutWindow(bool* p_open);
	void			shim_ShowStyleEditor(ImGuiStyle* ref);
	bool			shim_ShowStyleSelector(const char* label);
	void			shim_ShowFontSelector(const char* label);
	void			shim_ShowUserGuide();
	const char*		shim_GetVersion();
	void			shim_DebugTextEncoding(const char* text);
	void			shim_DebugFlashStyleColor(int idx);
	void			shim_DebugStartItemPicker();
	unsigned char	shim_DebugCheckVersionAndDataLayout(const char* version, unsigned long long sz_io, unsigned long long sz_style, unsigned long long sz_vec2, unsigned long long sz_vec4, unsigned long long sz_vert, unsigned long long sz_idx);

	// =====================================================================
	// MULTI-VIEWPORT
	// =====================================================================
	void    shim_UpdatePlatformWindows();
	void    shim_RenderPlatformWindowsDefault();
	void    shim_DestroyPlatformWindows();
	void*	shim_FindViewportByID(unsigned int viewport_id);
	void*	shim_FindViewportByPlatformHandle(void* handle);

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
	void shim_SetNextWindowScroll(float x, float y);
	void shim_SetNextWindowViewport(unsigned int viewport_id);
	void shim_SetWindowPosStr(const char* name, float x, float y, int cond);
	void shim_SetWindowSizeStr(const char* name, float w, float h, int cond);
	void shim_SetWindowCollapsedStr(const char* name, unsigned char collapsed, int cond);
	void shim_SetWindowFocusStr(const char* name);
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
	float		shim_GetWindowDpiScale();
	void*		shim_GetWindowViewport();

	// ============================================================
	// WINDOW MANIPULATION
	// ============================================================
	void shim_SetNextWindowPos(float x, float y, int cond, float px, float py);
	void shim_SetNextWindowSize(float w, float h, int cond);
	void shim_SetNextWindowSizeConstraints(float minx, float miny, float maxx, float maxy);
	void shim_SetNextWindowContentSize(float w, float h);
	void shim_SetNextWindowCollapsed(bool collapsed, int cond);
	void shim_SetNextWindowFocus();
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
	void		 shim_PushIDStrRange(const char* begin, const char* end);
	unsigned int shim_GetIDStrRange(const char* begin, const char* end);
	unsigned int shim_GetIDPtr(void* p);

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
	bool			shim_Button(const char* label, float w, float h);
	bool			shim_SmallButton(const char* label);
	bool			shim_InvisibleButton(const char* id, float w, float h, int flags);
	bool			shim_ArrowButton(const char* id, int dir);
	bool			shim_Checkbox(const char* label, bool* v);
	bool			shim_RadioButtonBool(const char* label, bool active);
	bool			shim_RadioButtonInt(const char* label, int* v, int v_button);
	void			shim_ProgressBar(float fraction, float w, float h, const char* overlay);
	bool			shim_TextLink(const char* label);
	bool			shim_TextLinkOpenURL(const char* label, const char* url);
	void			shim_Text(const char* text);
	unsigned char	shim_CheckboxFlagsInt(const char* label, int* flags, int flags_value);
	unsigned char	shim_CheckboxFlagsUInt(const char* label, unsigned int* flags, unsigned int flags_value);
	void			shim_Image(unsigned long long tex_id, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y);
	void			shim_ImageWithBg(unsigned long long tex_id, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y, float bgr, float bgg, float bgb, float bga, float tr, float tg, float tb, float ta);
	unsigned char	shim_ImageButton(const char* str_id, unsigned long long tex_id, float w, float h, float uv0x, float uv0y, float uv1x, float uv1y, float bgr, float bgg, float bgb, float bga, float tr, float tg, float tb, float ta);
	unsigned char	shim_ComboCallback(const char* label, int* current_item, const char* (*getter)(void* user_data, int idx), void* user_data, int items_count, int popup_max_height);
	unsigned char	shim_ComboZeros(const char* label, int* current_item, const char* items_separated_by_zeros, int popup_max_height);
	unsigned char	shim_DragFloatRange2(const char* label, float* v_cur_min, float* v_cur_max, float speed, float v_min, float v_max, const char* fmt, const char* fmt_max, int flags);
	unsigned char	shim_DragIntRange2(const char* label, int* v_cur_min, int* v_cur_max, float speed, int v_min, int v_max, const char* fmt, const char* fmt_max, int flags);
	unsigned char	shim_DragScalar(const char* label, int data_type, void* p_data, float speed, const void* p_min, const void* p_max, const char* fmt, int flags);
	unsigned char	shim_DragScalarN(const char* label, int data_type, void* p_data, int components, float speed, const void* p_min, const void* p_max, const char* fmt, int flags);
	unsigned char	shim_SliderScalar(const char* label, int data_type, void* p_data, const void* p_min, const void* p_max, const char* fmt, int flags);
	unsigned char	shim_SliderScalarN(const char* label, int data_type, void* p_data, int components, const void* p_min, const void* p_max, const char* fmt, int flags);
	unsigned char	shim_VSliderScalar(const char* label, float w, float h, int data_type, void* p_data, const void* p_min, const void* p_max, const char* fmt, int flags);
	unsigned char	shim_InputTextWithHint(const char* label, const char* hint, char* buf, unsigned long long size, int flags);
	unsigned char	shim_InputFloat2(const char* label, float* v, const char* fmt, int flags);
	unsigned char	shim_InputFloat3(const char* label, float* v, const char* fmt, int flags);
	unsigned char	shim_InputFloat4(const char* label, float* v, const char* fmt, int flags);
	unsigned char	shim_InputInt2(const char* label, int* v, int flags);
	unsigned char	shim_InputInt3(const char* label, int* v, int flags);
	unsigned char	shim_InputInt4(const char* label, int* v, int flags);
	unsigned char	shim_InputScalar(const char* label, int data_type, void* p_data, const void* p_step, const void* p_step_fast, const char* fmt, int flags);
	unsigned char	shim_InputScalarN(const char* label, int data_type, void* p_data, int components, const void* p_step, const void* p_step_fast, const char* fmt, int flags);
	unsigned char	shim_TreeNodeStrId(const char* str_id, const char* fmt);
	unsigned char	shim_TreeNodePtrId(const void* ptr_id, const char* fmt);
	unsigned char	shim_TreeNodeExStrId(const char* str_id, int flags, const char* fmt);
	unsigned char	shim_TreeNodeExPtrId(const void* ptr_id, int flags, const char* fmt);
	void			shim_TreePushStr(const char* str_id);
	void			shim_TreePushPtr(const void* ptr_id);
	void			shim_SetNextItemStorageID(unsigned int storage_id);
	unsigned char	shim_TreeNodeGetOpen(unsigned int storage_id);
	void*			shim_BeginMultiSelect(int flags, int selection_size, int items_count);
	void*			shim_EndMultiSelect();
	void			shim_SetNextItemSelectionUserData(long long selection_user_data);
	unsigned char	shim_IsItemToggledSelection();
	unsigned char	shim_ListBoxArray(const char* label, int* current_item, const char* const items[], int items_count, int height_in_items);
	unsigned char	shim_ListBoxCallback(const char* label, int* current_item, const char* (*getter)(void* user_data, int idx), void* user_data, int items_count, int height_in_items);

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
	bool			shim_InputTextMultiline(const char* label, char* buf, size_t size, float w, float h, int flags);
	bool			shim_InputFloat(const char* label, float* v, float step, float fast, const char* fmt, int flags);
	bool			shim_InputInt(const char* label, int* v, int step, int fast, int flags);
	bool			shim_InputDouble(const char* label, double* v, double step, double fast, const char* fmt, int flags);
    unsigned char	shim_IsKeyChordPressed(int key_chord);
    void			shim_SetNextFrameWantCaptureKeyboard(unsigned char want);
    unsigned char	shim_Shortcut(int key_chord, int flags);
    void			shim_SetNextItemShortcut(int key_chord, int flags);
    unsigned char	shim_SetItemKeyOwner(int key);
    unsigned char	shim_IsMouseReleasedWithDelay(int btn, float delay);
    int				shim_GetMouseClickedCount(int btn);
    unsigned char	shim_IsMousePosValid(float* pos);
    unsigned char	shim_IsAnyMouseDown();
    void			shim_GetMousePosOnOpeningCurrentPopup(float* x, float* y);
    void			shim_SetNextFrameWantCaptureMouse(unsigned char want);

	// ============================================================
	// COLOR
	// ============================================================
	bool		 shim_ColorEdit3(const char* label, float* col, int flags);
	bool		 shim_ColorEdit4(const char* label, float* col, int flags);
	bool		 shim_ColorPicker3(const char* label, float* col, int flags);
	bool		 shim_ColorPicker4(const char* label, float* col, int flags, const float* ref_col);
	bool		 shim_ColorButton(const char* id, float r, float g, float b, float a, int flags, float w, float h);extern "C" __declspec(dllexport) void    shim_ColorConvertU32ToFloat4(unsigned int in, float* out_rgba);
	unsigned int shim_ColorConvertFloat4ToU32(float r, float g, float b, float a);
	void		 shim_ColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v);
	void		 shim_ColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b);

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
	void*		shim_TableGetSortSpecs();
	int			shim_TableGetColumnFlags(int column_n);
	void		shim_TableSetColumnEnabled(int column_n, unsigned char v);
	int			shim_TableGetHoveredColumn();

	// =====================================================================
	// LEGACY COLUMNS
	// =====================================================================
	void    shim_Columns(int count, const char* id, unsigned char borders);
	void    shim_NextColumn();
	int     shim_GetColumnIndex();
	float   shim_GetColumnWidth(int column_index);
	void    shim_SetColumnWidth(int column_index, float width);
	float   shim_GetColumnOffset(int column_index);
	void    shim_SetColumnOffset(int column_index, float offset_x);
	int     shim_GetColumnsCount();

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

	// =====================================================================
	// DOCKING
	// =====================================================================
	unsigned int  shim_DockSpace(unsigned int dockspace_id, float w, float h, int flags, void* window_class);
	unsigned int  shim_DockSpaceOverViewport(unsigned int dockspace_id, void* viewport, int flags, void* window_class);
	void		  shim_SetNextWindowDockID(unsigned int dock_id, int cond);
	void		  shim_SetNextWindowClass(void* window_class);
	unsigned int  shim_GetWindowDockID();
	unsigned char shim_IsWindowDocked();

	// =====================================================================
	// ITEM UTILITIES
	// =====================================================================
	int     shim_GetItemFlags();
	int     shim_GetItemClickedCountWithSingleClickDelay(int btn, float delay);

	// ============================================================
	// VIEWPORT / DRAWLISTS
	// ============================================================
	ImGuiViewport* shim_GetMainViewport();
	ImDrawList* shim_GetBackgroundDrawList();
	ImDrawList* shim_GetForegroundDrawList();
	void shim_ImDrawList_AddRectFilled(ImDrawList* draw_list, float min_x, float min_y, float max_x, float max_y, unsigned int col, float rounding, int flags);
	void shim_ImDrawList_AddCircleFilled(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments);
	void shim_ImDrawList_AddText(ImDrawList* draw_list, float pos_x, float pos_y, unsigned int col, const char* text_begin, const char* text_end);
	void shim_ImDrawList_AddLine(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, unsigned int col, float thickness);
	void shim_ImDrawList_AddRect(ImDrawList* draw_list, float min_x, float min_y, float max_x, float max_y, unsigned int col, float rounding, int flags, float thickness);
	void shim_ImDrawList_AddRectFilledMultiColor(ImDrawList* draw_list, float min_x, float min_y, float max_x, float max_y, unsigned int col_upr_left, unsigned int col_upr_right, unsigned int col_bot_right, unsigned int col_bot_left);
	void shim_ImDrawList_AddTriangle(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, unsigned int col, float thickness);
	void shim_ImDrawList_AddTriangleFilled(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, unsigned int col);
	void shim_ImDrawList_AddQuad(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, float p4_x, float p4_y, unsigned int col, float thickness);
	void shim_ImDrawList_AddQuadFilled(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, float p4_x, float p4_y, unsigned int col);
	void shim_ImDrawList_AddCircle(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments, float thickness);
	void shim_ImDrawList_AddNgon(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments, float thickness);
	void shim_ImDrawList_AddNgonFilled(ImDrawList* draw_list, float center_x, float center_y, float radius, unsigned int col, int num_segments);
	void shim_ImDrawList_AddBezierCubic(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, float p4_x, float p4_y, unsigned int col, float thickness, int num_segments);
	void shim_ImDrawList_AddBezierQuadratic(ImDrawList* draw_list, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, unsigned int col, float thickness, int num_segments);
	void shim_ImDrawList_AddImage(ImDrawList* draw_list, unsigned long long user_texture_id, float p_min_x, float p_min_y, float p_max_x, float p_max_y);
	void shim_ImDrawList_AddImageRounded(ImDrawList* draw_list, unsigned long long user_texture_id, float p_min_x, float p_min_y, float p_max_x, float p_max_y, float uv_min_x, float uv_min_y, float uv_max_x, float uv_max_y, unsigned int col, float rounding, int flags);

	// ============================================================
	// MISC
	// ============================================================
	bool        shim_IsRectVisible(float w, float h);
	double      shim_GetTime();
	int         shim_GetFrameCount();
	const char* shim_GetStyleColorName(int idx);
	void        shim_CalcTextSize(float* ox, float* oy, const char* text, const char* end, bool hide_hash, float wrap_w);
	void*		shim_GetDrawListSharedData();
	void		shim_SetStateStorage(void* storage);
	void*	    shim_GetStateStorage();

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
	// DX9 BACKEND
	// ============================================================
	bool shim_ImplDX9_Init(void* device);
	void shim_ImplDX9_Shutdown();
	void shim_ImplDX9_NewFrame();
	void shim_ImplDX9_RenderDrawData(ImDrawData* draw_data);
	bool shim_ImplDX9_CreateDeviceObjects();
	void shim_ImplDX9_InvalidateDeviceObjects();

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
	// OPENGL3 BACKEND
	// ============================================================
	bool shim_ImplOpenGL3_Init(const char* glsl_version);
	void shim_ImplOpenGL3_Shutdown();
	void shim_ImplOpenGL3_NewFrame();
	void shim_ImplOpenGL3_RenderDrawData(ImDrawData* draw_data);

	// ============================================================
	// D3D12 HELPERS (ABI-safe wrappers for C#)
	// ============================================================
	unsigned long long shim_D3D12_GetCPUDescriptorHandleForHeapStart(void* heap);
	unsigned long long shim_D3D12_GetGPUDescriptorHandleForHeapStart(void* heap);
	void shim_D3D12_CreateRenderTargetView(void* device, void* resource, void* desc, unsigned long long cpuHandle);
	void shim_D3D12_OMSetRenderTargets(void* cmdList, unsigned int numRTVs, unsigned long long* rtvHandles, void* dsv);
	void shim_D3D12_ResourceBarrier(void* cmdList, unsigned int num, void* barriers);
	void shim_D3D12_CommandListReset(void* cmdList, void* allocator, void* pso);
	int  shim_D3D12_CommandListClose(void* cmdList);
	int  shim_D3D12_CommandAllocatorReset(void* allocator);
	void shim_D3D12_ExecuteCommandLists(void* queue, unsigned int numLists, void** lists);
	int  shim_D3D12_QueueSignal(void* queue, void* fence, unsigned long long value);
	unsigned long long shim_D3D12_FenceGetCompletedValue(void* fence);
	void shim_D3D12_SetDescriptorHeaps(void* cmdList, unsigned int numHeaps, void** heaps);\
	int shim_D3D12_CommandQueueGetType(void* queue);

	// ============================================================
	// WIN32 BACKEND
	// ============================================================
	bool     shim_ImplWin32_Init(void* hwnd);
	void     shim_ImplWin32_Shutdown();
	void     shim_ImplWin32_NewFrame();

	// =====================================================================
	// PARAMS STACKS
	// =====================================================================
	void* shim_GetFontBaked();
	void  shim_PushStyleVarX(int idx, float val_x);
	void  shim_PushStyleVarY(int idx, float val_y);
	void  shim_PushItemFlag(int option, unsigned char enabled);
	void  shim_PopItemFlag();

	// =====================================================================
	// STYLE READ
	// =====================================================================
	void			shim_GetFontTexUvWhitePixel(float* x, float* y);
	void			shim_GetStyleColorVec4(int idx, float* rgba);
	unsigned int	shim_GetColorU32Vec4(float r, float g, float b, float a);

#ifdef __cplusplus
}
#endif

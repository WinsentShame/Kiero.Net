# Kiero.Net

C# library for injecting a Dear ImGui overlay into a game process.
Write your UI in C#, inject one DLL, and the menu renders on top of the game.

**Supported APIs:** Direct3D 9, Direct3D 10, Direct3D 11, Direct3D 12, OpenGL 3.
**Platform:** Windows 10/11, x64.
**Stack:** Dear ImGui + MinHook + Kiero, wrapped in C# with NativeAOT.

## Quick start

```csharp
Overlay.onDraw = () =>
{
    if (Ui.Begin("Menu"))
    {
        Ui.Text("Hello, world");
        Ui.SliderFloat("Speed", ref speed, 0f, 10f);
    }
    Ui.End();
};

Bootstrap.StartAsync(); // detects the API, installs hooks, shows the overlay
```

## Consuming the library

To integrate `Kiero.Net` into your own project, clone the repository into your
solution folder and reference the source project. The native parts are
statically linked, so the final NativeAOT output is a single `.dll` with no
external native dependencies.

### 1. Project Setup

Add a `ProjectReference` to your `.csproj` and configure the static linking:

```xml
<Project Sdk="Microsoft.NET.Sdk">
	<PropertyGroup>
		<TargetFramework>net9.0</TargetFramework>
		<AllowUnsafeBlocks>true</AllowUnsafeBlocks>
		<LangVersion>latest</LangVersion>
		<Nullable>disable</Nullable>
		<OutputType>Library</OutputType>
		<AssemblyName>MyOverlay</AssemblyName>
		<RootNamespace>MyOverlay</RootNamespace>
		<PublishAot>true</PublishAot>
	</PropertyGroup>

	<ItemGroup>
		<ProjectReference Include="..\Kiero.Net\Kiero.Net.csproj" />
	</ItemGroup>

	<ItemGroup>
		<DirectPInvoke Include="kiero_native" />
		<NativeLibrary Include="..\Kiero.Net\Native\kiero_native.lib" />

		<DirectPInvoke Include="minhook" />
		<NativeLibrary Include="..\Kiero.Net\Native\Minhook\Minhook.lib" />

		<LinkerArg Include="d3d11.lib" />
		<LinkerArg Include="d3d12.lib" />
		<LinkerArg Include="dxgi.lib" />
		<LinkerArg Include="user32.lib" />
		<LinkerArg Include="gdi32.lib" />
		<LinkerArg Include="ole32.lib" />
	</ItemGroup>
</Project>
```

**Why these settings are required:**
- **`DirectPInvoke`** tells the NativeAOT compiler (ILC) to bind the `kiero_native`
  and `minhook` entry points to the statically linked libraries, avoiding runtime
  `LoadLibrary` calls.
- **`NativeLibrary`** passes the `.lib` files to the MSVC linker so the native
  code is embedded directly into your final DLL.
- **`LinkerArg`** links the standard Windows libraries required by the native shim.

### 2. Entry Point

Create your entry point using `[UnmanagedCallersOnly]`. Because NativeAOT
compiles this to native code, it acts as the `DllMain` entry point when the
binary is loaded by an injector.

```csharp
using Kiero.Net;

[UnmanagedCallersOnly(EntryPoint = "DllProcessAttach")]
public static void DllAttach(nint hModule)
{
    Logger.Initialize();

    Overlay.onDraw = () =>
    {
        if (Ui.Begin("Menu"))
        {
            Ui.Text("Hello, world");
        }
        Ui.End();
    };

    InputHook.Install(hModule);
    Bootstrap.StartAsync();
}
```

### 3. Build and Inject

```bash
dotnet publish -c Release -r win-x64
```

The output will be a single `MyOverlay.dll` located in
`bin/Release/net9.0/win-x64/publish/`. Inject this file into your target
process using any standard injector (e.g., Extreme Injector, Process Hacker).

# Kiero.Net UI API Reference

This document provides a comprehensive and strict reference for the `Kiero.Net.Ui` static class. The wrapper abstracts the C++ Dear ImGui API into C#, handling string marshalling (`Utf8`), pointer pinning, and struct conversions (`Vector2`, `Vector3`, `Vector4`) automatically.

---

## 1. Constants and Enums

The API uses static classes to expose ImGui flags and IDs as integers.

### Window Flags (`Ui.Win`)
| Flag | Description |
| :--- | :--- |
| `None` | No flags. |
| `NoTitleBar` | Disable title bar. |
| `NoResize` | Disable user resizing. |
| `NoMove` | Disable user moving the window. |
| `NoScrollbar` | Disable scrollbars. |
| `NoCollapse` | Disable collapsing via double-click. |
| `AlwaysAutoResize` | Resize window to fit content automatically. |
| `NoBackground` | Disable background drawing (useful for transparent overlays). |
| `NoDecoration` | Combination of `NoTitleBar \| NoResize \| NoScrollbar \| NoCollapse`. |

### Style Variables (`Ui.StyleVar`)
Used with `PushStyleVar`.
*Examples:* `Alpha`, `WindowPadding`, `WindowRounding`, `WindowBorderSize`, `FrameRounding`, `ItemSpacing`, `ScrollbarSize`, `GrabMinSize`.

### Colors (`Ui.Col`)
Used with `PushStyleColor`.
*Examples:* `Text`, `TextDisabled`, `WindowBg`, `ChildBg`, `PopupBg`, `Border`, `FrameBg`, `Button`, `ButtonHovered`, `ButtonActive`, `Header`, `Separator`.

### Condition Flags (`Ui.Cond`)
Used for `SetNextWindowPos` and `SetNextWindowSize`.
| Flag | Description |
| :--- | :--- |
| `Always` | Apply the value every frame. |
| `Once` | Apply once. |
| `FirstUseEver` | Apply only the first time the window is created. |
| `Appearing` | Apply when the window appears (e.g., after being collapsed). |

### Child Flags (`Ui.Child`)
Used with `BeginChild`.
| Flag | Description |
| :--- | :--- |
| `None` | No flags. |
| `Border` | Draw a border around the child window. |
| `AlwaysUseWindowPadding` | Inherit parent window padding. |
| `AlwaysAutoResize` | Fit child size to content. |

---

## 2. Window Management

```csharp
public static bool Begin(string name, int flags = 0);
public static bool Begin(string name, ref bool open, int flags = 0);
public static void End();

public static bool BeginChild(string id, float w = 0f, float h = 0f, Child childFlags = 0, int windowFlags = 0);
public static bool BeginChild(uint id, float w = 0f, float h = 0f, Child childFlags = 0, int windowFlags = 0);
public static void EndChild();
```

**Window Queries:**
```csharp
public static bool IsWindowAppearing();
public static bool IsWindowCollapsed();
public static bool IsWindowFocused(int flags = 0);
public static bool IsWindowHovered(int flags = 0);
public static float GetWindowWidth();
public static float GetWindowHeight();
public static float GetWindowDpiScale();
public static (float X, float Y) GetWindowPos();
public static (float W, float H) GetWindowSize();
```

**Next Window Settings:**
```csharp
public static void SetNextWindowPos(float x, float y, int cond = 0, float px = 0f, float py = 0f);
public static void SetNextWindowSize(float w, float h, int cond = 0);
public static void SetNextWindowSizeConstraints(float minW, float minH, float maxW, float maxH);
public static void SetNextWindowBgAlpha(float alpha);
public static void SetNextWindowCollapsed(bool collapsed, int cond = 0);
public static void SetNextWindowFocus();
```

---

## 3. Styling & Colors

```csharp
public static void PushStyleColor(int idx, float r, float g, float b, float a = 1f);
public static void PushStyleColor(int idx, Vector4 c);
public static void PushStyleColorU32(int idx, uint col);
public static void PopStyleColor(int count = 1);

public static void PushStyleVar(int idx, float val);
public static void PushStyleVar(int idx, float x, float y);
public static void PushStyleVar(int idx, Vector2 val); // Extras.cs
public static void PopStyleVar(int count = 1);

// Utility
public static uint GetColorU32(int styleCol, float alpha = 1f);
public static uint GetColorU32(float r, float g, float b, float a = 1f);
public static uint GetColorU32(Vector4 c);
public static Vector4 GetStyleColor(int idx);
public static string? GetStyleColorName(int idx);
```

---

## 4. Layout & Cursor

```csharp
public static void SetCursorPos(float x, float y);
public static (float X, float Y) GetCursorPos();
public static float GetCursorPosX();
public static float GetCursorPosY();

public static void SetCursorScreenPos(float x, float y);
public static (float X, float Y) GetCursorScreenPos();
public static void GetCursorScreenPos(out float x, out float y); // Extras.cs

public static (float W, float H) GetContentRegionAvail();
public static void GetContentRegionAvail(out float x, out float y); // Extras.cs

public static void SameLine(float offset = 0f, float spacing = -1f);
public static void NewLine();
public static void Spacing();
public static void Dummy(float w, float h);
public static void Indent(float w = 0f);
public static void Unindent(float w = 0f);
```

---

## 5. Text & Typography

```csharp
public static void Text(string text);
public static void TextColored(float r, float g, float b, float a, string text);
public static void TextColored(Vector4 c, string text);
public static void TextDisabled(string text);
public static void TextWrapped(string text);
public static void LabelText(string label, string text);
public static void BulletText(string text);
public static void Bullet();

public static (float W, float H) CalcTextSize(string text, bool hideAfterDoubleHash = false, float wrapWidth = -1f);
public static void PushTextWrapPos(float x = 0f);
public static void PopTextWrapPos();
```

---

## 6. Basic Widgets

```csharp
public static bool Button(string label, float w = 0f, float h = 0f);
public static bool SmallButton(string label);
public static bool InvisibleButton(string id, float w, float h, int flags = 0);
public static bool ArrowButton(string id, int dir);

public static bool Checkbox(string label, ref bool value);
public static bool CheckboxFlags(string label, ref int flags, int flagsValue);
public static bool CheckboxFlags(string label, ref uint flags, uint flagsValue);

public static bool RadioButton(string label, bool active);
public static bool RadioButton(string label, ref int v, int vButton);

public static void ProgressBar(float fraction, string? overlay = null, float w = -1f, float h = 0f);
public static bool TextLink(string label);
public static bool TextLinkOpenURL(string label, string url);

public static void Image(ulong texId, float w, float h, float u0 = 0f, float v0 = 0f, float u1 = 1f, float v1 = 1f);
```

---

## 7. Inputs, Sliders & Drags

```csharp
// Sliders
public static bool SliderFloat(string label, ref float v, float min, float max, string? fmt = null, int flags = 0);
public static bool SliderFloat2(string label, ref Vector2 v, float min, float max, string? fmt = null, int flags = 0);
public static bool SliderFloat3(string label, ref Vector3 v, float min, float max, string? fmt = null, int flags = 0);
public static bool SliderFloat4(string label, ref Vector4 v, float min, float max, string? fmt = null, int flags = 0);
public static bool SliderInt(string label, ref int v, int min, int max, string? fmt = null, int flags = 0);
public static bool VSliderFloat(string label, float w, float h, ref float v, float min, float max, string? fmt = null, int flags = 0);

// Drags
public static bool DragFloat(string label, ref float v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0);
public static bool DragFloat2(string label, ref Vector2 v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0);
public static bool DragFloat3(string label, ref Vector3 v, float speed = 1f, float min = 0f, float max = 0f, string? fmt = null, int flags = 0);
public static bool DragInt(string label, ref int v, float speed = 1f, int min = 0, int max = 0, string? fmt = null, int flags = 0);

// Text & Numbers
public static bool InputText(string label, ref string value, int maxBytes = 256, int flags = 0);
public static bool InputTextWithHint(string label, string hint, ref string value, int maxBytes = 256, int flags = 0);
public static bool InputTextMultiline(string label, ref string value, float w = 0f, float h = 0f, int maxBytes = 4096, int flags = 0);
public static bool InputFloat(string label, ref float v, float step = 0f, float fast = 0f, string? fmt = null, int flags = 0);
public static bool InputInt(string label, ref int v, int step = 1, int fast = 100, int flags = 0);
public static bool InputDouble(string label, ref double v, double step = 0.0, double fast = 0.0, string? fmt = null, int flags = 0);

// Color Pickers
public static bool ColorEdit3(string label, ref Vector3 c, int flags = 0);
public static bool ColorEdit4(string label, ref Vector4 c, int flags = 0);
public static bool ColorPicker3(string label, ref Vector3 c, int flags = 0);
public static bool ColorPicker4(string label, ref Vector4 c, int flags = 0);
public static bool ColorButton(string id, Vector4 c, int flags = 0, float w = 0f, float h = 0f);
```

---

## 8. Trees, Tabs, & Tables

```csharp
// Trees
public static bool TreeNode(string label);
public static bool TreeNodeEx(string label, int flags);
public static void TreePush(string id);
public static void TreePop();
public static bool CollapsingHeader(string label, int flags = 0);
public static bool CollapsingHeader(string label, ref bool open, int flags = 0);

// Tabs
public static bool BeginTabBar(string id, int flags = 0);
public static void EndTabBar();
public static bool BeginTabItem(string label, int flags = 0);
public static bool BeginTabItem(string label, ref bool open, int flags = 0);
public static void EndTabItem();

// Tables
public static bool BeginTable(string id, int columns, int flags = 0, float w = 0f, float h = 0f, float innerWidth = 0f);
public static void EndTable();
public static void TableNextRow(int flags = 0, float minHeight = 0f);
public static bool TableNextColumn();
public static bool TableSetColumnIndex(int n);
public static void TableSetupColumn(string label, int flags = 0, float initWidth = 0f, uint userId = 0);
public static void TableHeadersRow();
```

---

## 9. Popups, Modals & Tooltips

```csharp
// Tooltips
public static bool BeginTooltip();
public static void EndTooltip();
public static void SetTooltip(string text);
public static bool BeginItemTooltip();
public static void SetItemTooltip(string text);

// Popups
public static bool BeginPopup(string id, int flags = 0);
public static bool BeginPopupModal(string name, int flags = 0);
public static bool BeginPopupModal(string name, ref bool open, int flags = 0);
public static void EndPopup();
public static void OpenPopup(string id, int flags = 0);
public static void CloseCurrentPopup();
public static bool IsPopupOpen(string id, int flags = 0);

// Context Menus (Right Click)
public static bool BeginPopupContextItem(string? id = null, int flags = 1);
public static bool BeginPopupContextWindow(string? id = null, int flags = 1);
```

---

## 10. Custom Rendering (`ImDrawList`)

These methods bypass ImGui's layout system and draw directly to the screen buffer. Useful for custom shapes, backgrounds, and customized UI components.

```csharp
public static nint GetWindowDrawList();
public static nint GetBackgroundDrawList();
public static nint GetForegroundDrawList();

// Rectangles
public static void ImDrawListAddRectFilled(nint dl, float minX, float minY, float maxX, float maxY, uint col, float rounding = 0f, int flags = 0);
public static void ImDrawListAddRectFilled(nint dl, Vector2 min, Vector2 max, uint col, float rounding = 0f, int flags = 0); // Extras.cs
public static void ImDrawListAddRect(nint dl, float minX, float minY, float maxX, float maxY, uint col, float rounding, int flags, float thickness);
public static void ImDrawListAddRectFilledMultiColor(nint dl, float minX, float minY, float maxX, float maxY, uint colUprLeft, uint colUprRight, uint colBotRight, uint colBotLeft);

// Lines & Shapes
public static void ImDrawListAddLine(nint dl, float p1X, float p1Y, float p2X, float p2Y, uint col, float thickness);
public static void ImDrawListAddCircle(nint dl, float centerX, float centerY, float radius, uint col, int numSegments, float thickness);
public static void ImDrawListAddCircleFilled(nint dl, float centerX, float centerY, float radius, uint col, int numSegments);
public static void ImDrawListAddTriangleFilled(nint dl, float p1X, float p1Y, float p2X, float p2Y, float p3X, float p3Y, uint col);

// Text
public static void ImDrawListAddText(nint dl, float posX, float posY, uint col, byte* textBegin, byte* textEnd);
// Note: Use Utf8.Of("Text") to get the byte* pointers for AddText.

// Clipping
public static void PushClipRect(float minX, float minY, float maxX, float maxY, bool intersect = true);
public static void PopClipRect();
```

---

## 11. Item Interaction & State

Checks the state of the *last drawn* widget.
```csharp
public static bool IsItemHovered(int flags = 0);
public static bool IsItemActive();
public static bool IsItemFocused();
public static bool IsItemClicked(int btn = 0);
public static bool IsItemVisible();
public static bool IsItemEdited();
public static bool IsItemActivated();
public static bool IsItemDeactivated();

// Bounds of the last item
public static (float X, float Y) GetItemRectMin();
public static (float X, float Y) GetItemRectMax();
public static (float W, float H) GetItemRectSize();
```

**Global Input State:**
```csharp
public static bool IsMouseDown(int btn); // Extras.cs
public static bool IsMouseClicked(int btn = 0, bool repeat = false);
public static bool IsMouseReleased(int btn = 0);
public static bool IsMouseHoveringRect(float minX, float minY, float maxX, float maxY, bool clip = true); // Extras.cs
public static Vector2 GetMousePos(); // Extras.cs
public static void GetMousePos(out float x, out float y);

public static bool IsKeyDown(int key);
public static bool IsKeyPressed(int key, bool repeat = true);
public static bool IsKeyReleased(int key);
```

---

## 12. IDs & Scoping
*Crucial for avoiding ID collisions when rendering widgets inside loops.*

```csharp
public static void PushID(string id);
public static void PushID(int id);
public static void PushID(nint ptr);
public static void PopID();

public static uint GetID(string id);
public static uint GetID(int id);
public static uint GetID(nint ptr);
```

---

## 13. Menus & Menu Bars
*For creating top-level application menus or context dropdowns.*

```csharp
public static bool BeginMainMenuBar();
public static void EndMainMenuBar();
public static bool BeginMenuBar();
public static void EndMenuBar();

public static bool BeginMenu(string label, bool enabled = true);
public static void EndMenu();

public static bool MenuItem(string label, string? shortcut = null, bool selected = false, bool enabled = true);
public static bool MenuItem(string label, string? shortcut, ref bool selected, bool enabled = true);
```

---

## 14. Combos & ListBoxes
*Dropdown selection boxes and scrollable lists.*

```csharp
// Combos (Dropdowns)
public static bool BeginCombo(string label, string preview, int flags = 0);
public static void EndCombo();
public static bool Combo(string label, ref int current, string[] items, int popupMaxHeight = -1);

// ListBoxes
public static bool BeginListBox(string label, float w = 0f, float h = 0f);
public static void EndListBox();
public static bool ListBox(string label, ref int current, string[] items, float w = 0f, float h = 0f);

// Selectables (usually used inside ListBoxes)
public static bool Selectable(string label, bool selected = false, int flags = 0, float w = 0f, float h = 0f);
public static bool Selectable(string label, ref bool selected, int flags = 0, float w = 0f, float h = 0f);
```

---

## 15. Drag & Drop (Advanced)
*Passing data between widgets using type-safe C# structs.*

```csharp
public static bool BeginDragDropSource(int flags = 0);
public static void EndDragDropSource();
public static bool BeginDragDropTarget();
public static void EndDragDropTarget();

// Type-safe payload transfer (where T : unmanaged)
public static bool SetDragDropPayload<T>(string type, ref T data, int cond = 0);
public static bool AcceptDragDropPayload<T>(string type, out T value, int flags = 0);
```

---

## 16. Plotting & Graphs
*Drawing real-time charts.*

```csharp
public static void PlotLines(string label, ReadOnlySpan<float> values, int offset = 0, string? overlay = null, float min = float.MaxValue, float max = float.MaxValue, float w = 0f, float h = 0f);
public static void PlotHistogram(string label, ReadOnlySpan<float> values, int offset = 0, string? overlay = null, float min = float.MaxValue, float max = float.MaxValue, float w = 0f, float h = 0f);
```

---

## 17. State Control & Misc
*Manipulating focus, disabled states, and extracting values.*

```csharp
// Disabled States
public static void BeginDisabled(bool disabled = true);
public static void EndDisabled();

// Focus
public static void SetItemDefaultFocus();
public static void SetKeyboardFocusHere(int offset = 0);
public static void SetItemKeyOwner(int key);

// Values (Read-only text)
public static void Value(string prefix, bool v);
public static void Value(string prefix, int v);
public static void Value(string prefix, uint v);
public static void Value(string prefix, float v, string? fmt = null);

// Utilities
public static bool IsRectVisible(float w, float h);
public static double GetTime();
public static int GetFrameCount();
```

---

## 18. Nested Helper Classes (Namespaces)
The wrapper provides organized sub-classes for managing the underlying ImGui state.

### `Ui.Frame` (Lifecycle & IO)
```csharp
Ui.Frame.NewFrame();
Ui.Frame.EndFrame();
Ui.Frame.Render();
Ui.Frame.DisplaySize; 
Ui.Frame.SetDisplaySize(w, h);
Ui.Frame.SetDeltaTime(dt);
Ui.Frame.SetConfigFlags(flags);
Ui.Frame.StyleColorsDark();
Ui.Frame.SetIniFilename("path.ini");
```

### `Ui.Input` (IO Injection)
```csharp
Ui.Input.WantCaptureMouse; // bool
Ui.Input.WantCaptureKeyboard; // bool
Ui.Input.AddMousePos(x, y);
Ui.Input.AddMouseButton(btn, down);
Ui.Input.AddMouseWheel(wx, wy);
Ui.Input.AddKey(key, down);
Ui.Input.AddInputCharacter(c);
Ui.Input.AddFocusEvent(focused);
```

### `Ui.Clipboard`
```csharp
Ui.Clipboard.Get(); // returns string
Ui.Clipboard.Set("text");
```

### `Ui.Color` (Conversions)
```csharp
Ui.Color.Rgba(r, g, b, a); // Returns packed uint
Ui.Color.ConvertU32ToFloat4(uint);
Ui.Color.ConvertFloat4ToU32(Vector4);
Ui.Color.RgbToHsv(r, g, b);
Ui.Color.HsvToRgb(h, s, v);
```

### `Ui.Debug`
```csharp
Ui.Debug.ShowDemoWindow();
Ui.Debug.ShowMetricsWindow();
Ui.Debug.ShowStyleEditor();
```

### `Ui.Ini` (Settings Persistence)
```csharp
Ui.Ini.LoadFromDisk("file.ini");
Ui.Ini.SaveToDisk("file.ini");
```

## License

MIT License. See [LICENSE](LICENSE) for details.

Copyright (c) 2026 WinsentShame

## Third-party

- [Dear ImGui](https://github.com/ocornut/imgui) — MIT (Copyright (c) 2014-2025 Omar Cornut)
- [MinHook](https://github.com/TsudaKageyu/minhook) — BSD 2-Clause (Copyright (c) 2009-2017 Tsuda Kageyu)
- [Kiero](https://github.com/Rebzzel/kiero) — MIT (Copyright (c) 2014-2021 Rebzzel)

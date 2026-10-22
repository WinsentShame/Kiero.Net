@echo off
cd /d "%~dp0"

echo === ImGui core ===
cl /c /O2 /EHsc /MD /wd4996 /I. /IImGui ^
   ImGui\imgui.cpp ImGui\imgui_draw.cpp ImGui\imgui_widgets.cpp ImGui\imgui_tables.cpp ImGui\imgui_demo.cpp
if errorlevel 1 goto :err

echo === ImGui backends ===
cl /c /O2 /EHsc /MD /wd4996 /I. /IImGui /IBackends ^
   Backends\imgui_impl_win32.cpp Backends\imgui_impl_dx11.cpp Backends\imgui_impl_dx12.cpp Backends\imgui_impl_opengl3.cpp Backends\imgui_impl_dx9.cpp Backends\imgui_impl_dx10.cpp
if errorlevel 1 goto :err

echo === Kiero ===
cl /c /O2 /EHsc /MD /wd4996 /I. /IKiero ^
   Kiero\kiero.cpp Kiero\kiero_wrapper.cpp
if errorlevel 1 goto :err

echo === Shim ===
cl /c /O2 /EHsc /MD /wd4996 /I. /IImGui /IBackends ^
   Backends\shim.cpp
if errorlevel 1 goto :err

echo === archive ===
lib /OUT:kiero_native.lib *.obj
if errorlevel 1 goto :err

del *.obj
echo === done: kiero_native.lib ===
exit /b 0

:err
echo === BUILD FAILED ===
exit /b 1
@echo off
cd /d "%~dp0"

echo === ImGui core ===
cl /c /O2 /EHsc /MD /I. /Iimgui ^
   imgui\imgui.cpp ^
   imgui\imgui_draw.cpp ^
   imgui\imgui_widgets.cpp ^
   imgui\imgui_demo.cpp
if errorlevel 1 goto :err

echo === ImGui backends ===
cl /c /O2 /EHsc /MD /I. /Iimgui /Ibackends ^
   backends\imgui_impl_win32.cpp ^
   backends\imgui_impl_dx11.cpp ^
   backends\imgui_impl_dx12.cpp
if errorlevel 1 goto :err

echo === Kiero ===
cl /c /O2 /EHsc /MD /I. /Ikiero ^
   kiero\kiero.cpp ^
   kiero\kiero_wrapper.cpp
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
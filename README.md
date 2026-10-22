# Kiero.Net

C# library for injecting a Dear ImGui overlay into a game process.
Write your UI in C#, inject one DLL, and the menu renders on top of the game.

**Supported APIs:** Direct3D 9, Direct3D 10, Direct3D 11, Direct3D 12, OpenGL 3.
**Platform:** Windows 10/11, x64.
**Stack:** Dear ImGui + MinHook + Kiero, wrapped in C# with NativeAOT.

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

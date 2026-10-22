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

## License

MIT License. See [LICENSE](LICENSE) for details.

Copyright (c) 2026 WinsentShame

## Third-party

- [Dear ImGui](https://github.com/ocornut/imgui) — MIT (Copyright (c) 2014-2025 Omar Cornut)
- [MinHook](https://github.com/TsudaKageyu/minhook) — BSD 2-Clause (Copyright (c) 2009-2017 Tsuda Kageyu)
- [Kiero](https://github.com/Rebzzel/kiero) — MIT (Copyright (c) 2014-2021 Rebzzel)

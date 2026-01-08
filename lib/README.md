# Required Libraries for SPA II

Place the following DLL files in this folder to build the project:

## Required DLLs

### ScriptHookVDotNet3.dll
- **For Enhanced Edition**: Download from [ScriptHookVDotNet Enhanced (SHVDNE)](https://github.com/Chiheb-Bacha/ScriptHookVDotNetEnhanced)
- **For Legacy Edition**: Download from [ScriptHookVDotNet](https://github.com/crosire/scripthookvdotnet/releases)

### iFruitAddon2.dll
- Download from [GTA5-Mods](https://www.gta5-mods.com/tools/ifruitaddon2)

### INMNativeUI.dll
- Custom NativeUI library (may be bundled with the mod or available from the original source)

### Metadata.dll
- Custom metadata library (may be bundled with the mod or available from the original source)

## Installation

1. Copy all DLLs listed above to this `lib` folder
2. Build the solution in Visual Studio
3. Copy the output DLL to your GTA V `scripts` folder

## GTA V Enhanced Edition Notes

For GTA V Enhanced Edition (v1.0.3095.0 and later):
- Use ScriptHookVDotNet Enhanced (SHVDNE) instead of standard SHVDN
- SHVDNE is backwards compatible with Legacy Edition
- Config and save files are stored in `%localappdata%\SPA II\` on Enhanced
- Sound files should remain in `scripts\SPA II\Sounds\`

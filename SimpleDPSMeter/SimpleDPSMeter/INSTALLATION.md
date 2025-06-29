# SimpleDPSMeter Installation Guide

## Method 1: Development Installation (Recommended for Testing)

1. **Build the Plugin**
   ```powershell
   # Clone the repository
   git clone https://github.com/MelloCro/SimpleDPSMeter.git
   cd SimpleDPSMeter/SimpleDPSMeter
   
   # Build using the provided script
   ..\build.ps1
   ```

2. **Install in Dalamud**
   - Open FFXIV with XIVLauncher
   - Type `/xlsettings` in chat
   - Go to "Experimental" tab
   - Under "Dev Plugin Locations", click "+"
   - Add the path to `SimpleDPSMeter\dist\SimpleDPSMeter.dll`
   - Type `/xlplugins` in chat
   - Go to "Dev Tools" > "Installed Dev Plugins"
   - Enable SimpleDPSMeter

## Method 2: Third-Party Repository Installation

1. **Add the Repository**
   - Type `/xlsettings` in chat
   - Go to "Experimental" tab
   - Under "Custom Plugin Repositories", add:
     ```
     https://raw.githubusercontent.com/MelloCro/SimpleDPSMeter/main/repo.json
     ```
   - Save settings

2. **Install the Plugin**
   - Type `/xlplugins` in chat
   - Search for "SimpleDPSMeter"
   - Click Install

## Method 3: Manual Installation from Release

1. **Download the Latest Release**
   - Go to [Releases](https://github.com/MelloCro/SimpleDPSMeter/releases)
   - Download `SimpleDPSMeter.zip`

2. **Extract and Install**
   - Extract the zip file to a folder
   - Type `/xlsettings` in chat
   - Go to "Experimental" tab
   - Under "Dev Plugin Locations", add the path to the extracted folder
   - Enable the plugin in Plugin Installer

## Usage

- Type `/dpsmeter` to toggle the DPS meter window
- Right-click the window title to access settings
- Configure options through Dalamud Plugin Settings

## Troubleshooting

- **Plugin not showing**: Make sure you have the latest version of XIVLauncher and Dalamud
- **Build errors**: Ensure you have .NET 8.0 SDK installed
- **Missing dependencies**: The GitHub Actions workflow automatically downloads Dalamud libraries

## For Plugin Developers

To use the manifest URL directly in Dalamud:
```
https://raw.githubusercontent.com/MelloCro/SimpleDPSMeter/main/SimpleDPSMeter/SimpleDPSMeter.json
```

Note: This requires a built release to be available at the download links specified in the manifest.
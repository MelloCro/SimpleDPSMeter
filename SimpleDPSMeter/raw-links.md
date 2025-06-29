# SimpleDPSMeter Raw GitHub Links

Replace `yourusername` with your actual GitHub username after forking/creating the repository.

## Project Files Raw Links

### Core Files
- **Plugin.cs**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/Plugin.cs
- **Configuration.cs**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/Configuration.cs
- **SimpleDPSMeter.csproj**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/SimpleDPSMeter.csproj
- **SimpleDPSMeter.json**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/SimpleDPSMeter.json

### Data Files
- **CombatData.cs**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/Data/CombatData.cs
- **CombatDataManager.cs**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/Data/CombatDataManager.cs

### Windows Files
- **MainWindow.cs**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/Windows/MainWindow.cs
- **ConfigWindow.cs**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/Windows/ConfigWindow.cs

### Other Files
- **README.md**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/README.md
- **.gitignore**: https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main/.gitignore

## Quick Download Script

You can use this PowerShell script to download all files:

```powershell
$baseUrl = "https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main"
$files = @(
    "Plugin.cs",
    "Configuration.cs", 
    "SimpleDPSMeter.csproj",
    "SimpleDPSMeter.json",
    "Data/CombatData.cs",
    "Data/CombatDataManager.cs",
    "Windows/MainWindow.cs",
    "Windows/ConfigWindow.cs",
    "README.md",
    ".gitignore"
)

# Create directories
New-Item -ItemType Directory -Path "SimpleDPSMeter/Data" -Force
New-Item -ItemType Directory -Path "SimpleDPSMeter/Windows" -Force

# Download files
foreach ($file in $files) {
    $url = "$baseUrl/$file"
    $output = "SimpleDPSMeter/$file"
    Invoke-WebRequest -Uri $url -OutFile $output
    Write-Host "Downloaded: $file"
}
```

## Bash/Linux Download Script

```bash
#!/bin/bash
BASE_URL="https://raw.githubusercontent.com/yourusername/SimpleDPSMeter/main"

# Create directories
mkdir -p SimpleDPSMeter/Data
mkdir -p SimpleDPSMeter/Windows

# Download files
wget -P SimpleDPSMeter/ "$BASE_URL/Plugin.cs"
wget -P SimpleDPSMeter/ "$BASE_URL/Configuration.cs"
wget -P SimpleDPSMeter/ "$BASE_URL/SimpleDPSMeter.csproj"
wget -P SimpleDPSMeter/ "$BASE_URL/SimpleDPSMeter.json"
wget -P SimpleDPSMeter/Data/ "$BASE_URL/Data/CombatData.cs"
wget -P SimpleDPSMeter/Data/ "$BASE_URL/Data/CombatDataManager.cs"
wget -P SimpleDPSMeter/Windows/ "$BASE_URL/Windows/MainWindow.cs"
wget -P SimpleDPSMeter/Windows/ "$BASE_URL/Windows/ConfigWindow.cs"
wget -P SimpleDPSMeter/ "$BASE_URL/README.md"
wget -P SimpleDPSMeter/ "$BASE_URL/.gitignore"
```

## For Dalamud Plugin Repository

If you want to add this to a Dalamud plugin repository, you would use:

```json
{
    "Author": "YourName",
    "Name": "SimpleDPSMeter",
    "InternalName": "SimpleDPSMeter",
    "AssemblyVersion": "1.0.0.0",
    "Description": "A lightweight DPS meter for Final Fantasy XIV",
    "ApplicableVersion": "any",
    "RepoUrl": "https://github.com/yourusername/SimpleDPSMeter",
    "DalamudApiLevel": 9,
    "LoadRequiredState": 0,
    "LoadSync": false,
    "LoadPriority": 0,
    "DownloadLinkInstall": "https://github.com/yourusername/SimpleDPSMeter/releases/latest/download/SimpleDPSMeter.zip",
    "IsHide": false,
    "IsTestingExclusive": false,
    "DownloadLinkTesting": "https://github.com/yourusername/SimpleDPSMeter/releases/latest/download/SimpleDPSMeter.zip",
    "DownloadLinkUpdate": "https://github.com/yourusername/SimpleDPSMeter/releases/latest/download/SimpleDPSMeter.zip",
    "DownloadCount": 0,
    "LastUpdated": "0"
}
```
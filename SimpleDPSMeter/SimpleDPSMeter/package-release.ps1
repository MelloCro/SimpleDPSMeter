# SimpleDPSMeter Release Packaging Script
param(
    [Parameter(Mandatory=$true)]
    [string]$Version
)

Write-Host "Packaging SimpleDPSMeter v$Version..." -ForegroundColor Green

# Build the project first
& .\build.ps1 -Configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Cannot create release package." -ForegroundColor Red
    exit 1
}

# Update version in manifest
$manifestPath = ".\SimpleDPSMeter.json"
$manifest = Get-Content $manifestPath | ConvertFrom-Json
$manifest.AssemblyVersion = "$Version.0"
$manifest | ConvertTo-Json -Depth 10 | Set-Content $manifestPath

# Copy updated manifest to dist
Copy-Item $manifestPath -Destination ".\dist\" -Force

# Create release directory
$releaseDir = ".\releases\v$Version"
if (!(Test-Path $releaseDir)) {
    New-Item -ItemType Directory -Path $releaseDir -Force
}

# Create zip file
$zipPath = "$releaseDir\SimpleDPSMeter.zip"
Compress-Archive -Path ".\dist\*" -DestinationPath $zipPath -Force

Write-Host "Release package created: $zipPath" -ForegroundColor Green

# Create checksums
$hash = Get-FileHash -Path $zipPath -Algorithm SHA256
$hashContent = "$($hash.Hash.ToLower())  SimpleDPSMeter.zip"
Set-Content -Path "$releaseDir\SimpleDPSMeter.zip.sha256" -Value $hashContent

Write-Host "SHA256: $($hash.Hash.ToLower())" -ForegroundColor Cyan

# Create release notes template
$releaseNotes = @"
# SimpleDPSMeter v$Version

## Installation
1. Download \`SimpleDPSMeter.zip\` from the releases
2. Extract the contents to a folder
3. In XIVLauncher, go to Dalamud Settings > Experimental > Dev Plugin Locations
4. Add the path to the extracted folder
5. Enable the plugin in the Plugin Installer

## Changes
- [Add your changes here]

## SHA256 Checksum
\`\`\`
$($hash.Hash.ToLower())
\`\`\`
"@

Set-Content -Path "$releaseDir\RELEASE_NOTES.md" -Value $releaseNotes

Write-Host "Release notes template created: $releaseDir\RELEASE_NOTES.md" -ForegroundColor Green
Write-Host "" -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Edit the release notes in $releaseDir\RELEASE_NOTES.md" -ForegroundColor Yellow
Write-Host "2. Create a new release on GitHub with tag v$Version" -ForegroundColor Yellow
Write-Host "3. Upload $zipPath as a release asset" -ForegroundColor Yellow
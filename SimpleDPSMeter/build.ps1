# SimpleDPSMeter Build Script
param(
    [string]$Configuration = "Release",
    [string]$OutputPath = ".\bin\Release"
)

Write-Host "Building SimpleDPSMeter..." -ForegroundColor Green

# Clean previous builds
if (Test-Path $OutputPath) {
    Remove-Item -Path $OutputPath -Recurse -Force
}

# Build the project
dotnet restore
dotnet build --configuration $Configuration --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Build completed successfully!" -ForegroundColor Green

# Create output directory for packaging
$packageDir = ".\dist"
if (Test-Path $packageDir) {
    Remove-Item -Path $packageDir -Recurse -Force
}
New-Item -ItemType Directory -Path $packageDir -Force

# Copy required files
Copy-Item "$OutputPath\SimpleDPSMeter.dll" -Destination $packageDir
Copy-Item ".\SimpleDPSMeter.json" -Destination $packageDir

Write-Host "Files copied to $packageDir" -ForegroundColor Green
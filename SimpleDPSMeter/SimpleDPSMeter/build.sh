#!/bin/bash

# SimpleDPSMeter Build Script for Linux/macOS

CONFIGURATION=${1:-Release}
OUTPUT_PATH="./bin/Release"

echo "Building SimpleDPSMeter..."

# Clean previous builds
if [ -d "$OUTPUT_PATH" ]; then
    rm -rf "$OUTPUT_PATH"
fi

# Build the project
dotnet restore
dotnet build --configuration "$CONFIGURATION" --no-restore

if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

echo "Build completed successfully!"

# Create output directory for packaging
PACKAGE_DIR="./dist"
if [ -d "$PACKAGE_DIR" ]; then
    rm -rf "$PACKAGE_DIR"
fi
mkdir -p "$PACKAGE_DIR"

# Copy required files
cp "$OUTPUT_PATH/SimpleDPSMeter.dll" "$PACKAGE_DIR/"
cp "./SimpleDPSMeter.json" "$PACKAGE_DIR/"

echo "Files copied to $PACKAGE_DIR"

# Make the script executable
chmod +x build.sh
# SimpleDPSMeter

A lightweight DPS meter plugin for Final Fantasy XIV using the Dalamud framework.

## Features

- Real-time damage tracking for you and your party
- Configurable display options (damage numbers, DPS, percentages)
- Auto-reset on wipe or zone change
- Pet damage tracking
- Customizable UI with window locking and opacity
- Combat timeout detection
- Job abbreviation display

## Installation

1. Ensure you have XIVLauncher and Dalamud installed
2. Clone this repository
3. Build the project using Visual Studio or .NET CLI
4. Add the output DLL to your Dalamud dev plugin locations

## Usage

- `/dpsmeter` - Toggle the DPS meter window
- Access configuration through the Dalamud plugin settings

## Building

Requirements:
- .NET 8.0 SDK
- Visual Studio 2022 or compatible IDE

```bash
dotnet build
```

## Configuration Options

### General
- Show window on login
- Lock window position
- Show in combat only
- Window opacity

### Display
- Show/hide damage numbers
- Show/hide DPS values
- Show/hide percentages
- Show job abbreviations
- Maximum displayed combatants

### Combat
- Include pet damage
- Include DoT damage
- Combat timeout duration
- Auto-reset on party wipe
- Auto-reset on zone change

## Technical Details

The plugin uses Dalamud's hook system to intercept `ActionEffect` events and track damage dealt by the player and party members. It calculates DPS in real-time and displays the results in a customizable ImGui window.

## License

This project is licensed under the MIT License.
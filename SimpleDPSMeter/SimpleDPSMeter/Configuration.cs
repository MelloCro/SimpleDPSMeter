using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SimpleDPSMeter;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool ShowWindow { get; set; } = true;
    public bool LockWindow { get; set; } = false;
    public bool ShowInCombatOnly { get; set; } = false;
    public bool IncludePets { get; set; } = true;
    public bool IncludeDoTs { get; set; } = true;
    
    public float WindowOpacity { get; set; } = 1.0f;
    public int MaxDisplayedCombatants { get; set; } = 8;
    public int CombatTimeoutSeconds { get; set; } = 30;
    
    public bool ShowDamageNumbers { get; set; } = true;
    public bool ShowDPS { get; set; } = true;
    public bool ShowPercentage { get; set; } = true;
    public bool ShowJobIcons { get; set; } = true;
    
    public bool ResetOnWipe { get; set; } = true;
    public bool ResetOnZoneChange { get; set; } = true;

    [NonSerialized]
    private DalamudPluginInterface? pluginInterface;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
    }

    public void Save()
    {
        pluginInterface!.SavePluginConfig(this);
    }
}
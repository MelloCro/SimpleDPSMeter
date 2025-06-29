using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SimpleDPSMeter.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    private Configuration configuration;

    public ConfigWindow(Plugin plugin) : base(
        "SimpleDPSMeter Configuration",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.plugin = plugin;
        this.configuration = plugin.Configuration;

        Size = new Vector2(400, 500);
        SizeCondition = ImGuiCond.Always;
    }

    public override void Draw()
    {
        if (ImGui.BeginTabBar("ConfigTabs"))
        {
            if (ImGui.BeginTabItem("General"))
            {
                DrawGeneralTab();
                ImGui.EndTabItem();
            }
            
            if (ImGui.BeginTabItem("Display"))
            {
                DrawDisplayTab();
                ImGui.EndTabItem();
            }
            
            if (ImGui.BeginTabItem("Combat"))
            {
                DrawCombatTab();
                ImGui.EndTabItem();
            }
            
            ImGui.EndTabBar();
        }
        
        ImGui.Separator();
        
        if (ImGui.Button("Save & Close"))
        {
            configuration.Save();
            IsOpen = false;
        }
    }
    
    private void DrawGeneralTab()
    {
        var showWindow = configuration.ShowWindow;
        if (ImGui.Checkbox("Show Window on Login", ref showWindow))
        {
            configuration.ShowWindow = showWindow;
        }
        
        var lockWindow = configuration.LockWindow;
        if (ImGui.Checkbox("Lock Window Position", ref lockWindow))
        {
            configuration.LockWindow = lockWindow;
        }
        
        var showInCombatOnly = configuration.ShowInCombatOnly;
        if (ImGui.Checkbox("Show In Combat Only", ref showInCombatOnly))
        {
            configuration.ShowInCombatOnly = showInCombatOnly;
        }
        
        ImGui.Spacing();
        ImGui.Text("Window Opacity");
        var opacity = configuration.WindowOpacity;
        if (ImGui.SliderFloat("##Opacity", ref opacity, 0.1f, 1.0f, "%.2f"))
        {
            configuration.WindowOpacity = opacity;
        }
    }
    
    private void DrawDisplayTab()
    {
        var showDamageNumbers = configuration.ShowDamageNumbers;
        if (ImGui.Checkbox("Show Damage Numbers", ref showDamageNumbers))
        {
            configuration.ShowDamageNumbers = showDamageNumbers;
        }
        
        var showDPS = configuration.ShowDPS;
        if (ImGui.Checkbox("Show DPS", ref showDPS))
        {
            configuration.ShowDPS = showDPS;
        }
        
        var showPercentage = configuration.ShowPercentage;
        if (ImGui.Checkbox("Show Percentage", ref showPercentage))
        {
            configuration.ShowPercentage = showPercentage;
        }
        
        var showJobIcons = configuration.ShowJobIcons;
        if (ImGui.Checkbox("Show Job Abbreviations", ref showJobIcons))
        {
            configuration.ShowJobIcons = showJobIcons;
        }
        
        ImGui.Spacing();
        ImGui.Text("Max Displayed Combatants");
        var maxDisplay = configuration.MaxDisplayedCombatants;
        if (ImGui.SliderInt("##MaxDisplay", ref maxDisplay, 1, 24))
        {
            configuration.MaxDisplayedCombatants = maxDisplay;
        }
    }
    
    private void DrawCombatTab()
    {
        var includePets = configuration.IncludePets;
        if (ImGui.Checkbox("Include Pet Damage", ref includePets))
        {
            configuration.IncludePets = includePets;
        }
        
        var includeDoTs = configuration.IncludeDoTs;
        if (ImGui.Checkbox("Include DoT Damage", ref includeDoTs))
        {
            configuration.IncludeDoTs = includeDoTs;
        }
        
        ImGui.Spacing();
        ImGui.Text("Combat Timeout (seconds)");
        var timeout = configuration.CombatTimeoutSeconds;
        if (ImGui.SliderInt("##Timeout", ref timeout, 10, 120))
        {
            configuration.CombatTimeoutSeconds = timeout;
        }
        ImGui.TextWrapped("Combat will end after this many seconds of no damage.");
        
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
        
        ImGui.Text("Auto Reset Options");
        
        var resetOnWipe = configuration.ResetOnWipe;
        if (ImGui.Checkbox("Reset on Party Wipe", ref resetOnWipe))
        {
            configuration.ResetOnWipe = resetOnWipe;
        }
        
        var resetOnZone = configuration.ResetOnZoneChange;
        if (ImGui.Checkbox("Reset on Zone Change", ref resetOnZone))
        {
            configuration.ResetOnZoneChange = resetOnZone;
        }
    }

    public void Dispose()
    {
    }
}
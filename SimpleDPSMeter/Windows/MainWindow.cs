using System;
using System.Numerics;
using System.Linq;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SimpleDPSMeter.Data;

namespace SimpleDPSMeter.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;

    public MainWindow(Plugin plugin) : base(
        "DPS Meter", 
        ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.plugin = plugin;
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(300, 100),
            MaximumSize = new Vector2(800, 600)
        };
        
        Size = new Vector2(400, 300);
    }

    public override void Draw()
    {
        var config = plugin.Configuration;
        var encounter = plugin.CombatDataManager.CurrentEncounter;
        
        if (config.ShowInCombatOnly && !encounter.Active)
        {
            IsOpen = false;
            return;
        }
        
        if (config.LockWindow)
        {
            Flags |= ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize;
        }
        else
        {
            Flags &= ~(ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
        }
        
        ImGui.SetNextWindowBgAlpha(config.WindowOpacity);
        
        DrawEncounterInfo(encounter);
        ImGui.Separator();
        DrawCombatantList(encounter);
    }
    
    private void DrawEncounterInfo(Encounter encounter)
    {
        if (!encounter.Active && encounter.Combatants.Count == 0)
        {
            ImGui.Text("Waiting for combat...");
            return;
        }
        
        var duration = encounter.Active ? 
            (DateTime.Now - encounter.StartTime).TotalSeconds : 
            encounter.Duration;
        
        var totalDps = encounter.TotalDamage / Math.Max(1, duration);
        
        ImGui.Text($"Duration: {FormatTime(duration)}");
        ImGui.SameLine();
        ImGui.Text($"Total DPS: {totalDps:N0}");
        
        if (ImGui.Button("Reset"))
        {
            plugin.CombatDataManager.ResetEncounter();
        }
    }
    
    private void DrawCombatantList(Encounter encounter)
    {
        var combatants = encounter.Combatants.Values
            .OrderByDescending(c => c.TotalDamage)
            .Take(plugin.Configuration.MaxDisplayedCombatants)
            .ToList();
        
        if (combatants.Count == 0)
            return;
        
        ImGui.BeginChild("CombatantList");
        
        ImGui.Columns(4, "CombatantColumns", true);
        ImGui.SetColumnWidth(0, 150);
        ImGui.SetColumnWidth(1, 100);
        ImGui.SetColumnWidth(2, 80);
        ImGui.SetColumnWidth(3, 60);
        
        ImGui.Text("Name");
        ImGui.NextColumn();
        ImGui.Text("Damage");
        ImGui.NextColumn();
        ImGui.Text("DPS");
        ImGui.NextColumn();
        ImGui.Text("%");
        ImGui.NextColumn();
        ImGui.Separator();
        
        var totalDamage = encounter.TotalDamage;
        
        foreach (var combatant in combatants)
        {
            var percentage = totalDamage > 0 ? (combatant.TotalDamage / (double)totalDamage) * 100 : 0;
            
            if (plugin.Configuration.ShowJobIcons)
            {
                ImGui.Text($"[{GetJobAbbreviation(combatant.JobId)}] {combatant.Name}");
            }
            else
            {
                ImGui.Text(combatant.Name);
            }
            ImGui.NextColumn();
            
            if (plugin.Configuration.ShowDamageNumbers)
            {
                ImGui.Text($"{combatant.TotalDamage:N0}");
            }
            else
            {
                ImGui.Text("-");
            }
            ImGui.NextColumn();
            
            if (plugin.Configuration.ShowDPS)
            {
                ImGui.Text($"{combatant.DPS:N0}");
            }
            else
            {
                ImGui.Text("-");
            }
            ImGui.NextColumn();
            
            if (plugin.Configuration.ShowPercentage)
            {
                ImGui.Text($"{percentage:F1}%");
            }
            else
            {
                ImGui.Text("-");
            }
            ImGui.NextColumn();
        }
        
        ImGui.EndChild();
    }
    
    private string FormatTime(double seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);
        return $"{(int)time.TotalMinutes:D2}:{time.Seconds:D2}";
    }
    
    private string GetJobAbbreviation(uint jobId)
    {
        return jobId switch
        {
            1 => "GLA",
            2 => "PGL",
            3 => "MRD",
            4 => "LNC",
            5 => "ARC",
            6 => "CNJ",
            7 => "THM",
            8 => "CRP",
            9 => "BSM",
            10 => "ARM",
            11 => "GSM",
            12 => "LTW",
            13 => "WVR",
            14 => "ALC",
            15 => "CUL",
            16 => "MIN",
            17 => "BTN",
            18 => "FSH",
            19 => "PLD",
            20 => "MNK",
            21 => "WAR",
            22 => "DRG",
            23 => "BRD",
            24 => "WHM",
            25 => "BLM",
            26 => "ACN",
            27 => "SMN",
            28 => "SCH",
            29 => "ROG",
            30 => "NIN",
            31 => "MCH",
            32 => "DRK",
            33 => "AST",
            34 => "SAM",
            35 => "RDM",
            36 => "BLU",
            37 => "GNB",
            38 => "DNC",
            39 => "RPR",
            40 => "SGE",
            41 => "VPR",
            42 => "PCT",
            _ => "???"
        };
    }

    public void Dispose()
    {
    }
}
using System;
using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

namespace SimpleDPSMeter.Data;

public class CombatData
{
    public uint ObjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public uint JobId { get; set; }
    public ulong TotalDamage { get; set; }
    public DateTime CombatStartTime { get; set; }
    public DateTime LastActionTime { get; set; }
    public bool InCombat { get; set; }
    
    public double DPS => GetDPS();
    public double CombatDuration => (LastActionTime - CombatStartTime).TotalSeconds;
    
    private double GetDPS()
    {
        var duration = CombatDuration;
        if (duration <= 0) return 0;
        return TotalDamage / duration;
    }
    
    public void AddDamage(ulong damage)
    {
        TotalDamage += damage;
        LastActionTime = DateTime.Now;
        InCombat = true;
    }
    
    public void Reset()
    {
        TotalDamage = 0;
        CombatStartTime = DateTime.Now;
        LastActionTime = DateTime.Now;
        InCombat = false;
    }
}

public class Encounter
{
    public Dictionary<uint, CombatData> Combatants { get; } = new();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Active { get; set; }
    
    public ulong TotalDamage
    {
        get
        {
            ulong total = 0;
            foreach (var combatant in Combatants.Values)
            {
                total += combatant.TotalDamage;
            }
            return total;
        }
    }
    
    public double Duration => (EndTime - StartTime).TotalSeconds;
    
    public void AddDamage(uint sourceId, string sourceName, uint jobId, ulong damage)
    {
        if (!Combatants.TryGetValue(sourceId, out var combatant))
        {
            combatant = new CombatData
            {
                ObjectId = sourceId,
                Name = sourceName,
                JobId = jobId,
                CombatStartTime = DateTime.Now
            };
            Combatants[sourceId] = combatant;
        }
        
        combatant.AddDamage(damage);
        EndTime = DateTime.Now;
    }
    
    public void Reset()
    {
        Combatants.Clear();
        StartTime = DateTime.Now;
        EndTime = DateTime.Now;
        Active = false;
    }
}
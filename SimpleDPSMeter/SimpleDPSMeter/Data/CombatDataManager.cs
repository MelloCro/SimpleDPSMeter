using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Character;

namespace SimpleDPSMeter.Data;

public unsafe class CombatDataManager : IDisposable
{
    private readonly Plugin plugin;
    private Encounter currentEncounter = new();
    private DateTime lastCombatTime = DateTime.Now;
    
    public Encounter CurrentEncounter => currentEncounter;
    
    public delegate void ActionEffectDelegate(uint sourceId, Character* sourceCharacter, IntPtr pos, IntPtr effectHeader, IntPtr effectArray, IntPtr effectTrail);
    
    [Signature("40 55 53 57 41 54 41 55 41 56 41 57 48 8D AC 24", DetourName = nameof(ActionEffectDetour))]
    private Hook<ActionEffectDelegate>? actionEffectHook = null;
    
    public CombatDataManager(Plugin plugin)
    {
        this.plugin = plugin;
        
        SignatureHelper.Initialise(this);
        actionEffectHook?.Enable();
    }
    
    private void ActionEffectDetour(uint sourceId, Character* sourceCharacter, IntPtr pos, IntPtr effectHeader, IntPtr effectArray, IntPtr effectTrail)
    {
        actionEffectHook?.Original(sourceId, sourceCharacter, pos, effectHeader, effectArray, effectTrail);
        
        try
        {
            if (sourceCharacter == null || !plugin.ClientState.IsLoggedIn)
                return;
            
            ProcessActionEffect(sourceId, sourceCharacter, effectHeader, effectArray);
        }
        catch (Exception ex)
        {
            plugin.ChatGui.PrintError($"[SimpleDPSMeter] Error in ActionEffectDetour: {ex.Message}");
        }
    }
    
    private void ProcessActionEffect(uint sourceId, Character* sourceCharacter, IntPtr effectHeader, IntPtr effectArray)
    {
        if (effectHeader == IntPtr.Zero || effectArray == IntPtr.Zero)
            return;
        
        var actionId = *(ushort*)(effectHeader + 0x0);
        var targetCount = *(byte*)(effectHeader + 0x21);
        
        if (targetCount == 0)
            return;
        
        var sourceObj = plugin.ObjectTable.FirstOrDefault(x => x.ObjectId == sourceId);
        if (sourceObj == null || sourceObj is not BattleChara battleChara)
            return;
        
        if (!IsValidCombatant(battleChara))
            return;
        
        ulong totalDamage = 0;
        
        for (int i = 0; i < targetCount; i++)
        {
            var effectData = effectArray + (i * 8 * 8);
            
            for (int j = 0; j < 8; j++)
            {
                var effectType = *(byte*)(effectData + (j * 8) + 0x0);
                var damage = *(ushort*)(effectData + (j * 8) + 0x4);
                
                if (effectType == 0) break;
                
                if (IsValidDamageType(effectType))
                {
                    totalDamage += damage;
                }
            }
        }
        
        if (totalDamage > 0)
        {
            if (!currentEncounter.Active)
            {
                StartNewEncounter();
            }
            
            var jobId = battleChara.ClassJob.Id;
            currentEncounter.AddDamage(sourceId, sourceObj.Name.ToString(), jobId, totalDamage);
            lastCombatTime = DateTime.Now;
        }
    }
    
    private bool IsValidCombatant(BattleChara chara)
    {
        if (chara.ObjectId == plugin.ClientState.LocalPlayer?.ObjectId)
            return true;
        
        if (IsPartyMember(chara.ObjectId))
            return true;
        
        if (plugin.Configuration.IncludePets && chara.OwnerId == plugin.ClientState.LocalPlayer?.ObjectId)
            return true;
        
        if (plugin.Configuration.IncludePets && IsPartyMember(chara.OwnerId))
            return true;
        
        return false;
    }
    
    private bool IsPartyMember(uint objectId)
    {
        var partyList = plugin.ObjectTable.Where(x => x is BattleChara).Cast<BattleChara>();
        return partyList.Any(member => member.ObjectId == objectId);
    }
    
    private bool IsValidDamageType(byte effectType)
    {
        return effectType switch
        {
            0x03 => true,
            _ => false
        };
    }
    
    private void StartNewEncounter()
    {
        currentEncounter.Reset();
        currentEncounter.StartTime = DateTime.Now;
        currentEncounter.Active = true;
    }
    
    public void OnFrameworkUpdate(IFramework framework)
    {
        if (currentEncounter.Active)
        {
            var timeSinceLastAction = (DateTime.Now - lastCombatTime).TotalSeconds;
            
            if (timeSinceLastAction > plugin.Configuration.CombatTimeoutSeconds)
            {
                EndEncounter();
            }
            
            if (plugin.Configuration.ResetOnWipe && IsPartyWiped())
            {
                ResetEncounter();
            }
        }
        
        if (plugin.Configuration.ResetOnZoneChange && plugin.ClientState.TerritoryType != lastZoneId)
        {
            lastZoneId = plugin.ClientState.TerritoryType;
            ResetEncounter();
        }
    }
    
    private uint lastZoneId = 0;
    
    private bool IsPartyWiped()
    {
        var player = plugin.ClientState.LocalPlayer;
        if (player == null) return false;
        
        if (player is BattleChara battlePlayer && battlePlayer.CurrentHp == 0)
        {
            var partyMembers = plugin.ObjectTable.Where(x => x is BattleChara).Cast<BattleChara>();
            return partyMembers.All(member => member.CurrentHp == 0);
        }
        
        return false;
    }
    
    private void EndEncounter()
    {
        currentEncounter.Active = false;
        currentEncounter.EndTime = DateTime.Now;
    }
    
    public void ResetEncounter()
    {
        currentEncounter.Reset();
    }
    
    public void Dispose()
    {
        actionEffectHook?.Dispose();
    }
}
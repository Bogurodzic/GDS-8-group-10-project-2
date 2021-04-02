using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Ability {
    private static string _temporaryLog;

    public static string AttackUnit(AbilitiesData abilityData, Unit defender)
    {
        if (abilityData.abilityType == AbilityType.SingleTarget)
        {
            int damage = abilityData.damage;
            string damageLog = "Attacker will damage defender for " + damage + "\n";
            _temporaryLog += damageLog;
            DealDamage(damage, defender);

        } else if (abilityData.abilityType == AbilityType.Circle)
        {
            int damage = abilityData.damage;
            string damageLog = "Attacker will damage defender for " + damage + "\n";
            _temporaryLog += damageLog;
            DealDamage(damage, defender);
        }
        return _temporaryLog;
    }

    public static string HealUnit(AbilitiesData abilityData, Unit healTarget)
    {
        int heal = abilityData.heal;
        string healLog = "Healer will heal target for " + heal + "\n";
        _temporaryLog += healLog;
        Heal(heal, healTarget);
        return _temporaryLog;
    }
    
    private static void Heal(int heal, Unit healTarget)
    {
        int finalHeal = heal;
        string doHealLog;
        string healTargetHpLog;

        doHealLog = "Target recieved " + finalHeal + "\n";
        _temporaryLog += doHealLog;
        healTarget.GetStatistics().currentHp = healTarget.GetStatistics().currentHp + finalHeal;
        if (healTarget.GetStatistics().currentHp > healTarget.GetStatistics().maxHp)
        {
            healTarget.GetStatistics().currentHp = healTarget.GetStatistics().maxHp;
        }
        healTargetHpLog = "Current target hp: " +  healTarget.GetStatistics().currentHp + "\n";
        _temporaryLog += healTargetHpLog;
    }

    private static void DealDamage(int damage, Unit defender)
    {
        int finalDamage = damage;
        string dealDamageLog;
        string currentDefenderHpLog;


        dealDamageLog = "Defender recieved " + finalDamage + "\n";
        _temporaryLog += dealDamageLog;
        defender.GetStatistics().currentHp = defender.GetStatistics().currentHp - finalDamage;
        if (defender.GetStatistics().currentHp > 0)
        {
            currentDefenderHpLog = "Current defender hp: " + defender.GetStatistics().currentHp + "\n";
        }
        else
        {
            currentDefenderHpLog = "Defender is dead" + "\n";
        }
        _temporaryLog += currentDefenderHpLog;
    }
}

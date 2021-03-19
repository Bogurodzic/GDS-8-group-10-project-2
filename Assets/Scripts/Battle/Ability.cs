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

        }
        return _temporaryLog;
    }

    private static void DealDamage(int damage, Unit defender)
    {
        int finalDamage = damage;

        string dealDamageLog;


        dealDamageLog = "Defender recieved " + finalDamage + "\n";
        _temporaryLog += dealDamageLog;

        defender.GetStatistics().currentHp = defender.GetStatistics().currentHp - finalDamage;

        string currentDefenderHpLog;

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

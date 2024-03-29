﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attack
{
  private static string _temporaryLog;
    public static string AttackUnit(Unit attacker, Unit defender)
    {
        int damage = new System.Random().Next(attacker.GetStatistics().minAttack, attacker.GetStatistics().maxAttack);
        string damageLog = "Attacker will attack defender for " + damage + "\n";
        _temporaryLog += damageLog;
        DealDamage(damage, defender);
        return _temporaryLog;
    }

    private static void DealDamage(int damage, Unit unit)
    {
      int finalDamage;
      int defend = unit.GetStatistics().defend;
      bool flatDefend = false;
      string currentDefenderHpLog;
      string dealDamageLog;

      if (!flatDefend)
      {
        finalDamage = (int)((float)damage - ((float)damage * ((float)unit.GetStatistics().defend/100)));
      }
      else
      {
        finalDamage = damage - defend;
      }

      finalDamage = Mathf.RoundToInt(finalDamage);

      if (finalDamage < 0)
      {
        finalDamage = 0;
      }

      if (!flatDefend)
      {
        dealDamageLog = "Defender recieved " + finalDamage + "(" + damage + " reduced by "  + defend + "% resistance)" + "\n";
      } else 
      {
        dealDamageLog = "Defender recieved " + finalDamage + "(" + damage + " reduced by " +  defend  + " resistance)"+ "\n";
      }
      
      _temporaryLog += dealDamageLog;
      unit.GetStatistics().currentHp = unit.GetStatistics().currentHp - finalDamage;
      
      if (unit.GetStatistics().currentHp > 0)
      {
        currentDefenderHpLog = "Current defender hp: " + unit.GetStatistics().currentHp + "\n";
      }
      else
      {
        currentDefenderHpLog = "Defender is dead" + "\n";
      }
      
      _temporaryLog += currentDefenderHpLog;
    }
}



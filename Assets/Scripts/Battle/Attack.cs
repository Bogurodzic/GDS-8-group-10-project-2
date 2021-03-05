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
      if (true)
      {
        finalDamage = damage - (damage * unit.GetStatistics().defend);
      }
      else
      {
        finalDamage = damage - unit.GetStatistics().defend;
      }

      finalDamage = Mathf.RoundToInt(finalDamage);

      if (finalDamage < 0)
      {
        finalDamage = 0;
      }

      string dealDamageLog;

      if (true)
      {
        dealDamageLog = "Defender recieved " + finalDamage + "(" + damage + " reduced by "  + unit.GetStatistics().defend + "% resistance)" + "\n";
      } else 
      {
        dealDamageLog = "Defender recieved " + finalDamage + "(" + damage + " reduced by " +  unit.GetStatistics().defend  + " resistance)"+ "\n";
      }
      
      _temporaryLog += dealDamageLog;

      unit.GetStatistics().currentHp = unit.GetStatistics().currentHp - finalDamage;

      string currentDefenderHpLog;

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


/*

  private dealDamage(damage: number, unit: IUnit): void {
    let finalDamage: number;
    if (!this.flatReduction) {
      finalDamage = damage - (damage * (unit.resistance/100));
    } else {
      finalDamage = damage - unit.resistance;
    }
    finalDamage = Math.round(finalDamage);
    if (finalDamage < 0) {
      finalDamage = 0;
    }
    
    let dealDamageLog: string;

    if (!this.flatReduction) {
      dealDamageLog = 'Defender recieved ' + finalDamage + '(' + damage + ' reduced by ' + unit.resistance + '% resistance)';
    } else {
      dealDamageLog = 'Defender recieved ' + finalDamage + '(' + damage + ' reduced by ' + unit.resistance + ' resistance)';
    }
    this.log.push(dealDamageLog);

    unit.hp.current  = unit.hp.current - finalDamage;

    const currentDefenderHp: string =  unit.hp.current > 0 ? 'Current defender hp: ' +  unit.hp.current : 'Defender is dead';
    this.log.push(currentDefenderHp);
  }


*/
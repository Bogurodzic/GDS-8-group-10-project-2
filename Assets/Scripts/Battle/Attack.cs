using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Attack
{
    public static void AttackUnit(Unit attacker, Unit defender)
    {
        defender.GetStatistics().currentHp = defender.GetStatistics().currentHp - attacker.GetStatistics().attack;
    }
}

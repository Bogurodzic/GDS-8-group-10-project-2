using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatistics : MonoBehaviour
{
    public int team = 1;
    public int minAttack;
    public int maxAttack;
    public int defend;
    public int maxHp;
    public int currentHp;
    public bool flatDefend = true;

    public void LoadUnitStatistics(UnitData unitData)
    {
        minAttack = unitData.minAttack;
        maxAttack = unitData.maxAttack;
        defend = unitData.defend;
        maxHp = unitData.maxHp;
        ReloadCurrentHp();
    }

    private void ReloadCurrentHp()
    {
        currentHp = maxHp;
    }
}

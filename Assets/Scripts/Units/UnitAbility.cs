using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitAbility : MonoBehaviour
{

    private AbilitiesData _abilitiesData;
    private Grid _grid;
    private CombatLog _combatLog;
    void Start()
    {
        LoadGrid();
        LoadCombatLog();
    }

    void Update()
    {

    }
    
    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }
    
    private void LoadCombatLog()
    {
        _combatLog = GameObject.Find("CombatLog").GetComponent<CombatLog>();
    }

    public void LoadUnitAbility(UnitData unitData)
    {
        if (unitData.unitAbility)
        {
            _abilitiesData = unitData.unitAbility;
        }
    }

    public void ActiveAbility(int unitPositionX, int unitPositionY)
    {
        Debug.Log("Handle ability 5");

        if (_abilitiesData && _abilitiesData.abilityType != AbilityType.Dash)
        {
            Debug.Log("Handle ability 6");

            ShowAbilityRange(unitPositionX, unitPositionY);
        }
    }

    public bool ExecuteAbility(int targetX, int targetY)
    {
        if (_grid.GetCell(targetX, targetY).GetOccupiedBy() &&
            _grid.GetCell(targetX, targetY).GetPathNode().isAttackable)
        {
            if (_abilitiesData.damage > 0)
            {
                _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(targetX, targetY).GetOccupiedBy()));
                return true;  
            } else if (_abilitiesData.heal > 0)
            {
                _combatLog.LogCombat(Ability.HealUnit(_abilitiesData, _grid.GetCell(targetX, targetY).GetOccupiedBy()));
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void ShowAbilityRange(int unitPositionX, int unitPositionY)
    {
        Debug.Log("Handle ability 7");

        if (_abilitiesData.abilityType == AbilityType.SingleTarget)
        {
            Debug.Log("Handle ability 8");

            _grid.CalculateCostToAllTiles(unitPositionX, unitPositionY, 0, _abilitiesData.minRange, _abilitiesData.maxRange);
            _grid.HideRange();
            _grid.ShowRange(RangeType.Attack);
        }

        if (_abilitiesData.abilityType == AbilityType.Circle)
        {
            
        }
        
        
    }
}

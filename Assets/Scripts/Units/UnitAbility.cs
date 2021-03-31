using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitAbility : MonoBehaviour
{

    private AbilitiesData _abilitiesData;
    private Grid _grid;
    private CombatLog _combatLog;
    private int cdTurns = 0;
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

    public bool ExecuteAbility(int targetX, int targetY, int unitX, int unitY)
    {
        Debug.Log("ExecuteAbility1");
        if (_abilitiesData.abilityType == AbilityType.SingleTarget)
        {
            Debug.Log("ExecuteAbility2");

            if (_grid.GetCell(targetX, targetY).GetOccupiedBy() &&
                _grid.GetCell(targetX, targetY).GetPathNode().isAttackable)
            {
                Debug.Log("ExecuteAbility3");

                if (_abilitiesData.damage > 0)
                {
                    Debug.Log("ExecuteAbility4");

                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(targetX, targetY).GetOccupiedBy()));
                    PutAbilityOnCD();
                    _grid.GetCell(targetX, targetY).GetOccupiedBy().SetHealth();

                    return true;  
                } else if (_abilitiesData.heal > 0)
                {
                    Debug.Log("ExecuteAbility5");

                    _combatLog.LogCombat(Ability.HealUnit(_abilitiesData, _grid.GetCell(targetX, targetY).GetOccupiedBy()));
                    _grid.GetCell(targetX, targetY).GetOccupiedBy().SetHealth();

                    PutAbilityOnCD();
                    return true;
                }
            }
            else
            {
                Debug.Log("ExecuteAbility6");

                return false;
            }         
        } else if (_abilitiesData.abilityType == AbilityType.Circle)
        {
            Debug.Log("Handle ability 9.2");
            if (_grid.GetCell(targetX, targetY).GetOccupiedBy() &&
                _grid.GetCell(targetX, targetY).GetPathNode().isAttackable)
            {
                Debug.Log("Handle ability 10.2");
                if (_grid.GetCell(unitX + 1, unitY) != null && _grid.GetCell(unitX + 1, unitY).GetOccupiedBy())
                {
                    _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX + 1, unitY).GetOccupiedBy()));
                    _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().SetHealth();
                }
                    
                if (_grid.GetCell(unitX - 1, unitY) != null &&_grid.GetCell(unitX - 1, unitY ).GetOccupiedBy())
                {
                    _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX - 1, unitY).GetOccupiedBy()));
                    _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().SetHealth();
                }
                    
                if (_grid.GetCell(unitX, unitY + 1) != null &&_grid.GetCell(unitX, unitY + 1).GetOccupiedBy())
                {
                    _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX, unitY + 1).GetOccupiedBy()));
                    _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().SetHealth();
                }
                    
                if (_grid.GetCell(unitX, unitY - 1) != null &&_grid.GetCell(unitX, unitY - 1).GetOccupiedBy())
                {
                    _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX, unitY - 1).GetOccupiedBy()));
                    _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().SetHealth();
                }

                if ((_grid.GetCell(unitX + 1, unitY) != null && _grid.GetCell(unitX + 1, unitY).GetOccupiedBy()) ||
                    (_grid.GetCell(unitX - 1, unitY) != null && _grid.GetCell(unitX - 1, unitY).GetOccupiedBy()) ||
                    (_grid.GetCell(unitX, unitY + 1) != null && _grid.GetCell(unitX, unitY + 1).GetOccupiedBy())||
                    (_grid.GetCell(unitX, unitY - 1) != null && _grid.GetCell(unitX, unitY - 1).GetOccupiedBy()))
                {
                    Debug.Log("Handle ability 11.2");
                    PutAbilityOnCD();
                    return true;
                }
                else
                {
                    Debug.Log("Handle ability 12.2");
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        return false;

    }

    private void ShowAbilityRange(int unitPositionX, int unitPositionY)
    {
        int unitTeam = gameObject.GetComponentInParent<Unit>().GetStatistics().team;
        Debug.Log("Handle ability 7");

        if (_abilitiesData.abilityType == AbilityType.SingleTarget)
        {
            Debug.Log("Handle ability 8.1");

            _grid.CalculateCostToAllTiles(unitPositionX, unitPositionY, 0, _abilitiesData.minRange, _abilitiesData.maxRange, unitTeam);
            _grid.HideRange();
            _grid.ShowRange(RangeType.Attack);
        }

        if (_abilitiesData.abilityType == AbilityType.Circle)
        {
            Debug.Log("Handle ability 8.2");

            _grid.CalculateCostToAllTiles(unitPositionX, unitPositionY, 0, 1, 1, unitTeam);
            _grid.HideRange();
            _grid.ShowRange(RangeType.Attack);
        }
    }

    public bool IsAbilityReadyToCast()
    {
        if (_abilitiesData && cdTurns == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PutAbilityOnCD()
    {
        cdTurns = _abilitiesData.cooldown;
    }

    public void RemoveOneTurnFromAbilityCD()
    {
        if (cdTurns > 0)
        {
            cdTurns = cdTurns - 1;
        } 
    }

    public int GetRemainingAbilityCD()
    {
        return cdTurns;
    }

    public AbilitiesData GetAbilitiesData()
    {
        return _abilitiesData;
    }

    public AbilityType GetAbilityType()
    {
        if (_abilitiesData != null)
        {
            return _abilitiesData.abilityType;
        }
        else
        {
            return AbilityType.None;
        }
    }
}

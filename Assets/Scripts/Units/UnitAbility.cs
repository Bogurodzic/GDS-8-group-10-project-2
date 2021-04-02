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
    private UnitSounds _unitSounds;
    void Start()
    {
        LoadGrid();
        LoadCombatLog();
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

        _unitSounds = gameObject.GetComponent<UnitSounds>();
    }

    public void ActiveAbility(int unitPositionX, int unitPositionY)
    {
        if (_abilitiesData && _abilitiesData.abilityType != AbilityType.Dash)
        {
            ShowAbilityRange(unitPositionX, unitPositionY);
        }
    }

    public bool ExecuteAbility(int targetX, int targetY, int unitX, int unitY)
    {
        Unit unit = _grid.GetCell(unitX, unitY).GetOccupiedBy();

        if (_abilitiesData.abilityType == AbilityType.SingleTarget)
        {
            if (_grid.GetCell(targetX, targetY).GetOccupiedBy() &&
                _grid.GetCell(targetX, targetY).GetPathNode().isAttackable)
            {
                if (_abilitiesData.damage > 0 && _grid.GetCell(targetX, targetY).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                {
                    _unitSounds.PlayAbilitySound();
                    _grid.GetCell(targetX, targetY).GetOccupiedBy().GetUnitAnimations().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(targetX, targetY).GetOccupiedBy()));
                    PutAbilityOnCD();
                    _grid.GetCell(targetX, targetY).GetOccupiedBy().SetHealth();
                    _grid.GetCell(targetX, targetY).GetOccupiedBy().HandleDeath();

                    return true;  
                } else if (_abilitiesData.heal > 0 && _grid.GetCell(targetX, targetY).GetOccupiedBy().GetStatistics().team == unit.GetStatistics().team)
                {
                    _unitSounds.PlayAbilitySound();
                    _combatLog.LogCombat(Ability.HealUnit(_abilitiesData, _grid.GetCell(targetX, targetY).GetOccupiedBy()));
                    _grid.GetCell(targetX, targetY).GetOccupiedBy().SetHealth();

                    PutAbilityOnCD();
                    return true;
                }
            }
            else
            {
                return false;
            }         
        } else if (_abilitiesData.abilityType == AbilityType.Circle)
        {
            if (_grid.GetCell(targetX, targetY).GetOccupiedBy() &&
                _grid.GetCell(targetX, targetY).GetPathNode().isAttackable)
            {
                if (_grid.GetCell(unitX + 1, unitY) != null && _grid.GetCell(unitX + 1, unitY).GetOccupiedBy() && _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                {
                    _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().GetUnitAnimations().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX + 1, unitY).GetOccupiedBy()));
                    _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().SetHealth();
                    _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().HandleDeath();
                }
                    
                if (_grid.GetCell(unitX - 1, unitY) != null &&_grid.GetCell(unitX - 1, unitY ).GetOccupiedBy()  && _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                {
                    _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().GetUnitAnimations().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX - 1, unitY).GetOccupiedBy()));
                    _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().SetHealth();
                    _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().HandleDeath();
                }
                    
                if (_grid.GetCell(unitX, unitY + 1) != null &&_grid.GetCell(unitX, unitY + 1).GetOccupiedBy()  && _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                {
                    _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().GetUnitAnimations().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX, unitY + 1).GetOccupiedBy()));
                    _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().SetHealth();
                    _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().HandleDeath();
                }
                    
                if (_grid.GetCell(unitX, unitY - 1) != null &&_grid.GetCell(unitX, unitY - 1).GetOccupiedBy()  && _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                {
                    _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().GetUnitAnimations().AnimateUnit("HURT");
                    _combatLog.LogCombat(Ability.AttackUnit(_abilitiesData, _grid.GetCell(unitX, unitY - 1).GetOccupiedBy()));
                    _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().SetHealth();
                    _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().HandleDeath();
                }

                if ((_grid.GetCell(unitX + 1, unitY) != null && _grid.GetCell(unitX + 1, unitY).GetOccupiedBy()) && _grid.GetCell(unitX + 1, unitY).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team ||
                    (_grid.GetCell(unitX - 1, unitY) != null && _grid.GetCell(unitX - 1, unitY).GetOccupiedBy()) && _grid.GetCell(unitX - 1, unitY).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team ||
                    (_grid.GetCell(unitX, unitY + 1) != null && _grid.GetCell(unitX, unitY + 1).GetOccupiedBy()) && _grid.GetCell(unitX, unitY + 1).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team ||
                    (_grid.GetCell(unitX, unitY - 1) != null && _grid.GetCell(unitX, unitY - 1).GetOccupiedBy()) && _grid.GetCell(unitX, unitY - 1).GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                {
                    _unitSounds.PlayAbilitySound();
                    PutAbilityOnCD();
                    return true;
                }
                else
                {
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

        if (_abilitiesData.abilityType == AbilityType.SingleTarget)
        {
            _grid.CalculateCostToAllTiles(unitPositionX, unitPositionY, 0, _abilitiesData.minRange, _abilitiesData.maxRange, unitTeam);
            _grid.HideRange();
            _grid.ShowRange(RangeType.Attack);
        }

        if (_abilitiesData.abilityType == AbilityType.Circle)
        {
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

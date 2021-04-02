using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D movementCursor;
    public Texture2D attackCursor;
    public Texture2D healSkillCursor;
    public Texture2D throwSkillCursor;
    public Texture2D whirlwindSkillCursor;

    private bool _isCursorChangerInitialised = false;
    private Grid _grid;
    private GridManager _gridManager;
    
    private void ChangeCursorTo(Texture2D newCursor)
    {
        Cursor.SetCursor(newCursor, new Vector2(0, 25), CursorMode.ForceSoftware);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
    }

    public void ShowCursor(Unit unit)
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        if (mouseX < _grid.GetGridWidth() && mouseY < _grid.GetGridHeight() && mouseX >= 0 && mouseY >= 0)
        {
            GridCell gridCell = _grid.GetCell(mouseX, mouseY);
            PathNode pathNode = gridCell.GetPathNode();

            UnitPhase unitPhase = unit.GetUnitPhase();
        
            switch (unitPhase)
            {

                case UnitPhase.Standby:
                    if (pathNode.isMovable)
                    {
                        ChangeCursorTo(movementCursor);
                        break;
                    }
                    else if (pathNode.isAttackable && pathNode.isOccupied && gridCell.GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                    {
                        ChangeCursorTo(attackCursor);
                        break;
                    }
                    else
                    {
                        ResetCursor();
                        break;
                    }
                case UnitPhase.AbilityActivated:
                    if (pathNode.isAttackable)
                    {
                        if (unit.unitData.unitAbility.abilityName == "Whirlwind" && pathNode.isOccupied && gridCell.GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                        {
                            ChangeCursorTo(whirlwindSkillCursor);
                        } else if (unit.unitData.unitAbility.abilityName == "Throw" && pathNode.isOccupied && gridCell.GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                        {
                            ChangeCursorTo(throwSkillCursor);

                        } else if (unit.unitData.unitAbility.abilityName == "Heal" && pathNode.isOccupied && gridCell.GetOccupiedBy().GetStatistics().team == unit.GetStatistics().team)
                        {
                            ChangeCursorTo(healSkillCursor);
                        }
                        else
                        {
                            ResetCursor();
                        }
                        break;
                    }
                    else
                    {
                        ResetCursor();
                        break;
                    }
                case UnitPhase.AfterMovement:
                    if (pathNode.isAttackable && pathNode.isOccupied && gridCell.GetOccupiedBy().GetStatistics().team != unit.GetStatistics().team)
                    {
                        ChangeCursorTo(attackCursor);
                        break;
                    }
                    else
                    {
                        ResetCursor();
                        break;
                    }
                    break;
                case UnitPhase.AfterAttack:
                    if (pathNode.isMovable)
                    {
                        ChangeCursorTo(movementCursor);
                        break;
                    }
                    else
                    {
                        ResetCursor();
                        break;
                    }
                case UnitPhase.AfterDash:
                    break;
                case UnitPhase.OnCooldown:
                    break;
                default:
                    ResetCursor();
                    break;
            }
        }
        else
        {
            ResetCursor();
        }
    }

    public void Initialise(Grid grid, GridManager gridManager)
    {
        _grid = grid;
        _gridManager = gridManager;
        _isCursorChangerInitialised = true;
    }

    public bool IsCursorChangerInitialised()
    {
        return _isCursorChangerInitialised;
    }
}

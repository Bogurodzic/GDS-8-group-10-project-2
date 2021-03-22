using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int movementRange;
    private Grid _grid;
    private int _xPosition;
    private int _yPosition;
    void Start()
    {
        LoadGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }

        UpdateUnitPosition();
    }

    public void LoadUnitMovement(UnitData unitData)
    {
        movementRange = unitData.movementRange;
    }

    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }
    
    private void UpdateUnitPosition()
    {
        _grid.GetCellPosition(transform.position, out _xPosition, out _yPosition);
    }

    public int GetUnitXPosition()
    {
        return _xPosition;
    }
    
    public int GetUnitYPosition()
    {
        return _yPosition;
    }

    public void ShowMovementRange(bool hidePreviouseRange = true)
    {
        if (hidePreviouseRange)
        {
            _grid.HideRange();
        }

        _grid.ShowRange(RangeType.Movement);
    }
    
    public void HideMovementRange()
    {
        _grid.HideRange();
    }

    public bool IsInMovementRange(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _grid.GetGridWidth() && y < _grid.GetGridHeight() && _grid.GetCell(x, y).GetPathNode().isMovable &&  !_grid.GetCell(x, y).GetPathNode().isOccupied)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(int x, int y, Unit unit)
    {
        RemoveUnitFromCurrentCell();
        Vector3 cellPositionCenter = _grid.GetCellCenter(x, y);
        cellPositionCenter.z = -1;
        transform.position = cellPositionCenter;
        AddUnitToCurrentCell(unit);
    }

    public void MoveBeforeAttack(int x, int y, Unit unit)
    {
        PathNode targetNode = _grid.GetCell(x, y).GetPathNode();
        PathNode lastMovableNode = GetOptimalDistanceNode(targetNode, unit) ;
        
        Move(lastMovableNode.x, lastMovableNode.y, unit);
    }

    private PathNode GetOptimalDistanceNode(PathNode targetNode, Unit unit)
    {
        
        PathNode optimalDistanceNode = targetNode.lastMovableNode;
       /* int optimalDistance = targetNode.hCost - optimalDistanceNode.hCost;

        while (optimalDistance < unit.getUnitRange().maxRange)
        {
            if (targetNode.hCost - optimalDistanceNode.cameFromNode.hCost > unit.getUnitRange().maxRange)
            {
                break;
            }
            
            optimalDistanceNode = optimalDistanceNode.cameFromNode;
            optimalDistance = targetNode.hCost - optimalDistanceNode.hCost;
        } */

        return optimalDistanceNode;
    }

    private void RemoveUnitFromCurrentCell()
    {
        UpdateUnitPosition();
        _grid.GetCell(GetUnitXPosition(), GetUnitYPosition()).RemoveOccupiedBy();
    }

    private void AddUnitToCurrentCell(Unit unit)
    {
        UpdateUnitPosition();
        _grid.GetCell(GetUnitXPosition(), GetUnitYPosition()).AddOccupiedBy(unit);
    }
    
}

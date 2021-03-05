using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int movementRange = 5;
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

    public void ShowMovementRange()
    {
        _grid.HideRange();
        _grid.ShowRange(GetUnitXPosition(), GetUnitYPosition(), movementRange, movementRange, RangeType.Movement);
    }
    
    public void HideMovementRange()
    {
        _grid.HideRange();
    }

    public bool IsInMovementRange(int x, int y)
    {
        _grid.CalculateCostToAllTiles(_xPosition, _yPosition);
        if (_grid.IsPositionInRange(x, y, movementRange, movementRange, RangeType.Movement))
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

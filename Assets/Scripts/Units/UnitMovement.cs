using System.Collections;
using System.Collections.Generic;
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
        //LoadUnit();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           // HandleMovingUnit();
        }

        UpdateUnitPosition();
    }

    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }

    /*private void LoadUnit()
    {
        _unit = gameObject.GetComponent<Unit>();
    }

    private void HandleMovingUnit()
    {
        if (_unit.IsActive())
        {
            Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
            int x, y;
            _grid.GetCellPosition(mouseVector3, out x, out y);
            if (IsInMovementRange(x, y))
            {
                Move(x, y);
            }
        }
    }*/

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

    public bool IsInMovementRange(int x, int y)
    {
        _grid.CalculateCostToAllTiles(_xPosition, _yPosition);
        if (_grid.IsPositionInRange(x, y, movementRange))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(int x, int y)
    {
        Vector3 cellPositionCenter = _grid.GetCellCenter(x, y);
        cellPositionCenter.z = -1;
        transform.position = cellPositionCenter; 
    }
}

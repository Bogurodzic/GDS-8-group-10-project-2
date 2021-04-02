using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private GridCell[,] gridArray;
    private Pathfinding _pathfinding;
    private GridManager _gridManager;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, GridManager gridManager)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this._gridManager = gridManager;
        
        gridArray = new GridCell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridCell(x, y, null, GetCellPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, _gridManager);
            }
        }
        _pathfinding = new Pathfinding(this);
    }

    public bool IsClickInBoardRange(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    public GridCell GetCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return null;
        }
    }

    public Vector3 GetCellCenter(int x, int y)
    {
        return (new Vector3(x, y) * cellSize + originPosition) + new Vector3(cellSize/2, cellSize/2, 0) ;
    }
    
    public Vector3 GetCellCenter(Vector3 worldPosition)
    {
        int x, y;
        GetCellPosition(worldPosition, out x, out y);
        return GetCellCenter(x, y);
    }

    public Vector3 GetCellPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetCellPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void ShowRange(RangeType rangeType)
    {
        
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                
                if (rangeType == RangeType.Movement)
                {
                    if (GetCell(x, y).GetPathNode().isMovable)
                    {
                        _gridManager.ChangeColor(x, y, Color.blue);
                    }
                }

                if (rangeType == RangeType.Attack)
                {
                    if (GetCell(x, y).GetPathNode().isAttackable)
                    {
                        _gridManager.ChangeColor(x, y, Color.red);
                    } 
                }

            }
        }
    }
    
    public void HideRange()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                _gridManager.ResetColor(x, y);
            }
        }
    }
    
    public bool IsPositionInAttackRange(int x, int y, int minRange, int maxRange)
    {
        int cost = GetCell(x, y).GetPathNode().hCost;

        if (minRange <= cost && maxRange >= cost)
        {   
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetGridWidth()
    {
        return gridArray.GetLength(0);
    }

    public int GetGridHeight()
    {
        return gridArray.GetLength(1);
    }
    
    public void CalculateCostToAllTiles(int unitXPosition, int unitYPosition, int movementRange, int minAttackRange,
        int maxAttackRange, int team)
    {
        _pathfinding.CalculateCostToAllTiles(unitXPosition, unitYPosition, movementRange, minAttackRange, maxAttackRange, team);
    }
    
    public void HiglightLeftDeployArea()
    {
        
        for (int i = 0; i < GetGridHeight(); i++)
        {
            _gridManager.ChangeColor(0, i, Color.yellow);
        }
    }

    public void HiglightRightDeployArea()
    {
        for (int i = 0; i < GetGridHeight(); i++)
        {
            _gridManager.ChangeColor(GetGridWidth()-1, i, Color.yellow);
        }
    }

    public bool IsMouseOverDeploymentCell()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        GetCellPosition(mouseVector3, out mouseX, out mouseY);
        
        if (Turn.GetUnitTurn() == 1)
        {
            if (mouseX == 0 && (mouseY >= 0 && mouseY < GetGridHeight()) && !GetCell(mouseX, mouseY).GetPathNode().isOccupied)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (mouseX == GetGridWidth() - 1 && (mouseY >= 0 && mouseY < GetGridHeight()) && !GetCell(mouseX, mouseY).GetPathNode().isOccupied)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

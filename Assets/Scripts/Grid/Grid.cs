using System;
using System.Collections;
using System.Collections.Generic;
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
                //Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x, y+1), Color.white, 100f);
                //Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x+1, y), Color.white, 100f);
            }
        }
        
        //Debug.DrawLine(GetCellPosition(0, height), GetCellPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetCellPosition(width, 0), GetCellPosition(width, height), Color.white, 100f);
        _pathfinding = new Pathfinding(this);
    }

    public GridCell GetCell(int x, int y)
    {
        return gridArray[x, y];
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

    public void SetValue(int x, int y, string value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y].SetText(value + "");
        } 
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetCellPosition(worldPosition, out x, out y);
        SetValue(x, y, value + "");
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            GridCell cell = gridArray[x, y];
            return x + y;
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetCellPosition(worldPosition, out x, out y);
        return GetValue(x, y);
    }


    public void ShowRange(int unitXPosition, int unitYPosition, int range)
    {
        CalculateCostToAllTiles(unitXPosition, unitYPosition);
        
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (IsPositionInRange(x, y, range))
                {
                    _gridManager.ChangeColor(x, y, Color.red);
                }
                else
                {
                    _gridManager.ResetColor(x, y);
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
                SetValue(x, y, "");
            }
        }
    }

    public bool IsPositionInRange(int x, int y, int range)
    {
        int gCost = GetCell(x, y).GetPathNode().gCost;
        //SetValue(x, y, gCost + "");
        //Debug.Log("range:" + range + " X: " + x + " Y:" + y +  " G:" + gCost + "?" + (range <= gCost));
        if (range >= gCost)
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

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        return _pathfinding.FindPath(startX, startY, endX, endY);
    }

    public void CalculateCostToAllTiles(int x, int y)
    {
        _pathfinding.CalculateCostToAllTiles(x,y);
    }
}

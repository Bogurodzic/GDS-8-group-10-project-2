using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private GridCell[,] gridArray;

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new GridCell[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.Log(x + " " + y);
                gridArray[x, y] = new GridCell(x + "," + y, null, GetCellPosition(x, y) + new Vector3(cellSize, cellSize) * .5f);
                Debug.Log(gridArray[x,y]);
                Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x, y+1), Color.white, 100f);
                Debug.DrawLine(GetCellPosition(x, y), GetCellPosition(x+1, y), Color.white, 100f);
            }
        }
        
        Debug.DrawLine(GetCellPosition(0, height), GetCellPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetCellPosition(width, 0), GetCellPosition(width, height), Color.white, 100f);
        gridArray[2,2].SetText("69");
    }

    private Vector3 GetCellPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y].SetText(value + "");
        } 
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
}

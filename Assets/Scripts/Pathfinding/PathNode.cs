using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    private Grid _grid;
    
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;
    public float movementCost;
    public bool isObstacle;

    public PathNode cameFromNode;

    public PathNode(Grid grid, int x, int y, TileData tileData)
    {
        this._grid = grid;
        this.x = x;
        this.y = y;
        this.movementCost = tileData.movementCost;
        this.isObstacle = tileData.isObstacle;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + " " + y;
    }
}

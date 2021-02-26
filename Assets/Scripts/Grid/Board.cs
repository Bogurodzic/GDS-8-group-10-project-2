using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    private Grid grid;
    private GridManager _gridManager;
    private Pathfinding _pathfinding;

    private void Awake()
    {
        LoadGridManager();
        Debug.Log(_gridManager);
        //grid = new Grid(_gridManager.GetGridSize().x, _gridManager.GetGridSize().y, _gridManager.GetGridCellSize().x, new Vector3( _gridManager.GetGridPosition().x * _gridManager.GetGridCellSize().x,_gridManager.GetGridPosition().y * _gridManager.GetGridCellSize().x,0));
        _pathfinding = new Pathfinding(_gridManager.GetGridSize().x, _gridManager.GetGridSize().y, _gridManager.GetGridCellSize().x, new Vector3( _gridManager.GetGridPosition().x * _gridManager.GetGridCellSize().x,_gridManager.GetGridPosition().y * _gridManager.GetGridCellSize().x,0));
        Debug.Log(grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
            mouseVector3.z = 0;
            //Debug.Log(mouseVector3);
            //_pathfinding.GetGrid().SetValue(mouseVector3, 666);
            List<PathNode> path = _pathfinding.FindPath(0, 0, 5, 5);
            Debug.Log(path);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.Log(path[i]);

                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f , new Vector3(path[i+1].x, path[i+1].y) * 10f, Color.red);
                    _gridManager.DestroyTile(new Vector3Int(path[i].x, path[i].y, 0));
                }
            }
        }
    }
    

    public Grid GetGrid()
    {
        return grid;
    }

    private void LoadGridManager()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }
}

﻿using System;
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
            int endX, endY;
            _pathfinding.GetGrid().GetCellPosition(mouseVector3, out endX, out endY);
            List<PathNode> path = _pathfinding.FindPath(0, 0, endX, endY);
            Debug.Log(path);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.Log(i + ":" + path[i]);
                    _gridManager.DestroyTile(path[i].x, path[i].y);
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

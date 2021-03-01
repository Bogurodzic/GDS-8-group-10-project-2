using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    private Grid _grid;
    private GridManager _gridManager;
    private void Awake()
    {
        LoadGridManager();
        Debug.Log(_gridManager);
        _grid = new Grid(_gridManager.GetGridSize().x, _gridManager.GetGridSize().y, _gridManager.GetGridCellSize().x, new Vector3( _gridManager.GetGridPosition().x * _gridManager.GetGridCellSize().x,_gridManager.GetGridPosition().y * _gridManager.GetGridCellSize().x,0), this._gridManager);
        Debug.Log(_grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ChooseCell();
            //Move();
        }
    }

    private void Move()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int endX, endY;
        _grid.GetCellPosition(mouseVector3, out endX, out endY);
        List<PathNode> path = _grid.FindPath(0, 0, endX, endY);
        Debug.Log(path);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.Log(i + ":" + path[i]);
                _gridManager.ChangeColor(path[i].x, path[i].y, Color.blue);
            }
        }
    }

    private void ChooseCell()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        Debug.Log(mouseVector3);
        _grid.SetValue(mouseVector3, 666); 
    }
    

    public Grid GetGrid()
    {
        return _grid;
    }
    
    private void LoadGridManager()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }
}

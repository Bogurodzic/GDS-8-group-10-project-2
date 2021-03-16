using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    private Grid _grid;
    private GridManager _gridManager;
    private UnitList _unitList;
    private void Awake()
    {
        LoadGridManager();
        LoadUnitList();
        _grid = new Grid(_gridManager.GetGridSize().x, _gridManager.GetGridSize().y, _gridManager.GetGridCellSize().x, new Vector3( _gridManager.GetGridPosition().x * _gridManager.GetGridCellSize().x,_gridManager.GetGridPosition().y * _gridManager.GetGridCellSize().x,0), this._gridManager);
    }

    void Update()
    {
        if (Turn.GetCurrentTurnType() == TurnType.Deployment && _unitList.ReadyForDeploy())
        {
            HandleDeployment();
        }
    }

    private void HandleDeployment()
    {
        Debug.Log(_grid);
        Debug.Log(_unitList);
        Debug.Log(_unitList.GetNextUnitToDeploy());
        Debug.Log(_unitList.GetNextUnitToDeploy().GetComponent<Unit>());
        if (_grid.IsMouseOverDeploymentCell() && !_unitList.GetNextUnitToDeploy().GetComponent<Unit>().IsUnitPreDeployed())
        {
            ShowUnitOverCell();
        }
        else if (!_grid.IsMouseOverDeploymentCell() && !_unitList.GetNextUnitToDeploy().GetComponent<Unit>().IsUnitPreDeployed())
        {
            HideUnit();
        }

        if (Input.GetMouseButtonDown(0) && _grid.IsMouseOverDeploymentCell())
        {
            DeployUnit();
            if (_unitList.GetNextUnitToDeploy().GetComponent<Unit>().IsUnitDeployed())
            {
                if (_unitList.IsAnyUnitToDeploy())
                {
                    Turn.NextTurn();
                    _unitList.HandleNextUnitToDeploy();
                }
                else
                {
                    _grid.HideRange();
                    Turn.NextTurn();
                    Turn.SetTurnType(TurnType.RegularGame);
                }

            }
        }
    }

    private void DeployUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int x, y;
        _grid.GetCellPosition(mouseVector3, out x, out y);
        _unitList.GetNextUnitToDeploy().GetComponent<Unit>().HandleDeployment(x, y);
    }

    private void ShowUnitOverCell()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        Vector3 cellCenter = _grid.GetCellCenter(mouseVector3);
        _unitList.GetNextUnitToDeploy().transform.position = cellCenter;

    }

    private void HideUnit()
    {
        _unitList.GetNextUnitToDeploy().transform.position = Vector3.one * -999;
    }
    
    public Grid GetGrid()
    {
        return _grid;
    }
    
    private void LoadGridManager()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }
}

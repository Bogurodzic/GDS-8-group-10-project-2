using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitDeployment : MonoBehaviour
{
    private bool _deploymentActive = false;
    private Grid _grid;
    private UnitList _unitList;
    
    void Start()
    {
        LoadGrid();
        LoadUnitList();
    }

    void Update()
    {
        if (CanActiveDeployment() && !IsDeploymentActive())
        {
            ActiveDeployment();
        }
    }

    private void LoadGrid()
    {
        _grid = gameObject.GetComponent<Board>().GetGrid();
    }

    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }

    public void ActiveDeployment()
    {
        _deploymentActive = true;
        //_grid.HiglightLeftDeployArea();
       // _grid.HiglightRightDeployArea();
        _unitList.CreateUnitListForPlayers(PickedUnits.Get1PlayerPickedUnits(), PickedUnits.Get2PlayerPickedUnits());
        PrepareAreaForPlacingUnit();
    }

    private void PrepareAreaForPlacingUnit()
    {
        if (Turn.GetUnitTurn() == 1)
        {
            _grid.HideRange();
            _grid.HiglightLeftDeployArea();
            _unitList.HandleNextUnitToDeploy();
        }
        else
        {
            _grid.HideRange();
            _grid.HiglightRightDeployArea();
        }
    }

    public void DeactiveDeployment()
    {
        _deploymentActive = false;
    }

    public bool IsDeploymentActive()
    {
        return _deploymentActive;
    }

    private bool CanActiveDeployment()
    {
        return Turn.GetCurrentTurnType() == TurnType.Deployment;
    }
}

using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitRange : MonoBehaviour
{
    public int minRange = 2;
    public int maxRange = 3;
    
    private bool _showRange = false;
    private Grid _grid;
    private UnitMovement _unitMovement;
    
    void Start()
    {
        LoadComponents();
    }

    void Update()
    {

    }
    
    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }

    private void LoadUnitMovement()
    {
        _unitMovement = gameObject.GetComponent<UnitMovement>();
    }

    private void LoadComponents()
    {
        LoadGrid();
        LoadUnitMovement();
    }
    

    public void ShowUnitRange(bool hidePreviouseRange = true)
    {
        if (hidePreviouseRange)
        {
            _grid.HideRange();
        }

        _grid.ShowRange(RangeType.Attack);
    }

    public void HideRange()
    {
        _grid.HideRange();
    }
    
    public bool IsInAttackRange(int x, int y, int targetX, int targetY)
    {
        //_grid.CalculateCostToAllTiles(x, y, RangeType.Attack);
        if (_grid.GetCell(targetX, targetY).GetPathNode().isAttackable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

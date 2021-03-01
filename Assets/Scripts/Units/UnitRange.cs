using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitRange : MonoBehaviour
{
    public int range = 1;
    
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
    

    public void ShowUnitRange(int x, int y)
    {
        HideRange();
        _grid.ShowRange(x, y, range, RangeType.Attack);
    }

    public void HideRange()
    {
        _grid.HideRange();
    }
    
    public bool IsInAttackRange(int x, int y, int targetX, int targetY)
    {
        _grid.CalculateCostToAllTiles(x, y);
        if (_grid.IsPositionInRange(targetX, targetY, range, RangeType.Attack))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

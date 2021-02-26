using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange : MonoBehaviour
{
    public int range = 5;
    
    private bool _showRange = false;
    private Grid _grid;
    private UnitMovement _unitMovement;
    
    void Start()
    {
        LoadComponents();
    }

    void Update()
    {
        if (_showRange)
        {
            ShowUnitRange();
        }
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

    private void ShowUnitRange()
    {
        int unitXPosition = _unitMovement.GetUnitXPosition();
        int unitYPosition = _unitMovement.GetUnitYPosition();
        //Debug.Log("Show " + unitXPosition + " " + unitYPosition);
        _grid.ShowRange(unitXPosition, unitYPosition, range);
    }
}

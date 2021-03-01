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
        if (Input.GetKeyDown(KeyCode.Space) && !_showRange)
        {
            ShowPlayerRange();
            _showRange = true;
        } else if (Input.GetKeyDown(KeyCode.Space) && _showRange)
        {
            HideRange();
            _showRange = false;
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

    private void ShowPlayerRange()
    {
        int unitXPosition = _unitMovement.GetUnitXPosition();
        int unitYPosition = _unitMovement.GetUnitYPosition();
        ShowUnitRange(unitXPosition, unitYPosition, range);
    }

    private void ShowUnitRange(int x, int y, int range)
    {
        _grid.ShowRange(x, y, range);
    }

    private void HideRange()
    {
        _grid.HideRange();
    }
}

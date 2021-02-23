using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private Grid _grid;
    private bool _isActive = true;
    
    private int _xPosition;
    private int _yPosition;
    void Start()
    {
        LoadGrid();
    }

    void Update()
    {
        if (_isActive)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
                Vector3 cellPosition = _grid.GetCellCenter(mouseVector3);
                cellPosition.z = -1;
                transform.position = cellPosition;
            }
            
        }

        UpdateUnitPosition();
    }

    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
        Debug.Log(_grid);
    }

    private void UpdateUnitPosition()
    {
        _grid.GetCellPosition(transform.position, out _xPosition, out _yPosition);
        Debug.Log(_xPosition + " " + _yPosition);
    }

    public int GetUnitXPosition()
    {
        return _xPosition;
    }
    
    public int GetUnitYPosition()
    {
        return _yPosition;
    }
}

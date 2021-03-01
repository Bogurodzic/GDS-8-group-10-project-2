using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool _isActive = false;
    private Grid _grid;
    private GridManager _gridManager;
    private UnitMovement _unitMovement;
    void Start()
    {
        LoadGrid();
        LoadGridManager();
        LoadUnitMovement();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleActivatingUnit();
        }
    }

    private void HandleActivatingUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if (IsUnitTurn() && IsUnitClicked(mouseX, mouseY))
        {
            ActivateUnit();
        }
        else if (IsActive() && !_unitMovement.IsInMovementRange(mouseX, mouseY))
        {
            DeactivateUnit();
        } else if (IsActive() && _unitMovement.IsInMovementRange(mouseX, mouseY))
        {
            _unitMovement.Move(mouseX, mouseY);
            DeactivateUnit();
        }
    }
    

    public bool IsActive()
    {
        return _isActive;
    }

    public bool IsUnitTurn()
    {
        return true;
    }

    public bool IsUnitClicked(int mouseX, int mouseY)
    {
        int positionX, positionY;
        _grid.GetCellPosition(transform.position, out positionX, out positionY);
        if (mouseX == positionX && mouseY == positionY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void ActivateUnit()
    {
        _grid.HideRange();
        _grid.ShowRange(GetUnitXPosition(), GetUnitYPosition(), 5);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        _isActive = true;
    }

    public void DeactivateUnit()
    {
        _grid.HideRange();
        _gridManager.ResetColor(GetUnitXPosition(), GetUnitYPosition());
        _isActive = false;
    }
    
    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }

    private void LoadGridManager()
    {
        _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    private void LoadUnitMovement()
    {
        _unitMovement = gameObject.GetComponent<UnitMovement>();
    }

    private int GetUnitXPosition()
    {
        int positionX, positionY;
        _grid.GetCellPosition(transform.position, out positionX, out positionY);
        return positionX;
    }
    
    private int GetUnitYPosition()
    {
        int positionX, positionY;
        _grid.GetCellPosition(transform.position, out positionX, out positionY);
        return positionY;
    }
}

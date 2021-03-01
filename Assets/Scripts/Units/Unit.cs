using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool _isActive = false;
    private Grid _grid;
    private GridManager _gridManager;
    private UnitMovement _unitMovement;
    private UnitStatistics _unitStatistics;
    private UnitRange _unitRange;
    void Start()
    {
        LoadGrid();
        LoadGridManager();
        LoadUnitMovement();
        LoadUnitStatistics();
        LoadUnitRange();
        PlaceUnitOnBoard();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleActivatingUnit();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (IsActive())
            {
                _unitRange.ShowUnitRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition());
            }
        }
    }

    private void PlaceUnitOnBoard()
    {
        int positionX, positionY;
        _grid.GetCellPosition(transform.position, out positionX, out positionY);

        _grid.GetCell(positionX, positionY).AddOccupiedBy(this);
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
            _unitMovement.Move(mouseX, mouseY, this);
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
        _unitMovement.ShowMovementRange();
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        _isActive = true;
    }

    public void DeactivateUnit()
    {
        _unitMovement.HideMovementRange();
        _isActive = false;
    }

    public UnitStatistics GetStatistics()
    {
        return _unitStatistics;
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
    
    private void LoadUnitStatistics()
    {
        _unitStatistics = gameObject.GetComponent<UnitStatistics>();
    }

    private void LoadUnitRange()
    {
        _unitRange = gameObject.GetComponent<UnitRange>();
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

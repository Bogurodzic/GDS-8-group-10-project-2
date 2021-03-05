using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private bool _isActive = false;
    private Grid _grid;
    private GridManager _gridManager;
    private UnitMovement _unitMovement;
    private UnitStatistics _unitStatistics;
    private UnitRange _unitRange;
    private RangeType _activityType;
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
            HandleAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleActivatingAttackMode();
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
        else if (IsActive() && IsMovementActive() && !_unitMovement.IsInMovementRange(mouseX, mouseY))
        {
            DeactivateUnit();
        } else if (IsActive() && IsMovementActive() && _unitMovement.IsInMovementRange(mouseX, mouseY))
        {
            _unitMovement.Move(mouseX, mouseY, this);
            DeactivateUnit();
        }
    }

    private void HandleAttack()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        
        if (IsActive() && IsUnitTurn() && IsAttackActive() && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy());
            DeactivateUnit();
        }
        else if (IsActive() && IsUnitTurn() && IsAttackActive() && !_unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            DeactivateUnit();
        }
    }

    private bool IsCellOcuppiedByEnemy(int mouseX, int mouseY)
    {
        if (_grid.GetCell(mouseX, mouseY).GetOccupiedBy() != null && _grid.GetCell(mouseX, mouseY).GetOccupiedBy()._unitStatistics.team != _unitStatistics.team)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }

    private void HandleActivatingAttackMode()
    {
        if (IsActive() && IsUnitTurn())
        {
            _unitRange.ShowUnitRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition());
            _activityType = RangeType.Attack;
        }
    }
    

    public bool IsActive()
    {
        return _isActive;
    }

    public bool IsMovementActive()
    {
        if (_activityType == RangeType.Movement)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsAttackActive()
    {
        if (_activityType == RangeType.Attack)
        {
            return true;
        }
        else
        {
            return false;
        } 
    }

    public bool IsUnitTurn()
    {
        return Turn.IsUnitTurn(_unitStatistics.team);
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
        _activityType = RangeType.Movement;
        _unitMovement.ShowMovementRange();
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        _isActive = true;
    }

    public void DeactivateUnit()
    {
        _unitMovement.HideMovementRange();
        _isActive = false;
        Invoke("NextTurn", 0.05f);
    }

    private void NextTurn()
    {
        Turn.NextTurn();
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

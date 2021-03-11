using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Grid _grid;
    private GridManager _gridManager;
    private SpriteRenderer _sprite;
    private UnitMovement _unitMovement;
    private UnitStatistics _unitStatistics;
    private UnitRange _unitRange;
    private RangeType _activityType;
    private UnitPhase _unitPhase = UnitPhase.Inactive;
    private CombatLog _combatLog;
    void Start()
    {
        LoadSprite();
        LoadGrid();
        LoadGridManager();
        LoadCombatLog();
        LoadUnitMovement();
        LoadUnitStatistics();
        LoadUnitRange();
        PlaceUnitOnBoard();
        AddTeamColorToSprite();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAction();
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleActivatingAttackMode();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsUnitTurn())
            {
                SkipTurn();
            }
        }
    }

    private void PlaceUnitOnBoard()
    {
        int positionX, positionY;
        _grid.GetCellPosition(transform.position, out positionX, out positionY);

        _grid.GetCell(positionX, positionY).AddOccupiedBy(this);
    }

    private void AddTeamColorToSprite()
    {
        Color color;

        if (_unitStatistics.team == 1)
        {
            color = Color.blue;
        }
        else
        {
            color = Color.red;
        }

        _sprite.color = color;
    }

    private void HandleAction()
    {
        switch (_unitPhase)
        {
            case UnitPhase.Inactive:
                HandleTogglingUnit();
                break;
            case UnitPhase.Standby:
                HandleTogglingUnit();
                HandleDeactivatingUnit();
                HandleAttack();
                HandleMovement();
                break;
            case UnitPhase.AfterMovement:
                HandleAttack();
                break;
            case UnitPhase.AfterAttack:
                HandleDash();
                break;
            case UnitPhase.AfterDash:
                break;
            case UnitPhase.OnCooldown:
                break;
        }
  
    }

    private void EndAction(ActionType actionType)
    {
        switch (actionType)
        {         
            case ActionType.Activation:
                ActivateUnit();
                break;
            case ActionType.Deactivation:
                DeactivateUnit();
                break;
            case ActionType.Movement:
                SetUnitPhase(UnitPhase.AfterMovement);
                HandleActivatingAttackMode();
                break;
            case ActionType.Attack:
                ActivateDash();
                break;
            case ActionType.Ability:
                break;
            case ActionType.Dash:
                SkipTurn();
                break;
            case ActionType.SkipTurn:
                Invoke("NextTurn", 0.05f);
                break;
        }
    }
    
    private void HandleMovement()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        
        if (_unitPhase == UnitPhase.Standby && _unitMovement.IsInMovementRange(mouseX, mouseY))
        {
            _unitMovement.Move(mouseX, mouseY, this);
            EndAction(ActionType.Movement);
        }
    }
    
    private void HandleDash()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        
        if (_unitMovement.IsInMovementRange(mouseX, mouseY))
        {
            _unitMovement.Move(mouseX, mouseY, this);
            EndAction(ActionType.Dash);
        }
    }
    
    private void HandleAttack()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if ((_unitPhase == UnitPhase.Standby || _unitPhase == UnitPhase.AfterMovement) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            EndAction(ActionType.Attack);
            
        } else if ((_unitPhase == UnitPhase.Standby) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            EndAction(ActionType.Attack);
        }

    }
    
    public bool IsActive()
    {
        return _unitPhase != UnitPhase.Inactive && _unitPhase != UnitPhase.OnCooldown;
    }

    public bool IsUnitTurn()
    {
        return Turn.IsUnitTurn(_unitStatistics.team) && _unitPhase != UnitPhase.OnCooldown;
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
    
    private void ActivateUnit()
    {
        _activityType = RangeType.Movement;
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, _unitRange.minRange, _unitRange.maxRange);
        _unitMovement.ShowMovementRange(true);
        _unitRange.ShowUnitRange(false);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        SetUnitPhase(UnitPhase.Standby);
    }
    
    private void HandleTogglingUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if (IsUnitTurn() && IsUnitClicked(mouseX, mouseY))
        {
            EndAction(ActionType.Activation);
        }
        else if (IsActive() && IsUnitClicked(mouseX, mouseY))
        {
            EndAction(ActionType.Deactivation);
        }
    }

    private void HandleDeactivatingUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if (IsActive() && !_unitMovement.IsInMovementRange(mouseX, mouseY) && !_unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(),_unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            EndAction(ActionType.Deactivation);
        }
    }
    
    private void HandleActivatingAttackMode()
    {
        if (IsUnitTurn())
        {
            _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), 0, _unitRange.minRange, _unitRange.maxRange);
            _unitRange.ShowUnitRange(true);
        }
    }

    private void ActivateDash()
    {
        SetUnitPhase(UnitPhase.AfterAttack);
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, 0, 0);
        _unitMovement.ShowMovementRange(true);
    }

    public void DeactivateUnit()
    {
        SetUnitPhase(UnitPhase.Inactive);
        _grid.HideRange();
    }

    public void SkipTurn()
    {
        _grid.HideRange();
        SetUnitPhase(UnitPhase.OnCooldown);
        EndAction(ActionType.SkipTurn);
    }

    private void NextTurn()
    {
        Turn.NextTurn();
    }

    private void SetUnitPhase(UnitPhase unitPhase)
    {
        _unitPhase = unitPhase;
    }
    
    public UnitStatistics GetStatistics()
    {
        return _unitStatistics;
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
    
    private void LoadSprite()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    
    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }

    private void LoadCombatLog()
    {
        _combatLog = GameObject.Find("CombatLog").GetComponent<CombatLog>();
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
}

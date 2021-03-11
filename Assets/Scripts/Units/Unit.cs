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

    private void HandleTogglingUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if (IsUnitTurn() && IsUnitClicked(mouseX, mouseY))
        {
            ActivateUnit();
        }
        else if (IsActive() && IsUnitClicked(mouseX, mouseY))
        {
            DeactivateUnit();
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
            DeactivateUnit();
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
            ChangeUnitPhase(UnitPhase.AfterMovement);
            HandleActivatingAttackMode();
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
            SkipTurn();
        }
    }
    

    private void HandleAttack()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        //Debug.Log("HANDLE ATTACK");
        //Debug.Log((_unitPhase == UnitPhase.Standby || _unitPhase == UnitPhase.AfterMovement));
       // Debug.Log(IsCellOcuppiedByEnemy(mouseX, mouseY));
        //Debug.Log(_unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY));
        if ((_unitPhase == UnitPhase.Standby || _unitPhase == UnitPhase.AfterMovement) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            Debug.Log("Attack 1");
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            ActivateDash();
            
        } else if ((_unitPhase == UnitPhase.Standby) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            Debug.Log("Attack 2");
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            ActivateDash();
            
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
        if (IsUnitTurn())
        {
            _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), 0, _unitRange.minRange, _unitRange.maxRange);
            _unitRange.ShowUnitRange(true);
        }
    }
    

    public bool IsActive()
    {
        return _unitPhase != UnitPhase.Inactive && _unitPhase != UnitPhase.OnCooldown;
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
    
    private void ActivateUnit()
    {
        _activityType = RangeType.Movement;
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, _unitRange.minRange, _unitRange.maxRange);
        _unitMovement.ShowMovementRange(true);
        _unitRange.ShowUnitRange(false);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        _unitPhase = UnitPhase.Standby;
    }

    private void ActivateDash()
    {
        _unitPhase = UnitPhase.AfterAttack;
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, 0, 0);
        _unitMovement.ShowMovementRange(true);
        Debug.Log("ACTIVATING DASH");
    }

    public void DeactivateUnit()
    {
        _unitPhase = UnitPhase.Inactive;
        _grid.HideRange();
        //_unitMovement.HideMovementRange();
        //Invoke("NextTurn", 0.05f);
    }

    public void SkipTurn()
    {
        _grid.HideRange();
        _unitPhase = UnitPhase.OnCooldown;
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

    private void ChangeUnitPhase(UnitPhase unitPhase)
    {
        _unitPhase = unitPhase;
    }
    
}

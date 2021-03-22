using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;

    private Grid _grid;
    private GridManager _gridManager;
    private SpriteRenderer _sprite;
    private UnitMovement _unitMovement;
    private UnitStatistics _unitStatistics;
    private UnitRange _unitRange;
    private RangeType _activityType;
    private UnitPhase _unitPhase = UnitPhase.Inactive;
    private CombatLog _combatLog;
    private UnitAbility _unitAbility;
    private Healtbar _healtbar;

    private bool _isDeployed = false;
    private bool _isPreDeployed = false;
    private int _preDeployedX = -9999;
    private int _preDeployedY = -9999;
    private bool _isUnitHovered = false;

    
    void Start()
    {

    }

    void Update()
    {
        if (Turn.GetCurrentTurnType() == TurnType.RegularGame)
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
                if (IsUnitTurn() && IsActive())
                {
                    SkipTurn();
                }
            }

            HandleHoveringUnit();
        }
    }

    private void HandleHoveringUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        
        if (!IsActive() && mouseX == GetUnitXPosition() && mouseY == GetUnitYPosition() && !_isUnitHovered)
        {
            _isUnitHovered = true;
            _healtbar.SetSliderVisbility(true);
        } else if (!IsActive() && (mouseX != GetUnitXPosition() || mouseY != GetUnitYPosition()) && _isUnitHovered)
        {
            _isUnitHovered = false;
            _healtbar.SetSliderVisbility(false);
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
                SetHealth();
                HandleTogglingUnit();
                //HandleDeactivatingUnit();
                HandleAttack();
                HandleMovement();
                break;
            case UnitPhase.AbilityActivated:
                ExecuteAbility();
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
                if (_unitAbility.GetAbilityType() == AbilityType.Dash)
                {
                    ActivateDash();
                }
                else
                {
                    SkipTurn();
                }
                break;
            case ActionType.ActiveAbility:
                ActiveAbility();
                break;
            case ActionType.ExecuteAbility:
                if (_unitAbility.GetAbilityType() == AbilityType.Dash)
                {
                    ActivateDash();
                }
                else
                {
                    SkipTurn();
                }
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

        if (_unitPhase == UnitPhase.AfterMovement && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().SetHealth();
            EndAction(ActionType.Attack);
            
        } else if ((_unitPhase == UnitPhase.Standby) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            if (!_grid.IsPositionInAttackRange(mouseX, mouseY, _unitRange.minRange, _unitRange.maxRange))
            {            
                _unitMovement.MoveBeforeAttack(mouseX, mouseY, this);
            }
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().SetHealth();
            EndAction(ActionType.Attack);
        }

    }
    
    public bool IsActive()
    {
        return _unitPhase != UnitPhase.Inactive && _unitPhase != UnitPhase.OnCooldown;
    }

    public bool IsOnCD()
    {
        return _unitPhase == UnitPhase.OnCooldown;
    }

    public bool IsAbilityActive()
    {
        return IsActive() && unitData.unitAbility && _unitAbility.IsAbilityReadyToCast();
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
        if (_grid.IsClickInBoardRange(mouseX, mouseY) && _grid.GetCell(mouseX, mouseY).GetOccupiedBy() != null && _grid.GetCell(mouseX, mouseY).GetOccupiedBy()._unitStatistics.team != _unitStatistics.team)
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
        ReloadRanges();
        _unitRange.ShowUnitRange(true);
        _unitMovement.ShowMovementRange(false);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        _healtbar.SetSliderVisbility(true);
        SetUnitPhase(UnitPhase.Standby);
    }

    private void ReloadRanges()
    {
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, _unitRange.minRange, _unitRange.maxRange);
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

    public void ToggleAbility()
    {
        Debug.Log("Handle ability 3");
        EndAction(ActionType.ActiveAbility);
    }

    private void ActiveAbility()
    {
        SetUnitPhase(UnitPhase.AbilityActivated);
        Debug.Log("Handle ability 4");
        _unitAbility.ActiveAbility(GetUnitXPosition(), GetUnitYPosition());
    }

    private void ExecuteAbility()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if (_unitAbility.ExecuteAbility(mouseX, mouseY, GetUnitXPosition(), GetUnitYPosition()))
        {
            EndAction(ActionType.ExecuteAbility);
        }
    }



    public void DeactivateUnit()
    {
        _healtbar.SetSliderVisbility(false);
        SetUnitPhase(UnitPhase.Inactive);
        _grid.HideRange();
    }

    public void ResetUnitCD()
    {
        AddTeamColorToSprite();
        _unitAbility.RemoveOneTurnFromAbilityCD();
        SetUnitPhase(UnitPhase.Inactive);
    }

    public void SkipTurn()
    {
        _grid.HideRange();
        _sprite.color = Color.black;
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

    public UnitRange getUnitRange()
    {
        return _unitRange;
    }

    public UnitAbility GetUnitAbility()
    {
        return _unitAbility;
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

    private void LoadUnitAbility()
    {
        _unitAbility = gameObject.GetComponent<UnitAbility>();
    }

    private void LoadHealthbar()
    {
        _healtbar = gameObject.GetComponentInChildren<Healtbar>();
    }

    public void LoadUnitData(UnitData unitDataToLoad)
    {
        unitData = unitDataToLoad;
        LoadSprite();
        LoadGrid();
        LoadGridManager();
        LoadCombatLog();
        LoadUnitMovement();
        LoadUnitStatistics();
        LoadUnitRange();
        LoadUnitAbility();
        LoadHealthbar();
        //PlaceUnitOnBoard();
        AddTeamColorToSprite();
        ReloadUnitData();
        SetHealth();
    }

    public void SetHealth()
    {
        _healtbar.SetHealth(_unitStatistics.currentHp, _unitStatistics.maxHp);
    }

    public void ReloadUnitData()
    {
        _unitMovement.LoadUnitMovement(unitData);
        _unitStatistics.LoadUnitStatistics(unitData);
        _unitRange.LoadUnitRange(unitData);
        _unitAbility.LoadUnitAbility(unitData);
        _sprite.sprite = unitData.unitSprite;
    }

    public bool IsUnitDeployed()
    {
        return _isDeployed;
    }

    public bool IsUnitPreDeployed()
    {
        return _isPreDeployed;
    }

    public void HandleDeployment(int x, int y)
    {
        if (!IsUnitPreDeployed())
        {
            PreDeploy(x, y);
        } else if (!IsUnitDeployed() && x == _preDeployedX && y == _preDeployedY)
        {
            Deploy(x, y);
        }
        else
        {
            PreDeploy(x, y);
        }
    }

    public void Deploy(int x, int y)
    {
        _isDeployed = true;
        PlaceUnitOnBoard();
    }

    public void PreDeploy(int x, int y)
    {
        _isPreDeployed = true;
        Vector3 cellCenterPosition = _grid.GetCellCenter(x, y);
        gameObject.transform.position = cellCenterPosition;
        _preDeployedX = x;
        _preDeployedY = y;
    }
}

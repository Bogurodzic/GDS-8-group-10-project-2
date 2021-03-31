﻿using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Spine.Unity;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;

    private Grid _grid;
    private GridManager _gridManager;
    private SpriteRenderer _sprite;
    private SkeletonAnimation _skeletonAnimation;
    private UnitMovement _unitMovement;
    private UnitStatistics _unitStatistics;
    private UnitRange _unitRange;
    private RangeType _activityType;
    private UnitPhase _unitPhase = UnitPhase.Inactive;
    private CombatLog _combatLog;
    private UnitAbility _unitAbility;
    private Healtbar _healtbar;
    private UnitList _unitList;
    private UnitListPanel _unitListPanel;
    private bool _isDeployed = false;
    private bool _isPreDeployed = false;
    private int _preDeployedX = -9999;
    private int _preDeployedY = -9999;
    private bool _isUnitHovered = false;
    private UnitPhase _phaseBeforeAbility;
    private bool _isAlive = true;
    private bool _rangesOnHoverVisible = true;

    private int _attackAfterMovementXTarget = -9999;
    private int _attackAfterMovementYTarget = -9999;
    
    void Update()
    {
        if (Turn.GetCurrentTurnType() == TurnType.RegularGame)
        {
            HandleHoveringUnit();
            
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

            
        }
    }

    private void HandleHoveringUnit()
    {
        
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;

        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        bool isCellOccupied = (mouseX < _grid.GetGridWidth() && mouseY < _grid.GetGridHeight() && mouseX >= 0 && mouseY >= 0) && _grid.GetCell(mouseX, mouseY).GetPathNode().isOccupied;
        
        Debug.Log("HANDLE HOVERING 1:" + mouseX + "-" + mouseY);
        if (!IsActive() && mouseX == GetUnitXPosition() && mouseY == GetUnitYPosition() && !_isUnitHovered)
        {
            Debug.Log("HANDLE HOVERING 2:" + mouseX + "-" + mouseY);

            _isUnitHovered = true;
            _healtbar.SetSliderVisbility(true);

            /*if (!Turn.IsUnitTurn(GetStatistics().team) && Turn.IsFirstUnitInTurnSelected())
            {
                Debug.Log("HANDLE HOVERING 3:" + mouseX + "-" + mouseY);

                ShowRangesOnHover();
            } */
            
            if (!Turn.IsFirstUnitInTurnSelected())
            {
                Debug.Log("HANDLE HOVERING 4:" + mouseX + "-" + mouseY);

                ShowRangesOnHover();
                _unitListPanel.OnHoverUnit(this);
            }

            if (Turn.IsFirstUnitInTurnSelected() && !Turn.IsUnitTurn(GetStatistics().team))
            {
                _unitListPanel.OnHoverUnit(this);
            }
        } else if (!IsActive() && (mouseX != GetUnitXPosition() || mouseY != GetUnitYPosition()) && _isUnitHovered)
        {
            Debug.Log("HANDLE HOVERING 5:" + mouseX + "-" + mouseY);

            _isUnitHovered = false;
            _healtbar.SetSliderVisbility(false);

            /*if (Turn.IsFirstUnitInTurnSelected() && !isCellOccupied)
            {
                Debug.Log("HANDLE HOVERING 6:" + mouseX + "-" + mouseY);

                _unitList.GetActiveUnit().GetComponent<Unit>().ShowActiveUnitsRanges();
            }*/
            if (!Turn.IsFirstUnitInTurnSelected() &&  !isCellOccupied)
            {
                Debug.Log("HANDLE HOVERING 7:" + mouseX + "-" + mouseY);

                _grid.HideRange();
                _unitListPanel.OnHoverOutUnit();
            }

            if (!isCellOccupied && !Turn.IsUnitTurn(GetStatistics().team))
            {
                _unitListPanel.OnHoverOutUnit();
            }
            
        }

        
        
        
    }

    public void HoverFromFrame()
    {
        if (!Turn.IsFirstUnitInTurnSelected())
        {
            ShowRangesOnHover();
            _unitListPanel.ShowUnitInfo(this);
        }

        if (Turn.IsFirstUnitInTurnSelected() && !Turn.IsUnitTurn(GetStatistics().team))
        {
            _unitListPanel.ShowUnitInfo(this);
        }
    }

    public void HoverOutFromFrame()
    {
        if (!Turn.IsFirstUnitInTurnSelected())
        {
            _grid.HideRange();
            _unitListPanel.HideUnitInfo();
        }
        else if (Turn.IsFirstUnitInTurnSelected() && !Turn.IsUnitTurn(GetStatistics().team) )
        {
            _unitListPanel.HideUnitInfo();
        }
    }

    private void PlaceUnitOnBoard()
    {
        int positionX, positionY;
        _grid.GetCellPosition(transform.position, out positionX, out positionY);

        _grid.GetCell(positionX, positionY).AddOccupiedBy(this);
    }

    private void ReloadSprite()
    {
        _skeletonAnimation.skeletonDataAsset = unitData.skeletonDataAsset;
        
        if (_unitStatistics.team == 1)
        {
            switch (unitData.unitName)
            {
                case "Archer":
                    _skeletonAnimation.initialSkinName = "ARCHER BLUE";
                    break;
                case "Boss":
                    _skeletonAnimation.initialSkinName = "BOSS BLUE";
                    break;
                case "Infantry":
                    _skeletonAnimation.initialSkinName = "SWORDMAN BLUE";
                    break;
                case "Mage":
                    _skeletonAnimation.initialSkinName = "WIZARD BLUE";
                    break;
                case "Medic":
                    _skeletonAnimation.initialSkinName = "MEDIC BLUE";
                    break;
                case "Rogue":
                    _skeletonAnimation.initialSkinName = "ROGUE BLUE";
                    break;
                case "Spearman":
                    _skeletonAnimation.initialSkinName = "SPEARMAN BLUE";
                    break;
            }
            
        }
        else
        {
            _skeletonAnimation.initialFlipX = true;
            switch (unitData.unitName)
            {
                case "Archer":
                    _skeletonAnimation.initialSkinName = "ARCHER RED";
                    break;
                case "Boss":
                    _skeletonAnimation.initialSkinName = "BOSS RED";
                    break;
                case "Infantry":
                    _skeletonAnimation.initialSkinName = "SWORDMAN RED";
                    break;
                case "Mage":
                    _skeletonAnimation.initialSkinName = "WIZARD RED";
                    break;
                case "Medic":
                    _skeletonAnimation.initialSkinName = "MEDIC RED";
                    break;
                case "Rogue":
                    _skeletonAnimation.initialSkinName = "ROGUE RED";
                    break;
                case "Spearman":
                    _skeletonAnimation.initialSkinName = "SPEARMAN RED";
                    break;
            }
        }
        _skeletonAnimation.Initialize(true); 
        _skeletonAnimation.Skeleton.SetSkin(_skeletonAnimation.initialSkinName); // set the skin
        _skeletonAnimation.Skeleton.SetSlotsToSetupPose(); // use the active attachments from setup pose.
        _skeletonAnimation.AnimationState.Apply( _skeletonAnimation.Skeleton); // use the active attachments from the active animations.
        
        _skeletonAnimation.AnimationState.SetAnimation(0, "IDLE", true);


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
                HandleDeactivatingUnit();
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

    public void EndAction(ActionType actionType)
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
                AnimateIdle();
                HandleActivatingAttackMode();
                break;
            case ActionType.Attack:
                _unitList.CheckWinCondition();
                if (_unitAbility.GetAbilityType() == AbilityType.Dash)
                {
                    ActivateDash();
                }
                else
                {
                    SkipTurn();
                }
                break;
            case ActionType.MovementBeforeAttack:
                HandleAttackUnit(_attackAfterMovementXTarget, _attackAfterMovementYTarget);
                break;
            case ActionType.ActiveAbility:
                ActiveAbility();
                break;
            case ActionType.ExecuteAbility:
                _unitList.CheckWinCondition();
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
                AnimateIdle();
                SkipTurn();
                break;
            case ActionType.SkipTurn:
                Invoke("NextTurn", 0.4f);
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
            Turn.BlockTurn();
            AnimateLoopUnit("WALK");
            _unitMovement.Move(mouseX, mouseY, this, ActionType.Movement);
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
            AnimateLoopUnit("WALK");
            _unitMovement.Move(mouseX, mouseY, this, ActionType.Dash);
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
            
            AnimateUnit("ATTACK");
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().AnimateUnit("HURT");
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().SetHealth();
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().HandleDeath();
            Turn.BlockTurn();
            EndAction(ActionType.Attack);
        } else if ((_unitPhase == UnitPhase.Standby) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            if (!_grid.IsPositionInAttackRange(mouseX, mouseY, _unitRange.minRange, _unitRange.maxRange))
            {
                AnimateLoopUnit("WALK");
                _attackAfterMovementXTarget = mouseX;
                _attackAfterMovementYTarget = mouseY;
                _unitMovement.MoveBeforeAttack(mouseX, mouseY, this, ActionType.MovementBeforeAttack);
                Turn.BlockTurn();
            }
            else
            {
                Turn.BlockTurn();
                HandleAttackUnit(mouseX, mouseY);
            }
        }
    }

    private void HandleAttackUnit(int mouseX, int mouseY)
    {
        AnimateUnit("ATTACK");
        _grid.GetCell(mouseX, mouseY).GetOccupiedBy().AnimateUnit("HURT");
        _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
        _grid.GetCell(mouseX, mouseY).GetOccupiedBy().SetHealth();
        _grid.GetCell(mouseX, mouseY).GetOccupiedBy().HandleDeath();
        EndAction(ActionType.Attack);  
    }
    
    public bool IsActive()
    {
        return _unitPhase != UnitPhase.Inactive && _unitPhase != UnitPhase.OnCooldown;
    }

    public bool IsOnCD()
    {
        return _unitPhase == UnitPhase.OnCooldown;
    }

    public bool IsStandby()
    {
        return _unitPhase == UnitPhase.Standby;
    }

    public bool IsAbilityActive()
    {
        return IsActive() && unitData.unitAbility && _unitAbility.IsAbilityReadyToCast();
    }
    

    public bool IsUnitTurn(bool extended = false)
    {
        Unit currentActiveUnit = _unitList.FindActiveUnit(GetStatistics().team).GetComponent<Unit>();
        bool isTurn = Turn.IsUnitTurn(_unitStatistics.team) && _unitPhase != UnitPhase.OnCooldown;
        
        if (currentActiveUnit.unitData.name != unitData.name && Turn.IsTurnBlocked())
        {
            isTurn = false;
        }
        
        
        return isTurn;
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
        _unitList.DeactivateAllPlayerUnits(GetStatistics().team);
        _unitListPanel.ActivateUnitPortrait(this);
        _activityType = RangeType.Movement;
        Turn.SetIsFirstUnitInTurnSelected(true);
        ReloadRanges();
        _unitRange.ShowUnitRange(true);
        _unitMovement.ShowMovementRange(false);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
        _healtbar.SetSliderVisbility(true);
        SetUnitPhase(UnitPhase.Standby);
    }

    private void ReloadRanges()
    {
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, _unitRange.minRange, _unitRange.maxRange, GetStatistics().team);
    }

    private void ShowRangesOnHover()
    {
        _rangesOnHoverVisible = true;
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, _unitRange.minRange, _unitRange.maxRange, GetStatistics().team);
        _unitRange.ShowUnitRange(true);
        _unitMovement.ShowMovementRange(false);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
    }

    private void ShowActiveUnitsRanges()
    {
        ReloadRanges();
        _unitRange.ShowUnitRange(true);
        _unitMovement.ShowMovementRange(false);
        _gridManager.ChangeColor(GetUnitXPosition(), GetUnitYPosition(), Color.magenta);
    }
    
    private void HandleTogglingUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        if (IsAlive() && IsUnitTurn(true) && !IsActive() && IsUnitClicked(mouseX, mouseY))
        {       
            EndAction(ActionType.Activation);
        }
        else if (IsAlive() && IsUnitTurn() && IsActive() && IsUnitClicked(mouseX, mouseY))
        {
            EndAction(ActionType.Deactivation);
        }
    }

    public bool TryDeactivate()
    {
        if (IsAlive() && IsUnitTurn() && IsActive() && IsStandby())
        {
            EndAction(ActionType.Deactivation);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HandleTogglingFromFrame()
    {
        if (IsUnitTurn() && !IsActive())
        {
            EndAction(ActionType.Activation);
            return true;
        } 
        else if (IsUnitTurn() && IsActive())
        {
            EndAction(ActionType.Deactivation);
            return false;
        }

        return false;
    }

    private void HandleDeactivatingUnit()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);
        if (IsActive() && !_unitMovement.IsInMovementRange(mouseX, mouseY) && !_unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(),_unitMovement.GetUnitYPosition(),mouseX, mouseY) &&
            (mouseX < _grid.GetGridWidth() && mouseY < _grid.GetGridHeight() && mouseX >= 0 && mouseY >= 0))
        {
            EndAction(ActionType.Deactivation);
        }
    }
    
    private void HandleActivatingAttackMode()
    {
        if (IsUnitTurn())
        {
            _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), 0, _unitRange.minRange, _unitRange.maxRange, GetStatistics().team);
            _unitRange.ShowUnitRange(true);
        }
    }

    private void ActivateDash()
    {
        SetUnitPhase(UnitPhase.AfterAttack);
        _grid.CalculateCostToAllTiles(GetUnitXPosition(), GetUnitYPosition(), _unitMovement.movementRange, 0, 0, GetStatistics().team);
        _unitMovement.ShowMovementRange(true);
    }

    public void ToggleAbility()
    {
        _phaseBeforeAbility = _unitPhase;
        EndAction(ActionType.ActiveAbility);
    }

    public void DeactivateAbility()
    {
        if (_phaseBeforeAbility == UnitPhase.Standby)
        {
            EndAction(ActionType.Activation);
        } else if (_phaseBeforeAbility == UnitPhase.AfterMovement)
        {
            EndAction(ActionType.Movement);
        }
    }

    private void ActiveAbility()
    {
        Turn.BlockTurn();
        SetUnitPhase(UnitPhase.AbilityActivated);
        _unitAbility.ActiveAbility(GetUnitXPosition(), GetUnitYPosition());
    }

    private void ExecuteAbility()
    {
        Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
        mouseVector3.z = 0;
        int mouseX, mouseY;
        _grid.GetCellPosition(mouseVector3, out mouseX, out mouseY);

        if (mouseX < _grid.GetGridWidth() && mouseY < _grid.GetGridHeight() && mouseX >= 0 && mouseY >= 0 && _unitAbility.ExecuteAbility(mouseX, mouseY, GetUnitXPosition(), GetUnitYPosition()))
        {
            AnimateUnit("SKILL");
            EndAction(ActionType.ExecuteAbility);
        }
    }



    public void DeactivateUnit()
    {
        if (IsUnitTurn())
        {
            Turn.SetIsFirstUnitInTurnSelected(false);
            _unitListPanel.DeactivateUnitPortrait(this);
            _healtbar.SetSliderVisbility(false);
            SetUnitPhase(UnitPhase.Inactive);
            _grid.HideRange(); 
        }
    }

    public void ResetUnitCD()
    {
        _skeletonAnimation.skeleton.A = 1f;
        _unitAbility.RemoveOneTurnFromAbilityCD();
        SetUnitPhase(UnitPhase.Inactive);
    }

    public void SkipTurn()
    {
        Turn.SetIsFirstUnitInTurnSelected(false);
        _grid.HideRange();
        _skeletonAnimation.skeleton.A = 0.5f;
        SetUnitPhase(UnitPhase.OnCooldown);
        EndAction(ActionType.SkipTurn);
    }

    public void HandleDeath()
    {
        
        if (!CheckIfUnitIsAlive())
        {
            AnimateOnce("DEATH");
            Invoke("Death", 1f);
        }
    }

    private void Death()
    {
        _unitMovement.RemoveUnitFromCurrentCell();
        _healtbar.TurnOffHealthBar();
        _isAlive = false;
    }

    private bool CheckIfUnitIsAlive()
    {
        return _unitStatistics.currentHp > 0;
    }

    private void NextTurn()
    {
        _grid.HideRange();
        Turn.NextTurn();
    }

    public bool IsAlive()
    {
        return _isAlive;
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

    public SkeletonAnimation GetSkeletonAnimation()
    {
        return _skeletonAnimation;
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
        _skeletonAnimation = gameObject.GetComponent<SkeletonAnimation>();
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
        LoadUnitList();
        LoadUnitListPanel();
        ReloadUnitData();
        SetHealth();
    }

    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }

    private void LoadUnitListPanel()
    {
        if (GetStatistics().team == 1)
        {
            _unitListPanel = GameObject.Find("UnitListPanelLeft").GetComponent<UnitListPanel>();
        } else if (GetStatistics().team == 2)
        {
            _unitListPanel = GameObject.Find("UnitListPanelRight").GetComponent<UnitListPanel>();
        }
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
        ReloadSprite();
    }

    public bool IsUnitDeployed()
    {
        return _isDeployed;
    }

    public bool IsUnitPreDeployed()
    {
        return _isPreDeployed;
    }

    public bool IsAbilityActivated()
    {
        return _unitPhase == UnitPhase.AbilityActivated;
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

    public void AnimateOnce(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
    }

    public void AnimateUnit(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
        _skeletonAnimation.AnimationState.AddAnimation(0, "IDLE", true, 0f);
    }

    public void AnimateLoopUnit(string animationName)
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
    }

    public void AnimateIdle()
    {
        _skeletonAnimation.AnimationState.SetAnimation(0, "IDLE", true);
    }
}

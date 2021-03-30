using System;
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
    private bool _isDeployed = false;
    private bool _isPreDeployed = false;
    private int _preDeployedX = -9999;
    private int _preDeployedY = -9999;
    private bool _isUnitHovered = false;
    private UnitPhase _phaseBeforeAbility;
    private bool _isAlive = true;

    private int _attackAfterMovementXTarget = -9999;
    private int _attackAfterMovementYTarget = -9999;

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
        Debug.Log("GRID 1");
        Debug.Log(_unitStatistics.team);
        Debug.Log(unitData.name);
        Debug.Log(_grid);
        Debug.Log("GRID 2");

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

        //_skeletonAnimation.loop = true;
        //_skeletonAnimation.AnimationName = "IDLE";
        //SpineEditorUtilities.ReloadSkeletonDataAssetAndComponent(_skeletonAnimation);
        
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
                Debug.Log("END ACTIVATION 1");
                Debug.Log("END ACTIVATION 2");
                Debug.Log("END ACTIVATION 3");
                Debug.Log("END ACTIVATION 4");
                Debug.Log("END ACTIVATION 5");
                ActivateUnit();
                break;
            case ActionType.Deactivation:
                Debug.Log("END Deactivation 1");
                Debug.Log("END Deactivation 2");
                Debug.Log("END Deactivation 3");
                Debug.Log("END Deactivation 4");
                Debug.Log("END Deactivation 5");
                DeactivateUnit();
                break;
            case ActionType.Movement:
                Debug.Log("END ACTION MOVEMENT 1");
                Debug.Log("END ACTION MOVEMENT 2");

                Debug.Log("END ACTION MOVEMENT 3");
                Debug.Log("END ACTION MOVEMENT 4");
                Debug.Log("END ACTION MOVEMENT 5");

                SetUnitPhase(UnitPhase.AfterMovement);
                AnimateIdle();
                HandleActivatingAttackMode();
                break;
            case ActionType.Attack:
                _unitList.CheckWinCondition();
                Debug.Log("END ACTION ATTACK 1");
                Debug.Log("END ACTION ATTACK 2");

                Debug.Log("END ACTION ATTACK 3");
                Debug.Log("END ACTION ATTACK 4");
                Debug.Log("END ACTION ATTACK 5");
                if (_unitAbility.GetAbilityType() == AbilityType.Dash)
                {
                    Debug.Log("END ACTION ATTACK 11");
                    Debug.Log("END ACTION ATTACK 22");

                    Debug.Log("END ACTION ATTACK 33");
                    Debug.Log("END ACTION ATTACK 44");
                    Debug.Log("END ACTION ATTACK 55");
                    ActivateDash();
                }
                else
                {
                    Debug.Log("END ACTION ATTACK 111");
                    Debug.Log("END ACTION ATTACK 222");

                    Debug.Log("END ACTION ATTACK 333");
                    Debug.Log("END ACTION ATTACK 444");
                    Debug.Log("END ACTION ATTACK 555");
                    SkipTurn();
                }
                break;
            case ActionType.MovementBeforeAttack:
                HandleAttackUnit(_attackAfterMovementXTarget, _attackAfterMovementYTarget);
                break;
            case ActionType.ActiveAbility:
                Debug.Log("END ACTION ACTIVATE ABILITY 1");
                Debug.Log("END ACTION ACTIVATE ABILITY 2");
                Debug.Log("END ACTION ACTIVATE ABILITY 3");
                Debug.Log("END ACTION ACTIVATE ABILITY 4");
                Debug.Log("END ACTION ACTIVATE ABILITY 5");

                ActiveAbility();
                break;
            case ActionType.ExecuteAbility:
                _unitList.CheckWinCondition();
                if (_unitAbility.GetAbilityType() == AbilityType.Dash)
                {
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 1");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 2");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 3");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 4");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 5");
                    ActivateDash();
                }
                else
                {
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 11");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 22");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 33");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 44");
                    Debug.Log("END EXECUTE ACTIVATE ABILITY 55");
                    SkipTurn();
                }
                break;
            case ActionType.Dash:
                Debug.Log("END DASH 1");
                Debug.Log("END DASH 2");
                Debug.Log("END DASH 3");
                Debug.Log("END DASH 4");
                Debug.Log("END DASH 5");
                AnimateIdle();
                SkipTurn();
                break;
            case ActionType.SkipTurn:
                Debug.Log("END SKIP 1");
                Debug.Log("END SKIP 2");
                Debug.Log("END SKIP 3");
                Debug.Log("END SKIP 4");
                Debug.Log("END SKIP 5");
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
            AnimateLoopUnit("WALK");
            _unitMovement.Move(mouseX, mouseY, this, ActionType.Movement);
            //EndAction(ActionType.Movement);
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
            Debug.Log("HANDLE ATTACK 1");
            Debug.Log("HANDLE ATTACK 2");
            Debug.Log("HANDLE ATTACK 3");
            Debug.Log("HANDLE ATTACK 4");
            Debug.Log("HANDLE ATTACK 5");

            AnimateUnit("ATTACK");
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().AnimateUnit("HURT");
            _combatLog.LogCombat(Attack.AttackUnit(this, _grid.GetCell(mouseX, mouseY).GetOccupiedBy()));
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().SetHealth();
            _grid.GetCell(mouseX, mouseY).GetOccupiedBy().HandleDeath();
            EndAction(ActionType.Attack);
            
        } else if ((_unitPhase == UnitPhase.Standby) && IsCellOcuppiedByEnemy(mouseX, mouseY) && _unitRange.IsInAttackRange(_unitMovement.GetUnitXPosition(), _unitMovement.GetUnitYPosition(),mouseX, mouseY))
        {
            if (!_grid.IsPositionInAttackRange(mouseX, mouseY, _unitRange.minRange, _unitRange.maxRange))
            {            
                Debug.Log("HANDLE ATTACK 11");
                Debug.Log("HANDLE ATTACK 22");
                Debug.Log("HANDLE ATTACK 33");
                Debug.Log("HANDLE ATTACK 44");
                Debug.Log("HANDLE ATTACK 55");
                AnimateLoopUnit("WALK");
                _attackAfterMovementXTarget = mouseX;
                _attackAfterMovementYTarget = mouseY;
                _unitMovement.MoveBeforeAttack(mouseX, mouseY, this, ActionType.MovementBeforeAttack);
            }
            else
            {
                Debug.Log("HANDLE ATTACK 111");
                Debug.Log("HANDLE ATTACK 222");
                Debug.Log("HANDLE ATTACK 333");
                Debug.Log("HANDLE ATTACK 444");
                Debug.Log("HANDLE ATTACK 555");
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

        if (currentActiveUnit.unitData.name != unitData.name &&
            (currentActiveUnit.IsActive() && !currentActiveUnit.IsStandby()))
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
        
        Debug.Log("Handle Toggling Unit 1");
        Debug.Log(IsUnitTurn());
        Debug.Log(IsUnitClicked(mouseX, mouseY));

        if (IsAlive() && IsUnitTurn(true) && !IsActive() && IsUnitClicked(mouseX, mouseY))
        {       
            Debug.Log("Handle Toggling Unit 2");
            EndAction(ActionType.Activation);
        }
        else if (IsAlive() && IsUnitTurn() && IsActive() && IsUnitClicked(mouseX, mouseY))
        {
            Debug.Log("Handle Toggling Unit 3");

            EndAction(ActionType.Deactivation);
        }
    }

    public bool TryDeactivate()
    {
        if (IsAlive() && IsUnitTurn() && IsActive() && IsStandby())
        {
            Debug.Log("Handle Toggling Unit 3");

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

        Debug.Log("HandleDeactivatingUnit");
        Debug.Log(mouseX);
        Debug.Log(mouseY);
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
        Debug.Log("NEXT TURN 1");
        Debug.Log("NEXT TURN 2");
        Debug.Log("NEXT TURN 3");
        Debug.Log("NEXT TURN 4");
        Debug.Log("NEXT TURN 5");

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
        //PlaceUnitOnBoard();
        ReloadUnitData();
        SetHealth();
    }

    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
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

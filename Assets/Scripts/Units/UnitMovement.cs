using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int movementRange;
    public int movementSpeed = 1000;
    private Grid _grid;
    private int _xPosition;
    private int _yPosition;
    private Unit _unit;
    private LinkedList<PathNode> _unitPath;
    private PathNode _nextPathNode;
    private Vector3 _nextUnitPosition;
    private bool _animateMovement = false;
    private ActionType _actionToExecuteAfterMovement;
    void Start()
    {
        LoadGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }

        UpdateUnitPosition();
        HandleAnimatingMovement();

    }

    public void LoadUnitMovement(UnitData unitData)
    {
        movementRange = unitData.movementRange;
    }

    private void LoadGrid()
    {
        _grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
    }
    
    private void UpdateUnitPosition()
    {
        _grid.GetCellPosition(transform.position, out _xPosition, out _yPosition);
    }

    public int GetUnitXPosition()
    {
        return _xPosition;
    }
    
    public int GetUnitYPosition()
    {
        return _yPosition;
    }

    public void ShowMovementRange(bool hidePreviouseRange = true)
    {
        if (hidePreviouseRange)
        {
            _grid.HideRange();
        }

        _grid.ShowRange(RangeType.Movement);
    }
    
    public void HideMovementRange()
    {
        _grid.HideRange();
    }

    public bool IsInMovementRange(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _grid.GetGridWidth() && y < _grid.GetGridHeight() && _grid.GetCell(x, y).GetPathNode().isMovable &&  !_grid.GetCell(x, y).GetPathNode().isOccupied)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(int x, int y, Unit unit, ActionType actionType)
    {
        
        _grid.HideRange();
        _unit = unit;
        _actionToExecuteAfterMovement = actionType;
        RemoveUnitFromCurrentCell();
        HandleMovementTo(x, y, unit);
        
        

        /*RemoveUnitFromCurrentCell();
        Vector3 cellPositionCenter = _grid.GetCellCenter(x, y);
        cellPositionCenter.z = -1;
        transform.position = cellPositionCenter;
        AddUnitToCurrentCell(unit);*/

    }

    private void HandleMovementTo(int x, int y, Unit unit)
    {
        LinkedList<PathNode> unitPath = new LinkedList<PathNode>();
        _nextPathNode = _grid.GetCell(x, y).GetPathNode();
        unitPath.AddFirst(_nextPathNode);
        while (_nextPathNode.cameFromNode != null)
        {
            _nextPathNode = _nextPathNode.cameFromNode;
            unitPath.AddFirst(_nextPathNode);
        }

        _unitPath = unitPath;
        SetNewNextUnitPosition();
    }

    private void HandleAnimatingMovement()
    {
        if (_animateMovement)
        {
            float step = 20f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _nextUnitPosition, step);
        }

        if (transform.position == _nextUnitPosition && _animateMovement)
        {
            _animateMovement = false;
            SetNewNextUnitPosition();
        }
    }

    private void SetNewNextUnitPosition()
    {
        if (_unitPath.Count > 0)
        {
            _nextPathNode = _unitPath.First.Value;
            Vector3 cellPositionCenter = _grid.GetCellCenter(_nextPathNode.x, _nextPathNode.y);
            _nextUnitPosition = cellPositionCenter;
            _animateMovement = true;
            _unitPath.RemoveFirst();
        }
        else
        {
            AddUnitToCurrentCell(_unit);
            
            gameObject.GetComponent<Unit>().EndAction(_actionToExecuteAfterMovement);
            
            _unit = null;
        }
    }

    public void MoveBeforeAttack(int x, int y, Unit unit, ActionType actionType)
    {
        PathNode targetNode = _grid.GetCell(x, y).GetPathNode();
        PathNode lastMovableNode = targetNode.lastMovableNode;
        Debug.Log("MoveBeforeAttackE: " + x + " " + y);

        Debug.Log("LAST MOVABLE NODE: " + lastMovableNode.x + " " + lastMovableNode.y);
        Move(lastMovableNode.x, lastMovableNode.y, unit, actionType);
    }

    private PathNode GetOptimalDistanceNode(PathNode targetNode, Unit unit)
    {
        
        PathNode optimalDistanceNode = targetNode.lastMovableNode;
        return optimalDistanceNode;
    }

    public void RemoveUnitFromCurrentCell()
    {
        UpdateUnitPosition();
        _grid.GetCell(GetUnitXPosition(), GetUnitYPosition()).RemoveOccupiedBy();
    }

    private void AddUnitToCurrentCell(Unit unit)
    {
        UpdateUnitPosition();
        _grid.GetCell(GetUnitXPosition(), GetUnitYPosition()).AddOccupiedBy(unit);
    }
    
}

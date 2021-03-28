using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int movementRange;
    public int movementSpeed = 1000;
    private Grid _grid;
    private int _xPosition;
    private int _yPosition;

    private LinkedList<PathNode> _unitPath;
    private PathNode _nextPathNode;
    private Vector3 _nextUnitPosition;
    private bool _animateMovement = false;
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

    public void Move(int x, int y, Unit unit)
    {
        //RemoveUnitFromCurrentCell();
        _grid.HideRange();
        //PathNode cameFromNode = _grid.GetCell(x, y).GetPathNode().cameFromNode;
        //while (cameFromNode != null)
        //{
        //    Debug.Log(cameFromNode.x + ":" + cameFromNode.y);
        //    _grid.HighliteCell(cameFromNode.x, cameFromNode.y);
        //    cameFromNode = cameFromNode.cameFromNode;
        //}
        //cellPositionCenter.z = -1;
        //transform.position = cellPositionCenter;
        //AddUnitToCurrentCell(unit);
        HandleMovmentTo(x, y, unit);
    }

    private void HandleMovmentTo(int x, int y, Unit unit)
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
        //Vector3 cellPositionCenter = _grid.GetCellCenter(x, y);
        //_nextUnitPosition = cellPositionCenter;
        //_animateMovement = true;
    }

    private void HandleAnimatingMovement()
    {
        if (_animateMovement)
        {
            float step = 10f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _nextUnitPosition, step);
        }

        if (transform.position == _nextUnitPosition)
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
    }

    public void MoveBeforeAttack(int x, int y, Unit unit)
    {
        PathNode targetNode = _grid.GetCell(x, y).GetPathNode();
        PathNode lastMovableNode = GetOptimalDistanceNode(targetNode, unit) ;
        
        Move(lastMovableNode.x, lastMovableNode.y, unit);
    }

    private PathNode GetOptimalDistanceNode(PathNode targetNode, Unit unit)
    {
        
        PathNode optimalDistanceNode = targetNode.lastMovableNode;
       /* int optimalDistance = targetNode.hCost - optimalDistanceNode.hCost;

        while (optimalDistance < unit.getUnitRange().maxRange)
        {
            if (targetNode.hCost - optimalDistanceNode.cameFromNode.hCost > unit.getUnitRange().maxRange)
            {
                break;
            }
            
            optimalDistanceNode = optimalDistanceNode.cameFromNode;
            optimalDistance = targetNode.hCost - optimalDistanceNode.hCost;
        } */

        return optimalDistanceNode;
    }

    private void RemoveUnitFromCurrentCell()
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

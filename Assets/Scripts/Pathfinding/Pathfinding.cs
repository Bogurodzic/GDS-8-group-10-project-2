﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using Enums;
using UnityEngine;

public class Pathfinding
{

    private const int MOVE_STRAIGHT_COST = 1;

    private Grid _grid;
    private List<PathNode> _openList;
    private List<PathNode> _closedList;
    
    public Pathfinding(Grid grid)
    {
        this._grid = grid;
    }
    
    public void CalculateCostToAllTiles(int startX, int startY, int movementRange, int minAttackRange,
        int maxAttackRange, int team)
    {
        PathNode startNode = _grid.GetCell(startX, startY).GetPathNode();
        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        ResetPathNodes();
        
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, startNode);
        startNode.CalculateFCost(); 
        
        IterateOverOpenListWithoutEndNode(startNode, movementRange, minAttackRange, maxAttackRange, team);
    }

    private void ResetPathNodes()
    {
        for (int x = 0; x < _grid.GetGridWidth(); x++)
        {
            for (int y = 0; y < _grid.GetGridHeight(); y++)
            {
                PathNode pathNode = _grid.GetCell(x, y).GetPathNode();
                pathNode.gCost = 9999;
                pathNode.hCost = 9999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
                pathNode.isAttackable = false;
                pathNode.isMovable = false;
            }
        }
    }
    
    private void IterateOverOpenListWithoutEndNode(PathNode startNode, int movementRange, int minAttackRange,
        int maxAttackRange, int team)
    {
        while (_openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(_openList);

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);


            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (_closedList.Contains(neighbourNode)) continue;
                if (neighbourNode.isObstacle)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                } else if ((neighbourNode.isOccupied && _grid.GetCell(neighbourNode.x, neighbourNode.y).GetOccupiedBy().GetStatistics().team != team))
                {
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, startNode);
                    int gCost = currentNode.gCost + (int) neighbourNode.movementCost;

                    if (movementRange >= gCost && minAttackRange <= neighbourNode.hCost)
                    {
                        neighbourNode.isAttackable = true;
                        neighbourNode.lastMovableNode = currentNode;
                    }
                    _closedList.Add(neighbourNode);
                    continue;
                }
                
                int tentativeGCost = currentNode.gCost + (int) neighbourNode.movementCost;
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, startNode);
                    neighbourNode.CalculateFCost();

                    if (movementRange >= neighbourNode.gCost)
                    {
                        neighbourNode.isMovable = true;
                        neighbourNode.lastMovableNode = currentNode;
                    } else if (neighbourNode.gCost > movementRange && currentNode.gCost <= movementRange)
                    {
                        if (!currentNode.isOccupied || movementRange == 0)
                        {
                            IterateOverOpenListWithoutEndNodeForAttack(currentNode, minAttackRange, maxAttackRange);
                        }
                    }

                    if (!_openList.Contains(neighbourNode))
                    {
                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

    }

    private void IterateOverOpenListWithoutEndNodeForAttack(PathNode startNode, int minAttackRange, int maxAttackRange)
    {
        List<PathNode> openList = new List<PathNode> { startNode };
        List<PathNode> closedList = new List<PathNode>();
        
        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int hCost = CalculateDistanceCost(neighbourNode, startNode);
                
                if (minAttackRange <= hCost && maxAttackRange >= hCost)
                {
                    neighbourNode.lastMovableNode = startNode;
                    if (!neighbourNode.isObstacle)
                    {
                        neighbourNode.isAttackable = true;
                    }
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
                else
                {
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
    }
    
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            neighbourList.Add(_grid.GetCell(currentNode.x - 1, currentNode.y).GetPathNode());
        }

        if (currentNode.x + 1 < _grid.GetGridWidth())
        {
            neighbourList.Add(_grid.GetCell(currentNode.x + 1, currentNode.y).GetPathNode());
        }
        
        if (currentNode.y - 1 >= 0)
        {
            neighbourList.Add(_grid.GetCell(currentNode.x, currentNode.y -1).GetPathNode());
        }
        
        if (currentNode.y + 1 < _grid.GetGridHeight())
        {
            neighbourList.Add(_grid.GetCell(currentNode.x, currentNode.y + 1).GetPathNode());
        }

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        return (xDistance + yDistance) * MOVE_STRAIGHT_COST;
    }


    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public Grid GetGrid()
    {
        return _grid;
    }
}

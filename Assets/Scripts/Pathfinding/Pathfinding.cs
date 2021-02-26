using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding
{

    private const int MOVE_STRAIGHT_COST = 1;

    private Grid _grid;
    private List<PathNode> _openList;
    private List<PathNode> _closedList;
    
    public Pathfinding(int width, int height, float cellSize, Vector3 originPosition)
    {
        _grid = new Grid(width, height, cellSize, originPosition);
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = _grid.GetCell(startX, startY).GetPathNode();
        PathNode endNode = _grid.GetCell(endX, endY).GetPathNode();
        Debug.Log("Width: " + _grid.GetGridWidth() + " Height: " + _grid.GetGridHeight());
        Debug.Log("start node");
        Debug.Log(startNode);
        Debug.Log("end node");
        Debug.Log(endNode);

        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        for (int x = 0; x < _grid.GetGridWidth(); x++)
        {
            for (int y = 0; y < _grid.GetGridHeight(); y++)
            {
                PathNode pathNode = _grid.GetCell(x, y).GetPathNode();
                pathNode.gCost = 9999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
                Debug.Log(pathNode);
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (_openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(_openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            Debug.Log("current node");
            Debug.Log(currentNode);
            Debug.Log("current node costs g" + currentNode.gCost + " h" + currentNode.hCost + " f" + currentNode.fCost);
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                Debug.Log("neighbourNode");
                Debug.Log(neighbourNode);

                if (_closedList.Contains(neighbourNode)) continue;

                Debug.Log("not in closed list");

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                Debug.Log("tentative G cost: +" + tentativeGCost + "current G Cost: " + neighbourNode.gCost);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    Debug.Log("is less");
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    Debug.Log("is in open?: " + !_openList.Contains(neighbourNode) );
                    if (!_openList.Contains(neighbourNode))
                    {
                        Debug.Log("adding neighbourhood node");

                        _openList.Add(neighbourNode);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            //left
            neighbourList.Add(_grid.GetCell(currentNode.x - 1, currentNode.y).GetPathNode());
        }

        if (currentNode.x + 1 < _grid.GetGridWidth())
        {
            //right
            neighbourList.Add(_grid.GetCell(currentNode.x + 1, currentNode.y).GetPathNode());

        }
        
        if (currentNode.y - 1 >= 0)
        {
            //down
            neighbourList.Add(_grid.GetCell(currentNode.x, currentNode.y -1).GetPathNode());
        }
        
        if (currentNode.y + 1 < _grid.GetGridHeight())
        {
            //right
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

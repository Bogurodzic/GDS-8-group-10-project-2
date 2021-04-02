using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell
{
    private string text;
    private Transform parent;
    private Vector3 localPosition;
    private GameObject cellGameObject;
    private PathNode _pathNode;
    private Unit _occupiedBy;

    public GridCell(int x, int y, Transform parent, Vector3 localPosition, GridManager gridManager)
    {
        this._pathNode = new PathNode(null, x, y, gridManager.GetTileData(x, y));
        this.text = x + " "  + y;
        this.parent = parent;
        this.localPosition = localPosition;
        RenderCell();
    }

    private void RenderCell()
    {
        GameObject gameObject = new GameObject(text, typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(this.parent, false);
        transform.localPosition = this.localPosition;
        this.cellGameObject = gameObject;

        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.fontSize = 40;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
    }

    public PathNode GetPathNode()
    {
        return _pathNode;
    }

    public void RemoveOccupiedBy()
    {
        _occupiedBy.GetStatistics().defend = 0;
        _occupiedBy = null;
        _pathNode.isOccupied = false;
        
    }

    public void AddOccupiedBy(Unit unit)
    {
        _occupiedBy = unit;
        _pathNode.isOccupied = true;
        unit.GetStatistics().defend = (int) _pathNode.bonusDef;

    }

    public Unit GetOccupiedBy()
    {
        
        return _occupiedBy;
    }
}

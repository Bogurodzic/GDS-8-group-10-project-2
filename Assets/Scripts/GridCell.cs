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

    public GridCell(string text, Transform parent, Vector3 localPosition)
    {
        this.text = text;
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
        textMesh.text = this.text;
        textMesh.fontSize = 40;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;

    }
}

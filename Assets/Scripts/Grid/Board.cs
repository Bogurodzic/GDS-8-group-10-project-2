using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Grid grid;

    private void Awake()
    {
        grid = new Grid(14, 8, 10f, new Vector3(-70,-40,0));
        Debug.Log(grid);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
            mouseVector3.z = 0;
            grid.SetValue(mouseVector3, 666);
        }
    }
    

    public Grid GetGrid()
    {
        return grid;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Grid grid;
    private bool _isActive = true;
    void Start()
    {
        LoadGrid();
    }

    void Update()
    {
        if (_isActive)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseVector3 = GridUtils.GetMouseWorldPosition(Input.mousePosition);
                Vector3 cellPosition = grid.GetCellCenter(mouseVector3);
                cellPosition.z = -1;
                transform.position = cellPosition;
            }
            
        }
    }

    private void LoadGrid()
    {
        grid = GameObject.Find("Testing").GetComponent<Board>().GetGrid();
        Debug.Log(grid);
    }
}

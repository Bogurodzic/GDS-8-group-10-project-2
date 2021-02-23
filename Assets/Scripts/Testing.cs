using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid grid;
    private void Start()
    {
        grid = new Grid(5, 3, 10f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseVector3 = GetMouseWorldPosition(Input.mousePosition);
            mouseVector3.z = 0;
            Debug.Log(mouseVector3);
            Debug.Log(Input.mousePosition);
            grid.SetValue(mouseVector3, 666);
        }
    }
    
    private Vector3 GetMouseWorldPosition(Vector3 mousePosotion)
    {
        return GetMouseWorldPosition(mousePosotion, Camera.main);
    }

    private Vector3 GetMouseWorldPosition(Vector3 mousePosition, Camera camera)
    {
        return camera.ScreenToWorldPoint(mousePosition);
    }
}

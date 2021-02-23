using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils 
{
    public static Vector3 GetMouseWorldPosition(Vector3 mousePosotion)
    {
        return GetMouseWorldPosition(mousePosotion, Camera.main);
    }

    public static Vector3 GetMouseWorldPosition(Vector3 mousePosition, Camera camera)
    {
        return camera.ScreenToWorldPoint(mousePosition);
    }

}

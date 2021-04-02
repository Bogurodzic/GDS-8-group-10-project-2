using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class SkipTurn : MonoBehaviour
{
    private UnitList _unitList;
    private Vector3 _initialPosition;
    
    void Start()
    {
        LoadUnitList();
        _initialPosition = gameObject.transform.position;
    }

    private void Update()
    {
        if ( Turn.GetCurrentTurnType() == TurnType.RegularGame && IsUnitActive())
        {
            transform.position = _initialPosition;
        }
        else
        {
            transform.position = new Vector3(9999, 9999, 9999);
        }
    }

    public void HandleClick()
    {
        Unit unit = _unitList.GetActiveUnit().GetComponent<Unit>();
        if (unit.IsActive())
        {
            unit.SkipTurn();
        }
    }
    
    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }

    private bool IsUnitActive()
    {
        Unit unit = _unitList.GetActiveUnit().GetComponent<Unit>();
        return unit.IsActive();
    }
}

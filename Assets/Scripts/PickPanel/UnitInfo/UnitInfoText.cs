using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoText : MonoBehaviour
{
    protected UnityEngine.UI.Text txt;
    protected UnitData _unitData;
    protected String _initialText;
    public void Start()
    {
        txt = GetComponent<UnityEngine.UI.Text>();
        LoadInitialText();
    }

    protected void LoadInitialText()
    {
        _initialText = txt.text;
    }

    public void Update()
    {
    }


    public void LoadUnitData(UnitData unitData)
    {
        _unitData = unitData;
        LoadUnitText();
    }

    protected virtual void LoadUnitText()
    {
        
    }

    public void ResetText()
    {
        txt.text = "";
    }
}

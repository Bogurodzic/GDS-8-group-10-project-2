using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitNameInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " " + _unitData.name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDefInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " " + _unitData.defend;
    }
}

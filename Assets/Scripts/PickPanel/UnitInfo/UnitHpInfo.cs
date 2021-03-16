using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " " + _unitData.maxHp;
    }
}

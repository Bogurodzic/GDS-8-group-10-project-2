using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRangeInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " " + _unitData.minAttackRange + " - " + _unitData.maxAttackRange;
    }
}

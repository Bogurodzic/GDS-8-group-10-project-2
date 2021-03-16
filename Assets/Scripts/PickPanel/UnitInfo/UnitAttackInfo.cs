using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " " + _unitData.minAttack + " - " + _unitData.maxAttack;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHpInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " " + _unitData.maxHp;
    }

    public void ShowUnitHP(int unitCurrentHp, int unitMaxHp)
    {
        txt.text = _initialText + " " + unitCurrentHp + "/" + unitMaxHp;
    }
}

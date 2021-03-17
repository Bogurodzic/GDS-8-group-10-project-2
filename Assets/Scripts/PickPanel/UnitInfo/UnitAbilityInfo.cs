using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        if (_unitData.unitAbility)
        {
            txt.text = _initialText + " " + _unitData.unitAbility.description;
        }
        else
        {
            txt.text = _initialText + " None";
        }
    }
    
}

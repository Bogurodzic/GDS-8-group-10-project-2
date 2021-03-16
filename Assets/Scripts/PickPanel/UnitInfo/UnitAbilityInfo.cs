using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityInfo : UnitInfoText
{
    protected override void LoadUnitText()
    {
        txt.text = _initialText + " ";
    }
    
}

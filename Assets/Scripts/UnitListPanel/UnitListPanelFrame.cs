using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitListPanelFrame : MonoBehaviour
{
    public GameObject unitName;
    public GameObject unitAttack;
    public GameObject unitHP;

    private UnitNameInfo _unitNameInfo;
    private UnitAttackInfo _unitAttackInfo;
    private UnitHpInfo _unitHpInfo;
    void Start()
    {
        LoadInfo();
        gameObject.SetActive(false);
    }
    void Update()
    {
        
    }

    private void LoadInfo()
    {
        _unitNameInfo = unitName.GetComponent<UnitNameInfo>();
        _unitAttackInfo = unitAttack.GetComponent<UnitAttackInfo>();
        _unitHpInfo = unitHP.GetComponent<UnitHpInfo>();
    }

    public void ShowUnitInfo(Unit unit)
    {
        _unitNameInfo.LoadUnitData(unit.unitData);
        _unitAttackInfo.LoadUnitData(unit.unitData);
        _unitHpInfo.ShowUnitHP(unit.GetStatistics().currentHp, unit.GetStatistics().maxHp);
    }
    
    public void ResetText()
    {
        _unitNameInfo.ResetText();
        _unitHpInfo.ResetText();
        _unitAttackInfo.ResetText();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    public UnitData _unitData;
    private UnitNameInfo _unitName;
    private UnitHpInfo _unitHp;
    private UnitAttackInfo _unitAttack;
    private UnitRangeInfo _unitRangeInfo;
    private UnitDefInfo _unitDefInfo;
    private UnitMovementInfo _unitMovementInfo;
    private UnitAbilityInfo _unitAbilityInfo;
    void Start()
    {
        LoadUnitName();
        LoadUnitHp();
        LoadUnitAttack();
        LoadUnitRangeInfo();
        LoadUnitDefInfo();
        LoadUnitMovementInfo();
        LoadUnitAbilityInfo();
    }

    void Update()
    {
        
    }

    private void LoadUnitName()
    {
        _unitName = gameObject.GetComponentInChildren<UnitNameInfo>();
    }

    private void LoadUnitHp()
    {
        _unitHp = gameObject.GetComponentInChildren<UnitHpInfo>();
    }
    
    private void LoadUnitAttack()
    {
        _unitAttack = gameObject.GetComponentInChildren<UnitAttackInfo>();
    }
    
    private void LoadUnitRangeInfo()
    {
        _unitRangeInfo = gameObject.GetComponentInChildren<UnitRangeInfo>();
    }
    
    private void LoadUnitDefInfo()
    {
        _unitDefInfo = gameObject.GetComponentInChildren<UnitDefInfo>();
    }
    
    private void LoadUnitMovementInfo()
    {
        _unitMovementInfo = gameObject.GetComponentInChildren<UnitMovementInfo>();
    }

    private void LoadUnitAbilityInfo()
    {
        _unitAbilityInfo = gameObject.GetComponentInChildren<UnitAbilityInfo>();
    }

    
    public void LoadUnitData(UnitData unitData)
    {
        _unitData = unitData;
        _unitName.LoadUnitData(unitData);
        _unitHp.LoadUnitData(unitData);
        _unitAttack.LoadUnitData(unitData);
        _unitRangeInfo.LoadUnitData(unitData);
        _unitDefInfo.LoadUnitData(unitData);
        _unitMovementInfo.LoadUnitData(unitData);
        _unitAbilityInfo.LoadUnitData(unitData);
    }

    public void ResetText()
    {
        _unitName.ResetText();
        _unitHp.ResetText();
        _unitAttack.ResetText();
        _unitRangeInfo.ResetText();
        _unitDefInfo.ResetText();
        _unitMovementInfo.ResetText();
        _unitAbilityInfo.ResetText();
    }
}

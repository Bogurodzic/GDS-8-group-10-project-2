using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

public class UnitListPanel : MonoBehaviour
{
    public int team;
    public GameObject[] unitListPortraits;
    public GameObject abilityButton;
    public GameObject unitListPanelFrame;
    private UnitList _unitList;
    private LinkedList<GameObject> _playerUnitList;
    private UnitListPanelFrame _unitListPanelFrame;
    private bool _panelInitialised = false;

    void Update()
    {
        if (!_panelInitialised && Turn.GetCurrentTurnType() == TurnType.RegularGame)
        {
            InitialisePanel();
        }

        if (_panelInitialised)
        {
            ReloadButton();
        }
    }


    private void ReloadButton()
    {
        if (IsUnitTurn() && HasUnitAbility())
        {
            abilityButton.SetActive(true);
        }
        else
        {
            abilityButton.SetActive(false);
        }

    }
    
    private bool IsUnitTurn()
    {
        return _unitList.GetActiveUnit().GetComponent<Unit>().GetStatistics().team == team && Turn.IsUnitTurn(team) && _unitList.GetActiveUnit().GetComponent<Unit>().IsActive();
    }

    private bool HasUnitAbility()
    {
        return _unitList.GetActiveUnit().GetComponent<Unit>().GetUnitAbility().GetAbilityType() != AbilityType.None;
    }


    private void InitialisePanel()
    {
        LoadUnitList();
        LoadPlayerUnits();
        LoadUnitListPanelFrame();
        RenderUnitListPortrait();
        unitListPanelFrame.SetActive(true);
        HideUnitInfo();
        abilityButton.SetActive(false);
        _panelInitialised = true;
    }

    private void LoadUnitListPanelFrame()
    {
        _unitListPanelFrame = unitListPanelFrame.GetComponent<UnitListPanelFrame>();
    }

    private void LoadPlayerUnits()
    {
        _playerUnitList = _unitList.GetPlayerUnitList(team);
    }
    
    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }

    private void RenderUnitListPortrait()
    {
        int index = 0;
        foreach (var unitListPortrait in unitListPortraits)
        {
            unitListPortrait.SetActive(true);
            UnitListPortrait portrait = unitListPortrait.GetComponent<UnitListPortrait>();
            portrait.LoadUnitData(_playerUnitList.ElementAt(index).GetComponent<Unit>());
            index++;
        }
    }
    

    public void DeactivateAllPlayersPortraits()
    {
        foreach (var unitListPortrait in unitListPortraits)
        {
            unitListPortrait.GetComponent<UnitListPortrait>().SetPotraitActive(false);
        }
    }

    public void DeactivateUnitPortrait(Unit unit)
    {
        GameObject unitPortrait = FindUnitPortrait(unit);
        if (unitPortrait)
        {
            unitPortrait.GetComponent<UnitListPortrait>().SetPotraitActive(false);
        }
    }
    
    public void ActivateUnitPortrait(Unit unit)
    {
        GameObject unitPortrait = FindUnitPortrait(unit);
        if (unitPortrait)
        {
            unitPortrait.GetComponent<UnitListPortrait>().SetPotraitActive(true);
        }
    }

    public void OnHoverUnit(Unit unit)
    {
        if (_panelInitialised)
        {
            HoverOutAllUnitPortrait();
            ShowUnitInfo(unit);
            HoverUnitPortrait(unit);        
        }

    }

    public void OnHoverOutUnit()
    {
        if (_panelInitialised)
        {
            HideUnitInfo();
            HoverOutAllUnitPortrait();
        }
    } 

    private void HoverUnitPortrait(Unit unit)
    {
        GameObject unitPortrait = FindUnitPortrait(unit);
        if (unitPortrait)
        {
            unitPortrait.GetComponent<UnitListPortrait>().OnHover();
        }
    }

    private void HoverOutAllUnitPortrait()
    {
        foreach (var unitListPortrait in unitListPortraits)
        {
            unitListPortrait.GetComponent<UnitListPortrait>().OnHoverOut();
 
        }
    }

    private GameObject FindUnitPortrait(Unit unit)
    {
        GameObject unitPortrait = null;
        
        foreach (var unitListPortrait in unitListPortraits)
        {
            if (unitListPortrait.GetComponent<UnitListPortrait>().GetUnitData().unitName == unit.unitData.unitName)
            {
                unitPortrait = unitListPortrait;
            }    
        }

        return unitPortrait;
    }

    public void ShowUnitInfo(Unit unit)
    {
        _unitListPanelFrame.ShowUnitInfo(unit);
    }

    public void HideUnitInfo()
    {
        _unitListPanelFrame.ResetText();
    }
}

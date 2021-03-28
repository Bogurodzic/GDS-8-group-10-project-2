﻿using System.Collections;
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
    void Start()
    {
        
    }

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
        _panelInitialised = true;
        LoadUnitList();
        LoadPlayerUnits();
        LoadUnitListPanelFrame();
        RenderUnitListPortrait();
        unitListPanelFrame.SetActive(true);
        HideUnitInfo();
        abilityButton.SetActive(false);
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

    public void DeactivateAllPlayerUnits()
    {
        _unitList.DeactivateAllPlayerUnits(team);
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
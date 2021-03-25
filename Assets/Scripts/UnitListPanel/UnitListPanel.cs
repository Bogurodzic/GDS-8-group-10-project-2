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
    private UnitList _unitList;
    private LinkedList<GameObject> _playerUnitList;
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
    }

    private void InitialisePanel()
    {
        _panelInitialised = true;
        LoadUnitList();
        LoadPlayerUnits();
        RenderUnitListPortrait();
        abilityButton.SetActive(true);
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
            portrait.LoadUnitData(_playerUnitList.ElementAt(index).GetComponent<Unit>().unitData);
            index++;
        }
    }
}

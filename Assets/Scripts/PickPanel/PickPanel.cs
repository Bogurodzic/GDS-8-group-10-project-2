using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class PickPanel : MonoBehaviour
{
    public UnitData[] unitsToPick;
    public GameObject portraitPrefab;

    public int currentPlayerPickingTurn = 1;
    public int maxPlayerChoices = 4;
    public int player1ActiveChoices = 0;
    public int player2ActiveChoices = 0;

    public GameObject[] portraits;
    private ReadyButton _readyButton;
    private UnitInfo _unitInfo;
    void Start()
    {
        LoadReadyButton();
        LoadUnitInfo();
    }

    void Update()
    {
        
    }
    
    private void LoadReadyButton()
    {
        _readyButton = GameObject.Find("ReadyButton").GetComponent<ReadyButton>();
    }

    private void LoadUnitInfo()
    {
        _unitInfo = GameObject.Find("UnitInfo").GetComponent<UnitInfo>();
    }

    private void ResetPortraits()
    {
        foreach (var portrait in portraits)
        {
            portrait.GetComponent<Portrait>().SetPotraitActive(false);
        }
    }
    
    public bool CanActivatePotrait()
    {
        ReloadActivePortrait();
        if (CanCurrentPlayerActivatePortrait())
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    private bool CanCurrentPlayerActivatePortrait()
    {
        if (currentPlayerPickingTurn == 1 && player1ActiveChoices < maxPlayerChoices)
        {
            return true;
        }
        else if (currentPlayerPickingTurn == 2 && player2ActiveChoices < maxPlayerChoices)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReloadActivePortrait()
    {
        int playerActiveChoices = 0;

        foreach (var portrait in portraits)
        {
            if (portrait.GetComponent<Portrait>().isActive())
            {
                playerActiveChoices += 1;
            }
        }

        SetActivePlayerChoices(playerActiveChoices);
    }

    public void ReloadReadyButton()
    {
        if (IsCurrentPlayerReady())
        {
            _readyButton.SetReady(true);
        }
        else
        {
            _readyButton.SetReady(false);
        }
    }

    public void ReloadPickPanel()
    {
        ReloadActivePortrait();
        ReloadReadyButton();
    }

    public void HandleReadyClicked()
    {
        if (currentPlayerPickingTurn == 1)
        {
            NextPlayerPickingTurn();
        }
        else
        {
            DeactivatePanel();
        }
    }

    private void NextPlayerPickingTurn()
    {
        PickedUnits.AddPlayerPickedUnits(GetCurrentPlayerPickedUnits());
        Turn.NextTurn();
        currentPlayerPickingTurn = 2;
        ResetPortraits();
        ReloadReadyButton();
    }

    private LinkedList<UnitData> GetCurrentPlayerPickedUnits()
    {
        LinkedList<UnitData> pickedUnits = new LinkedList<UnitData>();

        foreach (var portrait in portraits)
        {
            Portrait portraitInstance = portrait.GetComponent<Portrait>();
            if (portraitInstance.isActive())
            {
                pickedUnits.AddLast(portraitInstance.GetUnitData());
            }
        }

        return pickedUnits;
    }

    private void DeactivatePanel()
    {
        PickedUnits.AddPlayerPickedUnits(GetCurrentPlayerPickedUnits());
        Turn.NextTurn();
        Turn.SetTurnType(TurnType.Deployment);
        Destroy(GameObject.Find("PickPanelWrapper"));
    }

    private bool IsCurrentPlayerReady()
    {
        return (currentPlayerPickingTurn == 1 && player1ActiveChoices == maxPlayerChoices) || (currentPlayerPickingTurn == 2 && player2ActiveChoices ==  maxPlayerChoices);
    }

    private void SetActivePlayerChoices(int choices)
    {
        if (currentPlayerPickingTurn == 1)
        {
            player1ActiveChoices = choices;
        }
        else
        {
            player2ActiveChoices = choices;
        }
    }

    public void DisplayUnitInfo(UnitData unitData)
    {
        _unitInfo.LoadUnitData(unitData);
    }

    public void ResetUnitInfo()
    {
        _unitInfo.ResetText();
    }

    public int GetRemainingSelections()
    {
        if (currentPlayerPickingTurn == 1)
        {
            return maxPlayerChoices - player1ActiveChoices;
        }
        else
        {
            return maxPlayerChoices - player2ActiveChoices;
        }
    }
}

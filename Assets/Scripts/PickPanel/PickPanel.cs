using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPanel : MonoBehaviour
{
    public UnitData[] unitsToPick;
    public GameObject portraitPrefab;

    public int currentPlayerPickingTurn = 1;
    public int maxPlayerChoices = 4;
    public int player1ActiveChoices = 0;
    public int player2ActiveChoices = 0;

    private LinkedList<GameObject> portraits = new LinkedList<GameObject>();
    private ReadyButton _readyButton;
    void Start()
    {
        LoadReadyButton();
        CreatePortraits();
    }

    void Update()
    {
        
    }

    private void CreatePortraits()
    {
        for (int i = 0; i < unitsToPick.Length; i++)
        {
            GameObject portrait = Instantiate(portraitPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            portrait.transform.parent = gameObject.transform;
            portrait.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            portrait.transform.position = new Vector3(925 + i * 550, portrait.transform.position.y,
                  portrait.transform.position.z);
            portrait.GetComponent<Portrait>().LoadUnitData(unitsToPick[i]);
            portraits.AddLast(portrait);
        }
    }

    private void LoadReadyButton()
    {
        _readyButton = GameObject.Find("ReadyButton").GetComponent<ReadyButton>();
        Debug.Log("Rady button");
        Debug.Log(_readyButton);
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
        currentPlayerPickingTurn = 2;
        ResetPortraits();
        ReloadReadyButton();
    }

    private void DeactivatePanel()
    {
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
}

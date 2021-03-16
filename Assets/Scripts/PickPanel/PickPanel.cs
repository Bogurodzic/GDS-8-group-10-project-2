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
    void Start()
    {
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
    
    public bool CanActivatePotrait()
    {
        ReloadActivePortrait();
        if (player1ActiveChoices < maxPlayerChoices)
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

        player1ActiveChoices = playerActiveChoices;
    }
}

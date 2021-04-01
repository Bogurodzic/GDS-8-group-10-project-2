using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{

    public GameObject unitPrefab;
    public UnitData bossUnitData;
    public GameObject playerWinPanel;
    private LinkedList<GameObject> player1UnitList = new LinkedList<GameObject>();
    private LinkedList<GameObject> player2UnitList = new LinkedList<GameObject>();
    private GameObject _nextUnitToDeploy;
    private bool _readyForDeploy = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateUnitListForPlayers(LinkedList<UnitData> player1PickedUnits,
        LinkedList<UnitData> player2PickedUnits)
    {
        GameObject boss1 = Instantiate(unitPrefab);
        boss1.GetComponent<UnitStatistics>().team = 1;
        boss1.transform.position = Vector3.one * -999;
        boss1.GetComponent<Unit>().LoadUnitData(bossUnitData);
        player1UnitList.AddLast(boss1);
        
        GameObject boss2 = Instantiate(unitPrefab);
        boss2.GetComponent<UnitStatistics>().team = 2;
        boss2.transform.position = Vector3.one * -999;
        boss2.GetComponent<Unit>().LoadUnitData(bossUnitData);
        player2UnitList.AddLast(boss2);
        
        
        foreach (var player1PickedUnit in player1PickedUnits)
        {
            GameObject pickedUnit = Instantiate(unitPrefab);
            pickedUnit.GetComponent<UnitStatistics>().team = 1;
            pickedUnit.transform.position = Vector3.one * -999;
            pickedUnit.GetComponent<Unit>().LoadUnitData(player1PickedUnit);
            player1UnitList.AddLast(pickedUnit);
        }
        
        foreach (var player2PickedUnit in player2PickedUnits)
        {
            GameObject pickedUnit = Instantiate(unitPrefab);
            pickedUnit.GetComponent<UnitStatistics>().team = 2;
            pickedUnit.transform.position = Vector3.one * -999;
            pickedUnit.GetComponent<Unit>().LoadUnitData(player2PickedUnit);
            player2UnitList.AddLast(pickedUnit);
        }

        _readyForDeploy = true;
    }

    public void HandleNextUnitToDeploy()
    {
        foreach (var o in player1UnitList)
        {
            Debug.Log(o);
        }
        foreach (var o in player2UnitList)
        {
            Debug.Log(o);
        }

        if (Turn.GetUnitTurn() == 1)
        {
            _nextUnitToDeploy = FindNextUnitToDeploy(player1UnitList);
        }
        else
        {
            _nextUnitToDeploy = FindNextUnitToDeploy(player2UnitList);
        }
    }

    public bool IsAnyUnitToDeploy()
    {
        GameObject last1playerUnit = FindNextUnitToDeploy(player1UnitList);
        GameObject last2playerUnit = FindNextUnitToDeploy(player2UnitList);

        if (!last1playerUnit.GetComponent<Unit>().IsUnitDeployed() ||
            !last2playerUnit.GetComponent<Unit>().IsUnitDeployed())
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private GameObject FindNextUnitToDeploy(LinkedList<GameObject> playerUnitList)
    {
        LinkedList<GameObject>.Enumerator playerUnitListEnumerator = playerUnitList.GetEnumerator();
        playerUnitListEnumerator.MoveNext();
        GameObject nextUnitToDeploy = playerUnitListEnumerator.Current;
        Unit unit = nextUnitToDeploy.GetComponent<Unit>();

        while (unit.IsUnitDeployed() && playerUnitListEnumerator.MoveNext())
        {
            nextUnitToDeploy = playerUnitListEnumerator.Current;
            unit = nextUnitToDeploy.GetComponent<Unit>();
        }
        
        return nextUnitToDeploy;
    }

    private GameObject FindActiveUnit(LinkedList<GameObject> playerUnitList)
    {
        LinkedList<GameObject>.Enumerator playerUnitListEnumerator = playerUnitList.GetEnumerator();
        playerUnitListEnumerator.MoveNext();
        GameObject activeUnit = playerUnitListEnumerator.Current;
        
        Unit unit = activeUnit.GetComponent<Unit>();
        while (!unit.IsActive() && playerUnitListEnumerator.MoveNext())
        {
            activeUnit = playerUnitListEnumerator.Current;
            unit = activeUnit.GetComponent<Unit>();
        }
        
        return activeUnit;
    }

    public GameObject FindActiveUnit(int team)
    {
        if (team == 1)
        {
            return FindActiveUnit(player1UnitList);
        }
        else
        {
            return FindActiveUnit(player2UnitList);
        }
    }

    private void ResetAllUnitsOnCD()
    {
        foreach (var o in player1UnitList)
        {
            if (o.GetComponent<Unit>().IsAlive())
            {
                o.GetComponent<Unit>().ResetUnitCD();
            }
        }
        
        foreach (var o in player2UnitList)
        {
            if (o.GetComponent<Unit>().IsAlive())
            {
                o.GetComponent<Unit>().ResetUnitCD();
            }
        }
        
    }

    private bool AreAllTeamUnitsOnCD(int team)
    {
        bool allUnitsAreOnCD = true;
        
        if (team == 1)
        {
            foreach (var o in player1UnitList)
            {
                if (!o.GetComponent<Unit>().IsOnCD() && o.GetComponent<Unit>().IsAlive())
                {
                    allUnitsAreOnCD = false;
                }
            }
        } else if (team == 2)
        {
            foreach (var o in player2UnitList)
            {
                if (!o.GetComponent<Unit>().IsOnCD() && o.GetComponent<Unit>().IsAlive())
                {
                    allUnitsAreOnCD = false;
                }
            }
        }

        return allUnitsAreOnCD;
    }
    
    private bool AreAllTeamUnitsDead(int team)
    {
        bool allUnitsAreDead = true;
        
        if (team == 1)
        {
            foreach (var o in player1UnitList)
            {
                if (o.GetComponent<Unit>().IsAlive())
                {
                    allUnitsAreDead = false;
                }
            }
        } else if (team == 2)
        {
            foreach (var o in player2UnitList)
            {
                if (o.GetComponent<Unit>().IsAlive())
                {
                    allUnitsAreDead = false;
                }
            }
        }

        return allUnitsAreDead;
    }

    private bool AreAllUnitsOnCD()
    {
        bool allUnitsAreOnCD = true;
        foreach (var o in player1UnitList)
        {
            if (!o.GetComponent<Unit>().IsOnCD() && o.GetComponent<Unit>().IsAlive())
            {
                allUnitsAreOnCD = false;
            }
        }
        foreach (var o in player2UnitList)
        {
            if (!o.GetComponent<Unit>().IsOnCD() && o.GetComponent<Unit>().IsAlive())
            {
                allUnitsAreOnCD = false;
            }
        }
        return allUnitsAreOnCD;
    }

    public GameObject GetNextUnitToDeploy()
    {
        return _nextUnitToDeploy;
    }

    public bool ReadyForDeploy()
    {
        return _readyForDeploy;
    }
    

   public GameObject GetActiveUnit()
   {
       CheckWinCondition();
       
        if (AreAllUnitsOnCD())
        {
            ResetAllUnitsOnCD();
        }

        if (AreAllTeamUnitsOnCD(1) && Turn.IsUnitTurn(1))
        {
            Turn.NextTurn();
        }
        
        if (AreAllTeamUnitsOnCD(2) && Turn.IsUnitTurn(2))
        {
            Turn.NextTurn();
        }

        GameObject activeUnitFromPlayer1UnitList = FindActiveUnit(player1UnitList);

        if (activeUnitFromPlayer1UnitList.GetComponent<Unit>().IsActive())
        {
            return activeUnitFromPlayer1UnitList;
        }
        
        GameObject activeUnitFromPlayer2UnitList = FindActiveUnit(player2UnitList);
        
        return activeUnitFromPlayer2UnitList;

    }

   public LinkedList<GameObject> GetPlayerUnitList(int playerTeam)
   {
       if (playerTeam == 1)
       {
           return player1UnitList;
       }
       else
       {
           return player2UnitList;
       }
   }
   
   public void DeactivateAllPlayerUnits(int team)
   {
       if (team == 1)
       {
           foreach (var o in player1UnitList)
           {
               o.GetComponent<Unit>().DeactivateUnit();
           } 
       }

       if (team == 2)
       {
           foreach (var o in player2UnitList)
           {
               o.GetComponent<Unit>().DeactivateUnit();
           }
       }
   }

   public void CheckWinCondition()
   {
       bool boss1Dead = false;
       bool boss2Dead = false;
       
       foreach (var o in player1UnitList)
       {
           if (o.GetComponent<Unit>().unitData.unitName == "Boss" && !o.GetComponent<Unit>().IsAlive())
           {
               boss1Dead = true;
           }
       }

       foreach (var o in player2UnitList)
       {
           if (o.GetComponent<Unit>().unitData.unitName == "Boss" && !o.GetComponent<Unit>().IsAlive())
           {
               boss2Dead = true;
           }
       }

       if (boss1Dead)
       {
           if (Turn.IsUnitTurn(1))
           {
               Turn.NextTurn();
               playerWinPanel.SetActive(true);
           }
           else
           {
               playerWinPanel.SetActive(true);
           }
       } else if (boss2Dead)
       {
           if (Turn.IsUnitTurn(2))
           {
               Turn.NextTurn();
               playerWinPanel.SetActive(true);
           }
           else
           {
               playerWinPanel.SetActive(true);
           }
       }
   }
}

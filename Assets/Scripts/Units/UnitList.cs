using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public GameObject unitPrefab;
    public UnitData bossUnitData;
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
        Debug.Log("activeUnit");
        Debug.Log(activeUnit);

        Unit unit = activeUnit.GetComponent<Unit>();
        while (!unit.IsActive() && playerUnitListEnumerator.MoveNext())
        {
            activeUnit = playerUnitListEnumerator.Current;
            unit = activeUnit.GetComponent<Unit>();
        }

        return activeUnit;
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
        GameObject activeUnitFromPlayer1UnitList = FindActiveUnit(player1UnitList);

        if (activeUnitFromPlayer1UnitList.GetComponent<Unit>().IsActive())
        {
            return activeUnitFromPlayer1UnitList;
        }
        
        GameObject activeUnitFromPlayer2UnitList = FindActiveUnit(player2UnitList);

        return activeUnitFromPlayer2UnitList;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public GameObject unitPrefab;
    private LinkedList<GameObject> player1UnitList = new LinkedList<GameObject>();
    private LinkedList<GameObject> player2UnitList = new LinkedList<GameObject>();

    private GameObject _nextUnitToDeploy;
    private bool _readyForDeploy = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUnitListForPlayers(LinkedList<UnitData> player1PickedUnits,
        LinkedList<UnitData> player2PickedUnits)
    {
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
        Debug.Log("HandleNextUnitToDeploy");
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
            Debug.Log("THERE IS TILL UNIT TO DEPLOY");
            return true;
        }
        else
        {
            Debug.Log("THERE IS NO MORE UNIT TO DEPLOY");

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

    public GameObject GetNextUnitToDeploy()
    {
        return _nextUnitToDeploy;
    }

    public bool ReadyForDeploy()
    {
        return _readyForDeploy;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitList : MonoBehaviour
{
    public GameObject unitPrefab;
    private LinkedList<GameObject> player1UnitList = new LinkedList<GameObject>();
    private LinkedList<GameObject> player2UnitList = new LinkedList<GameObject>();

    private GameObject _nextUnitToDeploy;

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
            pickedUnit.transform.position = Vector3.one * -999;
            pickedUnit.GetComponent<Unit>().LoadUnitData(player1PickedUnit);
            player1UnitList.AddLast(pickedUnit);
        }
        
        foreach (var player2PickedUnit in player2PickedUnits)
        {
            GameObject pickedUnit = Instantiate(unitPrefab);
            pickedUnit.transform.position = Vector3.one * -999;
            pickedUnit.GetComponent<Unit>().LoadUnitData(player2PickedUnit);
            player2UnitList.AddLast(pickedUnit);
        }
    }

    public void HandleNextUnitToDeploy()
    {
        if (Turn.GetUnitTurn() == 1)
        {
            _nextUnitToDeploy = player1UnitList.First.Value;
        }
        else
        {
            _nextUnitToDeploy = player2UnitList.First.Value;
        }
    }

    public GameObject GetNextUnitToDeploy()
    {
        return _nextUnitToDeploy;
    }
}

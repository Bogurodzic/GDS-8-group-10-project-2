using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PickedUnits
{
    private static LinkedList<UnitData> player1PickedUnits;
    private static LinkedList<UnitData> player2PickedUnits;


    public static void AddPlayerPickedUnits(LinkedList<UnitData> pickedUnits)
    {
        if (Turn.GetUnitTurn() == 1)
        {
            player1PickedUnits = pickedUnits;
        }
        else
        {
            player2PickedUnits = pickedUnits;
        }
        
        Debug.Log(player1PickedUnits);
        Debug.Log(player2PickedUnits);
    }

}

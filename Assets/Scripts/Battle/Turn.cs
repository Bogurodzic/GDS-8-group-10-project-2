using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    private static int _currentTurn = 1;

    public static void NextTurn()
    {
        if (_currentTurn == 1)
        {
            _currentTurn = 2;
        }
        else
        {
            _currentTurn = 1;
        }
    }

    public static bool IsUnitTurn(int unitTeam)
    {
        if (unitTeam == _currentTurn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int GetUnitTurn()
    {
        return _currentTurn;
    }
}

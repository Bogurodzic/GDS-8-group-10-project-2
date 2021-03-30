using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public static class Turn
{
    private static TurnType _currentTurnType = TurnType.Pick;
    private static int _currentTurn = 1;
    private static bool _turnBlocked = false;

    public static bool IsTurnBlocked()
    {
        return _turnBlocked;
    }

    public static void BlockTurn()
    {
        _turnBlocked = true;
    }

    public static void UnlockTurn()
    {
        _turnBlocked = false;
    }

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
        
        UnlockTurn();
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

    public static TurnType GetCurrentTurnType()
    {
        return _currentTurnType;
    }

    public static void SetTurnType(TurnType turnType)
    {
        _currentTurnType = turnType;
    }

    public static void ResetGame()
    {
        SetTurnType(TurnType.Pick);
        _currentTurn = 1;
    }
}

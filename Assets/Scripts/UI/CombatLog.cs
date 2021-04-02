using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLog : MonoBehaviour
{
    private UnityEngine.UI.Text txt;
    private ArrayList _combatLog = new ArrayList();

    private int _xPosition = 0;
    private int _yPosition = -30;

    void Start()
    {
        txt = GetComponent<UnityEngine.UI.Text>();
    }

    void Update()
    {

    }

    public void LogCombat(string log)
    {
        _combatLog.Add(log);
        txt.text = log;
    }

    public int GetLogXPosition()
    {
        return _xPosition;
    }

    public int GetLogYPosition()
    {
        return _yPosition;
    }
}

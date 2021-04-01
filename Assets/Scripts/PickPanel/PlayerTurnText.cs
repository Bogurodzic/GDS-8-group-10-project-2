using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnText : MonoBehaviour
{
    protected UnityEngine.UI.Text txt;
    public void Start()
    {
        LoadTextComponent();
    }

    public void Update()
    {
        ReloadText();
    }

    private void ReloadText()
    {
        txt.text = Turn.GetUnitTurn() + "";
    }

    private void LoadTextComponent()
    {
        txt = GetComponent<UnityEngine.UI.Text>();
    }
}

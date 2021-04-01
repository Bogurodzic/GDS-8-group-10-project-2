using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class TurnText : MonoBehaviour
{
    private UnityEngine.UI.Text txt;
    void Start()
    {
        txt = GetComponent<UnityEngine.UI.Text>();

    }

    void Update()
    {
        txt.text = "PLAYER " + Turn.GetUnitTurn() + " TURN";
    }
}

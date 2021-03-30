using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinText : MonoBehaviour
{
    private Text _text;
    void Start()
    {
        LoadText();
    }

    void Update()
    {
        _text.text = "PLAYER " + Turn.GetUnitTurn() + " WINS";
    }

    private void LoadText()
    {
        _text = gameObject.GetComponent<Text>();
    }
}

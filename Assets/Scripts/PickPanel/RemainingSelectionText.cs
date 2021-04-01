using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainingSelectionText : MonoBehaviour
{
    private UnityEngine.UI.Text txt;
    private PickPanel _pickPanel;
    public void Start()
    {
        LoadTextComponent();
        LoadPickPanel();
    }

    public void Update()
    {
        ReloadText();
    }

    private void ReloadText()
    {
        txt.text = "" + _pickPanel.GetRemainingSelections();
    }

    private void LoadTextComponent()
    {
        txt = GetComponent<UnityEngine.UI.Text>();
    }

    private void LoadPickPanel()
    {
        _pickPanel = GameObject.Find("PickPanel").GetComponent<PickPanel>();
    }
}

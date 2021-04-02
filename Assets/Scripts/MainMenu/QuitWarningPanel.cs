using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitWarningPanel : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void OnCancelClick()
    {
        QuitWarning.Hide();
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}

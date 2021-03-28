using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public GameObject quitWarningPanel;

    public void Update()
    {
        if (quitWarningPanel && QuitWarning.IsVisible())
        {
            quitWarningPanel.SetActive(true);
        } else if (quitWarningPanel && !QuitWarning.IsVisible())
        {
            quitWarningPanel.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void QuitWithWarning()
    {
        if (!QuitWarning.IsVisible())
        {
            QuitWarning.Show();
        }
    }
}

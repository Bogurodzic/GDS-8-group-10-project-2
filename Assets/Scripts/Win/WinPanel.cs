using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void ReturnToMainMenu()
    {
        Turn.ResetGame();
        SceneManager.LoadScene("MainMenu");
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Start()
    {
        MusicManager.Instance.PlayMusicMenu();
    }

    public void StarGame()
    {
        MusicManager.Instance.PlayMusicDeployment();
        SceneManager.LoadScene("Scenes/Game");
    }
    
}

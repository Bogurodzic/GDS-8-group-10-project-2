using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public AudioClip startButtonClip;
    private AudioSource _audioSource;
    
    public void Start()
    {
        LoadAudioSource();
    }

    public void StarGame()
    {
        PlayStartButton();
        MusicManager.Instance.PlayMusicDeployment();
        SceneManager.LoadScene("Scenes/Game");
    }

    private void LoadAudioSource()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void PlayStartButton()
    {
        if (MusicManager.Instance.IsEnabled())
        {
            _audioSource.clip = startButtonClip;
            _audioSource.Play();
        }
    }
}

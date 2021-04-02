using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : GenericSingletonClass<MusicManager>
{
    public AudioClip musicMenu;
    public AudioClip musicDeployment;
    public AudioClip musicBattle;
    public AudioClip musicVictory;

    private AudioSource _audioSource;
    private bool _isEnabled = true;
    
    public void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public bool IsEnabled()
    {
        return _isEnabled;
    }

    public void Enable()
    {
        _isEnabled = true;
        _audioSource.volume = 1;
    }

    public void Disable()
    {
        _isEnabled = false;
        _audioSource.volume = 0;
    }

    public void PlayMusicMenu()
    {
        _audioSource.clip = musicMenu;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayMusicDeployment()
    {
        _audioSource.clip = musicDeployment;
        _audioSource.loop = true;
        _audioSource.Play();  
    }

    public void PlayMusicBattle()
    {
        _audioSource.clip = musicBattle;
        _audioSource.loop = true;
        _audioSource.Play();  
    }
    
    public void PlayMusicVictory()
    {
        _audioSource.clip = musicVictory;
        _audioSource.loop = true;
        _audioSource.Play();  
    }
}

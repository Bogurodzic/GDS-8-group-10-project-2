using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public UnitData unitData;
    public AudioClip selectMusic;
    private AudioSource _audioSource;
    private Image _portraitImage;
    private PortraitFrame _portraitFrame;
    private bool _isActive;
    private PickPanel _pickPanel;

    public void Start()
    {
        RenderUnitData();
        LoadAudioSource();
    }

    public void LoadPortraitImage()
    {
        _portraitImage = gameObject.GetComponentsInChildren<Image>()[1];
    }

    private void ReloadPortraitImage()
    {
        _portraitImage.sprite = unitData.unitPickerSprite;
    }

    public void LoadUnitData(UnitData unitDataToLoad)
    {
        unitData = unitDataToLoad;
        LoadPortraitImage();
        ReloadPortraitImage();
        LoadPortraitFrame();
        LoadPickPanel();
    }

    public void RenderUnitData()
    {
        LoadUnitData(unitData);
    }

    public void LoadAudioSource()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }
    
    public void TogglePortrait()
    {
        PlaySelectSound();
        if (_isActive)
        {
            _isActive = false;
        } else if (_pickPanel.CanActivatePotrait())
        {
            _isActive = true;
        }
        _portraitFrame.SetActive(_isActive);
        _pickPanel.ReloadPickPanel();
    }

    public void OnHover()
    {
        _pickPanel.DisplayUnitInfo(unitData);
        _portraitFrame.Hover();
    }

    public void OnHoverOut()
    {
        _pickPanel.ResetUnitInfo();
        _portraitFrame.HoverOut();
    }

    public void SetPotraitActive(bool isActive)
    {
        _isActive = isActive;
        _portraitFrame.SetActive((isActive));
    }

    public bool isActive()
    {
        return _isActive;
    }

    public UnitData GetUnitData()
    {
        return unitData;
    }
    
    private void LoadPortraitFrame()
    {
        _portraitFrame = gameObject.GetComponentInChildren<PortraitFrame>();
    }

    private void LoadPickPanel()
    {
        _pickPanel = gameObject.GetComponentInParent<PickPanel>();
    }
    
    private void PlaySelectSound()
    {
        if (MusicManager.Instance.IsEnabled())
        {
            _audioSource.clip = selectMusic;
            _audioSource.Play();       
        }
    }
}

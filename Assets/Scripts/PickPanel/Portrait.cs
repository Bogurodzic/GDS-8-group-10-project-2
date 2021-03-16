using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    private UnitData _unitData;
    private Image _portraitImage;
    private PortraitFrame _portraitFrame;
    private bool _isActive;
    private PickPanel _pickPanel;

    public void Start()
    {
    }

    public void LoadPortraitImage()
    {
        _portraitImage = gameObject.GetComponentsInChildren<Image>()[1];
    }

    private void ReloadPortraitImage()
    {
        Debug.Log(_portraitImage);
        _portraitImage.sprite = _unitData.unitPickerSprite;
    }

    public void LoadUnitData(UnitData unitDataToLoad)
    {
        _unitData = unitDataToLoad;
        LoadPortraitImage();
        ReloadPortraitImage();
        LoadPortraitFrame();
        LoadPickPanel();
    }

    private void LoadPortraitFrame()
    {
        _portraitFrame = gameObject.GetComponentInChildren<PortraitFrame>();
    }

    private void LoadPickPanel()
    {
        _pickPanel = gameObject.GetComponentInParent<PickPanel>();
    }

    public void TogglePortrait()
    {
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

    public void SetPotraitActive(bool isActive)
    {
        _isActive = isActive;
        _portraitFrame.SetActive((isActive));
    }

    public bool isActive()
    {
        return _isActive;
    }
}

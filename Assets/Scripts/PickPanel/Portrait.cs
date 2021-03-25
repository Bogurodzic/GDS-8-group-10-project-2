using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public UnitData unitData;
    private Image _portraitImage;
    private PortraitFrame _portraitFrame;
    private bool _isActive;
    private PickPanel _pickPanel;

    public void Start()
    {
        RenderUnitData();
    }

    public void LoadPortraitImage()
    {
        _portraitImage = gameObject.GetComponentsInChildren<Image>()[1];
    }

    private void ReloadPortraitImage()
    {
        Debug.Log(_portraitImage);
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

    public void OnHover()
    {
        _pickPanel.DisplayUnitInfo(unitData);
    }

    public void OnHoverOut()
    {
        _pickPanel.ResetUnitInfo();
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
}

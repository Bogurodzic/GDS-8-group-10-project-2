using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListPortrait : MonoBehaviour
{
    private UnitData _unitData;
    private Image _portraitImage;
    private PortraitFrame _portraitFrame;
    private bool _isActive;
    private UnitListPanel _unitListPanel;
    private Unit _unit;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (!_unit.IsAlive())
        {
            _portraitImage.color = Color.red;
        }
    }

    public void LoadPortraitImage()
    {
        _portraitImage = gameObject.GetComponentsInChildren<Image>()[1];
    }

    private void ReloadPortraitImage()
    {
        Debug.Log(_portraitImage);
        _portraitImage.sprite = _unitData.unitListSprite;
    }

    public void LoadUnitData(Unit unitToLoad)
    {
        _unit = unitToLoad;
        _unitData = unitToLoad.unitData;
        LoadPortraitImage();
        ReloadPortraitImage();
        LoadPortraitFrame();
        LoadUnitListPanel();
    }

    private void LoadPortraitFrame()
    {
        _portraitFrame = gameObject.GetComponentInChildren<PortraitFrame>();
    }

    private void LoadUnitListPanel()
    {
        _unitListPanel = gameObject.GetComponentInParent<UnitListPanel>();
    }



    public void OnHover()
    {
        _unitListPanel.ShowUnitInfo(_unit);
        _portraitFrame.Hover();
    }

    public void OnHoverOut()
    { 
        _unitListPanel.HideUnitInfo();
        _portraitFrame.HoverOut();
    }

    public void OnClick()
    {
        if (_unit.IsActive())
        {
            _unit.DeactivateUnit();
            SetPotraitActive(false);
        }
        else
        {
            _unitListPanel.DeactivateAllPlayerUnits();
            bool isFrameReadyToActivate = _unit.HandleTogglingFromFrame();
            Debug.Log("ISFRAMEREADYTOACTIVE");
            Debug.Log(isFrameReadyToActivate);
            if (isFrameReadyToActivate)
            {
                SetPotraitActive(true);
            }
            else
            {
                SetPotraitActive(false);
            }
        }
    }

    public void SetPotraitActive(bool isActive)
    {
        _isActive = isActive;
        _portraitFrame.SetActive((isActive));
    }

    public bool IsActive()
    {
        return _isActive;
    }

    public UnitData GetUnitData()
    {
        return _unitData;
    }
}
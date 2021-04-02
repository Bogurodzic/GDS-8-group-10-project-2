using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Image _image;
    private Button _button;
    private UnitList _unitList;
    private bool _isReady;
    private Text _buttonText;
    public Sprite readyButtonSprite;
    public Sprite notReadyButtonSprite;
    public int abilityTeam;
    void Start()
    {
        LoadSprite();
        LoadUnitList();
        LoadButtonText();
        LoadButton();
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Turn.GetCurrentTurnType() == TurnType.RegularGame)
        { 
            SetIsReady(IsUnitAbilityActive());
            if (_unitList.GetActiveUnit().GetComponent<Unit>().GetUnitAbility().GetAbilityType() != AbilityType.None)
            {
                ReloadSprite();
            }
        }

    }

    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }

    private void LoadSprite()
    {
        _image = gameObject.GetComponent<Image>();
    }

    private void LoadButtonText()
    {
        _buttonText = gameObject.GetComponentInChildren<Text>();
    }

    private void LoadButton()
    {
        _button = gameObject.GetComponent<Button>();
    }

    private void ReloadButtonStates(bool isActive)
    {
        SpriteState spriteState = new SpriteState();
        spriteState = _button.spriteState;

        if (isActive)
        {
            spriteState.pressedSprite = _unitList.GetActiveUnit().GetComponent<Unit>().unitData.unitAbility.abilityButtonActive;
            spriteState.highlightedSprite =
                _unitList.GetActiveUnit().GetComponent<Unit>().unitData.unitAbility.abilityButtonHover;
            _image.sprite = _unitList.GetActiveUnit().GetComponent<Unit>().unitData.unitAbility.abilityButtonNormal;
        }
        else
        {
            spriteState.pressedSprite = _unitList.GetActiveUnit().GetComponent<Unit>().unitData.unitAbility.abilityButtonActive;
            spriteState.highlightedSprite =
                _unitList.GetActiveUnit().GetComponent<Unit>().unitData.unitAbility.abilityButtonActive;
            _image.sprite = _unitList.GetActiveUnit().GetComponent<Unit>().unitData.unitAbility.abilityButtonActive;
        }

        
        _button.spriteState = spriteState;
    }

    private void ReloadSprite()
    {
        if (_isReady)
        {
            ReloadButtonStates(true);
        }
        else
        {
            ReloadButtonStates(false);
            if (GetRemainingAbilityCD() > 0)
            {
                _buttonText.text = "" + GetRemainingAbilityCD();
            }
            else
            {
                _buttonText.text = "";
            }
        }
    }
    
    private void SetIsReady(bool isReady)
    {
        _isReady = isReady;
    }
    
    private bool IsUnitAbilityActive()
    {
        return _unitList.GetActiveUnit().GetComponent<Unit>().IsAbilityActive() && _unitList.GetActiveUnit().GetComponent<Unit>().GetStatistics().team == abilityTeam;
    }

    private bool IsAbilityActivated()
    {
        return _unitList.GetActiveUnit().GetComponent<Unit>().IsAbilityActivated();
    }

    private int GetRemainingAbilityCD()
    {
        return _unitList.GetActiveUnit().GetComponent<Unit>().GetUnitAbility().GetRemainingAbilityCD();
    }

    public void HandleAbility()
    {
        if (IsUnitAbilityActive() && !IsAbilityActivated())
        {
            ActivateAbility();
        } else if (IsUnitAbilityActive() && IsAbilityActivated())
        {
            DeactivateAbility();
        }
    }

    private void ActivateAbility()
    {
        _unitList.GetActiveUnit().GetComponent<Unit>().ToggleAbility();
    }
    
    private void DeactivateAbility()
    {
        _unitList.GetActiveUnit().GetComponent<Unit>().DeactivateAbility();
    }
}

using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Image _image;
    private UnitList _unitList;
    private bool _isReady;
    private Text _buttonText;
    public Sprite readyButtonSprite;
    public Sprite notReadyButtonSprite;
    void Start()
    {
        LoadSprite();
        LoadUnitList();
        LoadButtonText();
    }

    void Update()
    {
        if (Turn.GetCurrentTurnType() == TurnType.RegularGame)
        { 
            SetIsReady(IsUnitAbilityActive());
            ReloadSprite();
        }

    }
    
    private void LoadUnitList()
    {
        _unitList = GameObject.Find("UnitList").GetComponent<UnitList>();
    }

    private void LoadSprite()
    {
        _image = gameObject.GetComponent<Image>();
        //_sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void LoadButtonText()
    {
        _buttonText = gameObject.GetComponentInChildren<Text>();
    }

    private void ReloadSprite()
    {
        if (_isReady)
        {
            //_sprite.sprite = readyButtonSprite;
            _image.sprite = readyButtonSprite;
        }
        else
        {
            _image.sprite = notReadyButtonSprite;
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
        return _unitList.GetActiveUnit().GetComponent<Unit>().IsAbilityActive();
    }

    private int GetRemainingAbilityCD()
    {
        return _unitList.GetActiveUnit().GetComponent<Unit>().GetUnitAbility().GetRemainingAbilityCD();

    }

    public void HandleAbility()
    {
        Debug.Log("Handle ability 1");
        if (IsUnitAbilityActive())
        {
            Debug.Log("Handle ability 2");
            ActivateAbility();
        }
    }

    private void ActivateAbility()
    {
        _unitList.GetActiveUnit().GetComponent<Unit>().ToggleAbility();

    }
}

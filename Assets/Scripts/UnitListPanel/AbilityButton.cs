using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private UnitList _unitList;
    private bool _isReady;
    public Sprite readyButtonSprite;
    public Sprite notReadyButtonSprite;
    void Start()
    {
        LoadSprite();
        LoadUnitList();
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
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    private void ReloadSprite()
    {
        if (_isReady)
        {
            _sprite.sprite = readyButtonSprite;
        }
        else
        {
            _sprite.sprite = notReadyButtonSprite;
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

    public void HandleAbility()
    {
        if (IsUnitAbilityActive())
        {
           _unitList.GetActiveUnit().GetComponent<Unit>().ToggleAbility();
        }
    }
}

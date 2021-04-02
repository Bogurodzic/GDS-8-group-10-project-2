using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSprite : MonoBehaviour
{
    private Image _sprite;
    void Start()
    {
        LoadSprite();
    }

    private void LoadSprite()
    {
        _sprite = gameObject.GetComponent<Image>();
        _sprite.color = Color.clear;
    }

    public void RenderSprite(UnitData unitData)
    {
        _sprite.color = Color.white;
        if (Turn.GetUnitTurn() == 1)
        {
            _sprite.sprite = unitData.unitPickerSpriteOverviewTeam1;
        }
        else
        {
            _sprite.sprite = unitData.unitPickerSpriteOverviewTeam2;
        }
    }

    public void ResetSprite()
    {
        _sprite.sprite = null;
        _sprite.color = Color.clear;
    }
}

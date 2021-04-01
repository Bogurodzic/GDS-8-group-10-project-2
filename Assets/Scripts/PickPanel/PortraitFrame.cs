using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitFrame : MonoBehaviour
{

    public Sprite inactiveSprite;
    public Sprite activeSprite;
    public Sprite higlitedSprite;
    public bool active = false;
    private Image _sprite;
    void Start()
    {
        LoadSprite();
        ReloadSprite();
    }

    void Update()
    {
        
    }

    private void LoadSprite()
    {
        _sprite = gameObject.GetComponent<Image>();
    }

    public void Hover()
    {
        if (_sprite)
        {
            if (!active)
            {
                _sprite.sprite = higlitedSprite;
            }
        }
    }

    public void HoverOut()
    {
        ReloadSprite();
    }

    private void ReloadSprite()
    {
        if (_sprite)
        {
            if (active)
            {
                _sprite.sprite = activeSprite;
            }
            else
            {
                _sprite.sprite = inactiveSprite;
            } 
        }
    }

    public void SetActive(bool isActive)
    {
        active = isActive;
        ReloadSprite();
    }
}

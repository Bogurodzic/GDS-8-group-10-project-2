using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitFrame : MonoBehaviour
{

    public Sprite inactiveSprite;
    public Sprite activeSprite;
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

    private void ReloadSprite()
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

    public void SetActive(bool isActive)
    {
        active = isActive;
        ReloadSprite();
    }
}

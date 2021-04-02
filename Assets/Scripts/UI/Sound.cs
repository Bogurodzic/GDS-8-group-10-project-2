using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    public Sprite normalButton;
    public Sprite hoverButton;
    public Sprite activeButton;
    
    private Button _button;
    private Image _image;
    private bool _isSoundMuted;
    
    void Start()
    {
        LoadButton();
        LoadImage();
    }

    void Update()
    {
        ReloadSprite();
    }

    private void LoadButton()
    {
        _button = gameObject.GetComponent<Button>();
    }

    private void LoadImage()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void ToggleSound()
    {
        if (MusicManager.Instance.IsEnabled())
        {
            MusicManager.Instance.Disable();
        }
        else
        {
            MusicManager.Instance.Enable();
        }
        ReloadSprite();
    }

    private void ReloadSprite()
    {
        SpriteState spriteState = new SpriteState();
        spriteState = _button.spriteState;

        if (!MusicManager.Instance.IsEnabled())
        {
            spriteState.pressedSprite = hoverButton;
                spriteState.highlightedSprite = activeButton;

            _image.sprite = activeButton;
        }
        else
        {
            spriteState.pressedSprite = activeButton;
                spriteState.highlightedSprite = hoverButton;

                    _image.sprite = normalButton;
        }
        
        _button.spriteState = spriteState;
    }
}

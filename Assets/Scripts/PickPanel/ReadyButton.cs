using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{

    public Sprite normalButton;
    public Sprite hoverButton;
    public Sprite activeButton;
    private PickPanel _pickPanel;
    private Image _image;
    private Button _button;
    private bool _isReady = false;
    void Start()
    {
        LoadPickPanel();
        LoadImage();
        LoadButton();
        ReloadSprite();
    }
    
    private void LoadPickPanel()
    {
        _pickPanel = GameObject.Find("PickPanel").GetComponentInParent<PickPanel>();
    }

    private void LoadImage()
    {
        _image = gameObject.GetComponent<Image>();
    }

    private void LoadButton()
    {
        _button = gameObject.GetComponent<Button>();
    }

    public void SetReady(bool isReady)
    {
        _isReady = isReady;
        ReloadSprite();
    }

    private void ReloadSprite()
    {
        SpriteState spriteState = new SpriteState();
        spriteState = _button.spriteState;

        if (!_isReady)
        {
            spriteState.pressedSprite = activeButton;
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

    public void ReadyClicked()
    {
        if (_isReady)
        {
            _pickPanel.HandleReadyClicked();
        }
    }
}

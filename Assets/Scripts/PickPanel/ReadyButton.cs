using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{

    public Sprite notReadySprite;
    public Sprite readySprite;
    private PickPanel _pickPanel;
    private Image _image;
    private bool _isReady = false;
    void Start()
    {
        LoadPickPanel();
        LoadImage();
    }

    void Update()
    {
        
    }
    
    private void LoadPickPanel()
    {
        _pickPanel = GameObject.Find("PickPanel").GetComponentInParent<PickPanel>();
    }

    private void LoadImage()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void SetReady(bool isReady)
    {
        _isReady = isReady;
        ReloadSprite();
    }

    private void ReloadSprite()
    {
        if (_isReady)
        {
            _image.sprite = readySprite;
        }
        else
        {
            _image.sprite = notReadySprite;
        }
    }

    public void ReadyClicked()
    {
        _pickPanel.HandleReadyClicked();
    }
}

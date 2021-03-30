using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class Healtbar : MonoBehaviour
{

    public Slider Slider;
    public Text healthText;
    public Text damageText;
    public Vector3 Offset;
    public float sliderSpeed = 10f;

    private Text _healthText;
    private Color Low = new Color(178/255f,34/255f,34/255f);
    private Color High = new Color(107/255f,142/255f,35/255f);
    private float targetProgress;
    private bool _isVisible = false;
    private bool _blockSlider = false;

    private float _previousHealth;
    
    
    public void SetHealth(float health, float maxHealth)
    {
 
        //Slider.gameObject.SetActive(health > 0);
        //Slider.value = health/maxHealth;
        //Slider.maxValue = 1f;
        if (Turn.GetCurrentTurnType() == TurnType.RegularGame)
        {
            BlockSlider();
            SetSliderVisbility(true);
        }
        
        if (health != _previousHealth)
        {
            if (health > _previousHealth)
            {
                float healed = health - _previousHealth;
                ShowHealText(healed);
                _previousHealth = health;
            }
            else
            {
                float dealedDamage = _previousHealth - health;
                ShowDamageText(dealedDamage);
                _previousHealth = health;
            }

        }
        
        targetProgress = health / maxHealth;
        
        Debug.Log(Slider.gameObject.active);
        Debug.Log(health);
        Debug.Log(maxHealth);

        ReloadSliderColor();
        //Slider.fillRect.GetComponentInChildren<Image>().color = Low;
        SetText(health, maxHealth);
    }
    void Start()
    {
        Slider.value = 1f;
        Slider.maxValue = 1f;
        LoadText();
        ReloadSliderColor();
    }

    private void ReloadSliderColor()
    {
        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    private void ReloadHealthbarVisibility()
    {
        if (_isVisible || _blockSlider)
        {
            Slider.gameObject.SetActive(true);
        }
        else
        {
            Slider.gameObject.SetActive(false);

        }
    }

    public void SetSliderVisbility(bool isVisible)
    {

            _isVisible = isVisible;
        
    }

    public void BlockSlider()
    {
        _blockSlider = true;
    }

    private void UnlockSlider()
    {
        _blockSlider = false;
    }

    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);

        if (Slider.value > targetProgress)
        {
            Slider.value = Slider.value - 0.01f * Time.deltaTime * sliderSpeed;
            ReloadSliderColor();
        }
        else
        {
            UnlockSlider();
        }

        ReloadHealthbarVisibility();
    }

    private void LoadText()
    {
        _healthText = gameObject.GetComponentInChildren<Text>();
    }

    private void SetText(float health, float maxHealth)
    {
        healthText.text = health + "/" + maxHealth;
    }

    private void ShowDamageText(float damage)
    {
        damageText.color = Color.red;
        damageText.text = damage + "";
        Invoke("HideDmageText", 2f);
    }
    
    private void ShowHealText(float heal)
    {
        damageText.color = Color.yellow;
        damageText.text = heal + "";
        Invoke("HideDmageText", 2f);
    }

    private void HideDmageText()
    {
        damageText.text = "";
    }

    public void TurnOffHealthBar()
    {
        gameObject.SetActive(false);
    }
}

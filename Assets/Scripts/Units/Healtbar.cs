using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healtbar : MonoBehaviour
{

    public Slider Slider;
    public Text healthText;
    public Vector3 Offset;
    public float sliderSpeed = 10f;

    private Text _healthText;
    private Color Low = new Color(178/255f,34/255f,34/255f);
    private Color High = new Color(107/255f,142/255f,35/255f);
    private float targetProgress;
    
    
    
    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health > 0);
        //Slider.value = health/maxHealth;
        //Slider.maxValue = 1f;
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

    void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);

        if (Slider.value > targetProgress)
        {
            Slider.value = Slider.value - 0.01f * Time.deltaTime * sliderSpeed;
            ReloadSliderColor();
        }
    }

    private void LoadText()
    {
        _healthText = gameObject.GetComponentInChildren<Text>();
    }

    private void SetText(float health, float maxHealth)
    {
        healthText.text = health + "/" + maxHealth;
    }
}

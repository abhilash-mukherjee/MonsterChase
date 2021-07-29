using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    private Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthSlider.maxValue = GameManager.Instance.MaxHealthForLevel;
        healthSlider.minValue = 0;
        healthSlider.value = healthSlider.maxValue;
    }
    private void OnEnable()
    {
        Player.OnDamageDone += ChangeSliderOnDamageDone;
    }
    private void OnDisable()
    {
        Player.OnDamageDone -= ChangeSliderOnDamageDone;
    }

    private void ChangeSliderOnDamageDone(int health)
    {
        healthSlider.value = health;
        
    }
    
}

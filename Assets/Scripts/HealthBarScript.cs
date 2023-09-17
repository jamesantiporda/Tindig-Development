using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider, redSlider;

    private float damageTimer = 0.0f, redSliderCooldown = 0.5f;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        redSlider.maxValue = health;
        redSlider.value = health;
    }

    public void SetHealth(int health)
    {
        damageTimer = 0.0f;
        slider.value = health;
    }

    private void Update()
    {
        damageTimer += Time.deltaTime;

        if(redSlider.value > slider.value && damageTimer >= redSliderCooldown)
        {
            redSlider.value -= 1;
        }
        else if(slider.value > redSlider.value)
        {
            redSlider.value = slider.value;
        }
    }
}

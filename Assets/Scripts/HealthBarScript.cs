using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider, redSlider;

    private float damageTimer = 0.0f, redSliderCooldown = 1.0f;

    private bool regenHealth = false;

    private int comboOfEnemy = 0, comboDamage = 0, lastCombo = 0, lastComboDamage = 0;

    public TMP_Text enemyCombo;

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

        if(health < slider.value)
        {
            comboOfEnemy += 1;
            comboDamage += (int)slider.value - health;
            enemyCombo.text = "" + comboOfEnemy;
            lastCombo = comboOfEnemy;
            lastComboDamage = comboDamage;
        }

        slider.value = health;
    }

    private void Update()
    {
        damageTimer += Time.deltaTime;

        if(redSlider.value > slider.value && damageTimer >= redSliderCooldown)
        {
            redSlider.value -= 5;
        }
        else if(slider.value > redSlider.value)
        {
            redSlider.value = slider.value;
        }

        if(regenHealth && damageTimer >= redSliderCooldown && slider.value < slider.maxValue && comboOfEnemy == 0)
        {
            slider.value += 5;
        }

        if(damageTimer >= redSliderCooldown)
        {
            comboOfEnemy = 0;
            comboDamage = 0;
            enemyCombo.text = "";
        }
    }

    public void SetHealthRegen(bool regen)
    {
        regenHealth = regen;
    }

    public int ReturnCombo()
    {
        return comboOfEnemy;
    }

    public int ReturnLastCombo()
    {
        return lastCombo;
    }

    public int ReturnLastComboDamage()
    {
        return lastComboDamage;
    }

    public float ReturnSliderValue()
    {
        return slider.value;
    }
}

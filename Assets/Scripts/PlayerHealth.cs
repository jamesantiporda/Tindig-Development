using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBarScript healthBar;
    private int health;
    private bool RCT = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
        healthBar.SetMaxHealth(health);
    }

    private void Update()
    {
        if(RCT && (health<1000))
        {
            health = (int) healthBar.ReturnSliderValue();
        }
    }

    public void TakeDamage(int damage)
    {
        if(health >= 0)
        {
            health -= damage / (1 + healthBar.ReturnCombo());
            Debug.Log("DAMAGE: " + damage / (1 + healthBar.ReturnCombo()));
            healthBar.SetHealth(health);
        }
    }

    public int ReturnHealth()
    {
        return health;
    }

    public void ResetHealth()
    {
        health = 1000;
        healthBar.SetHealth(health);
    }

    public void RegenHealth()
    {
        RCT = true;
        healthBar.SetHealthRegen(true);
    }
}

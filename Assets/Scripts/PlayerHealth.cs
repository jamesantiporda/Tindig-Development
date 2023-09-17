using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBarScript healthBar;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 1000;
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
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
}

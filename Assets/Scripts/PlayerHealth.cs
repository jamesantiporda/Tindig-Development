using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBarScript healthBar;
    private int health, attackDamage = 0;
    private bool RCT = false;

    private float damageScaling = 1.0f;

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

    public void TakeDamage(int damage, int sameAttackCounter)
    {
        damageScaling = (float) (1 / (1 + (float) healthBar.ReturnCombo()/2 + ((float) sameAttackCounter) / 3));

        attackDamage = damage / (1 + healthBar.ReturnCombo()/2 + sameAttackCounter / 3);

        if (health >= 0)
        {
            health -= attackDamage;
            Debug.Log("DAMAGE: " + attackDamage);
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

    public int ReturnAttackDamage()
    {
        return attackDamage;
    }

    public float ReturnDamageScaling()
    {
        return damageScaling;
    }
}

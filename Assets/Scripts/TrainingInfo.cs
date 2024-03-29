using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrainingInfo : MonoBehaviour
{
    public TMP_Text dummyHealth, comboDamage, damageScaling, damage, lastCombo, attackType;

    public PlayerHealth health;

    public HealthBarScript healthBar;

    public PlayerCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dummyHealth.text = "Dummy Health: " + health.ReturnHealth();
        comboDamage.text = "Combo Damage: " + healthBar.ReturnLastComboDamage();
        damageScaling.text = "Damage Scaling: " + health.ReturnDamageScaling().ToString("F2") + "x";
        damage.text = "Damage: " + health.ReturnAttackDamage();
        lastCombo.text = "Last Combo: " + healthBar.ReturnLastCombo();
        if(combat.ReturnAttackType() == "")
        {
            attackType.text = "Attack Type: None";
        }
        else
        {
            attackType.text = "Attack Type: " + combat.ReturnAttackType();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandList : MonoBehaviour
{
    public Animator attackPreview;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchAttack(string attack)
    {
        attackPreview.SetTrigger(attack);
    }

    public void ToggleCrouching()
    {
        if(attackPreview.GetBool("Crouch"))
        {
            attackPreview.SetBool("Crouch", false);
        }
        else
        {
            if(attackPreview.GetBool("Jump"))
            {
                attackPreview.SetBool("Jump", false);
            }
            attackPreview.SetBool("Crouch", true);
        }
    }

    public void ToggleJumping()
    {
        if (attackPreview.GetBool("Jump"))
        {
            attackPreview.SetBool("Jump", false);
        }
        else
        {
            if (attackPreview.GetBool("Crouch"))
            {
                attackPreview.SetBool("Crouch", false);
            }
            attackPreview.SetBool("Jump", true);
        }
    }
}

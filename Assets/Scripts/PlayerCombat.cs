using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement movement;

    private bool canAttack = true;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canAttack)
        {
            LightAttack();
        }

        if(Input.GetKeyDown(KeyCode.K) && canAttack)
        {
            MediumAttack();
        }

        if (Input.GetKeyDown(KeyCode.L) && canAttack)
        {
            HeavyAttack();
        }

        if (Input.GetKeyDown(KeyCode.I) && canAttack)
        {
            OverheadAttack();
        }
    }

    void LightAttack()
    {
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("LightAttack");
    }

    void MediumAttack()
    {
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("MediumAttack");
    }

    void HeavyAttack()
    {
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("HeavyAttack");
    }

    void OverheadAttack()
    {
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("OverheadAttack");
    }

    public void SetCanAttack(bool ready)
    {
        canAttack = ready;
    }
}

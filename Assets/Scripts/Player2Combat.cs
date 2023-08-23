using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Combat : MonoBehaviour
{
    public Animator animator;
    private Player2Movement movement;

    private bool canAttack = true;

    void Start()
    {
        movement = GetComponent<Player2Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4) && canAttack)
        {
            LightAttack();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5) && canAttack)
        {
            MediumAttack();
        }

        if (Input.GetKeyDown(KeyCode.Keypad6) && canAttack)
        {
            HeavyAttack();
        }

        if (Input.GetKeyDown(KeyCode.Keypad8) && canAttack)
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

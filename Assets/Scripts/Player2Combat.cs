using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.WSA;

public class Player2Combat : MonoBehaviour
{
    public Animator animator;
    private Player2Movement movement;

    private bool canAttack = true;

    private bool isLauncher = false, isSweep = false, isCrouchAttack = false;
    private int attackDamage = 50;

    private string attackType = "";

    // Accept Input
    private bool acceptInput;

    void Start()
    {
        movement = GetComponent<Player2Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (acceptInput)
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

            if (Input.GetKeyDown(KeyCode.Keypad2) && canAttack)
            {
                Special();
            }
        }
    }

    void LightAttack()
    {
        attackType = "Light";
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("LightAttack");
    }

    void MediumAttack()
    {
        attackType = "Medium";
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("MediumAttack");
    }

    void HeavyAttack()
    {
        attackType = "Heavy";
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("HeavyAttack");
    }

    void OverheadAttack()
    {
        attackType = "Overhead";
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("OverheadAttack");
    }

    void Special()
    {
        attackType = "Special";
        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("Special");
    }

    public void SetCanAttack(bool ready)
    {
        canAttack = ready;
    }

    public bool ReturnCanAttack()
    {
        return canAttack;
    }

    public void SetAttackDamage(int damage)
    {
        attackDamage = damage;
    }

    public int ReturnAttackDamage()
    {
        return attackDamage;
    }

    public void SetIsLauncher(bool type)
    {
        isLauncher = type;
    }

    public void SetIsSweep(bool type)
    {
        isSweep = type;
    }

    public void SetIsCrouchAttack(bool type)
    {
        isCrouchAttack = type;
    }

    public bool ReturnIsLauncher()
    {
        return isLauncher;
    }

    public bool ReturnIsSweep()
    {
        return isSweep;
    }

    public bool ReturnIsCrouchAttack()
    {
        return isCrouchAttack;
    }

    public string ReturnAttackType()
    {
        return attackType;
    }

    public void ResetAttackType()
    {
        attackType = "";
    }

    public void AcceptInput()
    {
        acceptInput = true;
    }

    public void DenyInput()
    {
        acceptInput = false;
    }
}

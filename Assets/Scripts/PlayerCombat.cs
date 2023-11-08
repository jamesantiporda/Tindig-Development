using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public KeyCode lightInput = KeyCode.J;
    public KeyCode mediumInput = KeyCode.K;
    public KeyCode heavyInput = KeyCode.L;
    public KeyCode overheadInput = KeyCode.I;
    public KeyCode specialInput = KeyCode.M;

    public Animator animator;
    private PlayerMovement movement;

    private bool canAttack = true;

    private bool isLauncher = false, isSweep = false, isCrouchAttack = false;
    private int attackDamage = 50;

    private string attackType = "";
    private string previousAttack = "";
    private int sameAttackCounter = 0;

    // Accept Input
    private bool acceptInput;

    // CPU AI
    public bool isCPU = false;
    private int randomInt;
    private bool isAttacking = false;
    private bool punishLowBlock = false;

    private float waitMin, waitMax;

    public bool easy = false, medium = true, hard = false, mahoraga = false;
    //

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        if (easy)
        {
            waitMin = 3;
            waitMax = 5;
        }
        else if (medium)
        {
            waitMin = 2;
            waitMax = 4;
        }
        else if (hard)
        {
            waitMin = 1;
            waitMax = 2;
        }
        else if (mahoraga)
        {
            waitMin = 0;
            waitMax = 1.25f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (acceptInput)
        {
            if (Input.GetKeyDown(lightInput) && canAttack)
            {
                LightAttack();
            }

            if (Input.GetKeyDown(mediumInput) && canAttack)
            {
                MediumAttack();
            }

            if (Input.GetKeyDown(heavyInput) && canAttack)
            {
                HeavyAttack();
            }

            if (Input.GetKeyDown(overheadInput) && canAttack)
            {
                OverheadAttack();
            }

            if (Input.GetKeyDown(specialInput) && canAttack)
            {
                Special();
            }

            // AI Behavior
            if (punishLowBlock)
            {
                OverheadAttack();
                punishLowBlock = false;
            }

            if (isCPU && movement.ReturnWithinAttackRange() && !isAttacking)
            {
                StartCoroutine(AttackPick());
            }
        }
    }

    void LightAttack()
    {
        previousAttack = attackType;
        attackType = "Light";

        if(previousAttack == attackType)
        {
            sameAttackCounter += 1;
        }
        else
        {
            sameAttackCounter = 0;
        }

        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("LightAttack");
    }

    void MediumAttack()
    {
        previousAttack = attackType;
        attackType = "Medium";

        if (previousAttack == attackType)
        {
            sameAttackCounter += 1;
        }
        else
        {
            sameAttackCounter = 0;
        }

        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("MediumAttack");
    }

    void HeavyAttack()
    {
        previousAttack = attackType;
        attackType = "Heavy";

        if (previousAttack == attackType)
        {
            sameAttackCounter += 1;
        }
        else
        {
            sameAttackCounter = 0;
        }

        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("HeavyAttack");
    }

    void OverheadAttack()
    {
        previousAttack = attackType;
        attackType = "Overhead";

        if (previousAttack == attackType)
        {
            sameAttackCounter += 1;
        }
        else
        {
            sameAttackCounter = 0;
        }

        movement.changeMoveState(false);
        canAttack = false;
        animator.SetTrigger("OverheadAttack");
    }

    void Special()
    {
        previousAttack = attackType;
        attackType = "Special";

        if (previousAttack == attackType)
        {
            sameAttackCounter += 1;
        }
        else
        {
            sameAttackCounter = 0;
        }

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

    public int ReturnSameAttackCounter()
    {
        return sameAttackCounter;
    }

    public void ResetAttackType()
    {
        attackType = attackType;
    }
    public void AcceptInput()
    {
        acceptInput = true;
    }

    public void DenyInput()
    {
        acceptInput = false;
    }

    private int ReturnRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    private float ReturnRandomFloat(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public void PunishLowBlock()
    {
        punishLowBlock = true;
    }

    IEnumerator AttackPick()
    {
        isAttacking = true;

        float attackWait = UnityEngine.Random.Range(waitMin, waitMax);

        randomInt = ReturnRandomInt(0, 4);

        if (randomInt == 0)
        {
            LightAttack();
        }
        else if (randomInt == 1)
        {
            MediumAttack();
        }
        else if (randomInt == 2)
        {
            HeavyAttack();
        }
        else if (randomInt == 3)
        {
            OverheadAttack();
        }

        yield return new WaitForSeconds(attackWait);

        isAttacking = false;
    }
}

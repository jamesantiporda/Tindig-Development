using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int playerNumber;

    public KeyCode lightInput = KeyCode.J;
    public KeyCode mediumInput = KeyCode.K;
    public KeyCode heavyInput = KeyCode.L;
    public KeyCode overheadInput = KeyCode.I;
    public KeyCode specialInput = KeyCode.M;

    public bool isMC = false;

    // Style Switching Input
    public KeyCode mcInput = KeyCode.W;
    public KeyCode boxingInput = KeyCode.D;
    public KeyCode sikaranInput = KeyCode.S;
    public KeyCode arnisInput = KeyCode.A;
    public KeyCode shiftStyleInput = KeyCode.LeftShift;

    // Styles
    public GameObject mcSprite, boxingSprite, sikaranSprite, arnisSprite;
    public GameObject mcHitPoint, boxingHitPoint, sikaranHitPoint, arnisHitPoint;
    private SpriteRenderer mcRenderer, boxingRenderer, sikaranRenderer, arnisRenderer;
    private GameObject currentSprite;
    private Vector3 mcSpriteSize, boxingSpriteSize, sikaranSpriteSize, arnisSpriteSize;
    private bool canSwitch = true, boxingUnlocked, sikaranUnlocked, arnisUnlocked;

    public Animator animator;
    public PlayerCombat player2Combat;
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

        mcRenderer = mcSprite.GetComponent<SpriteRenderer>();
        currentSprite = mcSprite;
        boxingRenderer = boxingSprite.GetComponent<SpriteRenderer>();
        sikaranRenderer = sikaranSprite.GetComponent<SpriteRenderer>();
        arnisRenderer = arnisSprite.GetComponent<SpriteRenderer>();

        mcSpriteSize = mcSprite.transform.localScale;
        boxingSpriteSize = boxingSprite.transform.localScale;
        sikaranSpriteSize = sikaranSprite.transform.localScale;
        arnisSpriteSize = arnisSprite.transform.localScale;

        if (isMC)
        {
            mcRenderer.enabled = true;
            boxingRenderer.enabled = false;
            sikaranRenderer.enabled = false;
            arnisRenderer.enabled = false;

            EnableSprite(mcSprite);
            DisableSprite(boxingSprite);
            DisableSprite(sikaranSprite);
            DisableSprite(arnisSprite);
        }

        if (easy)
        {
            waitMin = 3;
            waitMax = 5;
        }
        else if (medium)
        {
            waitMin = 1;
            waitMax = 4;
        }
        else if (hard)
        {
            waitMin = 0;
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

            if(Input.GetKey(shiftStyleInput) && isMC && canAttack && canSwitch)
            {
                if(Input.GetKeyDown(mcInput) && currentSprite != mcSprite)
                {
                    StartCoroutine(StyleSwitchCooldown());

                    //shift into mc
                    Debug.Log("MC");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    DisableSprite(currentSprite);
                    currentSprite = mcSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(mcHitPoint);
                }

                if(Input.GetKeyDown(boxingInput) && currentSprite != boxingSprite)
                {
                    StartCoroutine(StyleSwitchCooldown());

                    //shift into boxer
                    Debug.Log("Boxer");
                    DisableSprite(currentSprite);
                    currentSprite = boxingSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(boxingHitPoint);
                }

                if(Input.GetKeyDown(sikaranInput) && currentSprite != sikaranSprite)
                {
                    StartCoroutine(StyleSwitchCooldown());

                    //shift into sikaran
                    Debug.Log("Sikaran");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    DisableSprite(currentSprite);
                    currentSprite = sikaranSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(sikaranHitPoint);
                }

                if(Input.GetKeyDown(arnisInput) && currentSprite != arnisSprite)
                {
                    StartCoroutine(StyleSwitchCooldown());

                    //shift into arnis
                    Debug.Log("Arnis");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    DisableSprite(currentSprite);
                    currentSprite = arnisSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(arnisHitPoint);
                }
            }

            // AI Behavior
            if (isCPU && punishLowBlock && canAttack)
            {
                OverheadAttack();
                punishLowBlock = false;
            }

            if (isCPU && movement.ReturnWithinAttackRange() && !isAttacking && canAttack)
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

    public void ChangeHitPoint(GameObject hitPoint)
    {
        currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(hitPoint);
    }

    public void Die()
    {
        currentSprite.GetComponent<Animator>().SetTrigger("Die");
        currentSprite.GetComponent<Animator>().SetBool("Dead", true);
    }

    public void Reset()
    {
        currentSprite.GetComponent<Animator>().SetBool("Dead", false);
        currentSprite.GetComponent<Animator>().SetTrigger("Reset");

    }

    private void DisableSprite(GameObject spriteDisabled)
    {
        spriteDisabled.GetComponent<SpriteToPlayer>().SetIFrameOn();
        spriteDisabled.GetComponent<SpriteRenderer>().enabled = false;
        spriteDisabled.GetComponent<Animator>().enabled = false;
    }

    private void EnableSprite(GameObject spriteEnabled)
    {
        spriteEnabled.GetComponent<SpriteToPlayer>().SetIFrameOff();
        spriteEnabled.GetComponent<SpriteRenderer>().enabled = true;
        spriteEnabled.GetComponent<Animator>().enabled = true;
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

    private IEnumerator StyleSwitchCooldown()
    {
        canSwitch = false;

        //FindObjectOfType<HitStop>().Stop(0.5f);
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 1.0f;

        yield return new WaitForSeconds(0.5f);

        canSwitch = true;
    }
}

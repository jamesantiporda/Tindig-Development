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

            mcSprite.GetComponent<BoxCollider2D>().enabled = true;
            boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
            boxingSprite.GetComponent<BoxCollider2D>().enabled = false;
            sikaranSprite.GetComponent<BoxCollider2D>().enabled = false;
            arnisSprite.GetComponent<BoxCollider2D>().enabled = false;
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

            if(Input.GetKey(shiftStyleInput) && isMC)
            {
                if(Input.GetKeyDown(mcInput))
                {
                    //shift into mc
                    Debug.Log("MC");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    currentSprite.GetComponent<SpriteRenderer>().enabled = false;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = false;
                    currentSprite = mcSprite;
                    currentSprite.GetComponent<SpriteRenderer>().enabled = true;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = true;
                    currentSprite.transform.localScale = mcSpriteSize;
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(mcHitPoint);
                }

                if(Input.GetKeyDown(boxingInput))
                {
                    //shift into boxer
                    Debug.Log("Boxer");
                    currentSprite.GetComponent<SpriteRenderer>().enabled = false;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = false;
                    currentSprite = boxingSprite;
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOff();
                    currentSprite.GetComponent<SpriteRenderer>().enabled = true;
                    currentSprite.transform.localScale = boxingSpriteSize;
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(boxingHitPoint);
                }

                if(Input.GetKeyDown(sikaranInput))
                {
                    //shift into sikaran
                    Debug.Log("Sikaran");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    currentSprite.GetComponent<SpriteRenderer>().enabled = false;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = false;
                    currentSprite = sikaranSprite;
                    currentSprite.GetComponent<SpriteRenderer>().enabled = true;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = true;
                    currentSprite.transform.localScale = sikaranSpriteSize;
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    player2Combat.ChangeHitPoint(sikaranHitPoint);
                }

                if(Input.GetKeyDown(arnisInput))
                {
                    //shift into arnis
                    Debug.Log("Arnis");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    currentSprite.GetComponent<SpriteRenderer>().enabled = false;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = false;
                    currentSprite = arnisSprite;
                    currentSprite.GetComponent<SpriteRenderer>().enabled = true;
                    currentSprite.GetComponent<BoxCollider2D>().enabled = true;
                    arnisSprite.transform.localScale = new Vector3(1.84f, 1.84f, 1.84f);
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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    public bool isFinalBoss = false;

    // Style Switching Input
    public KeyCode mcInput = KeyCode.W;
    public KeyCode boxingInput = KeyCode.D;
    public KeyCode sikaranInput = KeyCode.S;
    public KeyCode arnisInput = KeyCode.A;
    public KeyCode shiftStyleInput = KeyCode.LeftShift;

    // Styles
    public GameObject mcSprite, boxingSprite, sikaranSprite, arnisSprite;
    public GameObject mcHitPoint, boxingHitPoint, sikaranHitPoint, arnisHitPoint;
    private GameObject[] styleHitPoints = new GameObject[4];
    private GameObject activeHitPoint;
    private SpriteRenderer mcRenderer, boxingRenderer, sikaranRenderer, arnisRenderer;
    private GameObject currentSprite;
    private Vector3 mcSpriteSize, boxingSpriteSize, sikaranSpriteSize, arnisSpriteSize;
    private bool canSwitch = true;
    private bool boxingUnlocked = false, sikaranUnlocked = false, arnisUnlocked = false;
    public GameObject styleIcons;

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
    public bool isTutorial = false;

    private float waitMin, waitMax;

    public bool easy = false, medium = true, hard = false, mahoraga = false;

    private int difficulty;

    private bool cpuStyleSwitchOnCooldown = false;

    private int cpuStyleSwitchInput = 0;

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

        if (isMC || isFinalBoss)
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

        difficulty = PlayerPrefs.GetInt("Difficulty");

        if (difficulty == 0)
        {
            waitMin = 3;
            waitMax = 5;
        }
        else if (difficulty == 1)
        {
            waitMin = 1;
            waitMax = 4;
        }
        else if (difficulty == 2)
        {
            waitMin = 0;
            waitMax = 2;
        }
        else if (difficulty == 3)
        {
            waitMin = 0;
            waitMax = 1.25f;
        }

        if(PlayerPrefs.HasKey("Boxing") && isMC)
        {
            boxingUnlocked = true;
            styleIcons.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            boxingUnlocked = false;
        }
        
        if(PlayerPrefs.HasKey("Sikaran") && isMC)
        {
            sikaranUnlocked = true;
            styleIcons.transform.GetChild(2).GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            sikaranUnlocked = false;
        }

        if (PlayerPrefs.HasKey("Arnis") && isMC)
        {
            styleIcons.transform.GetChild(3).GetComponent<SpriteRenderer>().color = Color.white;
            arnisUnlocked = true;
        }
        else
        {
            arnisUnlocked = false;
        }

        if (isFinalBoss)
        {
            boxingUnlocked = true;
            sikaranUnlocked = true;
            arnisUnlocked = true;
        }

        styleHitPoints[0] = mcHitPoint;
        styleHitPoints[1] = boxingHitPoint;
        styleHitPoints[2] = sikaranHitPoint;
        styleHitPoints[3] = arnisHitPoint;

        activeHitPoint = styleHitPoints[0];
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

            if((Input.GetKey(shiftStyleInput) && isMC && canAttack && canSwitch) || isCPU && cpuStyleSwitchInput > 0)
            {
                if(!isFinalBoss)
                {
                    styleIcons.SetActive(true);
                }

                if((Input.GetKeyDown(mcInput) && currentSprite != mcSprite))
                {
                    if(isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into mc
                    Debug.Log("MC");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    DisableSprite(currentSprite);
                    currentSprite = mcSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    activeHitPoint = mcHitPoint;
                    player2Combat.ChangeHitPoint(mcHitPoint);
                    currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(player2Combat.ReturnActiveHitPoint());
                }

                if((Input.GetKeyDown(boxingInput) && currentSprite != boxingSprite && boxingUnlocked) || isCPU && cpuStyleSwitchInput == 1 && currentSprite != boxingSprite)
                {
                    if (isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into boxer
                    Debug.Log("Boxer");
                    DisableSprite(currentSprite);
                    currentSprite = boxingSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    activeHitPoint = boxingHitPoint;
                    player2Combat.ChangeHitPoint(boxingHitPoint);
                    currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(player2Combat.ReturnActiveHitPoint());
                }

                if((Input.GetKeyDown(sikaranInput) && currentSprite != sikaranSprite && sikaranUnlocked) || isCPU && cpuStyleSwitchInput == 2 && currentSprite != sikaranSprite)
                {
                    if (isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into sikaran
                    Debug.Log("Sikaran");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    DisableSprite(currentSprite);
                    currentSprite = sikaranSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    activeHitPoint = sikaranHitPoint;
                    player2Combat.ChangeHitPoint(sikaranHitPoint);
                    currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(player2Combat.ReturnActiveHitPoint());
                }

                if((Input.GetKeyDown(arnisInput) && currentSprite != arnisSprite && arnisUnlocked) || isCPU && cpuStyleSwitchInput == 3 && currentSprite != arnisSprite)
                {
                    if (isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into arnis
                    Debug.Log("Arnis");
                    boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    DisableSprite(currentSprite);
                    currentSprite = arnisSprite;
                    EnableSprite(currentSprite);
                    animator = currentSprite.GetComponent<Animator>();
                    movement.ChangeStyle(currentSprite);
                    activeHitPoint = arnisHitPoint;
                    player2Combat.ChangeHitPoint(arnisHitPoint);
                    currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(player2Combat.ReturnActiveHitPoint());
                }
            }
            else
            {
                styleIcons.SetActive(false);
            }

            // AI Behavior
            if (isCPU && punishLowBlock && canAttack && !isTutorial)
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
        Debug.Log("Player: " + playerNumber);
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

    public GameObject ReturnCurrentSprite()
    {
        return currentSprite;
    }

    public GameObject ReturnActiveHitPoint()
    {
        return activeHitPoint;
    }

    public void SetIsCPU(bool makeCPU)
    {
        isCPU = makeCPU;
    }

    IEnumerator AttackPick()
    {
        if(!cpuStyleSwitchOnCooldown && isFinalBoss)
        {
            cpuStyleSwitchInput = ReturnRandomInt(0, 4);
        }

        isAttacking = true;

        float attackWait = UnityEngine.Random.Range(waitMin, waitMax);

        randomInt = ReturnRandomInt(0, 5);

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
        else if (randomInt == 4)
        {
            Special();
        }

        yield return new WaitForSeconds(attackWait);

        isAttacking = false;
    }

    private IEnumerator CPUStyleSwitchCooldown()
    {
        cpuStyleSwitchOnCooldown = true;

        yield return new WaitForSeconds(5.0f);

        cpuStyleSwitchOnCooldown = false;
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

    public void SetSprite(GameObject newSprite, GameObject newHitPoint)
    {
        //Set Sprite
        Debug.Log("Setting Sprite");
        currentSprite = newSprite;
        animator = currentSprite.GetComponent<Animator>();
        activeHitPoint = newHitPoint;
        player2Combat.ChangeHitPoint(activeHitPoint);
        currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(player2Combat.ReturnActiveHitPoint());
    }
}

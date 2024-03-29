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
    public GameObject mcSprite;
    public GameObject mcHitPoint;
    private GameObject activeHitPoint;
    private GameObject currentSprite;
    private bool canSwitch = true;
    private bool boxingUnlocked = false, sikaranUnlocked = false, arnisUnlocked = false;
    public GameObject styleIcons;

    // Styles Version 2
    public RuntimeAnimatorController defaultController, boxingController, sikaranController, arnisController;

    public Animator animator;
    public PlayerCombat player2Combat;
    private PlayerMovement movement;

    private bool canAttack = true;

    private bool isLauncher = false, isSweep = false, isCrouchAttack = false;
    private int attackDamage = 50;

    private string attackType = "";
    private string previousAttack = "";
    private int sameAttackCounter = 0;

    private int volleyballAmmo = 1;
    private float volleyballTimer = 0.0f;


    // Accept Input
    private bool acceptInput;
    private bool canLight = true, canMedium = true, canHeavy = true, canOverhead = true, canSpecial = true;

    // CPU AI
    public bool isCPU = false;
    public bool isLightGrunt = false, isMediumGrunt = false, isHeavyGrunt = false;
    private int randomInt;
    private bool isAttacking = false;
    private bool punishLowBlock = false;
    public bool isTutorial = false;

    private float waitMin, waitMax;

    public bool easy = false, medium = true, hard = false, mahoraga = false;

    private int difficulty;

    private bool cpuStyleSwitchOnCooldown = false;

    private int cpuStyleSwitchInput = 0;

    AudioManager audioManager;

    // DEBUGGING
    public bool unlockAllStyles = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();

        currentSprite = mcSprite;

        if (isMC || isFinalBoss)
        {

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

        if (isFinalBoss || unlockAllStyles)
        {
            boxingUnlocked = true;
            sikaranUnlocked = true;
            arnisUnlocked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(volleyballAmmo <= 0)
        {
            volleyballTimer += Time.deltaTime;
        }

        if (acceptInput)
        {
            if (Input.GetKeyDown(lightInput) && canAttack && canLight)
            {
                LightAttack();
            }

            if (Input.GetKeyDown(mediumInput) && canAttack && canMedium)
            {
                MediumAttack();
            }

            if (Input.GetKeyDown(heavyInput) && canAttack && canHeavy)
            {
                HeavyAttack();
            }

            if (Input.GetKeyDown(overheadInput) && canAttack && canOverhead)
            {
                if(isMC && volleyballAmmo <= 0 && animator.runtimeAnimatorController == defaultController)
                {
                    
                }
                else
                {
                    OverheadAttack();
                }
            }

            if (Input.GetKeyDown(specialInput) && canAttack && canSpecial)
            {
                Special();
            }

            if((Input.GetKey(shiftStyleInput) && isMC && canAttack && canSwitch) || isCPU && cpuStyleSwitchInput > 0)
            {
                if(!isFinalBoss)
                {
                    if(!styleIcons.activeSelf)
                    {
                        audioManager.PlayAudioClip(audioManager.menuOpen);
                    }
                    styleIcons.SetActive(true);
                }

                if((Input.GetKeyDown(mcInput) && animator.runtimeAnimatorController != defaultController))
                {
                    if(isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into mc
                    Debug.Log("MC");
                    //boxingSprite.GetComponent<SpriteToPlayer>().SetIFrameOn();
                    //DisableSprite(currentSprite);
                    //currentSprite = mcSprite;
                    //EnableSprite(currentSprite);
                    animator.runtimeAnimatorController = defaultController;
                    currentSprite.GetComponent<SpriteRenderer>().flipX = false;
                    //movement.ChangeStyle(currentSprite);
                    //activeHitPoint = mcHitPoint;
                    //player2Combat.ChangeHitPoint(mcHitPoint);
                    //currentSprite.GetComponent<SpriteToPlayer>().ChangeHitPoint(player2Combat.ReturnActiveHitPoint());
                }

                if((Input.GetKeyDown(boxingInput) && animator.runtimeAnimatorController != boxingController && boxingUnlocked) || isCPU && cpuStyleSwitchInput == 1 && animator.runtimeAnimatorController != boxingController)
                {
                    if (isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into boxer
                    Debug.Log("Boxer");
                    animator.runtimeAnimatorController = boxingController;
                    currentSprite.GetComponent<SpriteRenderer>().flipX = false;
                }

                if((Input.GetKeyDown(sikaranInput) && animator.runtimeAnimatorController != sikaranController && sikaranUnlocked) || isCPU && cpuStyleSwitchInput == 2 && animator.runtimeAnimatorController != sikaranController)
                {
                    if (isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into sikaran
                    Debug.Log("Sikaran");
                    animator.runtimeAnimatorController = sikaranController;
                    currentSprite.GetComponent<SpriteRenderer>().flipX = true;
                }

                if((Input.GetKeyDown(arnisInput) && animator.runtimeAnimatorController != arnisController && arnisUnlocked) || isCPU && cpuStyleSwitchInput == 3 && animator.runtimeAnimatorController != arnisController)
                {
                    if (isCPU)
                    {
                        StartCoroutine(CPUStyleSwitchCooldown());
                        cpuStyleSwitchInput = 0;
                    }

                    StartCoroutine(StyleSwitchCooldown());

                    //shift into arnis
                    Debug.Log("Arnis");
                    animator.runtimeAnimatorController = arnisController;
                    currentSprite.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            else
            {
                if(styleIcons.activeSelf)
                {
                    audioManager.PlayAudioClip(audioManager.menuClose);
                }
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

        if(isMC && animator.runtimeAnimatorController == defaultController)
        {
            volleyballAmmo -= 1;
            volleyballTimer = 0.0f;
        }
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

    public int ReturnVolleyballAmmo()
    {
        return volleyballAmmo;
    }

    public void AddVolleyball()
    {
        volleyballAmmo += 1;
    }

    public bool ReturnIsMC()
    {
        return isMC;
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
        volleyballAmmo = 1;

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


        //if(isLightGrunt)
        //{
        //    randomInt = 0;
        //}
        //else if (isMediumGrunt)
        //{
        //    randomInt = 1;
        //}
        //else if (isHeavyGrunt)
        //{
        //    randomInt = ReturnRandomInt(2, 5);
        //}

        if (randomInt == 0 && canLight)
        {
            LightAttack();
        }
        else if (randomInt == 1 && canMedium)
        {
            MediumAttack();
        }
        else if (randomInt == 2 && canHeavy)
        {
            HeavyAttack();
        }
        else if (randomInt == 3 && canOverhead)
        {
            OverheadAttack();
        }
        else if (randomInt == 4 && canSpecial)
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

    public void SetDifficulty(int diff)
    {
        if (diff == 0)
        {
            waitMin = 3;
            waitMax = 5;
        }
        else if (diff== 1)
        {
            waitMin = 1;
            waitMax = 4;
        }
        else if (diff == 2)
        {
            waitMin = 0;
            waitMax = 2;
        }
        else if (diff == 3)
        {
            waitMin = 0;
            waitMax = 1.25f;
        }
    }

    public void ToggleCanUseAttack(int attackType, bool unlocked)
    {
        switch(attackType)
        {
            case 0:
                canLight = unlocked;
                break;
            case 1:
                canMedium = unlocked;
                break;
            case 2:
                canHeavy = unlocked;
                break;
            case 3:
                canOverhead = unlocked;
                break;
            case 4:
                canSpecial = unlocked;
                break;
            default:
                Debug.Log("Invalid attack type");
                break;
        }
    }

    public float ReturnVolleyballTimer()
    {
        return volleyballTimer;
    }

    // To be used in tutorial
    public IEnumerator ForceMediumAttack()
    {
        MediumAttack();
        yield return new WaitForSeconds(1.0f);
    }
}

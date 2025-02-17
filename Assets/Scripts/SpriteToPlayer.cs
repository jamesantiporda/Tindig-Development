using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class SpriteToPlayer : MonoBehaviour
{
    public static event Action perfectGuard;

    public int playerNumber = 1;

    public GameObject player;
    public PlayerCombat player2combat;
    public PlayerMovement player2movement;

    public GameObject volleyballPrefab;

    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerHealth playerHealth;
    private Animator anim;
    public GameObject hitEffect;
    public GameObject hitPoint;

    private float launchForce = 15.0f;

    private bool isBlocking = false, isLowBlocking = false, lowBlocked, blocked, iFrame;
    private float blockingTime = 0f, lowBlockingTime = 0f;

    private bool isCountering = false;

    /// CPU AI /
    public bool isCPU = false, isBoxer = false;
    public bool easy = false, medium = true, hard = false, mahoraga = false;
    private int randomInt;
    private bool jumpDeciding;

    private float crouchReactionTime;
    private int maxRandom, numberToPunish;

    private int timesDamaged = 0;

    private int difficulty;

    public bool toggleLight = true, toggleMedium = true, toggleHeavy = true, toggleOverhead = true, toggleSpecial = true;

    // Audio Stuff

    AudioManager audioManager;
    public bool sandGround = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = gameObject.GetComponent<Animator>();

        difficulty = PlayerPrefs.GetInt("Difficulty");

        if (difficulty == 0)
        {
            crouchReactionTime = 4.0f;
            maxRandom = 10;
            numberToPunish = 5;
        }
        else if(difficulty == 1)
        {
            crouchReactionTime = 3.0f;
            maxRandom = 7;
            numberToPunish = 4;
        }
        else if(difficulty == 2)
        {
            crouchReactionTime = 1.5f;
            maxRandom = 5;
            numberToPunish = 3;
        }
        else if(difficulty == 3)
        {
            crouchReactionTime = 1f;
            maxRandom = 3;
            numberToPunish = 3;
        }

        anim.SetBool("isCPU", isCPU);

        combat.ToggleCanUseAttack(0, toggleLight);
        combat.ToggleCanUseAttack(1, toggleMedium);
        combat.ToggleCanUseAttack(2, toggleHeavy);
        combat.ToggleCanUseAttack(3, toggleOverhead);
        combat.ToggleCanUseAttack(4, toggleSpecial);

    }

    // Update is called once per frame
    void Update()
    {
        if(!player2movement.ReturnIsGrounded() && !jumpDeciding)
        {
            StartCoroutine(JumpDeciding());
        }

        if (movement.ReturnIsGrounded())
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }

        if(movement.ReturnIsCrouching() && movement.ReturnDirection() == 4 && combat.ReturnCanAttack())//Crouching + Backward Input + canattack = lowBlock
        {
            isLowBlocking = true;
            isBlocking = false;
        }
        else if (!movement.ReturnIsCrouching() && movement.ReturnDirection() == 4 && anim.GetFloat("Movement") != 0f)//Backward Input + moving = normalBlock
        {
            isBlocking = true;
            isLowBlocking = false;
        }
        else
        {
            isBlocking = false;
            isLowBlocking = false;
        }


        // Timer to see how long the player has been blocking
        if(!movement.ReturnIsCrouching() && movement.ReturnDirection() == 4)
        {
            blockingTime += Time.deltaTime;
        }
        else
        {
            blockingTime = 0f;
        }

        if(movement.ReturnIsCrouching() && movement.ReturnDirection() == 4)
        {
            lowBlockingTime += Time.deltaTime;
        }
        else
        {
            lowBlockingTime = 0f;
        }

        // AI BEHAVIOR
        if(player2movement.ReturnIsCPU() && player2movement.ReturnWithinAttackRange())
        {
            // If normal block for too long, start crouching
            if (blockingTime >= crouchReactionTime)
            {
                player2movement.StartCrouching();
            }
            else
            {
                player2movement.StopCrouching();
            }

            // If low block for too long, punish with overhead
            if(lowBlockingTime >= crouchReactionTime)
            {
                player2combat.PunishLowBlock();
                lowBlockingTime = 0f;
            }
        }
        else
        {
            player2movement.StopCrouching();
        }

        //if(!movement.ReturnIsRetreating() && timesDamaged >= 5)
        //{
         //   timesDamaged = 0;
        //    StartCoroutine(movement.Retreat());
        //}
        //Debug.Log("Blocking time: " + blockingTime);
    }

    public void MakePlayerMoveable()
    {
        movement.changeMoveState(true);
    }

    public void MakePlayerAble()
    {
        combat.SetCanAttack(true);
    }

    public void MakePlayerUnmoveable()
    {
        movement.changeMoveState(false);
    }

    public void MakePlayerUnable()
    {
        combat.SetCanAttack(false);
    }

    public void SetPlayerAttackDamage(int damage)
    {
        combat.SetAttackDamage(damage);
    }

    public void SetAttackIsLauncher(int type)
    {
        if(type == 0)
        {
            combat.SetIsLauncher(false);
        }
        else
        {
            combat.SetIsLauncher(true);
        }
    }

    public void SetAttackIsSweep(int type)
    {
        if (type == 0)
        {
            combat.SetIsSweep(false);
        }
        else
        {
            combat.SetIsSweep(true);
        }
    }

    public void SetAttackIsCrouchAttack(int type)
    {
        if (type == 0)
        {
            combat.SetIsCrouchAttack(false);
        }
        else
        {
            combat.SetIsCrouchAttack(true);
        }
    }

    public void SetLaunchForce(float force)
    {
        launchForce = force;
    }

    public void ResetAttackType()
    {
        combat.ResetAttackType();
    }

    public void Lunge()
    {
        movement.LungeMovement();
    }

    public void BackLunge()
    {
        movement.BackLungeMovement();
    }

    public void SetIFrameOn()
    {
        iFrame = true;
    }

    public void SetIFrameOff()
    {
        iFrame = false;
    }

    public void SetIsCPU(bool makeCPU)
    {
        isCPU = makeCPU;
    }

    public bool ReturnSpriteIsMC()
    {
        return combat.ReturnIsMC();
    }

    private void Damaged()
    {
        player2movement.changeMoveState(true);
        player2combat.SetCanAttack(true);
        playerHealth.TakeDamage(player2combat.ReturnAttackDamage(), player2combat.ReturnSameAttackCounter());
        Instantiate(hitEffect, new Vector3(hitPoint.transform.position.x, player.transform.position.y + hitPoint.transform.localPosition.y, player.transform.position.z), Quaternion.identity);
        switch (player2combat.ReturnAttackType())
        {
            case "Light":
                audioManager.PlayAudioClip(audioManager.lightHit);
                break;
            case "Medium":
                audioManager.PlayAudioClip(audioManager.mediumHit);
                break;
            case "Heavy":
                //FindObjectOfType<HitStop>().Stop(0.1f);
                audioManager.PlayAudioClip(audioManager.heavyHit);
                break;
            case "Overhead":
                audioManager.PlayAudioClip(audioManager.heavyHit);
                break;
            case "Special":
                audioManager.PlayAudioClip(audioManager.mediumHit);
                break;
            default:
                break;
        }
        MakePlayerUnmoveable();
        MakePlayerUnable();

        // Check if attack is a launching attack
        if (player2combat.ReturnIsLauncher())
        {
            movement.Launch(launchForce, 0.0f);
            anim.SetTrigger("Launched");
            //FindObjectOfType<HitStop>().Stop(0.1f);
        }
        else if(player2combat.ReturnIsSweep())
        {
            anim.SetTrigger("Launched");
            //FindObjectOfType<HitStop>().Stop(0.1f);
        }
        else
        {
            if(movement.ReturnIsGrounded())
            {
                if(movement.ReturnIsCrouching())
                {
                    anim.SetTrigger("CrouchHurt");
                }
                else
                {
                    anim.SetTrigger("Hurt");
                }
            }
            else
            {
                movement.Launch(17.5f, 0.2f);
                anim.SetTrigger("Launched");
            }
        }
    }

    private void Blocked()
    {
        //Blocking code
        if(blockingTime <= 0.25)
        {
            audioManager.PlayAudioClip(audioManager.parry);
            perfectGuard?.Invoke();
            MakePlayerAble();
            FindObjectOfType<HitStop>().Stop(0.1f);
        }
        else
        {
            audioManager.PlayAudioClip(audioManager.block);
            playerHealth.TakeDamage(player2combat.ReturnAttackDamage()/10, player2combat.ReturnSameAttackCounter());
        }
        SetIFrameOn();
        anim.SetTrigger("Blocked");
        MakePlayerUnmoveable();
        MakePlayerUnable();
    }

    private void LowBlocked()
    {
        //Blocking code
        if (lowBlockingTime <= 0.25)
        {
            audioManager.PlayAudioClip(audioManager.parry);
            perfectGuard?.Invoke();
            MakePlayerAble();
            FindObjectOfType<HitStop>().Stop(0.1f);
        }
        else
        {
            audioManager.PlayAudioClip(audioManager.block);
            playerHealth.TakeDamage(player2combat.ReturnAttackDamage() / 3, player2combat.ReturnSameAttackCounter());
        }
        SetIFrameOn();
        anim.SetTrigger("LowBlocked");
        MakePlayerUnmoveable();
        MakePlayerUnable();
    }

    private void Counter()
    {
        StartCoroutine(IFramer());
        anim.SetTrigger("CounterHit");
        Debug.Log("Counter!");
    }

    private int ReturnRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    private IEnumerator JumpDeciding()
    {
        jumpDeciding = true;
        float decideTime = UnityEngine.Random.Range(0.0f, 0.1f);

        yield return new WaitForSeconds(decideTime);

        int willJump = UnityEngine.Random.Range(0, maxRandom);

        if (willJump <= 1)
        {
            StartCoroutine(movement.Jump());
        }

        yield return new WaitForSeconds(1.0f);

        jumpDeciding = false;
    }

    public void ChangeHitPoint(GameObject newHitPoint)
    {
        hitPoint = newHitPoint;
    }

    public GameObject ReturnHitPoint()
    {
        return hitPoint;
    }

    public void EnableCountering()
    {
        isCountering = true;
    }

    public void DisableCountering()
    {
        isCountering = false;
    }

    // Iframe
    private IEnumerator IFramer()
    {
        SetIFrameOn();
        yield return new WaitForSeconds(0.25f);
        SetIFrameOff();
    }

    public void TriggerVolleyball()
    {
        var childTransforms = transform.GetComponentsInChildren<Transform>();
        if (childTransforms.Length > 0)
        {
            foreach (var childTransform in childTransforms)
            {
                if (childTransform.name == "Circle")
                {
                    Instantiate(volleyballPrefab, new Vector3(childTransform.position.x, 0.2f, player.transform.position.z), Quaternion.identity);
                }
            }
        }
    }

    // For Audio Control
    public void PlayAudio(int audioID)
    {
        audioManager.PlaySFX(audioID);
    }

    public void PlayFootstep()
    {
        audioManager.PlayFootstep();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerNumber == 1 && collision.gameObject.tag == "Player2Attack") && !iFrame || (playerNumber == 2 && collision.gameObject.tag == "Player1Attack") && !iFrame)
        {
            lowBlocked = isLowBlocking && player2combat.ReturnAttackType() != "Overhead";
            blocked = !player2combat.ReturnIsCrouchAttack() && isBlocking && !isLowBlocking;

            // If Countered
            if(isCountering)
            {
                Counter();
                return;
            }

            //If the GameObject's name matches the one you suggest, output this message in the console
            if (blocked)
            {
                //Debug.Log("Blocked!");
                Blocked();
            }
            else if(lowBlocked)
            {
                //Debug.Log("LowBlocked!");
                LowBlocked();
            }
            else
            {
                //Debug.Log("P1 DAMAGED!");

                // AI BLOCKING
                if (isCPU)
                {
                    randomInt = ReturnRandomInt(0, maxRandom);
                    if (randomInt <= 1 && combat.ReturnCanAttack() && movement.ReturnIsMoveable())
                    {
                        randomInt = ReturnRandomInt(0, 3);
                        if (randomInt == 1 && isBoxer && toggleSpecial)
                        {
                            anim.SetTrigger("Special");
                        }
                        else
                        {
                            if (player2movement.ReturnIsCrouching())
                            {
                                LowBlocked();
                            }
                            else
                            {
                                Blocked();
                            }

                            //randomInt = ReturnRandomInt(0, 3);
                            //if (randomInt <= 1 && isBoxer)
                            //{
                            //    anim.SetTrigger("Counter2");
                            //}
                        }
         
                        return;
                    }

                    if (player2combat.ReturnSameAttackCounter() >= numberToPunish && combat.ReturnCanAttack() && movement.ReturnIsMoveable())
                    {
                        randomInt = ReturnRandomInt(0, 3);
                        if (randomInt == 1 && isBoxer && toggleSpecial)
                        {
                            anim.SetTrigger("Special");
                        }
                        else
                        {
                            if (player2movement.ReturnIsCrouching())
                            {
                                LowBlocked();
                            }
                            else
                            {
                                Blocked();
                            }
                        }

                        StartCoroutine(IFramer());

                        return;
                    }
                }
                Damaged();
                timesDamaged += 1;
            }

            StartCoroutine(IFramer());
        }

        if(collision.gameObject.tag == "Volleyball")
        {
            if (combat.ReturnIsMC() && combat.ReturnVolleyballTimer() >= 1.0f && combat.ReturnVolleyballAmmo() < 1)
            {
                combat.AddVolleyball();
                Destroy(collision.gameObject);
                Debug.Log("picked up ball");
            }
        }
    }
}

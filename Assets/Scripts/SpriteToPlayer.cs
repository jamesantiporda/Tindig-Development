using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class SpriteToPlayer : MonoBehaviour
{
    public int playerNumber = 1;

    public GameObject player;
    public PlayerCombat player2combat;
    public PlayerMovement player2movement;

    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerHealth playerHealth;
    private Animator anim;
    public GameObject hitEffect;
    public GameObject hitPoint;

    private float launchForce = 12.5f;

    private bool isBlocking = false, isLowBlocking = false, lowBlocked, blocked, hit;
    private float blockingTime = 0f, lowBlockingTime = 0f;

    /// CPU AI /
    public bool isCPU = false;
    public bool easy = false, medium = true, hard = false, mahoraga = false;
    private int randomInt;

    private float crouchReactionTime;
    private int maxRandom, numberToPunish;

    private int timesDamaged = 0;

    ////////////

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = gameObject.GetComponent<Animator>();

        if(easy)
        {
            crouchReactionTime = 5.0f;
            maxRandom = 15;
            numberToPunish = 10;
        }
        else if(medium)
        {
            crouchReactionTime = 3.0f;
            maxRandom = 7;
            numberToPunish = 5;
        }
        else if(hard)
        {
            crouchReactionTime = 2f;
            maxRandom = 5;
            numberToPunish = 3;
        }
        else if(mahoraga)
        {
            crouchReactionTime = 1f;
            maxRandom = 3;
            numberToPunish = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        if(anim.GetInteger("Direction") == 4 && !anim.GetBool("Crouching"))
        {
            blockingTime += Time.deltaTime;
        }
        else
        {
            blockingTime = 0f;
        }

        if(anim.GetInteger("Direction") == 4 && anim.GetBool("Crouching"))
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

    private void Damaged()
    {
        player2movement.changeMoveState(true);
        player2combat.SetCanAttack(true);
        playerHealth.TakeDamage(player2combat.ReturnAttackDamage());
        Instantiate(hitEffect, new Vector3(hitPoint.transform.position.x, player.transform.position.y + hitPoint.transform.localPosition.y, player.transform.position.z), Quaternion.identity);
        MakePlayerUnmoveable();
        MakePlayerUnable();

        // Check if attack is a launching attack
        if (player2combat.ReturnIsLauncher())
        {
            movement.Launch(launchForce, 0.20f);
            anim.SetTrigger("Launched");
            FindObjectOfType<HitStop>().Stop(0.1f);
        }
        else if(player2combat.ReturnIsSweep())
        {
            anim.SetTrigger("Launched");
            FindObjectOfType<HitStop>().Stop(0.1f);
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
                movement.Launch(6f, 0.20f);
                anim.SetTrigger("Launched");
            }
        }
    }

    private void Blocked()
    {
        //Blocking code
        anim.SetTrigger("Blocked");
        MakePlayerUnmoveable();
        MakePlayerUnable();
    }

    private void LowBlocked()
    {
        //Blocking code
        anim.SetTrigger("LowBlocked");
        MakePlayerUnmoveable();
        MakePlayerUnable();
    }

    private int ReturnRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerNumber == 1 && collision.gameObject.tag == "Player2Attack") || (playerNumber == 2 && collision.gameObject.tag == "Player1Attack"))
        {
            lowBlocked = isLowBlocking && player2combat.ReturnAttackType() != "Overhead";
            blocked = !player2combat.ReturnIsCrouchAttack() && isBlocking && !isLowBlocking;

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
                    if (randomInt <= 1 && combat.ReturnCanAttack())
                    {
                        randomInt = ReturnRandomInt(0, 3);
                        if (randomInt == 1)
                        {
                            anim.SetTrigger("Counter");
                            anim.SetTrigger("CounterHit");
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

                            randomInt = ReturnRandomInt(0, 3);
                            if (randomInt <= 1)
                            {
                                anim.SetTrigger("Counter2");
                            }
                        }
         
                        return;
                    }

                    if (player2combat.ReturnSameAttackCounter() >= numberToPunish)
                    {
                        randomInt = ReturnRandomInt(0, 3);
                        if (randomInt == 1)
                        {
                            anim.SetTrigger("Counter");
                            anim.SetTrigger("CounterHit");
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

                        return;
                    }
                }
                Damaged();
                timesDamaged += 1;
            }
        }
    }
}

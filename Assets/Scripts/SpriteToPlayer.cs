using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class SpriteToPlayer : MonoBehaviour
{
    public GameObject player;
    public Player2Combat player2combat;
    public Player2Movement player2movement;

    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerHealth playerHealth;
    private Animator anim;
    public GameObject hitEffect;
    public GameObject hitPoint;

    private float launchForce = 12.5f;

    private bool isBlocking = false, isLowBlocking = false, lowBlocked, blocked;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = gameObject.GetComponent<Animator>();
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

        if(movement.ReturnIsCrouching() && movement.ReturnDirection() == 4 && combat.ReturnCanAttack())
        {
            isLowBlocking = true;
            isBlocking = false;
        }
        else if (movement.ReturnDirection() == 4 && anim.GetFloat("Movement") != 0f)
        {
            isBlocking = true;
            isLowBlocking = false;
        }
        else
        {
            isBlocking = false;
            isLowBlocking = false;
        }
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
            FindObjectOfType<HitStop>().Stop(0.3f);
        }
        else if(player2combat.ReturnIsSweep())
        {
            anim.SetTrigger("Launched");
            FindObjectOfType<HitStop>().Stop(0.3f);
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
            FindObjectOfType<HitStop>().Stop(0.1f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player2Attack")
        {
            lowBlocked = isLowBlocking && player2combat.ReturnAttackType() != "Overhead";
            blocked = !player2movement.ReturnIsCrouching() && isBlocking;

            //If the GameObject's name matches the one you suggest, output this message in the console
            if (blocked || lowBlocked)
            {
                if(movement.ReturnIsCrouching())
                {
                    Debug.Log("LowBlocked!");
                    LowBlocked();
                }
                else
                {
                    Debug.Log("Blocked!");
                    Blocked();
                }
            }
            else
            {
                //Debug.Log("P1 DAMAGED!");
                Damaged();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer2 : MonoBehaviour
{
    public GameObject player;

    public PlayerCombat player1combat;
    public PlayerMovement player1movement;

    private Player2Movement movement;
    private Player2Combat combat;
    private PlayerHealth playerHealth;
    private Animator anim;
    public GameObject hitEffect;
    public GameObject hitPoint;

    private float launchForce = 12.5f;

    private bool isBlocking = false, isLowBlocking = false, lowBlocked, blocked;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<Player2Movement>();
        combat = player.GetComponent<Player2Combat>();
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("hitpoint: " + hitPoint.transform.position.x + ", " + hitPoint.transform.position.y);
        if (movement.ReturnIsGrounded())
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }

        if (movement.ReturnIsCrouching() && movement.ReturnDirection() == 4 && combat.ReturnCanAttack())
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
        if (type == 0)
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
        playerHealth.TakeDamage(player1combat.ReturnAttackDamage());
        Instantiate(hitEffect, new Vector3(hitPoint.transform.position.x, player.transform.position.y + hitPoint.transform.localPosition.y, player.transform.position.z), Quaternion.identity);
        MakePlayerUnmoveable();
        MakePlayerUnable();

        // Check if attack is a launching attack
        if (player1combat.ReturnIsLauncher())
        {
            movement.Launch(launchForce, 0.20f);
            anim.SetTrigger("Launched");
            FindObjectOfType<HitStop>().Stop(0.3f);
        }
        else if (player1combat.ReturnIsSweep())
        {
            anim.SetTrigger("Launched");
            FindObjectOfType<HitStop>().Stop(0.3f);
        }
        else
        {
            if (movement.ReturnIsGrounded())
            {
                if (movement.ReturnIsCrouching())
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
        MakePlayerUnmoveable();
        MakePlayerUnable();
        anim.SetTrigger("Blocked");
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
        if (collision.gameObject.tag == "Player1Attack")
        {
            lowBlocked = isLowBlocking && player1combat.ReturnAttackType() != "Overhead";
            blocked = !player1movement.ReturnIsCrouching() && isBlocking;

            //If the GameObject's name matches the one you suggest, output this message in the console
            if (blocked)
            {
                Debug.Log("Blocked!");
                Blocked();
            }
            else if (lowBlocked)
            {
                Debug.Log("LowBlocked!");
                LowBlocked();
            }
            else
            {
                //Debug.Log("P1 DAMAGED!");
                Damaged();
                FindObjectOfType<HitStop>().Stop(0.1f);
            }
        }
    }
}

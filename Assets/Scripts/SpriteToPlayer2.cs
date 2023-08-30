using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer2 : MonoBehaviour
{
    public GameObject player;

    public PlayerCombat player1combat;

    private Player2Movement movement;
    private Player2Combat combat;
    private Animator anim;

    private float launchForce = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<Player2Movement>();
        combat = player.GetComponent<Player2Combat>();
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

    private void Damaged()
    {
        // Check if attack is a launching attack
        if (player1combat.ReturnIsLauncher())
        {
            movement.Launch(launchForce);
            anim.SetTrigger("Launched");
        }
        else if (player1combat.ReturnIsSweep())
        {
            anim.SetTrigger("Launched");
        }
        else
        {
            anim.SetTrigger("Hurt");
        }
        MakePlayerUnmoveable();
        MakePlayerUnable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1Attack")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("P2 DAMAGED!");
            Damaged();
        }
    }
}

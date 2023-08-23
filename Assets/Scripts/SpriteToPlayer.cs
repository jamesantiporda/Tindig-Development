using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer : MonoBehaviour
{
    public GameObject player;

    private PlayerMovement movement;
    private PlayerCombat combat;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void Damaged()
    {
        anim.SetTrigger("Hurt");
        MakePlayerUnmoveable();
        MakePlayerUnable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player2Attack")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("P1 DAMAGED!");
            Damaged();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer2 : MonoBehaviour
{
    public GameObject player;

    private Player2Movement movement;
    private Player2Combat combat;
    private Animator anim;

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
        if (collision.gameObject.tag == "Player1Attack")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("P2 DAMAGED!");
            Damaged();
        }
    }
}

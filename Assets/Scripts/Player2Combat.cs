using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Combat : MonoBehaviour
{
    public Animator animator;
    private Player2Movement movement;

    void Start()
    {
        movement = GetComponent<Player2Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            LightAttack();
        }
    }

    void LightAttack()
    {
        movement.changeMoveState(false);
        animator.SetTrigger("LightAttack");
    }
}

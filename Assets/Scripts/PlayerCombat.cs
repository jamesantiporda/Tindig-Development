using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
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

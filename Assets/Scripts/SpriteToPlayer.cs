using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer : MonoBehaviour
{
    public GameObject player;

    private PlayerMovement movement;
    private PlayerCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
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
}

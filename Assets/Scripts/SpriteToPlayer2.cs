using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer2 : MonoBehaviour
{
    public GameObject player;

    private Player2Movement movement;
    private Player2Combat combat;

    // Start is called before the first frame update
    void Start()
    {
        movement = player.GetComponent<Player2Movement>();
        combat = player.GetComponent<Player2Combat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakePlayerMoveable()
    {
        movement.changeMoveState(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private float centerOfFighters;
    private float centerOfFightersY;
    private float player1Xposition;
    private float player2Xposition;

    // Start is called before the first frame update
    void Start()
    {
        player1Xposition = player1.transform.position.x;
        player2Xposition = player2.transform.position.x;
        centerOfFighters = (player2Xposition - player1Xposition) / 2;

        centerOfFightersY = (player2.transform.position.y + player1.transform.position.y) / 2;

        transform.position = new Vector3(centerOfFighters, 3.0f + centerOfFightersY, -10f);
        //Debug.Log(centerOfFighters);
    }

    // Update is called once per frame
    void Update()
    {
        player1Xposition = player1.transform.position.x;
        player2Xposition = player2.transform.position.x;
        centerOfFighters = (player2Xposition + player1Xposition) / 2;
        centerOfFightersY = (player2.transform.position.y + player1.transform.position.y) / 2;
        transform.position = new Vector3(centerOfFighters, 3.0f + centerOfFightersY, -10f);
        //Debug.Log(centerOfFighters);
    }
}

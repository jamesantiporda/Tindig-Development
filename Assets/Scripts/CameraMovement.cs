using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    private float centerOfFighters;
    private float player1Xposition;
    private float player2Xposition;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player");
        player2 = GameObject.Find("Player2");

        player1Xposition = player1.transform.position.x;
        player2Xposition = player2.transform.position.x;
        centerOfFighters = (player2Xposition - player1Xposition) / 2;

        transform.position = new Vector3(centerOfFighters, 4.3f, -10f);
        Debug.Log(centerOfFighters);
    }

    // Update is called once per frame
    void Update()
    {
        player1Xposition = player1.transform.position.x;
        player2Xposition = player2.transform.position.x;
        centerOfFighters = (player2Xposition + player1Xposition) / 2;
        transform.position = new Vector3(centerOfFighters, 4.3f, -10f);
        Debug.Log(centerOfFighters);
    }
}

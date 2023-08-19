using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Damage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1Attack")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("P2 DAMAGED!");
        }
    }
}

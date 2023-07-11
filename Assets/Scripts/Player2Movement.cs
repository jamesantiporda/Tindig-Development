using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private float horizontalInput;
    private float horizontalMagnitude;

    private bool isGrounded = true;
    private bool isCrouching = false;

    private Rigidbody playerRb;
    public Animator anim;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            speed = 3.5f;
        }
        else
        {
            speed = 5.0f;
        }


        // Get Player Horizontal Input
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1;
        }
        else
        {
            horizontalInput = 0;
        }

        // Move the Player Horizontally
        if (!isCrouching)
        {
            horizontalMagnitude = speed * horizontalInput;
            transform.Translate(Vector3.right * Time.deltaTime * horizontalMagnitude);
        }
        else
        {
            horizontalMagnitude = 0.0f;
        }

        // Jump Input
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && !isCrouching)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Crouch Input
        if (Input.GetKey(KeyCode.DownArrow))
        {
            isCrouching = true;
            Debug.Log("P2 Crouching!");
        }
        else
        {
            isCrouching = false;
        }

        // Debugging
        if (!isCrouching)
        {
            Debug.Log("P2 Not Crouching!");
        }

        // Movement Animations
        anim.SetFloat("Movement", horizontalMagnitude);
        anim.SetBool("Crouching", isCrouching);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

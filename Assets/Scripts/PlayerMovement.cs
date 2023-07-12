using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private float horizontalInput;
    private float horizontalMagnitude;
    private float distanceFromEnemy;

    private bool isGrounded = true;
    private bool isCrouching = false;
    private bool isFacingRight = true;

    private Rigidbody playerRb;
    public Animator anim;

    public GameObject enemy;
    public GameObject sprite;

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

        distanceFromEnemy = transform.position.x - enemy.transform.position.x;

        if (distanceFromEnemy <= 0) 
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }

        if (isFacingRight )
        {
            sprite.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
        }
        else
        {
            sprite.transform.localScale = new Vector3(-3.3f, 3.3f, 3.3f);
        }

        // Get Player Horizontal Input
        if (Input.GetKey(KeyCode.D))
        {
            if (distanceFromEnemy <= 7.2)
            {
                if(isFacingRight)
                {
                    horizontalInput = 1;
                }
                else
                {
                    horizontalInput = 0.5f;
                }
            }
            else
            {
                horizontalInput = 0;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (distanceFromEnemy >= -6.3)
            {
                if(isFacingRight)
                {
                    horizontalInput = -0.5f;
                }
                else
                {
                    horizontalInput = -1;
                }
            }
            else
            {
                horizontalInput = 0;
            }
        }
        else
        {
            horizontalInput = 0;
        }

        // horizontalInput = Input.GetAxisRaw("Horizontal");


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
        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isCrouching)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Crouch Input
        if(Input.GetKey(KeyCode.S) && isGrounded)
        {
            isCrouching = true;
            Debug.Log("Crouching!");
        }
        else
        {
            isCrouching = false;
        }

        // Debugging
        if(!isCrouching)
        {
            Debug.Log("Not Crouching!");
        }

        // Movement Animations
        if (isFacingRight)
        {
            anim.SetFloat("Movement", horizontalMagnitude);
        }
        else
        {
            anim.SetFloat("Movement", -horizontalMagnitude);
        }
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

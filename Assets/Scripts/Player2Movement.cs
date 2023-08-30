using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private float horizontalInput;
    private float horizontalMagnitude;
    private float distanceFromEnemy;

    private int horizontalDirection = 5;

    private bool isGrounded = true;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    private bool canMove = true;

    private Rigidbody playerRb;
    public Animator anim;

    public GameObject enemy;
    public GameObject sprite;

    private Vector3 originalScale;
    private Vector3 flippedScale;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        originalScale = sprite.transform.localScale;
        flippedScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
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

        if (isFacingRight)
        {
            sprite.transform.localScale = originalScale;
        }
        else
        {
            sprite.transform.localScale = flippedScale;
        }

        // Get Player Horizontal Input
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isFacingRight)
            {
                horizontalDirection = 6;
            }
            else
            {
                horizontalDirection = 4;
            }

            if (distanceFromEnemy <= 7.2)
            {
                if (isFacingRight)
                {
                    horizontalInput = 1;
                }
                else
                {
                    horizontalInput = 0.8f;
                }
            }
            else
            {
                horizontalDirection = 5;
                horizontalInput = 0;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isFacingRight)
            {
                horizontalDirection = 4;
            }
            else
            {
                horizontalDirection = 6;
            }

            if (distanceFromEnemy >= -6.3)
            {
                if (isFacingRight)
                {
                    horizontalInput = -0.8f;
                }
                else
                {
                    horizontalInput = -1;
                }
            }
            else
            {
                horizontalInput = 0;
                horizontalDirection = 5;
            }
        }
        else
        {
            horizontalDirection = 5;
            horizontalInput = 0;
        }

        // horizontalInput = Input.GetAxisRaw("Horizontal");


        // Move the Player Horizontally
        if (!isCrouching && canMove)
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
        if (Input.GetKey(KeyCode.DownArrow) && isGrounded)
        {
            isCrouching = true;
            //Debug.Log("Crouching!");
        }
        else
        {
            isCrouching = false;
        }

        // Debugging
        //if (!isCrouching)
        //{
            //Debug.Log("Not Crouching!");
        //}

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
        anim.SetInteger("Direction", horizontalDirection);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void changeMoveState(bool moveable)
    {
        canMove = moveable;
    }

    public void Launch(float launchForce)
    {
        Debug.Log("Player hit with force: " + launchForce);
        playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        isGrounded = false;
    }

    public bool ReturnIsGrounded()
    {
        return isGrounded;
    }

    public int ReturnDirection()
    {
        return horizontalDirection;
    }

    public bool ReturnIsCrouching()
    {
        return isCrouching;
    }

    public void SetDirection(int newDirection)
    {
        horizontalDirection = newDirection;
    }
}

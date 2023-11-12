using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode forwardInput = KeyCode.W;
    public KeyCode backwardInput = KeyCode.S;
    public KeyCode leftInput = KeyCode.A;
    public KeyCode rightInput = KeyCode.D;

    public float originalSpeed = 5.0f;
    private float speed = 5.0f;
    public float jumpForce = 2.5f;
    private float horizontalInput;
    private float horizontalMagnitude;
    private float distanceFromEnemy;

    private float speedMultiplier = 1;

    private float lastForwardInput = 0f, timeSinceLastForward;
    private float lastBackwardInput = 0f, timeSinceLastBackward;

    private int horizontalDirection = 5;

    private bool isGrounded = true;
    private bool isCrouching = false;
    private bool isFacingRight = true;
    private bool canMove = true;
    private bool isSprinting = false;

    private Rigidbody playerRb;
    public Animator anim;

    public GameObject enemy;
    public GameObject sprite;

    private Vector3 originalScale;
    private Vector3 flippedScale;
    public Vector3 startPosition = new Vector3(2, 1, 0.0f);
    private Vector3 behind;

    //dashing variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 7.5f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.5f;

    // Accept Movement Input
    private bool acceptInput = false;

    // Sliding
    private bool sliding = false;

    // Inputs

    public bool isCPU = false;

    // Movement AI
    private bool aiRight = false;
    private bool aiLeft = false;
    private bool aiCrouch = false;
    private bool aiJump = false;
    private bool aiSprint = false;
    private bool aiBackdash = false;

    // AI Behavior
    private bool isApproaching;
    private bool isRetreating = false;
    private bool react = false;
    private int randomInt = 0;
    private float randomFloat = 0.0f;

    private bool isWandering;
    private bool attackRange = false;

    public bool easy = false, medium = true, hard = false, mahoraga = false;

    private float min, max;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        originalScale = sprite.transform.localScale;
        flippedScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        if (easy)
        {
            min = 3;
            max = 5;
        }
        else if (medium)
        {
            min = 1;
            max = 4;
        }
        else if (hard)
        {
            min = 0;
            max = 3;
        }
        else if (mahoraga)
        {
            min = 0;
            max = 1.25f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Setup per frame
        if (isDashing)
        {
            return;
        }

        if (!isGrounded)
        {
            canDash = false;
            speed = originalSpeed * 0.80f;
        }
        else
        {
            sliding = false;
            speed = originalSpeed;
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
            behind = Vector3.left;
        }
        else
        {
            sprite.transform.localScale = flippedScale;
            behind = Vector3.right;
        }


        // CPU AI Calculations and Behavior///////////////////////////

        // Approaching AI
        if (isWandering == false)
        {
            // If Enemy is far enough, start approaching
            if (isFacingRight)
            {
                if (distanceFromEnemy < -1f)
                {
                    attackRange = false;
                    StartCoroutine(Approach());
                }
            }
            else
            {
                if (distanceFromEnemy > 1f)
                {
                    attackRange = false;
                    StartCoroutine(Approach());
                }
            }
        }
        else
        {
            // If Enemy is close enough, stop approaching and start attacking
            if (isFacingRight)
            {
                if (distanceFromEnemy >= -1f)
                {
                    isWandering = false;
                    isApproaching = false;
                    attackRange = true;
                }
            }
            else
            {
                if (distanceFromEnemy <= 1f)
                {
                    isWandering = false;
                    isApproaching = false;
                    attackRange = true;
                }
            }

            // If enemy is defending backwards for a while start crouching
            
        }

        if (isApproaching)
        {
            if (isFacingRight)
            {
                aiRight = true;
            }
            else
            {
                aiLeft = true;
            }
        }
        else
        {
            aiRight = false;
            aiLeft = false;
        }


        //////////////////////////////////////


        // Get Player Horizontal Input
        if (isFacingRight)
        {
            if (((!Input.GetKey(rightInput) && !isCPU) || (aiRight && isCPU)) && isSprinting)
            {
                isSprinting = false;
                speedMultiplier = 1;
            }
        }
        else
        {
            if (((!Input.GetKey(leftInput) && !isCPU) || (aiLeft && isCPU)) && isSprinting)
            {
                isSprinting = false;
                speedMultiplier = 1;
            }
        }

        if (((Input.GetKey(rightInput) && !isCPU) || (aiRight && isCPU)) && acceptInput && !sliding)
        {
            if (distanceFromEnemy <= 7.2)
            {
                if (isFacingRight)
                {
                    if (Input.GetKeyDown(rightInput))
                    {
                        timeSinceLastForward = Time.time - lastForwardInput;
                        //Debug.Log("Last Forward input: " + timeSinceLastForward);
                        if (!isSprinting && timeSinceLastForward <= 0.2f)
                        {
                            isSprinting = true;
                            speedMultiplier = 1.5f;
                        }
                        lastForwardInput = Time.time;
                    }
                    horizontalDirection = 6;
                    horizontalInput = 1;
                }
                else
                {
                    if (Input.GetKeyDown(rightInput))
                    {
                        timeSinceLastBackward = Time.time - lastBackwardInput;
                        //Debug.Log("Last Backward input: " + timeSinceLastBackward);
                        if (timeSinceLastBackward <= 0.2f && canDash)
                        {
                            StartCoroutine(BackDash());
                        }
                        lastBackwardInput = Time.time;
                    }
                    horizontalDirection = 4;
                    horizontalInput = 0.8f;
                }
            }
            else
            {
                horizontalDirection = 5;
                horizontalInput = 0;
            }
        }
        else if (((Input.GetKey(leftInput) && !isCPU) || (aiLeft && isCPU)) && acceptInput && !sliding)
        {
            if (distanceFromEnemy >= -6.3)
            {
                if (isFacingRight)
                {
                    if (Input.GetKeyDown(leftInput))
                    {
                        timeSinceLastBackward = Time.time - lastBackwardInput;
                        //Debug.Log("Last Backward input: " + timeSinceLastBackward);
                        if (timeSinceLastBackward <= 0.2f && canDash)
                        {
                            StartCoroutine(BackDash());
                        }
                        lastBackwardInput = Time.time;
                    }
                    horizontalDirection = 4;
                    horizontalInput = -0.8f;
                }
                else
                {
                    if (Input.GetKeyDown(leftInput))
                    {
                        timeSinceLastForward = Time.time - lastForwardInput;
                        //Debug.Log("Last Forward input: " + timeSinceLastForward);
                        if (!isSprinting && timeSinceLastForward <= 0.2f)
                        {
                            isSprinting = true;
                            speedMultiplier = 1.5f;
                        }
                        lastForwardInput = Time.time;
                    }
                    horizontalDirection = 6;
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
            horizontalMagnitude = speed * speedMultiplier * horizontalInput;
            transform.Translate(Vector3.right * Time.deltaTime * horizontalMagnitude);
        }
        else
        {
            horizontalMagnitude = 0.0f;
        }

        // Jump Input
        if (((Input.GetKeyDown(forwardInput) && !isCPU) || (aiJump && isCPU)) && isGrounded && !isCrouching && acceptInput && canMove)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            isGrounded = false;
        }

        // Crouch Input
        if (((Input.GetKey(backwardInput) && !isCPU) || (aiCrouch && isCPU)) && isGrounded && acceptInput)
        {
            isCrouching = true;
            canDash = false;
            //Debug.Log("Crouching!");
        }
        else
        {
            isCrouching = false;
            canDash = true;
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
        if(canMove)
        {
            anim.SetBool("Crouching", isCrouching);
        }
        anim.SetInteger("Direction", horizontalDirection);
        anim.SetBool("Sprinting", isSprinting);
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

    public void Launch(float launchForceUp, float launchForceBehind)
    {
        playerRb.AddForce(Vector3.Normalize(Vector3.up + launchForceBehind * behind) * launchForceUp, ForceMode.Impulse);
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

    public bool ReturnWithinAttackRange()
    {
        return attackRange;
    }

    public void SetDirection(int newDirection)
    {
        horizontalDirection = newDirection;
    }

    public bool ReturnIsCPU()
    {
        return isCPU;
    }

    public bool ReturnIsRetreating()
    {
        return isRetreating;
    }

    public void ResetPosition()
    {
        //afterStart = false;
        //startTimer = 0.0f;
        playerRb.position = startPosition;
    }
    public void AcceptInput()
    {
        acceptInput = true;
    }

    public void DenyInput()
    {
        acceptInput = false;
    }

    public void LungeMovement()
    {
        playerRb.velocity = new Vector2(-behind.x * dashingPower, 0.0f);
    }


    private IEnumerator BackDash()
    {
        canDash = false;
        isDashing = true;
        anim.SetTrigger("Backdash");
        if (isFacingRight)
        {
            playerRb.velocity = new Vector2(-1f * dashingPower, 0f);
        }
        else
        {
            playerRb.velocity = new Vector2(1f * dashingPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator ReturnRandomFloat(float min, float max)
    {
        yield return new WaitForSeconds(1f);
        randomFloat = UnityEngine.Random.Range(min, max);
    }

    private IEnumerator ReturnRandomInt(int min, int max)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            randomInt = UnityEngine.Random.Range(min, max);
        }
    }

    IEnumerator Approach()
    {
        float approachWait = UnityEngine.Random.Range(min, max);
        float approachTime = UnityEngine.Random.Range(min, max);

        isWandering = true;

        yield return new WaitForSeconds(approachWait);

        isApproaching = true;

        yield return new WaitForSeconds(approachTime);

        isApproaching = false;

        isWandering = false;
    }

    public IEnumerator Retreat()
    {
        int retreatTime = UnityEngine.Random.Range(1, 3);

        isRetreating = true;

        yield return new WaitForSeconds(retreatTime);

        isRetreating = false;
    }

    public void StartCrouching()
    {
        Debug.Log("CROCUH");
        aiCrouch = true;
    }

    public void StopCrouching()
    {
        aiCrouch = false;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "PlayerHitBox")
        {
            Rigidbody collisionRb = collision.gameObject.transform.parent.GetComponent<Rigidbody>();
            //Debug.Log("Standing on Head!~");
            sliding = true;
            if (isFacingRight)
            {
                playerRb.velocity = new Vector2(-3, playerRb.velocity.y);
                collisionRb.velocity = new Vector2(3, collisionRb.velocity.y);
            }
            else
            {
                playerRb.velocity = new Vector2(3, playerRb.velocity.y);
                collisionRb.velocity = new Vector2(-3, collisionRb.velocity.y);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;
    public PlayerMovement enemyMovement;
    public PlayerCombat enemyCombat;
    public GameObject block;
    public GameObject tutorialPanel;
    public TMP_Text title, controls;
    private Animator playerAnimator;

    private int tutorialStage = -1;
    private int success = 0;
    private bool blockCooldown = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Forward and Backwards
        if(tutorialStage == 0)
        {
            title.text = "movement";
            controls.text = "A - LEFT, D - RIGHT";

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                success += 1;
            }

            if(success >= 5)
            {
                success = 0;
                tutorialStage = 1;
            }
        }

        // Jumping
        if (tutorialStage == 1)
        {
            title.text = "jumping";
            controls.text = "W - jump";

            if (Input.GetKeyDown(KeyCode.W))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 2;
            }
        }

        // Crouching
        if (tutorialStage == 2)
        {
            title.text = "crouching";
            controls.text = "S - crouch";

            if (Input.GetKeyDown(KeyCode.S))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 3;
            }
        }

        // Sprinting
        if (tutorialStage == 3)
        {
            title.text = "Sprinting";
            controls.text = "Forward X2 then Hold";

            if (playerMovement.ReturnIsSprinting() && !blockCooldown)
            {
                success += 1;
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 4;
            }
        }

        // Backdash
        if (tutorialStage == 4)
        {
            title.text = "Back Dashing";
            controls.text = "Backward X2";

            if (playerMovement.ReturnIsDashing() && !blockCooldown)
            {
                success += 1;
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 5;
            }
        }

        // Light
        if (tutorialStage == 5)
        {
            title.text = "Attacks";
            controls.text = "J - Light Attack";

            if (Input.GetKeyDown(KeyCode.J))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 6;
            }
        }

        // Medium
        if (tutorialStage == 6)
        {
            title.text = "Attacks";
            controls.text = "K - Medium Attack";

            if (Input.GetKeyDown(KeyCode.K))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 7;
            }
        }

        // Heavy
        if (tutorialStage == 7)
        {
            title.text = "Attacks";
            controls.text = "L - Heavy Attack";

            if (Input.GetKeyDown(KeyCode.L))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 8;
            }
        }

        // Overhead
        if (tutorialStage == 8)
        {
            title.text = "Attacks";
            controls.text = "I - Overhead Attack";

            if (Input.GetKeyDown(KeyCode.I))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 9;
            }
        }

        // Special
        if (tutorialStage == 9)
        {
            title.text = "Attacks";
            controls.text = "M - Special Attack";

            if (Input.GetKeyDown(KeyCode.M))
            {
                success += 1;
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 10;
            }
        }

        // Crouch Attacks
        if (tutorialStage == 10)
        {
            title.text = "Crouch Attacks";
            controls.text = "Crouch + J/K/L/I/M - Crouch Attack";

            if (Input.GetKey(KeyCode.S) && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.M)))
            {
                success += 1;
            }

            if (success >= 10)
            {
                success = 0;
                tutorialStage = 11;
            }
        }

        // Block
        if (tutorialStage == 11)
        {
            title.text = "Guarding";
            controls.text = "Walk Backwards - Normal Guard\nBlocks all except Crouch Attacks";

            enemyCombat.SetIsCPU(true);
            enemyMovement.SetWithinAttackRange(true);

            if (block.activeSelf && !blockCooldown)
            {
                success += 1;
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                tutorialStage = 12;
            }
        }

        // Crouch Block
        if (tutorialStage == 12)
        {
            title.text = "Guarding";
            controls.text = "Crouch + Backwards - Low Guard\nBlocks all except Overhead Attacks";

            enemyMovement.SetIsCPU(true);
            enemyMovement.StartCrouching();

            if (block.activeSelf && !blockCooldown)
            {
                success += 1;
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                enemyMovement.SetWithinAttackRange(false);
                enemyCombat.SetIsCPU(false);
                enemyMovement.SetIsCPU(false);
                tutorialStage = 13;
            }
        }

        // Crouch Attacks
        if (tutorialStage == 13)
        {
            title.text = "Style Switching";
            controls.text = "SHIFT + W/A/S/D - Switch Styles\nRequires unlocking Styles";

            if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
            {
                success += 1;
            }

            if (success >= 4)
            {
                success = 0;
                tutorialStage = 14;
                tutorialPanel.SetActive(false);
            }
        }

        Debug.Log(tutorialStage);
    }

    public void StartTutorial()
    {
        success = 0;
        tutorialStage = 0;
        tutorialPanel.SetActive(true);
    }

    private IEnumerator BlockCheckCooldown()
    {
        blockCooldown = true;

        yield return new WaitForSeconds(1);

        blockCooldown = false;
    }
}

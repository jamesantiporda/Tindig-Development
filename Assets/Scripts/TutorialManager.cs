using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TutorialManager : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerMovement playerMovement;
    public PlayerMovement enemyMovement;
    public PlayerCombat enemyCombat;
    public GameObject block;
    public GameObject tutorialPanel;
    public TMP_Text title, controls, moreTimes;
    private Animator playerAnimator;
    public Animator successAnimator;
    public SpriteToPlayer player1Sprite;

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
        if (tutorialStage == 0)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "movement";
            controls.text = "A - LEFT, D - RIGHT";
            moreTimes.text = 5 - success + " more time(s)";

            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 5)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Jumping
        if (tutorialStage == 1)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "jumping";
            controls.text = "W - jump";
            moreTimes.text = 3 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.W) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Crouching
        if (tutorialStage == 2)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "crouching";
            controls.text = "S - crouch";
            moreTimes.text = 3 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.S) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Sprinting
        if (tutorialStage == 3)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Sprinting";
            controls.text = "Forward X2 then Hold";
            moreTimes.text = 3 - success + " more time(s)";

            if (playerMovement.ReturnIsSprinting() && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Backdash
        if (tutorialStage == 4)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Back Dashing";
            controls.text = "Backward X2";
            moreTimes.text = 3 - success + " more time(s)";

            if (playerMovement.ReturnIsDashing() && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Light
        if (tutorialStage == 5)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Attacks";
            controls.text = "J - Light Attack\nLow Damage, Fast Attack\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.J) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Medium
        if (tutorialStage == 6)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Attacks";
            controls.text = "K - Medium Attack\nMedium damage, Medium Speed\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.K) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Heavy
        if (tutorialStage == 7)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Attacks";
            controls.text = "L - Heavy Attack\nHigh damage, Slow Attack\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.L) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Overhead
        if (tutorialStage == 8)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Attacks";
            controls.text = "I - Overhead Attack\nHigh Damage, Goes through Low Guard\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.I) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Special
        if (tutorialStage == 9)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Attacks";
            controls.text = "M - Special Attack\nAttack Unique to Fighting Style\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKeyDown(KeyCode.M) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Lunge Attacks
        if (tutorialStage == 10)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Lunge Attacks";
            controls.text = "Forward + L (Heavy) - \nHigh Damage and Propels Player Forward\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (playerMovement.ReturnDirection() == 6 && Input.GetKeyDown(KeyCode.L) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Crouch Attacks
        if (tutorialStage == 11)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Low Attacks";
            controls.text = "Crouch + J/K/L/I/M - Low Attack\n(Goes through normal Guard)\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 10 - success + " more time(s)";

            if (Input.GetKey(KeyCode.S) && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.M)) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 10)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Launcher
        if (tutorialStage == 12)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Launcher";
            controls.text = "Crouch + L (Heavy)\nLaunches opponent\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.L) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Sweep
        if (tutorialStage == 13)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Sweep";
            controls.text = "Crouch + Forward + L (Heavy)\nKnocks Down opponent\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKey(KeyCode.S) && playerMovement.ReturnDirection() == 6 && Input.GetKeyDown(KeyCode.L) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 4)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Jump Attacks
        if (tutorialStage == 14)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Jump Attacks";
            controls.text = "Jump + J/K/L/I/M - Jump Attack\nTip: Attack Damage gets lower if same attack is used";
            moreTimes.text = 10 - success + " more time(s)";

            if (!playerMovement.ReturnIsGrounded() && (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.M)) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 10)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Block
        if (tutorialStage == 15)
        {
            title.text = "Guarding";
            controls.text = "Walk Backwards - Normal Guard\nBlocks all except Crouch Attacks";
            moreTimes.text = 3 - success + " more time(s)";

            enemyCombat.SetIsCPU(true);
            enemyCombat.SetDifficulty(3);
            enemyMovement.SetIsCPU(true);
            enemyMovement.StopCrouching();

            if ((block.activeSelf) && !blockCooldown)
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Crouch Block
        if (tutorialStage == 16)
        {
            title.text = "Low Guarding";
            controls.text = "Crouch + Backwards - Low Guard\nBlocks all except Overhead Attacks";
            moreTimes.text = 3 - success + " more time(s)";

            enemyCombat.SetIsCPU(true);
            enemyCombat.SetDifficulty(3);
            enemyMovement.SetWithinAttackRange(true);
            enemyMovement.StartCrouching();
            StartCoroutine(enemyCombat.ForceMediumAttack());

            if ((block.activeSelf) && !blockCooldown && playerMovement.ReturnIsCrouching())
            {
                success += 1;
                successAnimator.SetTrigger("Success");
                StartCoroutine(BlockCheckCooldown());
            }

            if (success >= 3)
            {
                enemyMovement.SetWithinAttackRange(false);
                enemyCombat.SetIsCPU(false);
                enemyMovement.SetIsCPU(false);
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Perfect Guard
        if (tutorialStage == 17)
        {
            SpriteToPlayer.perfectGuard += PerfectGuardSuccess;

            title.text = "Perfect Guarding";
            controls.text = "Start Guarding as an attack lands\nBlocked Attack does no chip damage";
            moreTimes.text = 3 - success + " more time(s)";

            enemyCombat.SetIsCPU(true);
            enemyCombat.SetDifficulty(3);
            enemyMovement.SetIsCPU(true);

            if (success >= 3)
            {
                SpriteToPlayer.perfectGuard -= PerfectGuardSuccess;
                enemyCombat.SetIsCPU(false);
                enemyMovement.SetIsCPU(false);
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
            }
        }

        // Style Switching
        if (tutorialStage == 18)
        {
            enemyCombat.SetIsCPU(false);
            enemyMovement.SetIsCPU(false);
            title.text = "Style Switching";
            controls.text = "SHIFT + W/A/S/D - Switch Styles\nRequires unlocking Styles";
            moreTimes.text = 4 - success + " more time(s)";

            if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
            {
                success += 1;
                successAnimator.SetTrigger("Success");
            }

            if (success >= 4)
            {
                enemyCombat.SetIsCPU(false);
                enemyMovement.SetIsCPU(false);
                success = 0;
                blockCooldown = false;
                tutorialStage += 1;
                tutorialPanel.SetActive(false);
            }
        }

        //Debug.Log(tutorialStage);
    }

    public void StartTutorial()
    {
        success = 0;
        tutorialStage = 0;
        tutorialPanel.SetActive(true);
    }

    public void BackOneStep()
    {
        success = 0;

        if(tutorialStage <= 0)
        {
            tutorialStage = 0;
        }
        else
        {
            tutorialStage -= 1;
        }
    }

    public void ForwardOneStep()
    {
        success = 0;

        if(tutorialStage == 18)
        {
            tutorialStage = 19;
            tutorialPanel.SetActive(false);
        }
        else
        {
            tutorialStage += 1;
        }
    }

    private IEnumerator BlockCheckCooldown()
    {
        blockCooldown = true;

        yield return new WaitForSeconds(1);

        blockCooldown = false;
    }

    private void PerfectGuardSuccess()
    {
        if (!blockCooldown && successAnimator != null)
        {
            success += 1;
            successAnimator.SetTrigger("Success");
            StartCoroutine(BlockCheckCooldown());
        }
    }
}

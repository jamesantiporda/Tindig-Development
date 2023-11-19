using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;
using System.Security.Cryptography;

public class MatchManager : MonoBehaviour
{
    public Animator player1Animator, player2Animator;
    private float timer, timeElapsed;
    public TMP_Text timerText;

    public PlayerHealth player1Health, player2Health;

    public GameObject player1round1, player1round2, player2round1, player2round2;

    public PlayerMovement player1Movement;
    public PlayerMovement player2Movement;

    public PlayerCombat player1Combat;
    public PlayerCombat player2Combat;

    public BoxCollider2D player1HurtBox, player2HurtBox;

    private int player1RoundsWon, player2RoundsWon;

    private bool roundOngoing = false, matchEnded;

    private int roundNumber;

    public GameObject handa, laban, winner, ko, timeup;
    public TMP_Text winnerText;

    // Round Start Timer
    private float startTimer = 0.0f;
    private bool roundStarting = false;

    // Round End Timer
    private float endTimer = 0.0f;
    private bool roundEnding = false;

    private string player1Name = "", player2Name = "", winnerName = "";

    public GameObject Player1, Player2;

    public bool isTraining = false;


    // Start is called before the first frame update
    void Start()
    {
        roundNumber = 0;

        player1round1.SetActive(false);
        player1round2.SetActive(false);

        player2round1.SetActive(false);
        player2round2.SetActive(false);

        player1RoundsWon = 0;
        player2RoundsWon = 0;

        player1Movement.DenyInput();
        player2Movement.DenyInput();

        player1Combat.DenyInput();
        player2Combat.DenyInput();

        timeElapsed = 0.0f;
        timer = 90;

        winnerText.text = winnerName + " WINS!";

        player1Name = Player1.name;
        player2Name = Player2.name;

        if (!isTraining)
        {
            RoundStart();
        }
        else
        {
            player1Health.RegenHealth();
            player2Health.RegenHealth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTraining)
        {
            // Start Timer
            if (startTimer >= 3.5f && roundStarting)
            {
                roundStarting = false;
                roundOngoing = true;
                player1Movement.AcceptInput();
                player2Movement.AcceptInput();
                player1Combat.AcceptInput();
                player2Combat.AcceptInput();
                laban.SetActive(true);
            }
            else
            {
                startTimer += Time.deltaTime;

                if (startTimer >= 2.0f && !handa.activeSelf)
                {
                    handa.SetActive(true);
                }
            }

            // End Timer (ends into the Start Timer)
            if (endTimer >= 5.0f && roundEnding)
            {
                roundEnding = false;
                if (!matchEnded)
                {
                    RoundStart();
                }
            }
            else
            {
                endTimer += Time.deltaTime;
            }


            if (roundOngoing)
            {
                timer -= Time.deltaTime;
            }

            timerText.text = "" + Mathf.Floor(timer);


            if (player1Health.ReturnHealth() <= 0 && roundOngoing)
            {
                RoundEnd();
                UpdateRounds(2);
            }

            if (player2Health.ReturnHealth() <= 0 && roundOngoing)
            {
                RoundEnd();
                UpdateRounds(1);
            }

            if (timer < 1.0f && roundOngoing)
            {
                RoundEnd();
                UpdateRounds(0);
            }
        }
        else
        {
            roundStarting = false;
            roundOngoing = true;
            player1Movement.AcceptInput();
            player2Movement.AcceptInput();
            player1Combat.AcceptInput();
            player2Combat.AcceptInput();
        }
    }

    private void RoundStart()
    {
        laban.SetActive(false);
        handa.SetActive(false);
        ko.SetActive(false);
        timeup.SetActive(false);
        roundNumber += 1;
        if (roundNumber != 1)
        {
            player1Movement.ResetPosition();
            player2Movement.ResetPosition();
            player1Combat.Reset();
            player2Combat.Reset();
            player1HurtBox.enabled = true;
            player2HurtBox.enabled = true;
        }
        startTimer = 0.0f;
        roundStarting = true;
        timeElapsed = 0.0f;
        timer = 90;
        player1Health.ResetHealth();
        player2Health.ResetHealth();
    }

    private void UpdateRounds(int player)
    {
        if(player == 1)
        {
            if(!player2Animator.GetBool("Dead"))
            {
                Debug.Log("Kill1");
                player2Combat.Die();
            }
            player1RoundsWon += 1;

            if (player1RoundsWon < 2 && player2RoundsWon < 2)
            {
                ko.SetActive(true);
            }
        }
        else if(player == 2)
        {
            if(!player1Animator.GetBool("Dead"))
            {
                player1Combat.Die();
            }
            player2RoundsWon += 1;

            if (player1RoundsWon < 2 && player2RoundsWon < 2)
            {
                ko.SetActive(true);
            }
        }
        else if(player == 0)
        {
            if(player1Health.ReturnHealth() > player2Health.ReturnHealth())
            {
                player1RoundsWon += 1;
            }
            else if (player2Health.ReturnHealth() > player1Health.ReturnHealth())
            {
                player2RoundsWon += 1;
            }

            timeup.SetActive(true);
        }

        if(player1RoundsWon >= 1)
        {
            player1round1.SetActive(true);
        }
        
        if(player1RoundsWon >= 2)
        {
            player1round2.SetActive(true);
            winnerName = player1Name;
            MatchEnd();
        }

        if (player2RoundsWon >= 1)
        {
            player2round1.SetActive(true);
        }

        if (player2RoundsWon >= 2)
        {
            player2round2.SetActive(true);
            winnerName = player2Name;
            MatchEnd();
        }
    }

    private void RoundEnd()
    {
        roundOngoing = false;
        endTimer = 0.0f;
        roundEnding = true;
        player1HurtBox.enabled = false;
        player2HurtBox.enabled = false;
        player1Movement.DenyInput();
        player2Movement.DenyInput();
        player1Combat.DenyInput();
        player2Combat.DenyInput();
        player1Combat.SetCanAttack(false);
        player2Combat.SetCanAttack(false);
    }

    private void MatchEnd()
    {
        Time.timeScale = 0.25f;
        matchEnded = true;
        winnerText.text = winnerName + " WINS!";
        winner.SetActive(true);
    }
}

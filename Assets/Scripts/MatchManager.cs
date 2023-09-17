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
    public Player2Movement player2Movement;

    private int player1RoundsWon, player2RoundsWon;

    private bool roundOngoing = false;

    private int roundNumber;

    // Round Start Timer
    private float startTimer = 0.0f;
    private bool roundStarting = false;

    // Round End Timer
    private float endTimer = 0.0f;
    private bool roundEnding = false;


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

        timeElapsed = 0.0f;
        timer = 90;

        RoundStart();
    }

    // Update is called once per frame
    void Update()
    {
        // Start Timer
        if(startTimer >= 5.0f && roundStarting)
        {
            roundStarting = false;
            roundOngoing = true;
            player1Movement.AcceptInput();
            player2Movement.AcceptInput();
        }
        else
        {
            startTimer += Time.deltaTime;
        }

        // End Timer (ends into the Start Timer)
        if(endTimer >= 5.0f && roundEnding)
        {
            roundEnding = false;
            RoundStart();
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


        if(player1Health.ReturnHealth() <= 0 && roundOngoing)
        {
            RoundEnd();
            UpdateRounds(2);
        }
        
        if(player2Health.ReturnHealth() <= 0 && roundOngoing)
        {
            RoundEnd();
            UpdateRounds(1);
        }

        if(timer < 1.0f && roundOngoing)
        {
            RoundEnd();
            UpdateRounds(0);
        }
    }

    private void RoundStart()
    {
        roundNumber += 1;
        if (roundNumber != 1)
        {
            player1Movement.ResetPosition();
            player2Movement.ResetPosition();
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
            player1RoundsWon += 1;
        }
        else if(player == 2)
        {
            player2RoundsWon += 1;
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
        }

        if(player1RoundsWon >= 1)
        {
            player1round1.SetActive(true);
        }
        
        if(player1RoundsWon >= 2)
        {
            player1round2.SetActive(true);
        }

        if (player2RoundsWon >= 1)
        {
            player2round1.SetActive(true);
        }

        if (player2RoundsWon >= 2)
        {
            player2round2.SetActive(true);
        }
    }

    private void RoundEnd()
    {
        roundOngoing = false;
        endTimer = 0.0f;
        roundEnding = true;
        player1Movement.DenyInput();
        player2Movement.DenyInput();
    }


}

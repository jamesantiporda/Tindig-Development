using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

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

    public GameObject handa, laban, winner, ko, timeup, pauseScreen, winScreen;
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

    private bool isPaused = false;

    public bool isBoxingBoss, isSikaranBoss, isArnisBoss, isGrunt;

    public int gruntStyle = 0, gruntType = 0;

    public bool unlocksNextFighters = false;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        winScreen.SetActive(false);

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
            player2Combat.SetDifficulty(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                MatchResume();
            else
                MatchPause();
        }

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

        var foundVolleyballs = FindObjectsOfType<Volleyball>();

        foreach(var volleyball in foundVolleyballs)
        {
            Destroy(volleyball);
        }

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

            //if (player1RoundsWon < 2 && player2RoundsWon < 2)
            //{
            //    ko.SetActive(true);
            //}

            ko.SetActive(true);
        }
        else if(player == 2)
        {
            if(!player1Animator.GetBool("Dead"))
            {
                player1Combat.Die();
            }
            player2RoundsWon += 1;

            //if (player1RoundsWon < 2 && player2RoundsWon < 2)
            //{
            //    ko.SetActive(true);
            //}

            ko.SetActive(true);
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

            if(isBoxingBoss)
            {
                if(!PlayerPrefs.HasKey("Boxing"))
                {
                    PlayerPrefs.SetString("Boxing", "unlocked");
                    PlayerPrefs.SetInt("NewStyleUnlocked", 1);
                }

                if (unlocksNextFighters)
                {
                    PlayerPrefs.SetString("SikaranArea", "unlocked");
                    PlayerPrefs.SetString("BoxingGrunt", "unlocked");
                }
            }
            
            if(isSikaranBoss)
            {
                if(!PlayerPrefs.HasKey("Sikaran"))
                {
                    PlayerPrefs.SetString("Sikaran", "unlocked");
                    PlayerPrefs.SetInt("NewStyleUnlocked", 1);
                }
                
                if (unlocksNextFighters)
                {
                    PlayerPrefs.SetString("ArnisArea", "unlocked");
                    PlayerPrefs.SetString("SikaranGrunt", "unlocked");
                }
            }

            if(isArnisBoss)
            {
                if(!PlayerPrefs.HasKey("Arnis"))
                {
                    PlayerPrefs.SetString("Arnis", "unlocked");
                    PlayerPrefs.SetInt("NewStyleUnlocked", 1);
                }
                
                if (unlocksNextFighters)
                {
                    PlayerPrefs.SetString("FinalBossArea", "unlocked");
                    PlayerPrefs.SetString("ArnisGrunt", "unlocked");
                }
            }

            if(isGrunt)
            {
                switch(gruntStyle)
                {
                    case 0:
                        if(gruntType == 0)
                        {
                            PlayerPrefs.SetString("BoxingLight", "unlocked");
                            PlayerPrefs.SetString("BoxingGrunt", "unlocked");
                            if (!PlayerPrefs.HasKey("BoxingGruntDamage1"))
                            {
                                AddToDamageModifier(0.10f);
                                PlayerPrefs.SetString("BoxingGruntDamage1", "redeemed");
                            }
                        }
                        else if(gruntType == 1)
                        {
                            PlayerPrefs.SetString("BoxingMedium", "unlocked");
                            PlayerPrefs.SetString("BoxingGrunt", "unlocked");
                        }
                        else if(gruntType == 2)
                        {
                            PlayerPrefs.SetString("Boxing", "unlocked");
                            PlayerPrefs.SetInt("NewStyleUnlocked", 1);
                            if (!PlayerPrefs.HasKey("BoxingGruntDamage2"))
                            {
                                AddToDamageModifier(0.10f);
                                PlayerPrefs.SetString("BoxingGruntDamage2", "redeemed");
                            }
                        }
                        break;
                    case 1:
                        if (gruntType == 0)
                        {
                            PlayerPrefs.SetString("SikaranLight", "unlocked");
                            PlayerPrefs.SetString("SikaranGrunt", "unlocked");
                            if (!PlayerPrefs.HasKey("SikaranGruntDamage1"))
                            {
                                AddToDamageModifier(0.10f);
                                PlayerPrefs.SetString("SikaranGruntDamage1", "redeemed");
                            }
                        }
                        else if (gruntType == 1)
                        {
                            PlayerPrefs.SetString("SikaranMedium", "unlocked");
                            PlayerPrefs.SetString("SikaranGrunt", "unlocked");
                        }
                        else if (gruntType == 2)
                        {
                            PlayerPrefs.SetString("Sikaran", "unlocked");
                            PlayerPrefs.SetInt("NewStyleUnlocked", 1);
                            if(!PlayerPrefs.HasKey("SikaranGruntDamage2"))
                            {
                                AddToDamageModifier(0.10f);
                                PlayerPrefs.SetString("SikaranGruntDamage2", "redeemed");
                            }
                        }
                        break;
                    case 2:
                        if (gruntType == 0)
                        {
                            PlayerPrefs.SetString("ArnisLight", "unlocked");
                            PlayerPrefs.SetString("ArnisGrunt", "unlocked");
                            if(!PlayerPrefs.HasKey("ArnisGruntDamage1"))
                            {
                                AddToDamageModifier(0.10f);
                                PlayerPrefs.SetString("ArnisGruntDamage1", "redeemed");
                            }
                        }
                        else if (gruntType == 1)
                        {
                            PlayerPrefs.SetString("ArnisMedium", "unlocked");
                            PlayerPrefs.SetString("ArnisGrunt", "unlocked");
                        }
                        else if (gruntType == 2)
                        {
                            PlayerPrefs.SetString("Arnis", "unlocked");
                            PlayerPrefs.SetInt("NewStyleUnlocked", 1);

                            if(!PlayerPrefs.HasKey("ArnisGruntDamage2"))
                            {
                                AddToDamageModifier(0.10f);
                                PlayerPrefs.SetString("ArnisGruntDamage2", "redeemed");
                            }
                        }
                        break;
                    default:
                        Debug.Log("Not a valid gruntStyle");
                        break;
                }
            }

            MatchEnd();
        }

        if (player2RoundsWon >= 1)
        {
            player2round2.SetActive(true);
        }

        if (player2RoundsWon >= 2)
        {
            player2round1.SetActive(true);
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
        StartCoroutine(HideRoundEndText());
        // winner.SetActive(true);
        StartCoroutine(ShowWinner());
        StartCoroutine(ShowWinScreen());
    }

    public void MatchPause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseScreen.SetActive(true);
    }

    public void MatchResume()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScreen");
    }

    public void ReturnToStageSelect()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("StageSelect");
    }

    public void  ReturnToCharacterSelect()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("CharacterSelect");
    }

    public void RestartMatch()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ShowWinScreen()
    {
        yield return new WaitForSecondsRealtime(5.0f);
        Time.timeScale = 1.0f;
        //winScreen.SetActive(true);
        if(!isArnisBoss && !isBoxingBoss && !isSikaranBoss)
        {
            winScreen.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "BoxingBoss")
        {
            SceneManager.LoadScene("BoxerWinDialogue");
        }

        if (SceneManager.GetActiveScene().name == "SikaranBoss")
        {
            SceneManager.LoadScene("SikaranWinDialogue");
        }

        if (SceneManager.GetActiveScene().name == "ArnisBoss")
        {
            SceneManager.LoadScene("ArnisWinDialogue");
        }

        if (SceneManager.GetActiveScene().name == "FinalBoss")
        {
            SceneManager.LoadScene("FinalBossOutro");
        }
    }

    private IEnumerator ShowWinner()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        winner.SetActive(true);
    }

    private IEnumerator HideRoundEndText()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        ko.SetActive(false);
        timeup.SetActive(false);
    }

    private void AddToDamageModifier(float percentage)
    {
        if(!PlayerPrefs.HasKey("DamageModifier"))
        {
            PlayerPrefs.SetFloat("DamageModifier", 0.0f + percentage);
        }
        else
        {
            PlayerPrefs.SetFloat("DamageModifier", PlayerPrefs.GetFloat("DamageModifier") + percentage);
        }
    }
}

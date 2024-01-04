using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public TMP_Text player1Character, player2Character;
    public GameObject player1Ready, player2Ready, startText;

    public GameObject mcIcon, boxingIcon, sikaranIcon, arnisIcon, masterIcon, randomIcon, player1Highlight, player2Highlight;

    public GameObject player1Preview, player2Preview, mapSelect, characterSelect;

    private string[] characters;
    private GameObject[] icons;
    private int player1Selection, player2Selection;

    // Start is called before the first frame update
    void Start()
    {
        characters = new string[6];

        characters[0] = "MC";
        characters[1] = "Master of Panuntukan";
        characters[2] = "Master of Sikaran";
        characters[3] = "Master of Arnis";
        characters[4] = "Master of PH Martial Arts";
        characters[5] = "Random";

        icons = new GameObject[6];

        icons[0] = mcIcon;
        icons[1] = boxingIcon;
        icons[2] = sikaranIcon;
        icons[3] = arnisIcon;
        icons[4] = masterIcon;
        icons[5] = randomIcon;


        player1Selection = 0;
        player2Selection = 0;

        player1Highlight.transform.position = mcIcon.transform.position;
        player2Highlight.transform.position = mcIcon.transform.position;

        player1Ready.SetActive(false);
        player2Ready.SetActive(false);
        startText.SetActive(false);
        player1Preview.SetActive(false);
        player2Preview.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!mapSelect.activeSelf)
        {
            // Clamp player select to mins and maxes
            if (player1Selection < 0)
            {
                player1Selection = 0;
            }
            else if (player1Selection > 5)
            {
                player1Selection = 5;
            }

            if (player2Selection < 0)
            {
                player2Selection = 0;
            }
            else if (player2Selection > 5)
            {
                player2Selection = 5;
            }

            player1Character.text = characters[player1Selection];
            player2Character.text = characters[player2Selection];

            player1Highlight.transform.position = icons[player1Selection].transform.position;
            player2Highlight.transform.position = icons[player2Selection].transform.position;

            player1Preview.GetComponent<Image>().sprite = icons[player1Selection].GetComponent<Image>().sprite;
            player2Preview.GetComponent<Image>().sprite = icons[player2Selection].GetComponent<Image>().sprite;

            if (player1Ready.activeSelf && player2Ready.activeSelf)
            {
                startText.SetActive(true);
            }
            else
            {
                startText.SetActive(false);
            }


            // Player 1 Controls
            if (Input.GetKeyDown(KeyCode.W) && !player1Ready.activeSelf)
            {
                player1Selection -= 2;
            }

            if (Input.GetKeyDown(KeyCode.S) && !player1Ready.activeSelf)
            {
                player1Selection += 2;
            }

            if (Input.GetKeyDown(KeyCode.A) && !player1Ready.activeSelf)
            {
                player1Selection -= 1;
            }

            if (Input.GetKeyDown(KeyCode.D) && !player1Ready.activeSelf)
            {
                player1Selection += 1;
            }

            if (Input.GetKeyDown(KeyCode.Space) && startText.activeSelf)
            {
                mapSelect.SetActive(true);
                PlayerPrefs.SetInt("P1", player1Selection);
                PlayerPrefs.SetInt("P2", player2Selection);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !startText.activeSelf)
            {
                player1Ready.SetActive(true);
                player1Preview.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !mapSelect.activeSelf)
            {
                player1Ready.SetActive(false);
                player1Preview.SetActive(false);
            }


            // Player 2 Controls
            if (Input.GetKeyDown(KeyCode.UpArrow) && !player2Ready.activeSelf)
            {
                player2Selection -= 2;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && !player2Ready.activeSelf)
            {
                player2Selection += 2;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !player2Ready.activeSelf)
            {
                player2Selection -= 1;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && !player2Ready.activeSelf)
            {
                player2Selection += 1;
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                player2Ready.SetActive(true);
                player2Preview.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                player2Ready.SetActive(false);
                player2Preview.SetActive(false);
            }
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}

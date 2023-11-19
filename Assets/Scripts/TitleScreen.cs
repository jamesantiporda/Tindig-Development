using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject pressAnyButton, choicePanel;

    private bool choicesVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey && !choicesVisible)
        {
            choicesVisible = true;
            pressAnyButton.SetActive(false);
            choicePanel.SetActive(true);
        }
    }

    public void StageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void Training()
    {
        SceneManager.LoadSceneAsync("Training");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenScript : MonoBehaviour
{
    private LoadingScene loadingScreenManager;
    public GameObject newStyleText, newStyleButton;

    private void Start()
    {
        loadingScreenManager = FindObjectOfType<LoadingScene>();

        if(PlayerPrefs.GetInt("NewStyleUnlocked") == 1)
        {
            newStyleText.SetActive(true);
            newStyleButton.SetActive(true);
        }
        else
        {
            newStyleText.SetActive(false);
            newStyleButton.SetActive(false);
        }
    }

    public void GoToTraining()
    {
        loadingScreenManager.LoadScene("Training");
    }
}

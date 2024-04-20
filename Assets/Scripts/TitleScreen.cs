using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    public GameObject pressAnyButton, choicePanel, fadeToBlack, settingsPanel, splashTitle;

    private bool choicesVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetInt("Difficulty", 1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey && !choicesVisible)
        {
            choicesVisible = true;
            pressAnyButton.SetActive(false);
            splashTitle.SetActive(false);
            choicePanel.SetActive(true);
        }
    }

    public void CharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void StageSelect()
    {
        StartCoroutine(ToStageSelect());
    }

    private IEnumerator ToStageSelect()
    {
        fadeToBlack.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("StageSelect");
    }

    public void Training()
    {
        SceneManager.LoadSceneAsync("Training");
    }

    public void SetDifficulty(int diff)
    {
        PlayerPrefs.SetInt("Difficulty", diff);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void ResetStyles()
    {
        PlayerPrefs.DeleteKey("Boxing");
        PlayerPrefs.DeleteKey("Sikaran");
        PlayerPrefs.DeleteKey("Arnis");
        PlayerPrefs.DeleteKey("Stage");
        PlayerPrefs.DeleteKey("SikaranArea");
        PlayerPrefs.DeleteKey("ArnisArea");
        PlayerPrefs.DeleteKey("FinalBossArea");
        PlayerPrefs.DeleteKey("NotFirstTime");

        PlayerPrefs.DeleteKey("BoxingGrunt");
        PlayerPrefs.DeleteKey("BoxingGruntDamage1");
        PlayerPrefs.DeleteKey("BoxingGruntDamage2");

        PlayerPrefs.DeleteKey("SikaranGrunt");
        PlayerPrefs.DeleteKey("SikaranGruntDamage1");
        PlayerPrefs.DeleteKey("SikaranGruntDamage2");

        PlayerPrefs.DeleteKey("ArnisGrunt");
        PlayerPrefs.DeleteKey("ArnisGruntDamage1");
        PlayerPrefs.DeleteKey("ArnisGruntDamage2");

        PlayerPrefs.DeleteKey("DamageModifier");
    }
}

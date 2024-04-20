using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Scene scene;
    private string sceneName;

    private Queue<string> sentences;

    private LoadingScene loadingScreenManager;

    void Start()
    {
        loadingScreenManager = FindObjectOfType<LoadingScene>();
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void EndDialogue()
    {
        // boxing dialogue scenes

        if (sceneName == "BoxerLightDialogue")
        {
            loadingScreenManager.LoadScene("BoxingLight");
        }

        if(sceneName ==  "BoxerHeavyDialogue")
        {
            loadingScreenManager.LoadScene("BoxingHeavy");
        }

        if (sceneName == "BoxerBossDialogue")
        {
            loadingScreenManager.LoadScene("BoxingBoss");
        }

        if (sceneName == "BoxerLoseDialogue")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        // sikaran dialogue scenes

        if (sceneName == "SikaranLightDialogue")
        {
            loadingScreenManager.LoadScene("SikaranLight");
        }

        if (sceneName == "SikaranHeavyDialogue")
        {
            loadingScreenManager.LoadScene("SikaranHeavy");
        }

        if (sceneName == "SikaranBossDialogue")
        {
            loadingScreenManager.LoadScene("SikaranBoss");
        }

        if (sceneName == "SikaranLoseDialogue")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        // arnis dialogue scenes

        if (sceneName == "ArnisLightDialogue")
        {
            loadingScreenManager.LoadScene("ArnisLight");
        }

        if (sceneName == "ArnisHeavyDialogue")
        {
            loadingScreenManager.LoadScene("ArnisHeavy");
        }

        if (sceneName == "ArnisBossDialogue")
        {
            loadingScreenManager.LoadScene("ArnisBoss");
        }

        if (sceneName == "ArnisLoseDialogue")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        // win dialogue scenes

        if (sceneName == "BoxerWinDialogue")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        if (sceneName == "SikaranWinDialogue")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        if (sceneName == "ArnisWinDialogue")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        if (sceneName == "TindigIntro")
        {
            loadingScreenManager.LoadScene("StageSelect");
        }

        if (sceneName == "FinalBossIntro")
        {
            loadingScreenManager.LoadScene("FinalBoss");
        }

        if (sceneName == "FinalBossOutro")
        {
            loadingScreenManager.LoadScene("TitleScreen");
        }
    }
}

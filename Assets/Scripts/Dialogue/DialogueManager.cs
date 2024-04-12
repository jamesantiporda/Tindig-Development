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

    void Start()
    {
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
            SceneManager.LoadScene("BoxingLight");
        }

        if(sceneName ==  "BoxerHeavyDialogue")
        {
            SceneManager.LoadScene("BoxingHeavy");
        }

        if (sceneName == "BoxerBossDialogue")
        {
            SceneManager.LoadScene("BoxingBoss");
        }

        // sikaran dialogue scenes

        if (sceneName == "SikaranLightDialogue")
        {
            SceneManager.LoadScene("SikaranLight");
        }

        if (sceneName == "SikaranHeavyDialogue")
        {
            SceneManager.LoadScene("SikaranHeavy");
        }

        if (sceneName == "SikaranBossDialogue")
        {
            SceneManager.LoadScene("SikaranBoss");
        }

        // arnis dialogue scenes

        if (sceneName == "ArnisLightDialogue")
        {
            SceneManager.LoadScene("ArnisLight");
        }

        if (sceneName == "ArnisHeavyDialogue")
        {
            SceneManager.LoadScene("ArnisHeavy");
        }

        if (sceneName == "ArnisBossDialogue")
        {
            SceneManager.LoadScene("ArnisBoss");
        }

        // win dialogue scenes

        if (sceneName == "BoxerWinDialogue")
        {
            SceneManager.LoadScene("StageSelect");
        }

    }
}

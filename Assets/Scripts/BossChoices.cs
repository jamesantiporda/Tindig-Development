using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossChoices : MonoBehaviour
{
    private Animator bossChoicesAnim;

    public GameObject selection, fadeToBlack, sikaranLocks, arnisLocks, finalBossLocks;

    private int stageNumber = 0;

    private bool sikaranAreaUnlocked, arnisAreaUnlocked, finalBossUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        bossChoicesAnim = gameObject.GetComponent<Animator>();
        fadeToBlack.SetActive(false);

        if(PlayerPrefs.HasKey("Stage"))
        {
            bossChoicesAnim.SetTrigger("Load");
            bossChoicesAnim.SetInteger("StageToLoad", PlayerPrefs.GetInt("Stage"));
        }

        if(PlayerPrefs.HasKey("SikaranArea"))
        {
            if(PlayerPrefs.GetString("SikaranArea") == "unlocked")
            {
                Debug.Log("SIKARAN UNLOCKED");
                sikaranLocks.SetActive(false);
            }
            else
            {
                sikaranLocks.SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey("ArnisArea"))
        {
            if (PlayerPrefs.GetString("ArnisArea") == "unlocked")
            {
                Debug.Log("ARNIS UNLOCKED");
                arnisLocks.SetActive(false);
            }
            else
            {
                arnisLocks.SetActive(true);
            }
        }

        if (PlayerPrefs.HasKey("FinalBossArea"))
        {
            if (PlayerPrefs.GetString("FinalBossArea") == "unlocked")
            {
                Debug.Log("FINALBOSS UNLOCKED");
                finalBossLocks.SetActive(false);
            }
            else
            {
                finalBossLocks.SetActive(true);
            }
        }

        sikaranAreaUnlocked = !sikaranLocks.activeSelf;
        arnisAreaUnlocked = !arnisLocks.activeSelf;
        finalBossUnlocked = !finalBossLocks.activeSelf;

        bossChoicesAnim.SetBool("sikaranArea", sikaranAreaUnlocked);
        bossChoicesAnim.SetBool("arnisArea", arnisAreaUnlocked);
        bossChoicesAnim.SetBool("finalBoss", finalBossUnlocked);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Right");

            //stageNumber += 1;

            //if(stageNumber >= 4)
            //{
            //    stageNumber = 4;
            //}
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Left");

            //stageNumber -= 1;

            //if (stageNumber <= 1)
            //{
            //    stageNumber = 1;
            //}
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Up");

            //stageNumber += 1;

            //if(stageNumber >= 4)
            //{
            //    stageNumber = 4;
            //}
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Down");

            //stageNumber -= 1;

            //if (stageNumber <= 1)
            //{
            //    stageNumber = 1;
            //}
        }

        if (Input.GetKeyDown(KeyCode.Space) && selection.activeSelf)
        {
            switch(stageNumber)
            {
                case 1:
                    SceneManager.LoadScene("BoxingLight");
                    break;
                case 2:
                    SceneManager.LoadScene("BoxingHeavy");
                    break;
                case 3:
                    SceneManager.LoadScene("BoxingBoss");
                    break;
                case 4:
                    SceneManager.LoadScene("SikaranLight");
                    break;
                case 5:
                    SceneManager.LoadScene("SikaranHeavy");
                    break;
                case 6:
                    SceneManager.LoadScene("SikaranBoss");
                    break;
                case 7:
                    SceneManager.LoadScene("ArnisLight");
                    break;
                case 8:
                    SceneManager.LoadScene("ArnisHeavy");
                    break;
                case 9:
                    SceneManager.LoadScene("ArnisBoss");
                    break;
                case 10:
                    SceneManager.LoadScene("FinalBoss");
                    break;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitToTitle());
        }
    }

    public void EnableSelection()
    {
        selection.SetActive(true);
    }

    private void DisableSelection()
    {
        selection.SetActive(false);
    }

    private IEnumerator ExitToTitle()
    {
        fadeToBlack.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TitleScreen");
    }

    public void SetStageNumber(int stage)
    {
        stageNumber = stage;
        Debug.Log("Current Stage Selected: " + stageNumber);
        PlayerPrefs.SetInt("Stage", stageNumber);
    }

    public void BigSelection()
    {
        selection.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
    }

    public void SmallSelection()
    {
        selection.transform.localScale = new Vector3(1.640625f, 1.640625f, 1.640625f);
    }
}

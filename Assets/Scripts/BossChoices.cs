using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BossChoices : MonoBehaviour
{
    private Animator bossChoicesAnim;

    public GameObject selection, fadeToBlack, sikaranLocks, arnisLocks, finalBossLocks, boxingGruntLocks, sikaranGruntLocks, arnisGruntLocks;

    private int stageNumber = 0;

    private bool sikaranAreaUnlocked, arnisAreaUnlocked, finalBossUnlocked;

    private string[] stages;

    public LoadingScene loadSceneManager;

    public GameObject firstTimeTip;

    public TMP_Text damagePercentText;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("NotFirstTime"))
        {
            firstTimeTip.SetActive(false);
        }

        PlayerPrefs.SetInt("NotFirstTime", 1);

        stages = new string[11];

        stages[1] = "BoxerLightDialogue";
        stages[2] = "BoxerHeavyDialogue";
        stages[3] = "BoxerBossDialogue";
        stages[4] = "SikaranLightDialogue";
        stages[5] = "SikaranHeavyDialogue";
        stages[6] = "SikaranBossDialogue";
        stages[7] = "ArnisLightDialogue";
        stages[8] = "ArnisHeavyDialogue";
        stages[9] = "ArnisBossDialogue";
        stages[10] = "FinalBoss";

        bossChoicesAnim = gameObject.GetComponent<Animator>();
        fadeToBlack.SetActive(false);

        if(PlayerPrefs.HasKey("Stage"))
        {
            bossChoicesAnim.SetTrigger("Load");
            bossChoicesAnim.SetInteger("StageToLoad", PlayerPrefs.GetInt("Stage"));
        }

        if(PlayerPrefs.HasKey("BoxingGrunt"))
        {
            if(PlayerPrefs.GetString("BoxingGrunt") == "unlocked")
            {
                boxingGruntLocks.SetActive(false);
            }
            else
            {
                boxingGruntLocks.SetActive(true);
            }
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

        if (PlayerPrefs.HasKey("SikaranGrunt"))
        {
            if (PlayerPrefs.GetString("SikaranGrunt") == "unlocked")
            {
                sikaranGruntLocks.SetActive(false);
            }
            else
            {
                sikaranGruntLocks.SetActive(true);
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

        if (PlayerPrefs.HasKey("ArnisGrunt"))
        {
            if (PlayerPrefs.GetString("ArnisGrunt") == "unlocked")
            {
                arnisGruntLocks.SetActive(false);
            }
            else
            {
                arnisGruntLocks.SetActive(true);
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

        if(PlayerPrefs.HasKey("DamageModifier"))
        {
            float damagePercent = PlayerPrefs.GetFloat("DamageModifier") * 100;
            damagePercentText.text = damagePercent.ToString("F0") + "%";
        }
        else
        {
            damagePercentText.text = "0%";
        }

        sikaranAreaUnlocked = !sikaranLocks.activeSelf;
        arnisAreaUnlocked = !arnisLocks.activeSelf;
        finalBossUnlocked = !finalBossLocks.activeSelf;

        bossChoicesAnim.SetBool("sikaranArea", sikaranAreaUnlocked);
        bossChoicesAnim.SetBool("arnisArea", arnisAreaUnlocked);
        bossChoicesAnim.SetBool("finalBoss", finalBossUnlocked);
        bossChoicesAnim.SetBool("boxingGrunt", !boxingGruntLocks.activeSelf);
        bossChoicesAnim.SetBool("sikaranGrunt", !sikaranGruntLocks.activeSelf);
        bossChoicesAnim.SetBool("arnisGrunt", !arnisGruntLocks.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {
        if(!firstTimeTip.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (stageNumber != 10)
                {
                    DisableSelection();
                    bossChoicesAnim.SetTrigger("Right");
                }

                //stageNumber += 1;

                //if(stageNumber >= 4)
                //{
                //    stageNumber = 4;
                //}
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (stageNumber != 0)
                {
                    DisableSelection();
                    bossChoicesAnim.SetTrigger("Left");
                }

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

            if (Input.GetKeyDown(KeyCode.Space) && selection.activeSelf && stageNumber != 0)
            {
                StartCoroutine(LoadStage());
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(ExitToTitle());
            }
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

    private IEnumerator LoadStage()
    {
        fadeToBlack.SetActive(true);
        yield return new WaitForSeconds(1);
        loadSceneManager.LoadScene(stages[stageNumber]);

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

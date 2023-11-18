using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossChoices : MonoBehaviour
{
    private Animator bossChoicesAnim;

    public GameObject selection;

    private int stageNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        bossChoicesAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Right");

            stageNumber += 1;

            if(stageNumber >= 3)
            {
                stageNumber = 3;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Left");

            stageNumber -= 1;

            if (stageNumber <= 1)
            {
                stageNumber = 1;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(stageNumber == 1)
            {
                Debug.Log("LOAD");
                SceneManager.LoadScene("BoxingBoss");
            }
            else if(stageNumber == 2)
            {
                Debug.Log("LOAD");
                SceneManager.LoadScene("SikaranBoss");
            }
            else if(stageNumber == 3)
            {
                Debug.Log("LOAD");
                SceneManager.LoadScene("ArnisBoss");
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
}

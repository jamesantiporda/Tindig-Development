using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{

    public Animator bossChoicesAnim;

    public GameObject selection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Right");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            DisableSelection();
            bossChoicesAnim.SetTrigger("Left");
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

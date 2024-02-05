using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylesUpdated : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("NewStyleUnlocked") == 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            PlayerPrefs.SetInt("NewStyleUnlocked", 0);
            gameObject.SetActive(false);
        }
        
    }
}

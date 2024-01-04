using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    public GameObject boxingMap, aceMap, beachMap, lunetaMap, random, selection;

    public GameObject mapPanel;

    private GameObject[] maps;

    private int selectionNumber;

    // Start is called before the first frame update
    void Start()
    {
        maps = new GameObject[5];

        maps[0] = boxingMap;
        maps[1] = aceMap;
        maps[2] = beachMap;
        maps[3] = lunetaMap;
        maps[4] = random;
    }

    // Update is called once per frame
    void Update()
    {
        if(mapPanel.activeSelf)
        {
            if (selectionNumber < 0)
            {
                selectionNumber = 0;
            }
            else if (selectionNumber > maps.Length - 1)
            {
                selectionNumber = maps.Length - 1;
            }

            selection.transform.position = maps[selectionNumber].transform.position;

            if (Input.GetKeyDown(KeyCode.A))
            {
                selectionNumber -= 1;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                selectionNumber += 1;
            }

        }
    }

    public void StartGame()
    {
        if(selectionNumber == 4)
        {
            selectionNumber = UnityEngine.Random.Range(0, 3);

        }

        switch (selectionNumber)
        {
            case 0:
                SceneManager.LoadScene("BoxingMap");
                break;
            case 1:
                SceneManager.LoadScene("AceMap");
                break;
            case 2:
                SceneManager.LoadScene("BeachMap");
                break;
            case 3:
                SceneManager.LoadScene("LunetaMap");
                break;
            default:
                Debug.Log("Error! Not in Choices");
                break;
        }
    }
}

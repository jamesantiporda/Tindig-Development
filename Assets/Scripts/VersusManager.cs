using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersusManager : MonoBehaviour
{
    public GameObject player1MC, player2MC, player1NotMC, player2NotMC, p1HitPoint, p2HitPoint;

    public PlayerMovement player1Movement, player2Movement;
    public PlayerCombat player1Combat, player2Combat;

    private int player1Character, player2Character, map;

    private GameObject player1CurrentSprite, player2CurrentSprite;

    public RuntimeAnimatorController boxingController, sikaranController, arnisController;

    public GameObject boxingStage, aceStage, beachStage, lunetaStage;

    // Start is called before the first frame update
    private void Awake()
    {
        player1Character = PlayerPrefs.GetInt("P1");
        player2Character = PlayerPrefs.GetInt("P2");

        map = PlayerPrefs.GetInt("Map");
        aceStage.SetActive(false);

        switch(map)
        {
            case 0:
                boxingStage.SetActive(true);
                break;
            case 1:
                aceStage.SetActive(true);
                break;
            case 2:
                beachStage.SetActive(true);
                break;
            case 3:
                lunetaStage.SetActive(true);
                break;
            default:
                break;
        }

        if (player1Character == 5)
        {
            player1Character = UnityEngine.Random.Range(0, 4);
            PlayerPrefs.SetInt("P1", player1Character);

        }

        if (player2Character == 5)
        {
            player2Character = UnityEngine.Random.Range(0, 4);
            PlayerPrefs.SetInt("P2", player2Character);

        }

        switch (player1Character)
        {
            case 1:
                player1MC.SetActive(false);
                player1NotMC.SetActive(true);
                player1CurrentSprite = player1NotMC;
                player1CurrentSprite.GetComponent<Animator>().runtimeAnimatorController = boxingController;
                break;
            case 2:
                player1MC.SetActive(false);
                player1NotMC.SetActive(true);
                player1CurrentSprite = player1NotMC;
                player1CurrentSprite.GetComponent<Animator>().runtimeAnimatorController = sikaranController;
                player1CurrentSprite.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 3:
                player1MC.SetActive(false);
                player1NotMC.SetActive(true);
                player1CurrentSprite = player1NotMC;
                player1CurrentSprite.GetComponent<Animator>().runtimeAnimatorController = arnisController;
                player1CurrentSprite.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 4:
                Debug.Log("FinalBoss");
                player1CurrentSprite = player1MC;
                player1Movement.isMC = true;
                player1Combat.isMC = true;
                break;
            default:
                Debug.Log("MC");
                player1CurrentSprite = player1MC;
                player1Movement.isMC = true;
                player1Combat.isMC = true;
                break;
        }

        switch (player2Character)
        {
            case 1:
                player2MC.SetActive(false);
                player2NotMC.SetActive(true);
                player2CurrentSprite = player2NotMC;
                player2CurrentSprite.GetComponent<Animator>().runtimeAnimatorController = boxingController;
                break;
            case 2:
                player2MC.SetActive(false);
                player2NotMC.SetActive(true);
                player2CurrentSprite = player2NotMC;
                player2CurrentSprite.GetComponent<Animator>().runtimeAnimatorController = sikaranController;
                player2CurrentSprite.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 3:
                player2MC.SetActive(false);
                player2NotMC.SetActive(true);
                player2CurrentSprite = player2NotMC;
                player2CurrentSprite.GetComponent<Animator>().runtimeAnimatorController = arnisController;
                player2CurrentSprite.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case 4:
                Debug.Log("FinalBoss");
                player2CurrentSprite = player2MC;
                player2Movement.isMC = true;
                player2Combat.isMC = true;
                break;
            default:
                Debug.Log("MC");
                player2CurrentSprite = player2MC;
                player2Movement.isMC = true;
                player2Combat.isMC = true;
                break;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player1Character < 4 && player1Character > 0)
        {
            player1Combat.SetSprite(player1NotMC, p2HitPoint);
            player1Movement.SetSprite(player1NotMC);
        }

        if(player2Character < 4 && player2Character > 0)
        {
            player2Combat.SetSprite(player2NotMC, p2HitPoint);
            player2Movement.SetSprite(player2NotMC);
        }

        gameObject.SetActive(false);
    }
}

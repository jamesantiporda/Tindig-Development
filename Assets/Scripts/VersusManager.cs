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

    public RuntimeAnimatorController boxingController, sikaranController, arnisController, finalbossBoxingController, finalbossSikaranController, finalBossArnisController;

    public GameObject p1MCDefault, p1MCBoxing, p1MCSikaran, p1MCArnis, p2MCDefault, p2MCBoxing, p2MCSikaran, p2MCArnis;

    public GameObject p1DefaultSprite, p2DefaultSprite;

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
                player1CurrentSprite = p1DefaultSprite;
                player1CurrentSprite.transform.localScale = new Vector3(2, 2, 2);
                p1MCDefault.GetComponent<Animator>().runtimeAnimatorController = finalbossBoxingController;
                p1MCBoxing.GetComponent<Animator>().runtimeAnimatorController = finalbossBoxingController;
                p1MCSikaran.GetComponent<Animator>().runtimeAnimatorController = finalbossSikaranController;
                p1MCArnis.GetComponent<Animator>().runtimeAnimatorController = finalBossArnisController;
                p1MCDefault.GetComponent<SpriteRenderer>().color = Color.white;
                p1MCBoxing.GetComponent<SpriteRenderer>().color = Color.white;
                p1MCSikaran.GetComponent<SpriteRenderer>().color = Color.white;
                p1MCArnis.GetComponent<SpriteRenderer>().color = Color.white;
                player1Movement.isMC = true;
                player1Combat.isMC = true;
                break;
            default:
                Debug.Log("MC");
                player1CurrentSprite = p1DefaultSprite;
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
                player2CurrentSprite = p2DefaultSprite;
                player2CurrentSprite.transform.localScale = new Vector3(2, 2, 2);
                p2MCDefault.GetComponent<Animator>().runtimeAnimatorController = finalbossBoxingController;
                p2MCBoxing.GetComponent<Animator>().runtimeAnimatorController = finalbossBoxingController;
                p2MCSikaran.GetComponent<Animator>().runtimeAnimatorController = finalbossSikaranController;
                p2MCArnis.GetComponent<Animator>().runtimeAnimatorController = finalBossArnisController;
                player2Movement.isMC = true;
                player2Combat.isMC = true;
                break;
            default:
                Debug.Log("MC");
                player2CurrentSprite = p2DefaultSprite;
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

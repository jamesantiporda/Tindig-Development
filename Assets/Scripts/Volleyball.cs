using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volleyball : MonoBehaviour
{
    public GameObject volleyballPrefab;

    private float attackTimer = 0.0f;
    private float maxTime = 0.70f;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void DestroyVolleyball()
    {
        Destroy(gameObject);
    }

    public float ReturnActiveTime()
    {
        return attackTimer;
    }
}

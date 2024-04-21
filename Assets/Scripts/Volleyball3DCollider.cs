using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volleyball3DCollider : MonoBehaviour
{
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("COLLIDING");
            if (transform.position.x > 0)
            {
                parent.transform.Translate(Vector3.left * Time.deltaTime * 1);
            }
            else if (transform.position.x < 0)
            {
                parent.transform.Translate(Vector3.right * Time.deltaTime * 1);
            }
        }
    }
}

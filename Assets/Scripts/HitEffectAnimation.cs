using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectAnimation : MonoBehaviour
{
    void HitDone()
    {
        Destroy(gameObject);
    }
}

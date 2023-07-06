using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float time = 0.6f;
    void OnEnable()
    {
        Destroy(gameObject, time);
    }
}

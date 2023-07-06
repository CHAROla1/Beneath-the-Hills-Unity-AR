using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLocationManager : MonoBehaviour
{
    public static ARLocationManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
            // Your initialization code here...
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
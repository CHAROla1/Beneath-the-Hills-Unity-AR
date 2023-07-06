using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageManager : MonoBehaviour
{

    public static PackageManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            descriptions = new Dictionary<string, string>();
        }
        else
        {
            Debug.Log("PackageManager instance already exists!");
            Destroy(this);
        }
    }

    Dictionary<string, string> descriptions;

    public void AddDescription(string descriptionName, string description)
    {
        descriptions.Add(descriptionName, description);
    }

    public string GetDescription(string descriptionName)
    {
        return descriptions[descriptionName];
    }
}

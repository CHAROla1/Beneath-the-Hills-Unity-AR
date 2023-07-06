using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epoch : MonoBehaviour
{
    [SerializeField] GameObject epochPackageItems;
    [SerializeField] string hint;


    // Start is called before the first frame update
    void Start()
    {
        EpochSceneManager.Instance.hintText.text = hint; // set hint text
        if (epochPackageItems != null)
        {
            PackageManager.instance.AddDescription(gameObject.name, EpochSceneManager.Instance.GetDescription(gameObject.name)); // add description to package manager
            epochPackageItems.SetActive(true); // show the corresponding items in this epoch
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;
using UnityEditor;
using MoreMountains.Feedbacks;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject epochPopUp;
    GameObject skyboxCamera;
    [SerializeField] MMF_Player feedbacks;
    public Material[] materials;
    Transform device;

    bool wasInFront;
    bool inOtherWorld;
    bool isEpoch = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<ARAnchor>()) gameObject.AddComponent<ARAnchor>();
        EpochSceneManager.Instance.currentPortal = this;
        skyboxCamera = GameObject.Find("Skybox Camera");
        // gameObject.AddComponent<ARAnchor>();
        device = Camera.main.transform;
        materials = Resources.LoadAll<Material>("Materials/FilterMat");
        SetMaterials(false);
    }

    void SetMaterials(bool fullRender)
    {
        var stencilTest = fullRender ? CompareFunction.NotEqual : CompareFunction.Equal;
        foreach (var mat in materials)
        {
            mat.SetInt("_StencilTest", (int)stencilTest);
        }
    }

    void LateUpdate()
    {
        transform.forward = -device.forward;
    }

    // bool GetIsInFront()
    // {
    //     Vector3 pos = transform.InverseTransformPoint(device.position);
    //     return pos.z >= 0 ? true : false;
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform != device)
            return;

        StartCoroutine(IntoPortal());
        // Destroy(transform.parent.gameObject);
    }

    // void OnTriggerStay(Collider other)
    // {
    //     if (other.transform != device)
    //         return;

    //     bool isInFront = GetIsInFront();
    //     Debug.Log("Is in front: " + isInFront);
    //     Debug.Log("Was in front: " + wasInFront);
    //     if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
    //     {
    //         inOtherWorld = !inOtherWorld;
    //         SetMaterials(inOtherWorld);
    //         Debug.Log("In other world: " + inOtherWorld);
    //     }
    //     wasInFront = isInFront;
    // }

    public void Reset()
    {
        inOtherWorld = false;
        SetMaterials(inOtherWorld);
    }


    IEnumerator IntoPortal()
    {
        feedbacks.PlayFeedbacks();
        yield return new WaitForSeconds(3f);
        // if (transform.parent.name == EpochSceneManager.Instance._scenes[0].name) // if the player enters the portal of the first epoch
        //     TimeLineManager.Instance.Next();
        if (!isEpoch)
        {
            isEpoch = true; // the first time the player enters the portal, show the epoch popup
            epochPopUp = Instantiate(epochPopUp, GameObject.Find("Canvas").transform);
            epochPopUp.GetComponent<Popup>().SetText(EpochSceneManager.Instance.GetDescription());
            skyboxCamera?.SetActive(false);
        }

        EpochSceneManager.Instance.NextScene();
        inOtherWorld = !inOtherWorld;
        SetMaterials(inOtherWorld);
        // play the music of current epoch
        EpochSceneManager.Instance.currentScene.GetComponent<AudioSource>()?.Play();
        Destroy(gameObject);
    }
}

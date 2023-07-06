using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasFollowPlayer : MonoBehaviour
{
    // GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = EpochSceneManager.Instance.nextScene?.name;
        // player = Camera.main.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // transform.forward = player.transform.forward;
    }
}

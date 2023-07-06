using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{

    Transform player;

    void Start()
    {
        player = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // get the parent's position
        Vector3 parentPosition = player.position;

        // set the target position to be above the parent
        Vector3 targetPosition = parentPosition + Vector3.up * 100;
        transform.position = targetPosition;

        // set the rotation to match the parent's rotation
        Quaternion targetRotation = Quaternion.Euler(90f, player.rotation.eulerAngles.y, 0f);
        transform.rotation = targetRotation;

    }
}

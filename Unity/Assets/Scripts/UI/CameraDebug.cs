using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraDebug : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    GameObject skyCamera;
    // Start is called before the first frame update
    void Start()
    {
        skyCamera = GameObject.Find("Skybox Camera");
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "location: " + transform.position.ToString() + "\nrotation: " + transform.eulerAngles.ToString();
        if (Input.GetKey("up")) { transform.Translate(0, 0, 0.1f); }

        if (Input.GetKey("down")) { transform.Translate(0, 0, -0.1f); }

        if (Input.GetKey("left")) { transform.Rotate(0, -3, 0); }

        if (Input.GetKey("right")) { transform.Rotate(0, 3, 0); }
    }

    public void SetSkyboxCamera()
    {
        if (skyCamera)
        {
            if (skyCamera.activeSelf) skyCamera.SetActive(false);
            else skyCamera.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
public class PutLocation : MonoBehaviour
{
    [Tooltip("The latitude, in degrees.")]
    public double latitude;

    [Tooltip("The longitude, in degrees.")]
    public double longitude;

    public TextMeshProUGUI text;


    bool GPSEnabled = false;

    private void Awake()
    {
        if (!GetComponent<SelectableObj>())
            gameObject.AddComponent<SelectableObj>();
        gameObject.AddComponent<ARAnchor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateLocation(latitude, longitude);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = transform.position.ToString();
        if (!GPSEnabled)
        {
            if (GPSLocation.Instance.GPSEnabled)
            {
                UpdateLocation(latitude, longitude);
                GPSEnabled = true;
            }
        }

    }

    public float SceneDistance
    {
        get
        {
            var cameraPos = GPSLocation.Instance.mainCameraTransform.position;

            return Vector3.Distance(cameraPos, transform.position);
        }
    }


    public void UpdateLocation(double latitude, double longitude)
    {
        // GPSEncoder.SetLocalOrigin(new DoubleVector2(48.1805877685547, 11.5150718688965));
        DoubleVector3 v = GPSEncoder.GPSToUCS(latitude, longitude);
        transform.position = new Vector3((float)v.x, (float)v.y, (float)v.z);
    }


}

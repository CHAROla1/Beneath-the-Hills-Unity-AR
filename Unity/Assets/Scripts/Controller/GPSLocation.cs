using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GPSLocation : MonoBehaviour
{
    public static GPSLocation Instance { get; private set; }

    Transform canvas;
    public TextMeshProUGUI waitingText;
    public TextMeshProUGUI GPSText;
    // TextMeshProUGUI positionText;
    public bool GPSEnabled = false;

    public DoubleVector2 rootlocation = new DoubleVector2(0, 0);

    public Transform mainCameraTransform;


    // Start is called before the first frame update
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
        mainCameraTransform = transform;
        canvas = GameObject.Find("Canvas").transform;
        // positionText = canvas.Find("Position").GetComponent<TextMeshProUGUI>();
        InitGPS();
        StartCoroutine(StartGPS());
    }

    // Update is called once per frame
    void Update()
    {
        // positionText.text = "Position: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z;
        if (GPSEnabled)
        {
            rootlocation.y = Input.location.lastData.longitude;
            rootlocation.x = Input.location.lastData.latitude;
            UpdateMessage("Location: " + rootlocation.x + ", " + rootlocation.y);
            // transform.position = new Vector3(Input.location.lastData.longitude, Input.location.lastData.altitude, Input.location.lastData.latitude);
        }
    }

    IEnumerator StartGPS()
    {
        // First, check if user has location service enabled
        while (!Input.location.isEnabledByUser)
        {
            waitingText.text = "GPS is not enabled";
            yield return null;
        }

        // Start service before querying location
        Input.location.Start(5, 1);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            waitingText.text = "Initializing GPS...";
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            waitingText.text = "Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            waitingText.text = "Unable to determine device location";
            yield break;
        }

        // Access granted and location value could be retrieved
        else
        {
            GPSEnabled = true;
            rootlocation = new DoubleVector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
            UpdateMessage("Location: " + rootlocation.x + ", " + rootlocation.y);
            GPSEncoder.SetLocalOrigin(rootlocation);
            Input.compass.enabled = true;
        }
    }

    private void UpdateMessage(string message)
    {
        GPSText.text = message;
    }

    private void InitGPS()
    {
#if PLATFORM_ANDROID

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }

#endif
    }

    private void OnApplicationQuit()
    {
        GPSEnabled = false;
        Input.location.Stop();
    }
}

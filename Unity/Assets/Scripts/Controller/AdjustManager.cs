using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mapbox.Examples;
using UnityEngine.XR.ARFoundation;
using Mapbox.Unity.Map;

public class AdjustManager : MonoBehaviour
{
    public static AdjustManager Instance;

    private void Awake()
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

    public GameObject ARSessionOrigin;
    public GameObject mapPrefab;
    public GameObject wayPoint;
    public GameObject map { get; set; }
    public TextMeshProUGUI angleText;
    public bool finish; // finish generating map
    bool start = false; // start generating map
    bool layerFlag = false; // change map layer

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(NextTimeStep()); // Next time step, initialize the map

        ARSessionOrigin.GetComponent<ARSessionOrigin>().enabled = true;
        if (map) Destroy(map);
        map = Instantiate(mapPrefab);
        foreach (Transform child in map.transform)
        {
            child.gameObject.layer = 6;
        }
        // EpochSceneManager.Instance.NextScene(); // start the first scene
        wayPoint.SetActive(true); // start the navigation
        finish = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (GPSLocation.Instance.GPSEnabled)
        {
            angleText.text = Input.compass.trueHeading.ToString("F2");
            if (!start)
            {
                StartCoroutine(AdjustMap());
                start = true;
            }
        }
        if (!layerFlag) // change map layer
        {
            ChangeMapLayer();
        }

    }

    IEnumerator AdjustMap()
    {
        yield return new WaitForSeconds(1.5f);
        // 获取设备初始朝向
        while (Input.compass.trueHeading > 1.5f && Input.compass.trueHeading < 358f)
        {
            yield return null;
        }

        ARSessionOrigin.GetComponent<ARSessionOrigin>().enabled = true;
        // ARSessionOrigin.transform.forward = Camera.main.transform.forward;
        // ARSessionOrigin.transform.localEulerAngles = new Vector3(0f, ARSessionOrigin.transform.localEulerAngles.y, 0f);
        if (map) Destroy(map);
        map = Instantiate(mapPrefab);
        // map.GetComponent<AbstractMap>().ImageLayer.LayerType = Mapbox.Unity.Map.ImagerySourceType.MapboxStreets;
        // map.transform.localEulerAngles = new Vector3(0f, -90.0f, 0f);
        foreach (Transform child in map.transform)
        {
            child.gameObject.layer = 6;
        }
        wayPoint.SetActive(true); // start the navigation
        finish = true;

        ARSessionOrigin.GetComponent<ImmediatePositionWithLocationProvider>().enabled = true;

        StartCoroutine(NextTimeStep()); // Next time step, initialize the map
    }

    void ChangeMapLayer()
    {
        if (map)
        {
            if (map.transform.GetChild(0).childCount > 0)
            {
                if (map.transform.GetChild(0).GetChild(0).gameObject.layer != 6)
                {
                    foreach (Transform child in map.transform.GetChild(0))
                    {
                        layerFlag = true;
                        child.gameObject.layer = 6;
                    }
                }
            }
        }
    }

    IEnumerator NextTimeStep()
    {
        TimeLineManager.Instance.Next();
        yield return new WaitForSeconds(3.5f);
        TimeLineManager.Instance.Next();

        yield return new WaitForSeconds(7.5f);
        TimeLineManager.Instance.Next();
    }
}

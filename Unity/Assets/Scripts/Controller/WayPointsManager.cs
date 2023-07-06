using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(WayPointsManager))]
public class WayPointsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.HelpBox("Please place the waypoints in order from top to down in the `SpawnOnMap` component.", MessageType.Warning);

        // DrawDefaultInspector();
    }
}
#endif

[RequireComponent(typeof(SpawnOnMap))]
public class WayPointsManager : MonoBehaviour
{
    public static WayPointsManager Instance { get; private set; }

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


    List<GameObject> _wayPoints;
    public GameObject currentWayPoint { get; set; }

    [SerializeField, Tooltip("The arrow that points to the next waypoint.")]
    GameObject directionArrow;

    // Start is called before the first frame update
    void Start()
    {
        _wayPoints = GetComponent<SpawnOnMap>()._spawnedObjects;
        currentWayPoint = _wayPoints[0];
        // directionArrow.GetComponent<DirectionArrow>().target = currentWayPoint.transform; // set the first waypoint as the target of the arrow
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextPoint()
    {
        int index = _wayPoints.IndexOf(currentWayPoint);
        if (index < _wayPoints.Count - 1)
        {
            Destroy(currentWayPoint);
            currentWayPoint = _wayPoints[index + 1];
        }
        else
        {
            Destroy(currentWayPoint);
            directionArrow.SetActive(false);
            Debug.Log("You have reached the last waypoint.");

            //TODO: End of the game
        }
    }
}

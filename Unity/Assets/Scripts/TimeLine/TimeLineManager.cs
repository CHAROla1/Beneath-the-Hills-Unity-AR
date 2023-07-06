using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(TimeLineManager))]
public class TimeLineManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TimeLineManager myScript = (TimeLineManager)target;

        EditorGUILayout.HelpBox("Attention! Put the time line according to the order that you want to play from up and down.", MessageType.Warning);

        DrawDefaultInspector();
    }
}
#endif

public class TimeLineManager : MonoBehaviour
{


    public static TimeLineManager Instance { get; private set; }

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

    List<GameObject> _timelines;
    GameObject _currentTimeline;

    // Start is called before the first frame update
    void Start()
    {
        _timelines = new List<GameObject>();
        foreach (Transform child in transform)
        {
            _timelines.Add(child.gameObject);
        }
        _currentTimeline = _timelines[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Play the next time line.
    /// </summary>
    public void Next()
    {
        if (!_currentTimeline)
        {
            _currentTimeline = _timelines[0];
            _currentTimeline.GetComponent<PlayableDirector>().Play();
        }
        else
        {
            _currentTimeline.GetComponent<PlayableDirector>().Stop();
            int index = _timelines.IndexOf(_currentTimeline);
            if (index < _timelines.Count - 1)
            {
                _currentTimeline = _timelines[index + 1];
                _currentTimeline.GetComponent<PlayableDirector>().Play();
            }
        }
    }

    /// <summary>
    /// Play the end time line.
    /// </summary>
    public void End()
    {
        _currentTimeline = _timelines[_timelines.Count - 1];
        _currentTimeline.GetComponent<PlayableDirector>().Play();
    }
}

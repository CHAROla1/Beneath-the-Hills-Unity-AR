using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    Camera _mapCamera;
    RectTransform standPoint;

    // Start is called before the first frame update
    void Start()
    {
        _mapCamera = GameObject.Find("Map Camera").GetComponent<Camera>();
        standPoint = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void ZoomIn()
    {
        _mapCamera.orthographicSize -= 30;
        standPoint.localScale = new Vector3(standPoint.localScale.x * 1.1f, standPoint.localScale.y * 1.1f, standPoint.localScale.z);
        if (_mapCamera.orthographicSize < 30)
        {
            _mapCamera.orthographicSize = 30;
            standPoint.localScale = new Vector3(standPoint.localScale.x / 1.1f, standPoint.localScale.y / 1.1f, standPoint.localScale.z);
        }
    }

    public void ZoomOut()
    {
        _mapCamera.orthographicSize += 30;
        standPoint.localScale = new Vector3(standPoint.localScale.x * 0.9f, standPoint.localScale.y * 0.9f, standPoint.localScale.z);
        if (_mapCamera.orthographicSize > 360)
        {
            _mapCamera.orthographicSize = 360;
            standPoint.localScale = new Vector3(standPoint.localScale.x / 0.9f, standPoint.localScale.y / 0.9f, standPoint.localScale.z);
        }
    }
}

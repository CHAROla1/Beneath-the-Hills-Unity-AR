using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectManager : MonoBehaviour
{
    public static SelectManager Instance { get; private set; }

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
        canvas = GameObject.Find("Canvas").transform;
    }

    public Camera mainCrma;
    public GameObject aimPrefab;
    private RaycastHit objhit;
    private Ray _ray;
    Transform canvas;

    void Update()
    {
        if (mainCrma == null)
        {
            mainCrma = Camera.main;
            canvas = GameObject.Find("Canvas").transform;
        }

        if (Input.GetMouseButtonDown(0))
        {
            int layerMask = 1 << 7;
            _ray = mainCrma.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(_ray.origin, objhit.point, Color.red, 2);
            if (Physics.Raycast(_ray, out objhit, 500, layerMask))
            {
                // create aim
                var aim = Instantiate(aimPrefab, canvas, false);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas as RectTransform, Input.mousePosition, null, out Vector2 pos))
                {
                    aim.GetComponent<RectTransform>().anchoredPosition = pos;
                }
                GameObject gameObj = objhit.collider.gameObject;
                if (gameObj.CompareTag("Selectable"))
                    gameObj.GetComponent<SelectableObj>()?.SelectEvent();
            }

        }
    }
}

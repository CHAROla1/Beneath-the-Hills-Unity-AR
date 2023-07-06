using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonCreateAction : MonoBehaviour
{
    [SerializeField] GameObject _object;
    [SerializeField] bool isUIObject;
    [SerializeField] bool isIntantiate;
    Transform _canvas;
    Button _button;

    // Start is called before the first frame update
    void Start()
    {
        _canvas = GameObject.Find("Canvas").transform;
        _button = GetComponent<Button>();
        if (isUIObject)
            _button.onClick.AddListener(CreateObjectOnCanvas);
        else
            _button.onClick.AddListener(CreateObject);
    }

    public void CreateObject()
    {
        if (isIntantiate)
            Instantiate(_object, transform.position, transform.rotation);
        else
            _object.SetActive(true);
    }

    public void CreateObjectOnCanvas()
    {
        if (isIntantiate)
            Instantiate(_object, _canvas);
        else
            _object.SetActive(true);
    }
}

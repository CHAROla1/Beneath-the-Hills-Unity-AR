using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCreator : MonoBehaviour
{
    [SerializeField] GameObject popupPrefab;
    [SerializeField] GameObject popupCanvas;
    string text = "";

    void Start()
    {
        text = PackageManager.instance.GetDescription(gameObject.name);
        GetComponent<Button>().onClick.AddListener(CreatePopup);
    }

    public void CreatePopup()
    {
        var popup = Instantiate(popupPrefab, popupCanvas.transform);
        popup.GetComponent<Popup>()?.SetText(text);
    }
}

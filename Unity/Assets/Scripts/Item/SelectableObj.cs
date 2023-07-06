using System.Collections;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using UnityEngine.XR.ARFoundation;
using Mapbox.Examples;

public class SelectableObj : MonoBehaviour
{
    [Header("Selectable Object Class")]
    [SerializeField] MMFeedbacks _selectFeedbacks;
    [SerializeField] GameObject _popupPrefab;
    [SerializeField] Canvas mapLogo;
    [TextArea, SerializeField] string _description;

    // Start is called before the first frame update
    protected void Start()
    {
        tag = "Selectable";
        PackageManager.instance.AddDescription(gameObject.name, _description); // add description to package manager
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<MMAutoRotate>()?.OrbitCenterTransform == null) GetComponent<MMAutoRotate>().OrbitCenterTransform = Camera.main.transform;
    }

    public virtual void SelectEvent()
    {
        tag = "Untagged";
        GetComponent<Outline>().enabled = false;

        _selectFeedbacks?.PlayFeedbacks();
        EpochSceneManager.Instance.CollectItem(this.gameObject);
        if (transform.GetComponentInParent<SpawnOnMap>())
            transform.GetComponentInParent<SpawnOnMap>().numberOfMissionItems--;
        StartCoroutine(FlyToPlayer());
    }

    IEnumerator FlyToPlayer()
    {
        SelectManager.Instance.enabled = false; // disable the select manager
        if (transform.GetComponentInParent<SpawnOnMap>())
            transform.GetComponentInParent<SpawnOnMap>()._spawnedObjects.Remove(gameObject); // stop updating the position of the object according to map

        if (mapLogo)
            mapLogo.enabled = false; // hide the map logo
        Vector3 playerPosition = Camera.main.transform.position + Camera.main.transform.forward * 3f;
        while (Vector3.Distance(transform.position, playerPosition) > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, 0.04f);
            yield return null;
        }

        // after flying to player, show description of the item
        var popup = Instantiate(_popupPrefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<Popup>().SetText(_description);
        ObjectDestroyer.ObjectDestroyed.AddListener(OnPopupDestroyed);
    }

    void OnPopupDestroyed(GameObject popup)
    {
        SelectManager.Instance.enabled = true; // enable the select manager
        int numberOfMissionItems = 0;
        if (transform.GetComponentInParent<SpawnOnMap>())
            numberOfMissionItems = transform.GetComponentInParent<SpawnOnMap>().numberOfMissionItems;

        if (numberOfMissionItems > 0)
        { // if there are still items in the map, except portal
            // after popup destroyed, start rotating
            if (!GetComponent<MMAutoRotate>()) gameObject.AddComponent<MMAutoRotate>();
            GetComponent<MMAutoRotate>().Orbit(true);
            GetComponent<MMAutoRotate>().OrbitCenterTransform = Camera.main?.transform;
            GetComponent<MMAutoRotate>().OrbitCenterOffset = new Vector3(0f, -1f, 0f); // keep the item below the camera
            GetComponent<MMAutoRotate>().OrbitRotationSpeed = 200f;
        }
        else
        { // all the items are collected
            EpochSceneManager.Instance.ShowNextPortal();
        }


        ObjectDestroyer.ObjectDestroyed.RemoveListener(OnPopupDestroyed);
    }
}

namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;
    using UnityEngine.Assertions;
    using TMPro;
    using UnityEngine.XR.ARFoundation;

    public class SpawnOnMap : MonoBehaviour
    {
        // [SerializeField]
        AbstractMap _map;

        [SerializeField]
        [Geocode]
        string[] _locationStrings;

        Vector2d[] _locations;
        [SerializeField, Tooltip("The height (corresponding to player) of the items")] List<float> height;


        [SerializeField] bool useGPS;
        [SerializeField] float spawnRadius = 20f;

        // [SerializeField]
        // float _spawnScale = 100f;

        [SerializeField]
        List<GameObject> _spawnedObjectsPrefab;

        public int numberOfMissionItems;

        [SerializeField, Header("Debug Only (optional)")]
        TextMeshProUGUI _positionText;
        GameObject player;

        public List<GameObject> _spawnedObjects { get; private set; }

        void Awake()
        {
            Assert.AreEqual(_locationStrings.Length, height.Count); // The number of locations must be equal to the number of heights.

            // if (!GetComponent<ARAnchor>()) gameObject.AddComponent<ARAnchor>();
        }


        void OnEnable()
        {
            _map = AdjustManager.Instance?.map.transform.GetChild(0).GetComponent<AbstractMap>();
            player = EpochSceneManager.Instance.player;
            _spawnedObjects = new List<GameObject>();
            if (_map && useGPS)
            {
                _locations = new Vector2d[_locationStrings.Length];
                for (int i = 0; i < _locationStrings.Length; i++)
                {
                    var locationString = _locationStrings[i];
                    _locations[i] = Conversions.StringToLatLon(locationString);
                    GameObject instance;
                    int index = i;
                    if (_spawnedObjectsPrefab.Count < i + 1)// if the number of prefabs is less than the number of locations, use the last prefab to spawn the rest of the locations
                        index = _spawnedObjectsPrefab.Count - 1;
                    instance = Instantiate(_spawnedObjectsPrefab[index], _map.GeoToWorldPosition(_locations[i], true), Quaternion.identity);
                    // Camera.main.gameObject.SetActive(true);
                    instance.name = instance.name.Replace("(Clone)", "");
                    instance.transform.parent = transform;
                    instance.transform.localPosition += Vector3.up * (height[i] + player.transform.position.y);
                    _spawnedObjects.Add(instance);
                    // instance.AddComponent<ARAnchor>();
                    instance.gameObject.layer = 7; // set the layer to "VirtualItem"
                }
            }

            if (!useGPS)
            {
                int index = 0;
                foreach (GameObject obj in _spawnedObjectsPrefab)
                {
                    GameObject instance;
                    instance = Instantiate(obj, GetRandomPosition(height[index]), Quaternion.identity);
                    // Camera.main.gameObject.SetActive(true);
                    instance.name = instance.name.Replace("(Clone)", "");
                    instance.transform.parent = transform;
                    _spawnedObjects.Add(instance);
                    instance.AddComponent<ARAnchor>();
                    instance.gameObject.layer = 7; // set the layer to "VirtualItem"
                    index++;
                }
            }

        }

        private void Update()
        {
            if (_map && useGPS)
            {
                int count = _spawnedObjects.Count;
                for (int i = 0; i < count; i++)
                {
                    var spawnedObject = _spawnedObjects[i];
                    var location = _locations[i];
                    if (!spawnedObject) continue;
                    spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                    spawnedObject.transform.localPosition += Vector3.up * (height[i] + player.transform.position.y);
                }
                if (_positionText && count > 0 && _spawnedObjects[0]) _positionText.text = _spawnedObjects[0]?.transform.localPosition.ToString();
            }
        }

        private Vector3 GetRandomPosition(float height)
        {
            // random position in a circle
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = player.transform.position + new Vector3(randomCircle.x, height, randomCircle.y);
            return spawnPosition;
        }
    }
}
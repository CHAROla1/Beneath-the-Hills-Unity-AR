using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;

public class MoneyFlying : MonoBehaviour
{
    [SerializeField] float speed = 15f;
    [SerializeField] float height = 50f;
    [SerializeField] ParticleSystem moneyParticle;
    [SerializeField] ParticleSystem moneyBlast;
    [SerializeField, Geocode] string _destinationLocation;
    [SerializeField] bool useGPS;
    [SerializeField] Vector3 destination;


    AbstractMap _map;
    GameObject player;
    Vector3 _destination;
    Vector3 _skyPoint;
    bool inTheSky = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        _map = AdjustManager.Instance?.map.transform.GetChild(0).GetComponent<AbstractMap>();
        if (useGPS)
            _destination = _map.GeoToWorldPosition(Conversions.StringToLatLon(_destinationLocation), true);
        else
            _destination = destination;
        _skyPoint = new Vector3(transform.position.x, height, transform.position.z);
        transform.LookAt(_skyPoint); // fly to the sky first
    }

    public void Play()
    {
        moneyBlast?.Play();
        moneyParticle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        _destination = _map.GeoToWorldPosition(Conversions.StringToLatLon(_destinationLocation), true);
        _destination.y = -10f;
        if (!inTheSky && Vector3.Distance(transform.position, _skyPoint) < 0.1f)
        { // if the money reach the sky
            inTheSky = true;
            StartCoroutine(SmoothLookAt(_destination));
        }

        if (inTheSky) // if money is in the sky, fly to the destination
            transform.position = Vector3.MoveTowards(transform.position, _destination, speed * Time.deltaTime);
        else
        { // money fly to the sky
            _skyPoint.x = transform.position.x;
            _skyPoint.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, _skyPoint, speed * Time.deltaTime);
        }


        if (Vector3.Distance(transform.position, _destination) < 0.1f) // if the money reach the destination
        {
            inTheSky = false;
            gameObject.SetActive(false);
        }

    }

    IEnumerator SmoothLookAt(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        while (Vector3.Distance(transform.rotation.eulerAngles, direction) > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
            yield return null;
        }
    }
}

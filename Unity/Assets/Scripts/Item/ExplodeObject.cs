using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.XR.ARFoundation;

public class ExplodeObject : SelectableObj
{
    List<GameObject> _pieces;
    float groundPosition = -20f;
    bool isExploded = false;

    [Header("Explode objects")]
    [SerializeField] MMF_Player feedbacks;
    [SerializeField] bool willProduceObjects;
    [SerializeField] GameObject _explosionProductPrefab;
    [SerializeField] int _productNumber = 50;
    List<GameObject> _explosionProducts;

    void Awake()
    {
        _pieces = new List<GameObject>();

    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        if (willProduceObjects)
        {
            _explosionProducts = new List<GameObject>();
            for (int i = 0; i < _productNumber; i++)
            {
                float sizeX = GetComponent<BoxCollider>().size.x;
                float sizeY = GetComponent<BoxCollider>().size.y;
                float sizeZ = GetComponent<BoxCollider>().size.z;
                float x = Random.Range(-sizeX / 2, sizeX / 2);
                float y = Random.Range(-sizeY / 2, sizeY / 2);
                float z = Random.Range(-sizeZ / 2, sizeZ / 2);

                _explosionProducts.Add(Instantiate(_explosionProductPrefab, transform.position + new Vector3(x, y, z), Random.rotation, transform.parent));
                _explosionProducts[i].AddComponent<ARAnchor>();
                _explosionProducts[i].SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                _pieces.Add(child.gameObject);
            }

        }
    }

    void Update()
    {
        if (!isExploded) return;
        if (!willProduceObjects)
        {
            foreach (GameObject piece in _pieces) // limit the pieces' y position
            {
                if (piece && piece.transform.position.y < groundPosition)
                {
                    piece.transform.position = new Vector3(piece.transform.position.x, groundPosition, piece.transform.position.z);
                }
            }
        }
    }

    public override void SelectEvent()
    {
        Explode();
    }

    void Explode()
    {
        isExploded = true;
        feedbacks?.PlayFeedbacks();
        if (!willProduceObjects)
        {
            // if (transform.parent.GetComponent<SpawnOnMap>())
            //     transform.parent.GetComponent<SpawnOnMap>()._spawnedObjects.Remove(gameObject); // stop updating the position of the object according to map
            foreach (GameObject piece in _pieces)
            {
                Rigidbody pieceRigidbody = piece.GetComponent<Rigidbody>();
                if (!pieceRigidbody) continue;
                pieceRigidbody.useGravity = true;
                pieceRigidbody.isKinematic = false;
                pieceRigidbody.AddExplosionForce(50000, transform.position, 100, 0, ForceMode.Impulse);
                // give the pieces a drag to slow down
                pieceRigidbody.drag = 0.1f;
            }
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            StartCoroutine(InstantiateProducts());
        }
    }

    IEnumerator InstantiateProducts()
    {
        foreach (GameObject product in _explosionProducts)
        {
            product.SetActive(true);
            Rigidbody productRigidbody = product.GetComponent<Rigidbody>();
            if (!productRigidbody) continue;
            productRigidbody.useGravity = true;
            productRigidbody.isKinematic = false;
            productRigidbody.AddExplosionForce(5000, transform.position, 700, 0, ForceMode.Impulse);
            product.AddComponent<OutOfBound>();
            yield return null;
        }

        feedbacks?.StopFeedbacks();
        Destroy(gameObject);
    }
}

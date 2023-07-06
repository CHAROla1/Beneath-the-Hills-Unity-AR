using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBound : MonoBehaviour
{
    [SerializeField] bool isPooled;
    Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < _player.position.y - 30f)
        {
            if (!isPooled)
                Destroy(gameObject);
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}

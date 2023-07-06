using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMoney : MonoBehaviour
{
    public float gravity = 9.8f;
    public float airResistance = 0.5f;
    public float rotationSpeed = 100f;

    private Vector3 velocity;

    private void Start()
    {
        velocity = Vector3.zero;
    }

    private void Update()
    {
        // calculate the velocity
        velocity.y -= gravity * Time.deltaTime;

        // apply air resistance
        velocity *= 1f - airResistance * Time.deltaTime;

        // apply the velocity to the position
        transform.position += velocity * Time.deltaTime;

        // rotate the object
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

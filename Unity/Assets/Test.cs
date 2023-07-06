using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GPSEncoder.SetLocalOrigin(new DoubleVector2(31.578426360240492d, 124.22860642278647d));
    }

    // Update is called once per frame
    void Update()
    {
        DoubleVector2 v = GPSEncoder.USCToGPS(transform.position.ToDoubleVector3());
        Debug.Log(v);
    }
}

internal static class VectorUtil
{
    public static DoubleVector3 ToDoubleVector3(this Vector3 vector3)
    {
        double x = Convert.ToDouble(vector3.x);
        double y = Convert.ToDouble(vector3.y);
        double z = Convert.ToDouble(vector3.z);
        return new DoubleVector3(x, y, z);
    }
}
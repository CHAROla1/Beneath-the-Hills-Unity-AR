
using UnityEngine;

public class ObjectDestroyer
{
    public static DestroyEvent ObjectDestroyed = new DestroyEvent();

    public static void DestroyObject(GameObject objectToDestroy)
    {
        // destroy the object
        MonoBehaviour.Destroy(objectToDestroy);

        // invoke the event
        ObjectDestroyed.Invoke(objectToDestroy);
    }
}

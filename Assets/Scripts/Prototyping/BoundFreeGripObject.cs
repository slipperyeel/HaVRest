using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoundFreeGripObject : MonoBehaviour 
{
    [SerializeField]
    private BoxCollider col;

    private List<GripData> touchingColliders;

    void OnTriggerStay(Collider col)
    {

    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger Entered");
        if (touchingColliders.Count <= 2)
        {
            GripData data = touchingColliders.Find(x => x.Collider == col);
            if (data == null)
            {
                touchingColliders.Add(new GripData(col.transform, col));
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Trigger Exit");
        GripData data = touchingColliders.Find(x => x.Collider == col);
        if (data != null)
        {
            touchingColliders.Remove(data);
        }
    }
}

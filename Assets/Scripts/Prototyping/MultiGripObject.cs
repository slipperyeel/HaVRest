using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiGripObject : MonoBehaviour 
{
    [SerializeField]
    private BoxCollider col;

    [SerializeField]
    private Transform rotationRoot;

    private List<GripData> touchingColliders;

    void Start()
    {
        touchingColliders = new List<GripData>(2);
    }

    void FixedUpdate()
    {
        // here we need to do some intereting shit.
        if (touchingColliders.Count == 2)
        {
            Vector3 gripVec1 = touchingColliders[0].GripLocation.position;
            Vector3 gripVec2 = touchingColliders[1].GripLocation.position;

            Vector3 rootDir = rotationRoot.forward;

            Vector3 dirVec = gripVec2 - gripVec1;

            float angleBetween = Vector3.Angle(gripVec1, gripVec2);

            transform.rotation = Quaternion.LookRotation(dirVec);
        }
    }

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

public class GripData
{
    public Transform GripLocation;
    public Collider Collider;

    public GripData(Transform gripLoc, Collider col)
    {
        GripLocation = gripLoc;
        Collider = col;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FreeGripObject : MonoBehaviour 
{
    [SerializeField]
    private BoxCollider col;

    [SerializeField]
    private Transform gripTop;

    [SerializeField]
    private Transform gripBottom;

    private List<GripData> touchingColliders;

    void Start()
    {
        touchingColliders = new List<GripData>(2);
    }

    private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    Vector3 previousCenter;
    void FixedUpdate()
    {
        // here we need to do some interesting shit.
        if (touchingColliders.Count == 2)
        {
            Vector3 gripVec1 = touchingColliders[0].GripLocation.position;
            Vector3 gripVec2 = touchingColliders[1].GripLocation.position;

            Vector3 dirVec = gripVec2 - gripVec1;
            Vector3 forwardXzero = new Vector3(0.0f, transform.forward.y, transform.forward.z);
            Vector3 dirVecXzero = new Vector3(0.0f, dirVec.y, dirVec.z);
            Vector3 dirCross = Vector3.Cross(forwardXzero, dirVecXzero);
            //float angleBetween = AngleSigned(forwardXzero, dirVecXzero, dirCross);
            float angleBetween = Vector3.Angle(new Vector3(0.0f, transform.forward.y, transform.forward.z), new Vector3(0.0f, dirVec.y, dirVec.z));

            Debug.Log(angleBetween);

            Vector3 previousTopGripPos = gripTop.position;
            Vector3 previousBottomGripPos = gripBottom.position;
            Vector3 originalCenterPos = transform.position;
            //Debug.Log("PreviousTopGripPos: " + previousTopGripPos);


            Vector3 gripCenter = (gripVec1 + gripVec2) / 2.0f;
            Vector3 gripDif = gripCenter - previousCenter;
            previousCenter = gripCenter;
            transform.rotation = Quaternion.LookRotation(dirVec);

            //    transform.RotateAround(gripCenter, -Vector3.right, angleBetween);
            //transform.ro

            //Debug.Log("PreviousTopGripPos: " + gripTop.position);
            Vector3 topDisplacement = gripTop.position - previousTopGripPos;

            Debug.DrawLine(previousTopGripPos, topDisplacement, Color.red);

            Debug.DrawLine(Vector3.zero, originalCenterPos, Color.yellow);
            Debug.DrawLine(Vector3.zero, previousTopGripPos, Color.magenta);

           // Debug.Log(topDisplacement);
            Vector3 topPivotDifferential = Vector3.Project(originalCenterPos, previousTopGripPos);
            Debug.DrawLine(previousTopGripPos, topPivotDifferential, Color.blue);
            Vector3 transformDisplacement = topPivotDifferential + topDisplacement;
            Debug.DrawLine(originalCenterPos, transform.position + transformDisplacement, Color.white);
            // After we rotate, define position in space.
            Vector3 currentGripObjectSpaceVector = gripTop.position - gripBottom.position;


            //Deb
            //Transform newTrans = transform;
            transform.position = gripDif + transform.position;


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

#define GAMEPLAY_TEST_NOVR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using VRTK;

/// <summary>
/// Simple positional tracking system for orientating a given object about two transforms.
/// Assumes a given object is set up in the same way as the sample object, with the root transform and forward at the base of the tool.
/// TODO: Implement Steam VR stuff
/// TODO: Wrap with GAMEPLAY_TEST_NOVR for work without vive
/// </summary>
public class FreeGripObject : VRTK_InteractableObject
{
    [SerializeField]
    private BoxCollider mCol;

    private static readonly int NUM_GRIP_POINTS = 2;

    private List<GripData> touchingColliders;

    protected override void Start()
    {
        touchingColliders = new List<GripData>(NUM_GRIP_POINTS);
        base.Start();
    }

    protected override void FixedUpdate()
    {
        //if (touchingColliders.Count > 0)
        //{
        //    transform.position = touchingColliders[0].GripLocation.position;

        //    if (touchingColliders.Count == 1)
        //    {
        //        transform.rotation = Quaternion.LookRotation(touchingColliders[0].GripLocation.forward);
        //    }
        //    else if (touchingColliders.Count == NUM_GRIP_POINTS)
        //    {
        //        Vector3 gripVec1 = touchingColliders[0].GripLocation.position;
        //        Vector3 gripVec2 = touchingColliders[1].GripLocation.position;

        //        Vector3 dirVec = gripVec2 - gripVec1;
        //        transform.rotation = Quaternion.LookRotation(dirVec);
        //    }
        //}
        base.FixedUpdate();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);
        //if (col.tag == "Hands")
        //{
        //    if (touchingColliders.Count <= NUM_GRIP_POINTS)
        //    {
        //        GripData data = touchingColliders.Find(x => x.Collider == col);
        //        if (data == null)
        //        {
        //            touchingColliders.Add(new GripData(col.transform, col));
        //        }

        //        if (touchingColliders.Count == 1)
        //        {
        //            transform.position = col.gameObject.transform.position;
        //        }
        //    }
        //}
    }

    void OnTriggerExit(Collider col)
    {
        //if (col.tag == "Hands")
        //{
        //    GripData data = touchingColliders.Find(x => x.Collider == col);
        //    if (data != null)
        //    {
        //        touchingColliders.Remove(data);
        //    }
        //}
    }

    private VRTK_ControllerActions controllerActions;
    private VRTK_ControllerEvents controllerEvents;
    private float impactMagnifier = 120f;
    private float collisionForce = 0f;

    public float CollisionForce()
    {
        return collisionForce;
    }

    public override void Grabbed(GameObject grabbingObject)
    {
        Debug.Log("WHAW");
        base.Grabbed(grabbingObject);
        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
        controllerEvents = grabbingObject.GetComponent<VRTK_ControllerEvents>();
    }

    protected override void Awake()
    {
        base.Awake();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (controllerActions && controllerEvents && IsGrabbed())
        {
            collisionForce = controllerEvents.GetVelocity().magnitude * impactMagnifier;
            controllerActions.TriggerHapticPulse((ushort)collisionForce, 0.5f, 0.01f);
        }
        else
        {
            collisionForce = collision.relativeVelocity.magnitude * impactMagnifier;
        }
    }
}

// This is a temporary class to sort of store what a Grip is. 
// Currently I don't really know what we'll want here other than a transform, but we can add more if needed.
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
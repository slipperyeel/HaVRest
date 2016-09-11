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
    //[SerializeField]
    //private BoxCollider mCol;

    //[SerializeField]
    //private GameObject model;

    //private static readonly int NUM_GRIP_POINTS = 2;

    ////private GameObject firstGrabHand;
    ////private GameObject secondGrabHand;

    ////private Transform firstGripTransform;
    //private Transform secondGripTransform;

    //protected override void Start()
    //{
    //    base.Start();
    //    isTwoHanded = true;
    //}

    //protected override void FixedUpdate()
    //{
    //    if (IsGrabbed())
    //    {
    //        if(/*firstGripTransform != null && */secondGripTransform != null)
    //        {
    //            Vector3 dirVec = secondGripTransform.position - GetGrabbingObject().transform.position;
    //            transform.rotation = Quaternion.LookRotation(dirVec);
    //        }
    //    }
    //    base.FixedUpdate();
    //}

    ////void OnTriggerEnter(Collider col)
    ////{
    ////    Debug.Log(col.tag);
    ////    if (col.tag == "Hands")
    ////    {
    ////        // If we've already got a hand on the tool.
    ////        if (IsGrabbed())
    ////        {
    ////            if (col.gameObject != GetGrabbingObject())
    ////            {
    ////                secondGripTransform = col.gameObject.transform;
    ////            }
    ////        }
    ////    }
    ////}

    ////void OnTriggerExit(Collider col)
    ////{
    ////    if (col.tag == "Hands")
    ////    {
    ////        if (col.gameObject != GetGrabbingObject())
    ////        {
    ////            secondGripTransform = null;
    ////        }
    ////    }
    ////}

    //private VRTK_ControllerActions controllerActions;
    //private VRTK_ControllerEvents controllerEvents;
    //private float impactMagnifier = 120f;
    //private float collisionForce = 0f;

    //public float CollisionForce()
    //{
    //    return collisionForce;
    //}

    //public override bool IsGrabbable()
    //{
    //    return true;
    //}

    //public override void SetIsGrabbable(bool grabbable)
    //{
    //    // free grip objects are always grabbable!
    //    isGrabbable = true;
    //}

    //public override void Grabbed(GameObject grabbingObject)
    //{
    //    if (!IsGrabbed())
    //    {
    //        Debug.Log("Single Hand Grab");
    //        base.Grabbed(grabbingObject);
    //        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
    //        controllerEvents = grabbingObject.GetComponent<VRTK_ControllerEvents>();
    //        //firstGrabHand = grabbingObject;
    //        //firstGripTransform = grabbingObject.transform;
    //        //transform.position = grabbingObject.transform.position;
    //        rb.isKinematic = true;
    //    }
    //    else if (isTwoHanded && !hasSecondHand)
    //    {
    //        Debug.Log("Double Hand Grab");
    //        hasSecondHand = true;
    //        //secondGrabHand = grabbingObject;
    //        secondGripTransform = grabbingObject.transform;
    //    }
    //}

    //public override void Ungrabbed(GameObject previousGrabbingObject)
    //{
    //    //if (previousGrabbingObject == secondGrabHand)
    //    //{
    //        if (isTwoHanded && !hasSecondHand)
    //        {
    //            rb.isKinematic = false;
    //            //firstGripTransform = null;
    //            base.Ungrabbed(previousGrabbingObject);
    //            //firstGrabHand = null;
    //        }
    //        else
    //        {
    //            secondGripTransform = null;
    //            hasSecondHand = false;
    //            //;secondGrabHand = null;
    //        }
    //    //}
    //    //else
    //    //{
    //    //    if (isTwoHanded && hasSecondHand)
    //    //    {
    //    //        firstGrabHand = secondGrabHand;
    //    //        //firstGripTransform = secondGripTransform;
    //    //        secondGrabHand = null;
    //    //        secondGripTransform = null;
    //    //    }
    //    //    else
    //    //    {
    //    //        rb.isKinematic = false;
    //    //        //firstGripTransform = null;
    //    //        base.Ungrabbed(previousGrabbingObject);
    //    //        firstGrabHand = null;
    //    //    }
    //    //}
    //}

    //protected override void Awake()
    //{
    //    base.Awake();
    //    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (controllerActions && controllerEvents && IsGrabbed())
    //    {
    //        collisionForce = controllerEvents.GetVelocity().magnitude * impactMagnifier;
    //        controllerActions.TriggerHapticPulse((ushort)collisionForce, 0.5f, 0.01f);
    //    }
    //    else
    //    {
    //        collisionForce = collision.relativeVelocity.magnitude * impactMagnifier;
    //    }
    //}
}
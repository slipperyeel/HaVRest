﻿using UnityEngine;
using System.Collections;

// VR
using VRTK;

public class HVRInteractableObject : VRTK_InteractableObject
{
    private VRTK_ControllerActions[] controllerActionsArray = new VRTK_ControllerActions[(int)HVR_InteractionTypes.Count];
    private VRTK_ControllerEvents[] controllerEventsArray = new VRTK_ControllerEvents[(int)HVR_InteractionTypes.Count];

    public SteamVR_TrackedObject.EIndex grabbedControllerIndex = SteamVR_TrackedObject.EIndex.None;
    public SteamVR_TrackedObject.EIndex heldControllerIndex = SteamVR_TrackedObject.EIndex.None;

    protected override void FixedUpdate()
    {   
        if (grabbedControllerIndex != SteamVR_TrackedObject.EIndex.None && heldControllerIndex != SteamVR_TrackedObject.EIndex.None)
        {
            GameObject heldController = HVRControllerManager.Instance.GetControllerByIndex((int)heldControllerIndex);
            GameObject grabbingController = HVRControllerManager.Instance.GetControllerByIndex((int)grabbedControllerIndex);
            GameObject topHand, bottomHand;

            if(heldController != null && grabbingController != null)
            {
                float heldDist = Vector3.Distance(transform.position, heldController.transform.position);
                float grabbedDist = Vector3.Distance(transform.position, grabbingController.transform.position);
                if(heldDist >= grabbedDist)
                {
                    topHand = heldController;
                    bottomHand = grabbingController;
                }
                else
                {
                    topHand = grabbingController;
                    bottomHand = heldController;
                }

                Vector3 heldDir = topHand.transform.position - bottomHand.transform.position;
                transform.forward = heldDir.normalized;
                // this isn't quite there yet, but it's close
                //transform.Rotate(Quaternion.Euler(0f, 0f, ProperAngle(transform.up, heldController.transform.right)).eulerAngles);
                Vector3 actualDir = topHand.transform.position - transform.position;
                Vector3 handDir = topHand.transform.position - bottomHand.transform.position;
                Vector3 pos = actualDir;
                pos = Vector3.Project(pos, handDir.normalized);
                transform.position = topHand.transform.position - pos;
            }
        }
        base.FixedUpdate();
    }

    private float ProperAngle(Vector3 v1, Vector3 v2)
    {
        //  Acute angle [0,180]
        float angle = Vector3.Angle(v1, v2);

        //  -Acute angle [180,-179]
        float sign = Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(v1, v2)));
        float signed_angle = angle * sign;

        //  360 angle
        return (signed_angle <= 0) ? 360 + signed_angle : signed_angle;
    }

    public override void Grabbed(GameObject grabbingObject)
    {
        SteamVR_TrackedObject.EIndex index = grabbingObject.GetComponent<SteamVR_TrackedObject>().index;
        if (grabbedControllerIndex == SteamVR_TrackedObject.EIndex.None)
        {
            base.Grabbed(grabbingObject);
            controllerActionsArray[(int)HVR_InteractionTypes.Grabbed] = grabbingObject.GetComponent<VRTK_ControllerActions>();
            controllerEventsArray[(int)HVR_InteractionTypes.Grabbed] = grabbingObject.GetComponent<VRTK_ControllerEvents>();

            grabbedControllerIndex = index;
        }
        else if (heldControllerIndex == SteamVR_TrackedObject.EIndex.None && index != grabbedControllerIndex)
        {
            Debug.Log("SECOND CONTROLLER GRABBED");
            controllerActionsArray[(int)HVR_InteractionTypes.Held] = grabbingObject.GetComponent<VRTK_ControllerActions>();
            controllerEventsArray[(int)HVR_InteractionTypes.Held] = grabbingObject.GetComponent<VRTK_ControllerEvents>();

            heldControllerIndex = index;
        }
    }

    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        SteamVR_TrackedObject obj = previousGrabbingObject.GetComponent<SteamVR_TrackedObject>();
        if (obj != null)
        {
            SteamVR_TrackedObject.EIndex index = obj.index;
            if (grabbedControllerIndex != SteamVR_TrackedObject.EIndex.None && index == grabbedControllerIndex)
            {
                if (heldControllerIndex != SteamVR_TrackedObject.EIndex.None)
                {
                    grabbedControllerIndex = heldControllerIndex;
                    heldControllerIndex = SteamVR_TrackedObject.EIndex.None;
                }
                else
                {
                    PreviousParent = null;
                    PreviousKinematicState = false;
                    grabbedControllerIndex = SteamVR_TrackedObject.EIndex.None;
                }

                base.Ungrabbed(previousGrabbingObject);
                controllerActionsArray[(int)HVR_InteractionTypes.Grabbed] = null;
                controllerEventsArray[(int)HVR_InteractionTypes.Grabbed] = null;    
            }
            else if (heldControllerIndex != SteamVR_TrackedObject.EIndex.None && index == heldControllerIndex)
            {
                controllerActionsArray[(int)HVR_InteractionTypes.Held] = null;
                controllerEventsArray[(int)HVR_InteractionTypes.Held] = null;

                heldControllerIndex = SteamVR_TrackedObject.EIndex.None;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}

public enum HVR_InteractionTypes
{
    Grabbed = 0,
    Held,
    Count
}
using UnityEngine;
using System.Collections;

// VR
using VRTK;

public class HVRInteractableObject : VRTK_InteractableObject
{
    private static readonly int NO_CONTROLLER_INDEX = -999;

    private VRTK_ControllerActions[] controllerActionsArray = new VRTK_ControllerActions[(int)HVR_InteractionTypes.Count];
    private VRTK_ControllerEvents[] controllerEventsArray = new VRTK_ControllerEvents[(int)HVR_InteractionTypes.Count];

    private int grabbedControllerId = NO_CONTROLLER_INDEX;
    private int heldControllerId = NO_CONTROLLER_INDEX;

    public override void Grabbed(GameObject grabbingObject)
    {
        if (grabbedControllerId == NO_CONTROLLER_INDEX)
        {
            base.Grabbed(grabbingObject);
            controllerActionsArray[(int)HVR_InteractionTypes.Grabbed] = grabbingObject.GetComponent<VRTK_ControllerActions>();
            controllerEventsArray[(int)HVR_InteractionTypes.Grabbed] = grabbingObject.GetComponent<VRTK_ControllerEvents>();

            grabbedControllerId = controllerEventsArray[(int)HVR_InteractionTypes.Grabbed].ControllerIndex;
        }
        else if(heldControllerId == NO_CONTROLLER_INDEX)
        {
            Debug.Log("SECOND CONTROLLER GRABBED");
            controllerActionsArray[(int)HVR_InteractionTypes.Held] = grabbingObject.GetComponent<VRTK_ControllerActions>();
            controllerEventsArray[(int)HVR_InteractionTypes.Held] = grabbingObject.GetComponent<VRTK_ControllerEvents>();

            heldControllerId = controllerEventsArray[(int)HVR_InteractionTypes.Held].ControllerIndex;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HVR_InteractableObject : VRTK_InteractableObject
{
    [SerializeField] HVR_TwoHandedGrab mTwoHanded;

    public override void SaveCurrentState()
    {
        // TODO
        // if you're pulling from the bag, do nothing!
        // else, call base.SaveCurrentState()
    }

    //public override void Grabbed(GameObject currentGrabbingObject)
    //{
    //    //base.Grabbed(currentGrabbingObject);
    //    if (mTwoHanded != null)
    //        mTwoHanded.OnGrabbed(currentGrabbingObject);
    //}

    //public override void Ungrabbed(GameObject previousGrabbingObject)
    //{
    //    base.Ungrabbed(previousGrabbingObject);
    //    //if (mTwoHanded != null)
    //    //    mTwoHanded.UnGrabbed(previousGrabbingObject);
    //}
}

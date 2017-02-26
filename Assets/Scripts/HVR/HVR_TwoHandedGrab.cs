using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HVR_TwoHandedGrab : MonoBehaviour
{
    private Transform mInitalGrabPoint;
    private Transform mSecondaryGrabPoint;

    //public void OnGrabbed(GameObject grabbingObj)
    //{
    //    if (VRTK_DeviceFinder.IsControllerLeftHand(grabbingObj))
    //    {
    //        mInitalGrabPoint = VRTK_DeviceFinder.GetControllerLeftHand().transform;
    //        mSecondaryGrabPoint = VRTK_DeviceFinder.GetControllerRightHand().transform;
    //    }
    //    else if (VRTK_DeviceFinder.IsControllerRightHand(grabbingObj))
    //    {
    //        mInitalGrabPoint = VRTK_DeviceFinder.GetControllerRightHand().transform;
    //        mSecondaryGrabPoint = VRTK_DeviceFinder.GetControllerLeftHand().transform;
    //    }

    //    transform.rotation = Quaternion.LookRotation(mInitalGrabPoint.position - mSecondaryGrabPoint.position, mInitalGrabPoint.TransformDirection(Vector3.forward));
    //}

    //public void UnGrabbed(GameObject prevGrabbingObj)
    //{
    //    mInitalGrabPoint = null;
    //    mSecondaryGrabPoint = null;
    //}

    private void Update()
    {
        if (mInitalGrabPoint != null && mSecondaryGrabPoint != null)
        {
            transform.rotation = Quaternion.LookRotation(mInitalGrabPoint.position - mSecondaryGrabPoint.position, mInitalGrabPoint.TransformDirection(Vector3.forward));
        }
    }
}

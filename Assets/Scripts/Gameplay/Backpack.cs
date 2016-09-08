using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;

public enum ePouchState
{
    None = 0,
    Inactive,
    Active,
    Full
}

// This is a backpack.
public class Backpack : MonoBehaviour
{
    [SerializeField]
    private List<Pouch> mPouches;
    public List<Pouch> Pouches { get { return mPouches; } set { mPouches = value; } }
    [SerializeField]
    GameObject mItemSackPrefab;
    GameObject mItemSackObject;

    void Start()
    {
        HVRControllerManager.Instance.DeviceConnected += OnDeviceConnected;
    }

    void OnDeviceConnected(object sender, System.EventArgs e)
    {
        HVRControllerManager.ConnectedDeviceArgs dca = (HVRControllerManager.ConnectedDeviceArgs)e;
        int index = dca.controllerIndex;
        bool isConnected = dca.IsConnected;
        if ((index == HVRControllerManager.Instance.LeftIndex) && (isConnected))
        {
            HVRControllerManager.Instance.LeftEvents.ApplicationMenuPressed -= DoApplicationMenuPressedLeft;
            HVRControllerManager.Instance.LeftEvents.ApplicationMenuPressed += DoApplicationMenuPressedLeft;
        }
        if ((index == HVRControllerManager.Instance.RightIndex) && (isConnected))
        {
            HVRControllerManager.Instance.RightEvents.ApplicationMenuPressed -= DoApplicationMenuPressedRight;
            HVRControllerManager.Instance.RightEvents.ApplicationMenuPressed += DoApplicationMenuPressedRight;
        }
    }

    void HideRenderModels(Transform controller, bool hidden)
    {
        foreach (MeshRenderer child in controller.GetComponentsInChildren<MeshRenderer>())
            child.enabled = !hidden;
    }

    void DoApplicationMenuPressedLeft(object sender, ControllerInteractionEventArgs e)
    {
        if ((mItemSackObject != null) && (mItemSackObject.transform.parent == HVRControllerManager.Instance.right.transform))
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, false);
            HideRenderModels(HVRControllerManager.Instance.Left.transform, true);
            Destroy(mItemSackObject);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Left.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Left.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(45, 0, 0);
        }
        else if (mItemSackObject == null)
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, true);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Left.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Left.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(45, 0, 0);
        }
        else
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, false);
            Destroy(mItemSackObject);
        }
    }

    void DoApplicationMenuPressedRight(object sender, ControllerInteractionEventArgs e)
    {
        if ((mItemSackObject != null) && (mItemSackObject.transform.parent == HVRControllerManager.Instance.left.transform))
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, false);
            HideRenderModels(HVRControllerManager.Instance.Right.transform, true);
            Destroy(mItemSackObject);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Right.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Right.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(45, 0, 0);
        }
        else if (mItemSackObject == null)
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, true);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Right.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Right.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(45, 0, 0);
        }
        else
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, false);
            Destroy(mItemSackObject);
        }
    }
}

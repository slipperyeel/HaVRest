﻿using UnityEngine;
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
    public GameObject mItemSackPrefab;
    private GameObject mItemSackObject;
    [SerializeField]
    public RuntimeAnimatorController mItemFloatController;
    [SerializeField]
    public Material mRimLight;

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
        foreach (MeshRenderer child in controller.GetComponentsInChildren<MeshRenderer>(true))
            child.enabled = !hidden;
    }

    void SpawnInventoryUI()
    {
        //List<InventoryItem> inventoryItems = mPouches[0].InventoryItems;
        Transform slotContainer = mItemSackObject.transform.GetChild(0);
        for (int i = 0; i < 12; i++)
        {
            GameObject instantiatedObj;
            //HVRItemFactory.SpawnItem((ItemEnums)inventoryItems[i].Id, Vector3.zero, default(Quaternion), new Vector3(0.35f, 0.35f, 0.35f), out instantiatedObj, "InventorySlotItem");
            HVRItemFactory.SpawnItem(ItemEnums.EggPlant_Fruit, Vector3.zero, default(Quaternion), new Vector3(0.35f, 0.35f, 0.35f), out instantiatedObj, "InventorySlotItem");
            instantiatedObj.transform.SetParent(slotContainer.GetChild(i));
            instantiatedObj.transform.localPosition = Vector3.zero;
            instantiatedObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            Animator itemFloatController = instantiatedObj.AddComponent<Animator>() as Animator;
            instantiatedObj.GetComponent<Animator>().runtimeAnimatorController = mItemFloatController;
            if (i == 0)
            {
                Material[] newMaterials = new Material[instantiatedObj.GetComponent<MeshRenderer>().materials.Length];
                newMaterials[0] = mRimLight;
                instantiatedObj.GetComponent<MeshRenderer>().materials = newMaterials;
            }
            //itemFloatController.runtimeAnimatorController = mItemFloatController;
        }
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
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            SpawnInventoryUI();
        }
        else if (mItemSackObject == null)
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, true);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Left.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Left.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            SpawnInventoryUI();
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
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Right.transform.position, HVRControllerManager.Instance.Right.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Right.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            SpawnInventoryUI();
        }
        else if (mItemSackObject == null)
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, true);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Right.transform.position, HVRControllerManager.Instance.Right.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Right.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            SpawnInventoryUI();
        }
        else
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, false);
            Destroy(mItemSackObject);
        }
    }
}

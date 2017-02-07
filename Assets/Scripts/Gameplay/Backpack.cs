using UnityEngine;
using UnityEngine.UI;
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
    private Transform mSlotContainer;
    private List<GameObject> mInventorySlotItems;
    [SerializeField]
    public RuntimeAnimatorController mItemFloatController;
    [SerializeField]
    public Material mRimLight;
    private GameObject mTopObject;
    private bool mIsLeft;

    void Start()
    {
        HVRControllerManager.Instance.DeviceConnected += OnDeviceConnected;
        BagInteraction.OnEntered += OnBagTriggerEntered;
        BagInteraction.OnExited += OnBagTriggerExited;
    }

    void OnBagTriggerEntered(Collider other)
    {

    }

    void OnBagTriggerExited()
    {

    }

    void OnDeviceConnected(object sender, System.EventArgs e)
    {
        HVRControllerManager.ConnectedDeviceArgs dca = (HVRControllerManager.ConnectedDeviceArgs)e;
        int index = dca.controllerIndex;
        bool isConnected = dca.IsConnected;
        if ((index == HVRControllerManager.Instance.LeftIndex) && (isConnected))
        {
            HVRControllerManager.Instance.LeftEvents.ApplicationMenuPressed += DoApplicationMenuPressedLeft;
        }
        if ((index == HVRControllerManager.Instance.RightIndex) && (isConnected))
        {
            HVRControllerManager.Instance.RightEvents.ApplicationMenuPressed += DoApplicationMenuPressedRight;
        }
    }

    void DoControllerGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target == mTopObject)
        {
            //mTopObject.GetComponent<Rigidbody>().useGravity = true;
            //mTopObject.transform.parent = null;
            SubtractItemFromInventory(mTopObject.GetComponent<ItemFactoryData>().ItemEnum);
            e.target.transform.parent = null;
            e.target.layer = 0;
        }
    }

    void HideRenderModels(Transform controller, bool hidden)
    {
        foreach (MeshRenderer child in controller.GetComponentsInChildren<MeshRenderer>(true))
            child.enabled = !hidden;
    }

    void SpawnTopObject(ItemEnums itemEnum)
    {
        GameObject instantiatedObj;
        HVRItemFactory.SpawnItem(itemEnum, Vector3.zero, default(Quaternion), Vector3.one, out instantiatedObj, "TopItem");
        //instantiatedObj.GetComponent<Rigidbody>().useGravity = false;
        instantiatedObj.GetComponent<Rigidbody>().isKinematic = true;
        instantiatedObj.transform.SetParent(mItemSackObject.transform.GetChild(0).GetChild(1).GetChild(12));
        instantiatedObj.transform.localPosition = Vector3.zero;
        instantiatedObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        mTopObject = instantiatedObj;
        mTopObject.layer = 5;
    }

    public void AddItemToInventory(ItemEnums itemEnum)
    {
        if (itemEnum <= ItemEnums.Tool_Pouch_End)
        {
            int itemEnumPouchIndex = (int)itemEnum - (int)ItemEnums.Tool_Pouch_Start;
            mPouches[0].InventoryItems[itemEnumPouchIndex].Quantity++;
        }
        else if (itemEnum <= ItemEnums.Seed_Pouch_End)
        {
            int itemEnumPouchIndex = (int)itemEnum - (int)ItemEnums.Seed_Pouch_Start;
            mPouches[1].InventoryItems[itemEnumPouchIndex].Quantity++;
        }
    }

    private void SubtractItemFromInventory(ItemEnums itemEnum)
    {
        int quantity = 0;
        if (itemEnum <= ItemEnums.Tool_Pouch_End)
        {
            int itemEnumPouchIndex = (int)itemEnum - (int)ItemEnums.Tool_Pouch_Start;
            mPouches[0].InventoryItems[itemEnumPouchIndex].Quantity--;
            quantity = mPouches[0].InventoryItems[itemEnumPouchIndex].Quantity;
        }
        else if (itemEnum <= ItemEnums.Seed_Pouch_End)
        {
            int itemEnumPouchIndex = (int)itemEnum - (int)ItemEnums.Seed_Pouch_Start;
            mPouches[1].InventoryItems[itemEnumPouchIndex].Quantity--;
            quantity = mPouches[1].InventoryItems[itemEnumPouchIndex].Quantity;
        }
        if (quantity == 0)
        {
            DestroyInventoryUI();
            SpawnInventoryUI();
        }
        else
        {
            SpawnTopObject(itemEnum);
            // Update this when we have a selected id
            mSlotContainer.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = quantity.ToString();
        }
    }

    void FilterInventory(List<InventoryItem> inventoryItems)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].Quantity == 0)
            {
                inventoryItems.RemoveAt(i);
                i--;
            }
        }
    }

    public void SpawnInventoryUI()
    {
        mItemSackObject.GetComponent<BagInteraction>().mIsBagOnLeft = mIsLeft;

        List<InventoryItem> inventoryItems = mPouches[1].InventoryItems;
        List<InventoryItem> filteredInventoryItems = new List<InventoryItem>(inventoryItems);
        mInventorySlotItems = new List<GameObject>();
        FilterInventory(filteredInventoryItems);
        mSlotContainer = mItemSackObject.transform.GetChild(0).GetChild(1);
        for (int i = 0; i < 12; i++)
        {
            if (filteredInventoryItems.Count > i)
            {
                GameObject instantiatedObj;
                HVRItemFactory.SpawnItem(filteredInventoryItems[i].Id, Vector3.zero, default(Quaternion), new Vector3(0.35f, 0.35f, 0.35f), out instantiatedObj, "InventorySlotItem");
                mInventorySlotItems.Add(instantiatedObj);
                Destroy(instantiatedObj.GetComponent<Rigidbody>());
                Destroy(instantiatedObj.GetComponent<VRTK_InteractableObject>());
                instantiatedObj.transform.SetParent(mSlotContainer.GetChild(i));
                instantiatedObj.transform.localPosition = Vector3.zero;
                instantiatedObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                Animator itemFloatController = instantiatedObj.AddComponent<Animator>() as Animator;
                instantiatedObj.GetComponent<Animator>().runtimeAnimatorController = mItemFloatController;
                if (i == 0)
                {
                    Material[] newMaterials = new Material[instantiatedObj.GetComponent<MeshRenderer>().materials.Length];
                    newMaterials[0] = mRimLight;
                    instantiatedObj.GetComponent<MeshRenderer>().materials = newMaterials;
                    SpawnTopObject(filteredInventoryItems[i].Id);
                }
                //itemFloatController.runtimeAnimatorController = mItemFloatController;
                mSlotContainer.GetChild(i).GetChild(0).gameObject.SetActive(true);
                mSlotContainer.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = filteredInventoryItems[i].Quantity.ToString();
            }
            else
            {
                mSlotContainer.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }

        if (mIsLeft)
        {
            HVRControllerManager.Instance.right.transform.GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += new ObjectInteractEventHandler(DoControllerGrabInteractableObject);
        }
        else
        {
            HVRControllerManager.Instance.left.transform.GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject += new ObjectInteractEventHandler(DoControllerGrabInteractableObject);
        }
    }

    public void DestroyInventoryUI()
    {
        if (mInventorySlotItems != null)
        {
            for (int i = 0; i < mInventorySlotItems.Count; i++)
            {
                Destroy(mInventorySlotItems[i]);
                mSlotContainer.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
            mInventorySlotItems.Clear();
        }

        // We don't actually need to remove this, just stop from doing it more than once
        if (mIsLeft)
        {
            HVRControllerManager.Instance.right.transform.GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject -= new ObjectInteractEventHandler(DoControllerGrabInteractableObject);
        }
        else
        {
            HVRControllerManager.Instance.left.transform.GetComponent<VRTK_InteractGrab>().ControllerGrabInteractableObject -= new ObjectInteractEventHandler(DoControllerGrabInteractableObject);
        }
    }

    void DoApplicationMenuPressedLeft(object sender, ControllerInteractionEventArgs e)
    {
        mIsLeft = true;
        if ((mItemSackObject != null) && (mItemSackObject.transform.parent == HVRControllerManager.Instance.right.transform))
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, false);
            HideRenderModels(HVRControllerManager.Instance.Left.transform, true);
            mItemSackObject.GetComponent<BagInteraction>().UnsubscribeTriggers();
            Destroy(mItemSackObject);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Left.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Left.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            mItemSackObject.GetComponent<BagInteraction>().SubscribeTriggers();
            SpawnInventoryUI();
        }
        else if (mItemSackObject == null)
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, true);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Left.transform.position, HVRControllerManager.Instance.Left.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Left.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            mItemSackObject.GetComponent<BagInteraction>().SubscribeTriggers();
            SpawnInventoryUI();
        }
        else
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, false);
            mItemSackObject.GetComponent<BagInteraction>().UnsubscribeTriggers();
            DestroyInventoryUI();
            Destroy(mItemSackObject);
        }
    }

    void DoApplicationMenuPressedRight(object sender, ControllerInteractionEventArgs e)
    {
        mIsLeft = false;
        if ((mItemSackObject != null) && (mItemSackObject.transform.parent == HVRControllerManager.Instance.left.transform))
        {
            HideRenderModels(HVRControllerManager.Instance.Left.transform, false);
            HideRenderModels(HVRControllerManager.Instance.Right.transform, true);
            mItemSackObject.GetComponent<BagInteraction>().UnsubscribeTriggers();
            Destroy(mItemSackObject);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Right.transform.position, HVRControllerManager.Instance.Right.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Right.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            mItemSackObject.GetComponent<BagInteraction>().SubscribeTriggers();
            SpawnInventoryUI();
        }
        else if (mItemSackObject == null)
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, true);
            mItemSackObject = (GameObject)Instantiate(mItemSackPrefab, HVRControllerManager.Instance.Right.transform.position, HVRControllerManager.Instance.Right.transform.rotation);
            mItemSackObject.transform.SetParent(HVRControllerManager.Instance.Right.transform);
            mItemSackObject.transform.localPosition = Vector3.zero;
            mItemSackObject.transform.localEulerAngles = new Vector3(-45f, -180f, 0f);
            mItemSackObject.GetComponent<BagInteraction>().SubscribeTriggers();
            SpawnInventoryUI();
        }
        else
        {
            HideRenderModels(HVRControllerManager.Instance.Right.transform, false);
            mItemSackObject.GetComponent<BagInteraction>().UnsubscribeTriggers();
            DestroyInventoryUI();
            Destroy(mItemSackObject);
        }
    }
}

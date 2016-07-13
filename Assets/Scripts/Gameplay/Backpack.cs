using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// This is a backpack.
public class Backpack : MonoBehaviour
{
    [SerializeField]
    private List<Pouch> mPouches;
}

[Serializable]
public class Pouch : MonoBehaviour
{
    [SerializeField]
    private int mSlots;

    [SerializeField]
    private List<InventoryItem> mInventoryItems;

    void Awake()
    {
        mInventoryItems = new List<InventoryItem>();
    }

    public void AddItem(InventoryItem item)
    {
        if(mInventoryItems != null)
        {
            if (mInventoryItems.Count <= mSlots)
            {
                mInventoryItems.Add(item);
            }
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if(mInventoryItems != null)
        {
            mInventoryItems.Remove(item);
            // Spawn the item
            //DataManager.Instance.SpawnObject<>();

            // Attach the item to the hand or whatever
        }
    }

    public int Slots
    {
        get { return mSlots; }
    }
}

public enum ePouchState
{
    None = 0,
    Inactive,
    Active,
    Full
}

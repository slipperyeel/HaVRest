using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Pouch : MonoBehaviour
{
    [SerializeField]
    private int mSlots;
    public int Slots { get { return mSlots; } set { mSlots = value; } }

    [SerializeField]
    private List<InventoryItem> mInventoryItems;
    public List<InventoryItem> InventoryItems { get { return mInventoryItems; } set { mInventoryItems = value; } }

    public void AddItem(InventoryItem item)
    {
        if (mInventoryItems != null)
        {
            if (mInventoryItems.Count <= mSlots)
            {
                mInventoryItems.Add(item);
            }
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (mInventoryItems != null)
        {
            mInventoryItems.Remove(item);
            // Spawn the item
            //DataManager.Instance.SpawnObject<>();

            // Attach the item to the hand or whatever
        }
    }
}
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerMomento : GameObjectMomento
{
    private List<ResourceStore> mResourceStore;
    private static readonly string sPrefabName = "FarmingAdventurer";

    // We'll model the backpack as a list of lists of pouches for the momento.
    private List<List<InventoryItem>> mBackPack;

    public PlayerMomento()
    {
        mResourceStore = new List<ResourceStore>();
        mBackPack = new List<List<InventoryItem>>();
        mPrefabName = sPrefabName;

        Debug.Log("Count: " + mBackPack.Count);
    }

    public override void UpdateMomentoData(object obj, string prefabName)
    {
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                ResourceDependentObject resourceComp = go.GetComponent<ResourceDependentObject>();
                Backpack backPackComp = go.GetComponent<Player>().BackPack;

                if (resourceComp != null && backPackComp != null)
                {
                    mResourceStore = resourceComp.MyResourceStore;

                    // Copy over the inventory items.
                    mBackPack = new List<List<InventoryItem>>();
                    for (int i = 0; i < backPackComp.Pouches.Count; i++)
                    {
                        Debug.Log(backPackComp.Pouches[i].InventoryItems);
                        mBackPack.Add(backPackComp.Pouches[i].InventoryItems);
                    }

                    Debug.Log(mBackPack.Count);
                    base.UpdateMomentoData(go, prefabName);
                }
            }
        }
    }

    public override void ApplyMomentoData(object obj)
    {
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                ResourceDependentObject resourceComp = go.GetComponent<ResourceDependentObject>();
                Backpack backPackComp = go.GetComponent<Player>().BackPack;

                if (resourceComp != null && backPackComp != null)
                {
                    resourceComp.MyResourceStore = mResourceStore;
                    // Repopulate the inventory items.
                    for (int i = 0; i < mBackPack.Count; i++)
                    {
                        backPackComp.Pouches[i].InventoryItems = mBackPack[i];
                        backPackComp.Pouches[i].Slots = mBackPack[i].Count; 
                    }
                    base.ApplyMomentoData(resourceComp.gameObject);
                }
            }
        }
    }
}

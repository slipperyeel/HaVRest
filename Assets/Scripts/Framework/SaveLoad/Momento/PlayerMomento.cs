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
    private PlayerSkills mPlayerSkills;

    public PlayerMomento()
    {
        mResourceStore = new List<ResourceStore>();
        mBackPack = new List<List<InventoryItem>>();
        mPrefabName = sPrefabName;
        mPlayerSkills = new PlayerSkills();
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
                mPlayerSkills = go.GetComponent<Player>().PlayerSkills;

                if (resourceComp != null && backPackComp != null)
                {
                    mResourceStore = resourceComp.MyResourceStore;

                    // Copy over the inventory items.
                    mBackPack = new List<List<InventoryItem>>();
                    for (int i = 0; i < backPackComp.Pouches.Count; i++)
                    {
                        mBackPack.Add(backPackComp.Pouches[i].InventoryItems);
                    }

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
                PlayerSkills pSkills = go.GetComponent<Player>().PlayerSkills;

                if (resourceComp != null && backPackComp != null && pSkills != null)
                {
                    resourceComp.MyResourceStore = mResourceStore;

                    pSkills = mPlayerSkills;

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

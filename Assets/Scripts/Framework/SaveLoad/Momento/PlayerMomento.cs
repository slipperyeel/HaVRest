using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerMomento : GameObjectMomento
{
    private List<ResourceStore> mResourceStore;
    private static readonly string sPrefabName = "FarmingAdventurer";

    public PlayerMomento()
    {
        mResourceStore = new List<ResourceStore>();
        mPrefabName = sPrefabName;
    }

    public override void UpdateMomentoData(object obj, string prefabName)
    {
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                ResourceDependentObject resourceComp = go.GetComponent<ResourceDependentObject>();

                if (resourceComp != null)
                {
                    mResourceStore = resourceComp.MyResourceStore;

                    base.UpdateMomentoData(go, prefabName);
                }
            }
        }
    }

    public override void ApplyMomentoData(object obj)
    {
        Debug.Log(obj);
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                ResourceDependentObject resourceComp = go.GetComponent<ResourceDependentObject>();

                if (resourceComp != null)
                {
                    resourceComp.MyResourceStore = mResourceStore;

                    base.ApplyMomentoData(resourceComp.gameObject);
                }
            }
        }
    }
}

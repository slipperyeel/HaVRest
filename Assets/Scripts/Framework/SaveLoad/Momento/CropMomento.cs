using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class CropMomento : GameObjectMomento 
{
    private TemporalLifetime mTemporalLifetime;
    private List<TemporalTrigger> mTemporalTriggers;
    private List<ResourceStore> mResourceStore;

    public CropMomento()
    {
        mTemporalLifetime = new TemporalLifetime();
        mTemporalTriggers = new List<TemporalTrigger>();
        mResourceStore = new List<ResourceStore>();
    }

    public override void UpdateMomentoData(object obj, string prefabName)
    {
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                ResourceDependentCrop resourceComp = go.GetComponent<ResourceDependentCrop>();
                TemporalCrop temporalComp = go.GetComponent<TemporalCrop>();

                if (resourceComp != null && temporalComp != null)
                {
                    mTemporalLifetime = temporalComp.TemporalLifetime;
                    mTemporalTriggers = temporalComp.TemporalTriggers;
                    mResourceStore = resourceComp.MyResourceStore;

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
                ResourceDependentCrop resourceComp = go.GetComponent<ResourceDependentCrop>();
                TemporalCrop temporalComp = go.GetComponent<TemporalCrop>();

                if (resourceComp != null && temporalComp != null)
                {
                    temporalComp.TemporalLifetime = mTemporalLifetime;
                    temporalComp.TemporalTriggers = mTemporalTriggers;
                    resourceComp.MyResourceStore = mResourceStore;

                    base.ApplyMomentoData(resourceComp.gameObject);
                }
            }
        }
    }
}

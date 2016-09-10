using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FoodStuffMomento : GameObjectMomento
{
    private TemporalLifetime mTemporalLifetime;
    private List<TemporalTrigger> mTemporalTriggers;
    private Resource mResource;
    private int mMeshArrayIndex;

    public FoodStuffMomento()
    {
        mTemporalLifetime = new TemporalLifetime();
        mTemporalTriggers = new List<TemporalTrigger>();
        mResource = new Resource();
        mMeshArrayIndex = 0;
    }

    public override void UpdateMomentoData(object obj, string prefabName)
    {
        if (obj != null)
        {
            GameObject go = (GameObject)obj;
            if (go != null)
            {
                TemporalFoodStuff temporalFoodStuff = go.GetComponent<TemporalFoodStuff>();

                if (temporalFoodStuff != null)
                {
                    mTemporalLifetime = temporalFoodStuff.TemporalLifetime;
                    mTemporalTriggers = temporalFoodStuff.TemporalTriggers;
                    mResource = temporalFoodStuff.Resource;
                    mMeshArrayIndex = temporalFoodStuff.MeshArrayIndex;

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
                TemporalFoodStuff temporalFoodStuff = go.GetComponent<TemporalFoodStuff>();

                if (temporalFoodStuff != null)
                {
                    temporalFoodStuff.TemporalLifetime = mTemporalLifetime;
                    temporalFoodStuff.TemporalTriggers = mTemporalTriggers;
                    temporalFoodStuff.Resource = mResource;
                    temporalFoodStuff.MeshArrayIndex = mMeshArrayIndex;

                    base.ApplyMomentoData(temporalFoodStuff.gameObject);
                }
            }
        }
    }
}

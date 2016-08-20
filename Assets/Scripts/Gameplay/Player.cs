using UnityEngine;
using System.Collections;
using System;

public class Player : ResourceDependentObject
{
    // Designer Tweakable Values
    // TODO: These are just test values.
    [SerializeField]
    private int sMinHealthForWarning = 50;

    [SerializeField]
    private int sMinStaminaSluggish = 25;

    [SerializeField]
    private int sMinFoodStarving = 10;

    [SerializeField]
    private int sMinWaterThirsty = 40;

    [SerializeField]
    private int sMinHeatFreezing = 30;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update ()
    {
        base.Update();
	}

    ////////////////////////////////////////////////////////////////////////////
    /// Implementation of ResourceDependentObject (HaVRest)
    ////////////////////////////////////////////////////////////////////////////
    protected override void CheckResourceStoreStatus()
    {
        if (MyResourceStore != null && MyResourceStore.Count > 0)
        {
            int resCount = MyResourceStore.Count;
            for (int i = 0; i < resCount; i++)
            {
                switch (MyResourceStore[i].Resource.Type)
                {
                    case eResourceType.Health:
                        if (MyResourceStore[i].Resource.Quantity >= sMinHealthForWarning)
                        {
                            Debug.Log("Health is getting low!");
                        }
                        break;
                    case eResourceType.Stamina:
                        if (MyResourceStore[i].Resource.Quantity >= sMinStaminaSluggish)
                        {
                            Debug.Log("You're getting sluggish.");
                        }
                        break;
                    case eResourceType.Water:
                        if (MyResourceStore[i].Resource.Quantity >= sMinWaterThirsty)
                        {
                            Debug.Log("Thirsty...");
                        }
                        break;
                    case eResourceType.Food:
                        if (MyResourceStore[i].Resource.Quantity >= sMinFoodStarving)
                        {
                            Debug.Log("You're starving!");
                        }
                        break;
                    case eResourceType.Heat:
                        if (MyResourceStore[i].Resource.Quantity >= sMinHeatFreezing)
                        {
                            Debug.Log("You're freezing!");
                        }
                        break;
                }
            }
        }
    }
}

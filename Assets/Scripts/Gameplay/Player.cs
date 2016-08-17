using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class Player : ResourceDependentObject
{
    // Data Storage
    private static readonly string kSaveFileName = "PlayerData";

    // Formatter
    IFormatter mFormatter = new BinaryFormatter();

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

    private FarmingAdventurer farmer;
    public FarmingAdventurer FarmerSelf
    {
        get { return farmer; }
    }

    protected override void Awake()
    {
        //farmer = new FarmingAdventurer("james", 29);
        Deserialize();
        base.Awake();
    }

    protected override void Update ()
    {
        base.Update();
	}

    ////////////////////////////////////////////////////////////////////////////
    /// Serialization (HaVRest)
    ////////////////////////////////////////////////////////////////////////////
    public void Serialize()
    {
        FileStream s = new FileStream(Application.persistentDataPath + "/" + kSaveFileName, FileMode.OpenOrCreate);
        mFormatter.Serialize(s, farmer);
        s.Close();
    }

    public void Deserialize()
    {
        FileStream s = new FileStream(Application.persistentDataPath + "/" + kSaveFileName, FileMode.Open);
        farmer = (FarmingAdventurer)mFormatter.Deserialize(s);
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
                        farmer.PlayerHealth = MyResourceStore[i].Resource.Quantity;
                        if (MyResourceStore[i].Resource.Quantity >= sMinHealthForWarning)
                        {
                            Debug.Log("Health is getting low!");
                        }
                        break;
                    case eResourceType.Stamina:
                        farmer.PlayerStamina = MyResourceStore[i].Resource.Quantity;
                        if (MyResourceStore[i].Resource.Quantity >= sMinStaminaSluggish)
                        {
                            Debug.Log("You're getting sluggish.");
                        }
                        break;
                    case eResourceType.Water:
                        farmer.PlayerWater = MyResourceStore[i].Resource.Quantity;
                        if (MyResourceStore[i].Resource.Quantity >= sMinWaterThirsty)
                        {
                            Debug.Log("Thirsty...");
                        }
                        break;
                    case eResourceType.Food:
                        farmer.PlayerFood = MyResourceStore[i].Resource.Quantity;
                        if (MyResourceStore[i].Resource.Quantity >= sMinFoodStarving)
                        {
                            Debug.Log("You're starving!");
                        }
                        break;
                    case eResourceType.Heat:
                        farmer.PlayerHeat = MyResourceStore[i].Resource.Quantity;
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

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

/// <summary>
/// Player Class!
/// This is likely only going to be used for restoring / creating the player, as it'll be a live object that will live with a momento.
/// </summary>
[Serializable]
public class FarmingAdventurer : ISerializable
{
    // Keys
    private static readonly string kPlayerNameKey = "farmingGame_PlayerNameKey";
    private static readonly string kPlayerAgeKey = "farmingGame_PlayerAgeKey";
    private static readonly string kPlayerHealthKey = "farmingGame_PlayerHealthKey";
    private static readonly string kPlayerStaminaKey = "farmingGame_PlayerStaminaKey";
    private static readonly string kPlayerHeatKey = "farmingGame_PlayerHeatKey";
    private static readonly string kPlayerFoodKey = "farmingGame_PlayerFoodKey";
    private static readonly string kPlayerWaterKey = "farmingGame_PlayerWaterKey";

    // Values
    private string mPlayerName = "";
    public string PlayerName { get { return mPlayerName; } }

    private int mPlayerAge = 0;
    public int PlayerAge { get { return mPlayerAge; } }

    private int mPlayerHealth = 100;
    public int PlayerHealth { get { return mPlayerHealth; } set { mPlayerHealth = value; } }

    private int mPlayerStamina = 100;
    public int PlayerStamina { get { return mPlayerStamina; } set { mPlayerStamina = value; } }

    private int mPlayerHeat = 100;
    public int PlayerHeat { get { return mPlayerHeat; } set { mPlayerHeat = value; } }

    private int mPlayerFood = 100;
    public int PlayerFood { get { return mPlayerFood; } set { mPlayerFood = value; } }

    private int mPlayerWater = 100;
    public int PlayerWater { get { return mPlayerWater; } set { mPlayerWater = value; } }

    // Inventory
    private SerializableList<int> mItemIndicies = new SerializableList<int>();
    public SerializableList<int> ItemIndicies { get { return mItemIndicies; } }

    public FarmingAdventurer()
    {
        // EMPTY CTOR FOR COMPLIATION
    }

    public FarmingAdventurer(string name, int age)
    {
        mPlayerName = name;
        mPlayerAge = age;
        mItemIndicies = new SerializableList<int>();
    }

    ////////////////////////////////////////////////////////////////////////////
    /// Implementation of ISerializable (C# .Net)
    ////////////////////////////////////////////////////////////////////////////

    public FarmingAdventurer(SerializationInfo information, StreamingContext context)
    {
        mPlayerAge = (int)information.GetValue(kPlayerAgeKey, typeof(int));
        mPlayerName = (string)information.GetValue(kPlayerNameKey, typeof(string));
        mPlayerHealth = (int)information.GetValue(kPlayerHealthKey, typeof(int));
        mPlayerStamina = (int)information.GetValue(kPlayerStaminaKey, typeof(int));
        mPlayerHeat = (int)information.GetValue(kPlayerHeatKey, typeof(int));
        mPlayerFood = (int)information.GetValue(kPlayerFoodKey, typeof(int));
        mPlayerWater = (int)information.GetValue(kPlayerWaterKey, typeof(int));
        mItemIndicies = new SerializableList<int>(information, context);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(kPlayerAgeKey, mPlayerAge);
        info.AddValue(kPlayerNameKey, mPlayerName);
        info.AddValue(kPlayerHealthKey, mPlayerHealth);
        info.AddValue(kPlayerStaminaKey, mPlayerStamina);
        info.AddValue(kPlayerHeatKey, mPlayerHeat);
        info.AddValue(kPlayerFoodKey, mPlayerFood);
        info.AddValue(kPlayerWaterKey, mPlayerWater);
        mItemIndicies.GetObjectData(info, context);
    }
}

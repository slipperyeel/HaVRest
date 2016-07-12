using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

/// <summary>
/// Player Class!
/// </summary>
[Serializable]
public class FarmingAdventurer : ISerializable
{
    // Keys
    static private readonly string kTestKey = "farmingAdventurer_TestKey";

    // Values
    private int val = 1337;

    public FarmingAdventurer()
    {
        // EMPTY CTOR FOR COMPLIATION
    }

    ////////////////////////////////////////////////////////////////////////////
    /// Implementation of ISerializable (C# .Net)
    ////////////////////////////////////////////////////////////////////////////

    public FarmingAdventurer(SerializationInfo information, StreamingContext context)
    {
        val = (int)information.GetValue(kTestKey, typeof(int));
        Debug.Log("Testing Farming Adventurer, value is: " + val);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(kTestKey, val);
    }
}

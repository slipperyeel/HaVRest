using UnityEngine;
using System;
using System.Collections;
using HVRTime;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// A farming game! Stores everything needed to restore the players game state.
/// </summary>
[Serializable]
public class FarmingGame : ISerializable
{
    private HVRDateTime mDateTime;
    public HVRDateTime DateTime { get { return mDateTime; } }

    private FarmingAdventurer mPlayer;
    public FarmingAdventurer Player { get { return mPlayer; } }


    // Data Storage
    private static readonly string kSaveFileName = "GameData";

    // Formatter
    IFormatter mFormatter = new BinaryFormatter();

    // Initial Game Settings
    private static readonly int kInitialHour = 6;
    private static readonly int kInitialDay = 1;
    private static readonly int kInitialMonth = 1;
    private static readonly int kInitlaYear = 2016;

    // Custom Serialized Values
    private bool mHasBeenInitialized = false;

    // Serialization Keys
    private static readonly string kInitializationFlagKey = "farmingGame_InitFlagKey";

    public FarmingGame()
    {
        // EMPTY CTOR FOR COMPLIATION
    }

    public void Initialize()
    {
        if (!mHasBeenInitialized)
        {
            mDateTime = new HVRDateTime(kInitialHour, kInitialDay, kInitialMonth, kInitlaYear, TimeConstants.SECONDS_PER_HOUR * kInitialHour);
            mPlayer = new FarmingAdventurer("james", 29);
            mHasBeenInitialized = true;
        }
    }

    public void Serialize()
    {
        FileStream s = new FileStream(Application.persistentDataPath + "/" + kSaveFileName, FileMode.OpenOrCreate);
        mFormatter.Serialize(s, this);
        s.Close();
    }

    public void Deserialize()
    {
        FileStream s = new FileStream(Application.persistentDataPath + "/" + kSaveFileName, FileMode.Open);
        FarmingGame readObj = (FarmingGame)mFormatter.Deserialize(s);

        if (readObj != null)
        {
            this.mPlayer = readObj.Player;
            this.mDateTime = readObj.DateTime;
        }

        readObj = null;
    }

    ////////////////////////////////////////////////////////////////////////////
    /// Implementation of ISerializable (C# .Net)
    ////////////////////////////////////////////////////////////////////////////
   
    protected FarmingGame(SerializationInfo information, StreamingContext context)
    {
        mHasBeenInitialized = (bool)information.GetValue(kInitializationFlagKey, typeof(bool));
        mDateTime = new HVRDateTime(information, context);
        mPlayer = new FarmingAdventurer(information, context);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(kInitializationFlagKey, mHasBeenInitialized);

        if (mDateTime != null && mPlayer != null)
        {
            mDateTime.GetObjectData(info, context);
            mPlayer.GetObjectData(info, context);
        }
    }
}
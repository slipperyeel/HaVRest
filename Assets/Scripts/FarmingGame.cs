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
    IFormatter formatter = new BinaryFormatter();

    public FarmingGame()
    {
        // EMPTY CTOR FOR COMPLIATION
    }

    public void Initialize()
    {
        // TODO: Make this based on something
        mDateTime = new HVRDateTime(0, 1, 1, 1999, TimeConstants.SECONDS_PER_HOUR * 4.0f);
        mPlayer = new FarmingAdventurer();
    }

    public void Serialize()
    {
        FileStream s = new FileStream(Application.persistentDataPath + "/" + kSaveFileName, FileMode.OpenOrCreate);
        formatter.Serialize(s, this);
        s.Close();
    }

    public void Deserialize()
    {
        FileStream s = new FileStream(Application.persistentDataPath + "/" + kSaveFileName, FileMode.Open);
        FarmingGame readObj = (FarmingGame)formatter.Deserialize(s);

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
        mDateTime = new HVRDateTime(information, context);
        mPlayer = new FarmingAdventurer(information, context);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (mDateTime != null && mPlayer != null)
        {
            mDateTime.GetObjectData(info, context);
            mPlayer.GetObjectData(info, context);
        }
    }
}
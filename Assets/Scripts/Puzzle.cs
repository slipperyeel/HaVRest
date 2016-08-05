using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HVRPuzzle
{
    public enum eType { Temporal, Physical };
    public enum eReward { Currency, Resource, Tool, Seed };

    public enum eTemporalType { TimeOfDay, Duration };
    public enum ePhysicalType { PlayerLocation, ItemLocation, PlayerStateChange, ItemStateChange };

    [System.Serializable]
    public class Puzzle
    {
        public class RewardData
        {
            public eReward RewardType;
            public int RewardAmount;
            public GameObject RewardObject;
        }

        public class TypeData
        {
            public eType Type;

            public eTemporalType TemporalSubType;
            public TimeOfDay TemporalTimeOfDay;
            public float TemporalDuration;

            public ePhysicalType PhysicalSubType;
            public Collider PlayerCollider;
            public Vector3 PlayerPosition;
            public Collider ItemCollider;
            public Vector3 ItemPosition;
            //public ePlayerState PlayerState;
            //public eItemState ItemState;
        }

        [SerializeField]
        private string mName;
        public string Name { get { return mName; } set { mName = value; } }

        [SerializeField]
        private string mDesc;
        public string Description { get { return mDesc; } set { mDesc = value; } }

        [SerializeField]
        private RewardData mReward;
        public RewardData Reward { get { return mReward; } }

        [SerializeField]
        private TypeData mData;
        public TypeData Data { get { return mData; } }

        [SerializeField]
        private TypeData mTrigger;
        public TypeData Trigger { get { return mTrigger; } }

        [SerializeField]
        private bool mIsActive;
        public bool IsActive { get { return mIsActive; } set { mIsActive = value; } }

        public Puzzle()
        {
            mName = "None";
            mDesc = "None";

            mReward = new RewardData();
            mTrigger = new TypeData();
            mData = new TypeData();

            mIsActive = false;
        }

        public Puzzle( eType trigger, eType puzzle, eReward reward, string name = "None", string desc = "None")
        {
            mName = name;
            mDesc = desc;

            mReward = new RewardData();
            mTrigger = new TypeData();
            mData = new TypeData();

            mReward.RewardType = reward;
            mTrigger.Type = trigger;
            mData.Type = puzzle;

            mIsActive = false;
        }
    }
}

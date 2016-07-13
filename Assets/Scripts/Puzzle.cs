using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HVRPuzzle
{
    public enum Type { Temporal, Physical }; // need to be able to set sub-types, such as temporal length and physical trigger object
    public enum Reward { Currency, Resource, Tool, Seed }; // need to be able to set sub-types, such as amount of currency/resource and type of tool/seed

    [System.Serializable]
    public class Puzzle
    {
        private string mName;
        public string Name { get { return mName; } set { mName = value; } }

        private string mDesc;
        public string Description { get { return mDesc; } set { mDesc = value; } }

        private Type mTriggerType;
        public Type TriggerType { get { return mTriggerType; } set { mTriggerType = value; } }

        private Type mPuzzleType;
        public Type PuzzleType { get { return mPuzzleType; } set { mPuzzleType = value; } }

        private List<GameObject> mObjects;
        public List<GameObject> Objects { get { return mObjects; } }

        private Reward mRewardType;
        public Reward RewardType { get { return mRewardType; } set { mRewardType = value; } }

        private bool mIsActive;
        public bool IsActive { get { return mIsActive; } set { mIsActive = value; } }

        public Puzzle()
        {
            mName = "None";
            mDesc = "None";
            mObjects = new List<GameObject>();
            mIsActive = false;
        }

        public Puzzle( Type trigger, Type puzzle, Reward reward, string name = "None", string desc = "None")
        {
            mName = name;
            mDesc = desc;
            mTriggerType = trigger;
            mPuzzleType = puzzle;
            mObjects = new List<GameObject>();
            mRewardType = reward;
            mIsActive = false;
        }
    }
}

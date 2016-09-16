using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// Play Skills
/// </summary>
[Serializable]
public class PlayerSkills
{
    private static readonly int STARTING_SKILL_VALUE = 1;

    private PlayerSkill[] mSkillsArray;
   
    // ctor
    public PlayerSkills()
    {
        mSkillsArray = new PlayerSkill[(int)PlayerSkillsEnum.Count];
    }

    public void InitStats()
    {
        mSkillsArray = new PlayerSkill[(int)PlayerSkillsEnum.Count];
        int skillCount = (int)PlayerSkillsEnum.Count;
        for (int i = 0; i < skillCount; i++)
        {
            mSkillsArray[i] = new PlayerSkill(STARTING_SKILL_VALUE, 0);
        }
    }

    public int GetSkillValue(PlayerSkillsEnum skill)
    {
        int skillIndex = (int)skill;
        int val = 0;
        if (mSkillsArray != null && mSkillsArray.Length > skillIndex)
        {
            val = mSkillsArray[skillIndex].Level;
        }
        return val;
    }

    public PlayerSkill GetPlayerSkill(PlayerSkillsEnum skill)
    {
        int skillIndex = (int)skill;
        PlayerSkill playerSkill = new PlayerSkill();
        if (mSkillsArray != null && mSkillsArray.Length > skillIndex)
        {
            playerSkill = mSkillsArray[skillIndex];
        }
        return playerSkill;
    }

    public void SetPlayerSkill(PlayerSkillsEnum skill, int value)
    {
        int skillIndex = (int)skill;
        if (mSkillsArray != null && mSkillsArray.Length > skillIndex)
        {
            mSkillsArray[skillIndex].Level = value;
        }
    }
}

[Serializable]
public class PlayerSkill
{
    public int Level = 0;
    public int Uses = 0;
    public int Max = 100;

    public PlayerSkill(int level = 0, int uses = 0)
    {
        Level = level;
        Uses = uses;
    }
}


public enum PlayerSkillsEnum
{
    // Attributes
    Strength = 0,
    Stamina,
    
    // Tools
    Hoe,
    Axe,
    Shovel,
    Rake,

    Count
}
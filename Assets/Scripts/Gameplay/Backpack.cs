using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// This is a backpack.
public class Backpack : MonoBehaviour
{
    [SerializeField]
    private List<Pouch> mPouches;
    public List<Pouch> Pouches { get { return mPouches; } set { mPouches = value; } }
}

public enum ePouchState
{
    None = 0,
    Inactive,
    Active,
    Full
}

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A consumable resource base class. It is a temporal object. 
/// i.e; we could have meat that spoils after a while, or water that dries up.
/// </summary>
public abstract class ResourceObject : TemporalObject
{
    public Resource Resource;

    public abstract void OnConsumption();

    public abstract void OnConsumptionFailed();
}

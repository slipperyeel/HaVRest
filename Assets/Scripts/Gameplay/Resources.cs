using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Resource
{
	public eResourceType Type = eResourceType.None; // TODO this should be a list so that resources can have more than one type
	public bool IsPlantable = false;
	public bool IsCraftable = false;
    public int Quantity = 0;
}

[Serializable]
public class ResourceStore
{
    public Resource Resource = new Resource();
    public int Capacity = 0;
    public int DepletionRate = 0;
    public eTemporalTriggerType TimeTrigger = eTemporalTriggerType.None;
}

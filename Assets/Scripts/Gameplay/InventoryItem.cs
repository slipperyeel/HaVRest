using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// Base class for anything that can be put into an inventory. If it doesn't have this component it cannot be placed in your inventory.
/// </summary>
[Serializable]
public class InventoryItem
{
    public int Id;
}

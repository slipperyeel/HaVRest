using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class: PhysicalItem
/// SubClass of: MonoBehaviour
/// Purpose: Defines basic interactions and features of 
/// all physical items.
/// </summary>
public abstract class PhysicalItem : MonoBehaviour
{
    // Serialized Variables
    [SerializeField]
    protected float sMaxDurability = 1.0f;

    [SerializeField]
    protected float sBaseDurabilityCostPerUse = 1.0f;

    [SerializeField]
    protected float sImpactStrength = 1.0f;

    [SerializeField]
    protected eImpactType sImpactType = eImpactType.Blunt;

    /// Member Variables
    protected float mCurrentDurability;

    // Methods
    void ItemCollisionDetector_CollisionEnter(Collision col)
    {
        HandleItemCollisionEnter(col);
    }

    void ItemCollisionDetector_CollisionExit(Collision col)
    {
        HandleItemCollisionExit(col);
    }

    void ItemCollisionDetector_CollisionStay(Collision col)
    {
        HandleItemCollisionStay(col);
    }

    // Abstract Implementation
    protected abstract void HandleItemCollisionEnter(Collision col);
    protected abstract void HandleItemCollisionExit(Collision col);
    protected abstract void HandleItemCollisionStay(Collision col);
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class: Physical Object
/// Subclass Of: MonoBehaviour
/// Purpose: To provide contextual data for a collision with this object
/// so that a PhysicalItem may react to the collision
/// </summary>
public class PhysicalObject : MonoBehaviour
{
    // Member Variables
    private Collider mCollider;
    private Rigidbody mRigidBody;

    [SerializeField]
    private List<PhysicalResistance> sPhysicalResistances;

    void Awake()
    {
        mCollider = GetComponent<Collider>();
        mRigidBody = GetComponent<Rigidbody>();

        Debug.Assert(mCollider != null, "PhysicalObject needs a Collider in order to work.");
        Debug.Assert(mRigidBody != null, "PhysicalObject needs a RigidBody in order to work.");
        Debug.Assert(sPhysicalResistances != null, "Physical Resistances is null, this is a useless component.");
    }

    public float GetPhysicalResistanceByImpactType(eImpactType impactType)
    {
        if (sPhysicalResistances != null)
        {
            PhysicalResistance resistance = sPhysicalResistances.Find(r => r.ImpactType == impactType);
            return resistance.Resistance;
        }
        else
        {
            return 0.0f;
        }
    }
}

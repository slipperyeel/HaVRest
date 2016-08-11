using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Struct: PhysicalResistance
/// Purpose: To provide a link of physical resistances based
/// on eImpactType
/// </summary>
[Serializable]
public struct PhysicalResistance
{
    public eImpactType ImpactType;
    public float Resistance;
}

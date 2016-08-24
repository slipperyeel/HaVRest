using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Temporal Crop
/// </summary>
public class TemporalCrop : TemporalObject
{
    [SerializeField]
    private Mesh[] meshArray;

    private MeshFilter mMeshFilter;

    private int mMeshArrayIndex = 0;
    public int MeshArrayIndex { get { return mMeshArrayIndex; } set { mMeshArrayIndex = value; } }
    
    protected override void ApplyTemporalOutcome(eTemporalTriggerOutcome outcome)
    {
        switch (outcome)
        {
            case eTemporalTriggerOutcome.Crop_Grow:
                mMeshArrayIndex++;
                if (mMeshFilter != null && meshArray.Length >= mMeshArrayIndex)
                {
                    mMeshFilter.sharedMesh = meshArray[mMeshArrayIndex];
                }
                else
                {
                    Debug.Assert(false, "Invalid mesh array index for crop: " + this.name);
                }
                break;
            case eTemporalTriggerOutcome.Crop_Harvestable:
                break;
            case eTemporalTriggerOutcome.Crop_Spoiled:
                break;
            case eTemporalTriggerOutcome.Crop_Dead:
                GameObject.Destroy(this.gameObject);
                break;
            default:
                break;
        }
    }

    protected override void Awake()
    {
        mMeshFilter = GetComponent<MeshFilter>();
        Debug.Assert(mMeshFilter != null, "Mesh filter is null on crop: " + this.name);

        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// Override of the TimeChangedEvent handler method. 
    /// We could do something unique here, like a shimmer fx or sound fx.
    /// </summary>
    /// <param name="tce">Time Changed Event from the TimeManager</param>
    protected override void HandleTimeChangedEvent(TimeChangedEvent tce)
    {
        base.HandleTimeChangedEvent(tce);
    }
}
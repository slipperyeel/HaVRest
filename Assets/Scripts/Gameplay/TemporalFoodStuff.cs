using UnityEngine;
using System.Collections;
using System;

public class TemporalFoodStuff : ResourceObject
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
            case eTemporalTriggerOutcome.Food_Ripe:
            case eTemporalTriggerOutcome.Food_Spoiled:
            case eTemporalTriggerOutcome.Food_Rotten:
                mMeshArrayIndex++;
                if (mMeshFilter != null && meshArray.Length >= mMeshArrayIndex)
                {
                    mMeshFilter.sharedMesh = meshArray[mMeshArrayIndex];
                }
                else
                {
                    Debug.Assert(false, "Invalid mesh array index for crop: " + this.name);
                }
                // TODO(jamesp): Do stuff for the resoucess when these happen
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

    public override void OnConsumption()
    {
        Debug.Log("Consumption of " + this.name);
    }

    public override void OnConsumptionFailed()
    {
        Debug.Log("YOU SOMEHOW FAILED TO EAT ME!");
    }
}

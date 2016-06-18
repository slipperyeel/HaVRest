using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A temporal crop! Bare bones now, to test out the code.
/// </summary>
public class TemporalCrop : TemporalObject
{
    protected override void ApplyTemporalOutcome(eTemporalTriggerOutcome outcome)
    {
        switch (outcome)
        {
            case eTemporalTriggerOutcome.Crop_Grow:
                Debug.Log("This Crop Grew up! Be proud.");
                break;
            case eTemporalTriggerOutcome.Crop_Harvestable:
                Debug.Log("This Crop is now harvestable! Yeah!");
                break;
            case eTemporalTriggerOutcome.Crop_Spoiled:
                Debug.Log("This crop be spoiled. You failed.");
                break;
            case eTemporalTriggerOutcome.Crop_Dead:
                Debug.Log("This crop died. oh well, isn't that sad.");
                break;
            default:
                break;
        }
    }

    protected override void Awake()
    {
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
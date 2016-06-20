using UnityEngine;
using System.Collections;
using System;

public class TestResource : ResourceObject
{
    public override void OnConsumption()
    {
        Debug.Log("I was consumed!");
        GameObject.Destroy(this);
    }

    public override void OnConsumptionFailed()
    {
        Debug.Log("Consumption failed.");
    }

    protected override void ApplyTemporalOutcome(eTemporalTriggerOutcome outcome)
    {
        // Do nothing. 
    }
}

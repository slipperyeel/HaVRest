using System;
using UnityEngine;
using System.Collections;
using HVRTime;

/// <summary>
/// A Temporal trigger is meant to be a tunable field that defines when an outcome happens, what that outcome is, and what triggers it.
/// By using tuned outcomes and triggers, we can finely tune and generate unique and interesting temporal objects.
/// </summary>
[Serializable]
public class TemporalTrigger
{
    public eTemporalTriggerType Type = eTemporalTriggerType.None;
    public eTemporalTriggerOutcome Outcome = eTemporalTriggerOutcome.None;
    public int Ticks = 0;
    public bool Triggered = false;
}

/// <summary>
/// Temporal Triggers. These are the types of temporal triggers that can affect a temporal object
/// </summary>
public enum eTemporalTriggerType
{
    None = -1,
    Hour,
    Day,
    Month,
    Year,
    Count = Year
}

/// <summary>
/// Temporal Trigger Outcomes. This list will probably get really large.
/// </summary>
public enum eTemporalTriggerOutcome
{
    None = -1,
    Crop_Grow,
    Crop_Harvestable,
    Crop_Spoiled,
    Crop_Dead,
    Count = Crop_Dead
}
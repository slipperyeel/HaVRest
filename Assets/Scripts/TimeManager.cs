using UnityEngine;
using System.Collections;

/// <summary>
/// Houses the core logic for dictating the passage of time within the game
/// TODO(JP): Finish this concept. It's just me thinking a bit before bed so far.
/// </summary>
public class TimeManager : Singleton<TimeManager>
{
    /// <summary>
    /// Private Members
    /// </summary>
    private long totalTimePassed;
    private TimeOfDay currentTimeOfDay = TimeOfDay.EarlyMorning;

    /// <summary>
    /// Public Members
    /// </summary>
    public TimeOfDay GetCurrentTimeOfDay()
    {
        return currentTimeOfDay;
    }

    /// <summary>
    /// Constants
    /// </summary>
    private static readonly long HOURS_PER_TOD_CYCLE = 4;
    private static readonly long SECONDS_PER_MINUTE = 60;
    private static readonly long SECONDS_PER_HOUR = SECONDS_PER_MINUTE * 60;
    private static readonly long SECONDS_PER_DAY = SECONDS_PER_HOUR * 24;
    private static readonly long SECONDS_PER_TOD_CYCLE = SECONDS_PER_HOUR * HOURS_PER_TOD_CYCLE;

    void Awake()
    {
        // TODO(JP): Use a real save system. Currently this means that we'll always start at early morning.
        currentTimeOfDay = TimeOfDay.EarlyMorning;
        totalTimePassed = 0;
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
        totalTimePassed += (long)Time.deltaTime;
	}
}

/// <summary>
/// The game will always "Start" at a TimeOfDay, and the clock will tick from there.
/// TODO(JP): Lock saves to each time of day.
/// </summary>
public enum TimeOfDay
{
    EarlyMorning = 1,       // 4AM->8AM
    Morning,                // 8AM->12PM   
    Afternoon,              // 12PM->4PM
    LateAfternoon,          // 4PM->8PM
    Dusk,                   // 8PM->12AM
    Night                   // 12AM->4AM
}


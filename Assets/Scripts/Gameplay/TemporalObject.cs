#define DEVELOPMENT_NO_SAVELOAD

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HVRTime;
using slipperyeel;

/// <summary>
/// The base class for any object that is affected by the passage of time.
/// I went with Hours, days, months and years instead of a simple single integer because I figured there would be a very large amount of objects and I wanted to keep
/// Event processing to a minimum. A crop, for instance, might (and probably will) only have day triggers.
/// A sign post that displays the current year, only really needs to get yearly updates.
/// Your player character needs to get all of them.
/// By creating a "slightly" more complex data structure for time, we can drastically simplify the work each individual object has to do to process it's temporal events.
/// </summary>
public abstract class TemporalObject : MonoBehaviour
{
    // Designed temporal triggers. There can be as many or few of these as a temporal object wants, though it makes no sense to have none.
    [SerializeField]
    protected List<TemporalTrigger> mTemporalTriggers;

    // I intend this to be the save structure for a temporal object.
    protected TemporalLifetime mTemporalLifetime;

    // Set up the event listeners.
    protected virtual void Awake()
    {
        SEEventManager.Instance.AddListener<TimeChangedEvent>(HandleTimeChangedEvent);

    // Debug time because we have no save load yet.
#if DEVELOPMENT_NO_SAVELOAD
        mTemporalLifetime = new TemporalLifetime();
#endif
    }

    protected virtual void OnDestroy()
    {
        SEEventManager.Instance.RemoveListener<TimeChangedEvent>(HandleTimeChangedEvent);
    }

    /// <summary>
    /// SE Event Handler for TimeChangedEvent
    /// Increments lifetype counters and checks/processes relevant triggers.
    /// </summary>
    /// <param name="tce">A TCE Event, from the TimeManager</param>
    protected virtual void HandleTimeChangedEvent(TimeChangedEvent tce)
    {
        if (tce.HourChanged)
        {
            mTemporalLifetime.Hours++;
            CheckTemporalTriggerStatus(eTemporalTriggerType.Hour, mTemporalLifetime.Hours);
        }

        if (tce.DayChanged)
        {
            mTemporalLifetime.Days++;
            CheckTemporalTriggerStatus(eTemporalTriggerType.Day, mTemporalLifetime.Days);
        }

        if (tce.MonthChanged)
        {
            mTemporalLifetime.Months++;
            CheckTemporalTriggerStatus(eTemporalTriggerType.Month, mTemporalLifetime.Months);
        }

        if (tce.YearChanged)
        {
            mTemporalLifetime.Years++;
            CheckTemporalTriggerStatus(eTemporalTriggerType.Year, mTemporalLifetime.Years);
        }
    }

    /// <summary>
    /// Checks the temporal trigger status by observing the set of triggers this particular
    /// TemporalObject has and then attempts to process it.
    /// </summary>
    /// <param name="type">Current temporal type we are processing</param>
    /// <param name="ticksPassed">Number of ticks passed for this particular trigger type</param>
    private void CheckTemporalTriggerStatus(eTemporalTriggerType type, int ticksPassed)
    {
        if (mTemporalTriggers != null)
        {
            for (int i = 0; i < mTemporalTriggers.Count; i++)
            {
                if (mTemporalTriggers[i] != null)
                {
                    if (mTemporalTriggers[i].Type == type)
                    {
                        ProcessTemporalTrigger(mTemporalTriggers[i], ticksPassed);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Attempts to process a specific TemporalTrigger belonging to this TemporalObject
    /// If the trigger has not yet been triggered, it checks the tick count and if requirements are met, Triggers the event and calls
    /// The abstract method ApplyTemporalOutcome which is to be implemented by the specific TemporalObject subclass.
    /// </summary>
    /// <param name="trigger">Current processing trigger</param>
    /// <param name="ticksPassed">Current ticks passed for this triggers type</param>
    protected void ProcessTemporalTrigger(TemporalTrigger trigger, int ticksPassed)
    {
        if (!trigger.Triggered)
        {
            if (trigger.Ticks <= ticksPassed)
            {
                trigger.Triggered = true;

                ApplyTemporalOutcome(trigger.Outcome);
            }
        }
    }

    /// <summary>
    /// Abstract method to be implemented by each temporal object subclass.
    /// The intention here is that the subclass handles what it does when certain outcomes are triggered.
    /// </summary>
    /// <param name="outcome">A temporal outcome. This could be a crop being harvestable, dying, a sign changing, a tool rusting, etc.</param>
    protected abstract void ApplyTemporalOutcome(eTemporalTriggerOutcome outcome);
}
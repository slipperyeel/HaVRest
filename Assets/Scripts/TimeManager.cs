using UnityEngine;
using System.Collections;
using System;

namespace HVRTime
{
    public delegate void OnDayChanged(object sender, EventArgs e);
    public delegate void OnMonthChanged(object sender, EventArgs e);
    public delegate void OnYearChanged(object sender, EventArgs e);

    /// <summary>
    /// Houses the core logic for dictating the passage of time within the game
    /// TODO(JP): Finish this concept. It's just me thinking a bit before bed so far.
    /// </summary>
    public class TimeManager : Singleton<TimeManager>
    {
        /// <summary>
        /// Private Members
        /// </summary>
        private float mTotalTimePassed;
        private TimeOfDay mCurrentTimeOfDay = TimeOfDay.EarlyMorning;
        private HVRDateTime mDateTime;

        private int mCurrentDay;
        private int mCurrentMonth;
        private int mCurrentYear;

        /// <summary>
        /// Public Members
        /// </summary>
        public TimeOfDay GetCurrentTimeOfDay()
        {
            return mCurrentTimeOfDay;
        }

        /// <summary>
        /// Events
        /// </summary>
        public event OnDayChanged OnDayChanged;
        public event OnMonthChanged OnMonthChanged;
        public event OnYearChanged OnYearChanged;

        /// <summary>
        /// Tunable Fields
        /// </summary>
        [SerializeField]
        private int tTimeScale = 5;

        void Awake()
        {
            // TODO(JP): Use a real save system. Currently this means that we'll always start at early morning.
            mDateTime = new HVRDateTime(1, 1, 1999, TimeConstants.SECONDS_PER_HOUR * 4.0f);
            mCurrentTimeOfDay = TimeOfDay.EarlyMorning;
            mTotalTimePassed = 0.0f;

            mCurrentDay = mDateTime.GetDay();
            mCurrentMonth = mDateTime.GetMonth();
            mCurrentYear = mDateTime.GetYear();
        }

        void Start()
        {
            if (mDateTime != null)
            {
                mDateTime.OnDateChanged += new OnDateChanged(OnPlayerDateTimeChanged);
            }
        }

        void Update()
        {
            // Increment our time and apply the time scaler 
            float mTimePassedDelta = Time.deltaTime * tTimeScale;
            mTotalTimePassed += mTimePassedDelta;

            // Increment our passage of time
            mDateTime.ApplyPassageOfTime(mTimePassedDelta);
            //mDateTime.PrintDateTime();
        }

        private void OnPlayerDateTimeChanged(object sender, EventArgs e)
        {
            Debug.Log("DateTimeChanged!");
            if (sender != null)
            {
                HVRDateTime timeFromEvent = (HVRDateTime)sender;
                if (timeFromEvent != null)
                {

                    if (mCurrentDay != timeFromEvent.GetDay())
                    {
                        // Day Changed.
                        if (OnDayChanged != null)
                        {
                            OnDayChanged(this, EventArgs.Empty);
                        }
                    }

                    if (mCurrentMonth != timeFromEvent.GetMonth())
                    {
                        // Month Changed.
                        if (OnMonthChanged != null)
                        {
                            OnMonthChanged(this, EventArgs.Empty);
                        }
                    }

                    if (mCurrentYear != timeFromEvent.GetYear())
                    {
                        // Year Changed.
                        if (OnYearChanged != null)
                        {
                            OnYearChanged(this, EventArgs.Empty);
                        }
                    }

                    // Store the current date values locally
                    mCurrentDay = timeFromEvent.GetDay();
                    mCurrentMonth = timeFromEvent.GetMonth();
                    mCurrentYear = timeFromEvent.GetYear();

                    timeFromEvent.PrintDateTime();
                }
            }
        }
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
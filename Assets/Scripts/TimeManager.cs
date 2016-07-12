using UnityEngine;
using System.Collections;
using System;
using slipperyeel;

namespace HVRTime
{
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

        private int mCurrentHour;
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
        /// Tunable Fields
        /// </summary>
        [SerializeField]
        private int tTimeScale = 5;

        /// <summary>
        /// Components
        /// </summary>
        [SerializeField]
        private GameObject cSun;

        void Awake()
        {
            // Awake should only be used in GameManager... OOO
        }

        void Start()
        {
            mDateTime = GameManager.Instance.Game.DateTime;

            mCurrentTimeOfDay = TimeOfDay.EarlyMorning;
            mTotalTimePassed = 0.0f;

            mCurrentHour = mDateTime.GetHour();
            mCurrentDay = mDateTime.GetDay();
            mCurrentMonth = mDateTime.GetMonth();
            mCurrentYear = mDateTime.GetYear();

            Debug.Assert(cSun == null, "No sun set!");

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
            RotateSun();
        }

        // mDateTime.GetDayTimeSeconds() is the current time in seconds.
        // 0 degrees is sun rise. 90 degrees is 12pm noon. is 43200seconds
        private void RotateSun()
        {
            if(cSun != null)
            {
                float curTimeSeconds = mDateTime.GetDayTimeSeconds();
                float rotation = Mathf.Lerp(0.0f, 360.0f, (curTimeSeconds / TimeConstants.SECONDS_PER_DAY));
                Quaternion sunRotation = cSun.transform.rotation;
                sunRotation = Quaternion.Euler(rotation, sunRotation.y, sunRotation.z);
                cSun.transform.rotation = sunRotation;
            }
        }

        private void OnPlayerDateTimeChanged(object sender, EventArgs e)
        {
            Debug.Log("DateTimeChanged!");
            if (sender != null)
            {
                HVRDateTime timeFromEvent = (HVRDateTime)sender;
                TimeChangedEvent tce = new TimeChangedEvent(timeFromEvent, false, false, false, false);
                if (timeFromEvent != null)
                {
                    if(mCurrentHour != timeFromEvent.GetHour())
                    {
                        tce.HourChanged = true;
                    }

                    if (mCurrentDay != timeFromEvent.GetDay())
                    {
                        tce.DayChanged = true;
                    }

                    if (mCurrentMonth != timeFromEvent.GetMonth())
                    {
                        tce.MonthChanged = true;
                    }

                    if (mCurrentYear != timeFromEvent.GetYear())
                    {
                        tce.YearChanged = true;
                    }

                    // Store the current date values locally
                    mCurrentHour = timeFromEvent.GetHour();
                    mCurrentDay = timeFromEvent.GetDay();
                    mCurrentMonth = timeFromEvent.GetMonth();
                    mCurrentYear = timeFromEvent.GetYear();

                    timeFromEvent.PrintDateTime();

                    SEEventManager.Instance.TriggerEvent(tce);
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

public class TimeChangedEvent : SEGameEvent
{
    public HVRTime.HVRDateTime dateTime;
    public bool HourChanged;
    public bool DayChanged;
    public bool MonthChanged;
    public bool YearChanged;

    public TimeChangedEvent(HVRTime.HVRDateTime dt, bool hourChanged, bool dayChanged, bool monthChanged, bool yearChanged)
    {
        this.dateTime = dt;
        this.HourChanged = hourChanged;
        this.DayChanged = dayChanged;
        this.MonthChanged = monthChanged;
        this.YearChanged = yearChanged;
    }
}

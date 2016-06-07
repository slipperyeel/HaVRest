using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// DateTime Object specific to HVR
/// </summary>
namespace HVRTime
{
    // Event delegate definitions
    public delegate void OnDateChanged(object sender, EventArgs e);

    public class HVRDateTime
    {
        /// <summary>
        /// Private Members
        /// </summary>
        private int mDay;
        private int mMonth;
        private int mYear;
        private float mDayTimeSeconds;

        /// <summary>
        /// Accessors
        /// </summary>
        /// <returns></returns>
        public int GetDay()
        {
            return mDay;
        }

        public int GetMonth()
        {
            return mMonth;
        }

        public int GetYear()
        {
            return mYear;
        }

        public float GetDayTimeSeconds()
        {
            return mDayTimeSeconds;
        }

        /// <summary>
        /// Class Management
        /// </summary>

        // Default ctor
        public HVRDateTime()
        {
            mDay = 1;
            mMonth = 1;
            mYear = 2000;
            mDayTimeSeconds = 0.0f;
        }

        // Specific DateTime ctor
        public HVRDateTime(int day, int month, int year, float dayTimeSeconds)
        {
            mDay = day;
            mMonth = month;
            mYear = year;
            mDayTimeSeconds = dayTimeSeconds;
        }

        /// <summary>
        /// Events
        /// </summary>
        public event OnDateChanged OnDateChanged;

        /// <summary>
        /// We have 12 Months, each one has 30 days.
        /// Each day has 24 hours.
        /// </summary>
        /// <param name="timePassed"></param>
        public void ApplyPassageOfTime(float timePassed)
        {
            bool dateChanged = false;
            mDayTimeSeconds += timePassed;

            if (mDayTimeSeconds >= TimeConstants.SECONDS_PER_DAY)
            {
                mDay++;
                mDayTimeSeconds = 0.0f;
                dateChanged = true;

                if (mDay >= TimeConstants.DAYS_PER_MONTH)
                {
                    mMonth++;
                    mDay = 1;

                    if (mMonth >= TimeConstants.MONTHS_PER_YEAR)
                    {
                        mYear++;
                        mMonth = 1;
                    }
                }
            }

            if (dateChanged)
            {
                if (OnDateChanged != null)
                {
                    OnDateChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Debug
        /// </summary>
        public void PrintDateTime()
        {
            string debugPrint = string.Format("(CurrentTime) - [Year: {0}] [Month: {1}] [Day: {2}] [Seconds: {3}]", mYear, mMonth, mDay, mDayTimeSeconds);
            Debug.Log(debugPrint);
            debugPrint = null;
        }
    }
}
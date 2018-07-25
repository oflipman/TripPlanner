using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripPlanner.Models;

namespace TripPlanner.ViewModels
{
    public class DayOfTrip
    {
        public double StartHour;
        public double EndHour;
        public double LunchHour;
        public double DinnerHour;
        public ActivityTimeSlot MainActivity;

        public List<ActivityTimeSlot> SortedActivities { get; set; }

        public DayOfTrip(int intenseLevel, ActivityPoint mainActivity)
        {
            SortedActivities = new List<ActivityTimeSlot>();

            LunchHour = 12;
            DinnerHour = 18;
            switch (intenseLevel) 
            {
                case 1:
                    StartHour = 10;
                    EndHour = 19;
                    break;
                case 2:
                    StartHour = 09;
                    EndHour = 19;
                    break;
                case 3:
                    StartHour = 09;
                    EndHour = 20;
                    break;
                case 4:
                    StartHour = 09;
                    EndHour = 21;
                    break;
                default:
                    StartHour = 08;
                    EndHour = 22;
                    break;
            }

            MainActivity = new ActivityTimeSlot(new TimeSlot((int)mainActivity.OpeningTime + 1, (int)mainActivity.OpeningTime + 1 + (int)mainActivity.AvgSpendingTimeInHours), mainActivity);

            //avoiding collisions
            if (MainActivity.TimeSlot.IsInTimeSlot(StartHour, StartHour + 1)) //breakfest
            {
                StartHour = MainActivity.TimeSlot.StartTime - 1;
            }
            if (MainActivity.TimeSlot.IsInTimeSlot(LunchHour, LunchHour + 1)) //lunch
            {
                LunchHour = MainActivity.TimeSlot.EndTime;
            }
            if (MainActivity.TimeSlot.IsInTimeSlot(DinnerHour, DinnerHour + 1)) //dinner
            {
                DinnerHour = MainActivity.TimeSlot.EndTime;
            }
            SortedActivities.Add(MainActivity);
        }

        public bool IsValidToAdd(ActivityTimeSlot activityTimeSlot)
        {
            return SortedActivities.Any(a =>
                a.TimeSlot.StartTime < activityTimeSlot.TimeSlot.StartTime && a.TimeSlot.StartTime < activityTimeSlot.TimeSlot.EndTime
                || a.TimeSlot.EndTime > activityTimeSlot.TimeSlot.StartTime && a.TimeSlot.EndTime > activityTimeSlot.TimeSlot.EndTime);
        }

        public void AddActivity(ActivityTimeSlot activityTimeSlot)
        {
            SortedActivities.Add(activityTimeSlot);
            SortedActivities = SortedActivities.OrderBy(a => a.TimeSlot.StartTime).ToList();
        }

        public void AppendActivities(List<ActivityTimeSlot> activities)
        {
            SortedActivities.AddRange(activities);
            SortedActivities = SortedActivities.OrderBy(a => a.TimeSlot.StartTime).ToList();
        }

        public TimeWindow GetNextEmptyTimeWindow()
        {
            var startTime = StartHour;
            ActivityTimeSlot activityBefore = null;
            foreach (var activity in SortedActivities)
            {
                if (startTime < activity.TimeSlot.StartTime)
                {
                    return new TimeWindow(new TimeSlot(startTime, activity.TimeSlot.StartTime), activityBefore, activity);
                }

                startTime = activity.TimeSlot.EndTime;
                activityBefore = activity;
            }

            if (startTime < EndHour)
            {
                return new TimeWindow(new TimeSlot(startTime, EndHour), SortedActivities.LastOrDefault(), null);
            }

            return null;
        }
    }
}
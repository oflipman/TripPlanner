using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.UI.WebControls;
using TripPlanner.Extensions;
using TripPlanner.Models;
using TripPlanner.Repositories;
using TripPlanner.Utils;
using TripPlanner.ViewModels;

namespace TripPlanner.BL
{
    public static class ActivityPointBL
    {
        private static int nearByDistanceKm = 20;

        public static Trip GetTripPlan(UserRequest userRequest)
        {
            var trip = new Trip();
            using (var context = new ActivityPointsDbContext())
            {
                var activities = context.ActivityPoints.ToList();
                var nearByActivities = activities.Where(a => IsNearBy(a, userRequest.VacationDetails.SleepLocation, nearByDistanceKm)).ToList();

                trip.DayTrips = new List<DayOfTrip>();

                for (int i = 0; i < userRequest.VacationDetails.Days; i++)
                {
                    if (nearByActivities.Count == 0)
                    {
                        break;
                    }

                    var mainActivity = nearByActivities.Aggregate((currMax, a) =>
                        currMax == null || GetMatchGrade(a, userRequest.Preferences) <
                        GetMatchGrade(currMax, userRequest.Preferences)
                            ? a
                            : currMax);
                    var day = new DayOfTrip(userRequest.Preferences[eActivityType.Intencity], mainActivity);
                    nearByActivities.Remove(mainActivity);
                    FillDay(day, nearByActivities, userRequest.Preferences);
                    trip.DayTrips.Add(day);
                }
            }

            return trip;
        }

        public static bool IsNearBy(ActivityPoint activityPoint, Location startPoint, int nearByDistanceKm)
        {
            return LocationUtils.HaversineDistance(new Location(activityPoint.GEPSLatitude, activityPoint.GEPSLongtitude), startPoint) <= nearByDistanceKm;
        }

        public static int GetMatchGrade(ActivityPoint activityPoint, Dictionary<eActivityType, int> preferences)
        {
            if (!activityPoint.Accessibility && preferences[eActivityType.Walking] == 1)
            {
                return 0;
            }

            return preferences[(eActivityType)Enum.Parse(typeof(eActivityType), activityPoint.Type)];
        }

        public static void FillDay(DayOfTrip dayOfTrip, List<ActivityPoint> remainedActivities, Dictionary<eActivityType, int> preferences)
        {
            TimeSlot timeSlot;

            //sort first by near to main acitivity then by user prefrences
            //TODO: add sort by rank
            var sortedByLocationAcitivities = remainedActivities.OrderBy(a =>
                LocationUtils.HaversineDistance(new Location(a.GEPSLatitude, a.GEPSLongtitude),
                    new Location(dayOfTrip.MainActivity.Activity.GEPSLatitude, dayOfTrip.MainActivity.Activity.GEPSLongtitude)))
                .ThenByDescending(a => GetMatchGrade(a, preferences)).ToList();

            while ((timeSlot = dayOfTrip.GetNextEmptyTimeSlot()) != null)
            {
                ActivityTimeSlot activityTimeSlot = null;
                foreach (var activity in sortedByLocationAcitivities)
                {
                    if((activityTimeSlot = FindTimeSlotForActivity(activity, timeSlot)) != null)
                    {
                        dayOfTrip.AddActivity(activityTimeSlot);
                        sortedByLocationAcitivities.Remove(activity);
                        remainedActivities.Remove(activity);
                        break;
                    }
                }

                if (activityTimeSlot == null)
                {
                    dayOfTrip.AddActivity(new ActivityTimeSlot(timeSlot, null));
                }
            }

            dayOfTrip.SortedActivities.RemoveAll(a => a.Activity == null); //Remove empty time slots
        }

        public static ActivityTimeSlot FindTimeSlotForActivity(ActivityPoint activityPoint, TimeSlot freeTimeSlot)
        {
            var duration = (int)activityPoint.AvgSpendingTimeInHours;

            if (duration > freeTimeSlot.EndTime - freeTimeSlot.StartTime)
            {
                return null;
            }

            if (activityPoint.OpeningTime < freeTimeSlot.StartTime && freeTimeSlot.StartTime + duration <= activityPoint.ClosingTime)
            {
                return new ActivityTimeSlot(new TimeSlot(freeTimeSlot.StartTime, freeTimeSlot.StartTime + duration), activityPoint);
            }

            if (activityPoint.ClosingTime > freeTimeSlot.EndTime && freeTimeSlot.EndTime - duration >= activityPoint.OpeningTime)
            {
                return new ActivityTimeSlot(new TimeSlot(freeTimeSlot.EndTime - duration, freeTimeSlot.EndTime), activityPoint);
            }

            if (activityPoint.OpeningTime >= freeTimeSlot.StartTime && activityPoint.OpeningTime + duration <= freeTimeSlot.EndTime)
            {
                return new ActivityTimeSlot(new TimeSlot((int)activityPoint.OpeningTime, (int)activityPoint.OpeningTime + duration), activityPoint);
            }

            return null;
        }
    }
}
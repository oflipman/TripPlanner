using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Services.Protocols;
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
            TimeWindow timeWindow;

            //sort first by near to main acitivity then by user prefrences
            //TODO: add sort by rank
            var sortedByLocationAcitivities = remainedActivities.OrderBy(a =>
                LocationUtils.HaversineDistance(new Location(a.GEPSLatitude, a.GEPSLongtitude),
                    new Location(dayOfTrip.MainActivity.Activity.GEPSLatitude, dayOfTrip.MainActivity.Activity.GEPSLongtitude)))
                .ThenByDescending(a => GetMatchGrade(a, preferences)).ToList();

            while ((timeWindow = dayOfTrip.GetNextEmptyTimeWindow()) != null)
            {
                List<ActivityTimeSlot> activities = null;
                foreach (var activity in sortedByLocationAcitivities)
                {
                    if((activities = TryFindTimeForActivityAndTravel(activity, timeWindow)) != null)
                    {
                        dayOfTrip.AppendActivities(activities);
                        sortedByLocationAcitivities.Remove(activity);
                        remainedActivities.Remove(activity);
                        break;
                    }
                }

                if (activities == null)
                {
                    dayOfTrip.AddActivity(new ActivityTimeSlot(timeWindow.TimeSlot, null));
                }
            }

            dayOfTrip.SortedActivities.RemoveAll(a => a.Activity == null && a.IsTraveling == false); //Remove empty time slots
        }

        public static List<ActivityTimeSlot> TryFindTimeForActivityAndTravel(ActivityPoint activityPoint, TimeWindow freeTimeWindow)
        {
            var activityPointAndTravel = new List<ActivityTimeSlot>();
            var drivingTimeToActivity =
                CalcDrivingTime(freeTimeWindow.ActivityBefore?.Activity?.Location, activityPoint.Location);
            var drivingTimeFromActivity = CalcDrivingTime(activityPoint.Location, freeTimeWindow.ActivityAfter?.Activity?.Location);
            var duration = (int)activityPoint.AvgSpendingTimeInHours + drivingTimeToActivity + drivingTimeFromActivity;
            ActivityTimeSlot activityTimeSlot = null;

            if (duration > freeTimeWindow.TimeSlot.EndTime - freeTimeWindow.TimeSlot.StartTime)
            {
                return null;
            }

            if (activityPoint.OpeningTime < freeTimeWindow.TimeSlot.StartTime && freeTimeWindow.TimeSlot.StartTime + duration <= activityPoint.ClosingTime)
            {
                activityTimeSlot = new ActivityTimeSlot(new TimeSlot(freeTimeWindow.TimeSlot.StartTime + drivingTimeToActivity, freeTimeWindow.TimeSlot.StartTime + activityPoint.AvgSpendingTimeInHours + drivingTimeToActivity), activityPoint);
            }

            if (activityPoint.ClosingTime > freeTimeWindow.TimeSlot.EndTime && freeTimeWindow.TimeSlot.EndTime - duration >= activityPoint.OpeningTime)
            {
                activityTimeSlot = new ActivityTimeSlot(new TimeSlot(freeTimeWindow.TimeSlot.EndTime - activityPoint.AvgSpendingTimeInHours - drivingTimeFromActivity, freeTimeWindow.TimeSlot.EndTime - drivingTimeFromActivity), activityPoint);
            }

            if (activityPoint.OpeningTime >= freeTimeWindow.TimeSlot.StartTime && activityPoint.OpeningTime + duration <= freeTimeWindow.TimeSlot.EndTime)
            {
                activityTimeSlot = new ActivityTimeSlot(new TimeSlot(activityPoint.OpeningTime + drivingTimeToActivity, activityPoint.OpeningTime + activityPoint.AvgSpendingTimeInHours + drivingTimeToActivity), activityPoint);
            }

            if (activityTimeSlot == null)
            {
                return null;
            }

            var startTime = activityTimeSlot.TimeSlot.StartTime;
            var endTime = activityTimeSlot.TimeSlot.EndTime;
            if (drivingTimeToActivity > 0)
            {
                activityPointAndTravel.Add(new ActivityTimeSlot(new TimeSlot(startTime - drivingTimeToActivity, startTime), null, true));
            }

            activityPointAndTravel.Add(activityTimeSlot);

            if (drivingTimeFromActivity > 0)
            {
                activityPointAndTravel.Add(new ActivityTimeSlot(new TimeSlot(endTime, endTime + drivingTimeFromActivity), null, true));
            }

            return activityPointAndTravel;
        }

        public static double CalcDrivingTime(Location from, Location to)
        {
            if (from == null | to == null)
            {
                return 0;
            }

            return 1;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripPlanner.Models;
using TripPlanner.Utils;
using TripPlanner.ViewModels;

namespace TripPlanner.Extensions
{
    public static class ActivityPointExtensions
    {
        public static bool IsNearBy(this ActivityPoint activityPoint, Location startPoint, int nearByDistanceKm)
        {
            return LocationUtils.HaversineDistance(new Location(activityPoint.GEPSLatitude, activityPoint.GEPSLongtitude), startPoint) > nearByDistanceKm;
        }

        public static int GetMatchGrade(this ActivityPoint activityPoint, Dictionary<eActivityType, int> preferences)
        {
            if (!activityPoint.Accessibility && preferences[eActivityType.Walking] == 1)
            {
                return 0;
            }

            return preferences[(eActivityType)Enum.Parse(typeof(eActivityType), activityPoint.Type)];
        }
    }
}
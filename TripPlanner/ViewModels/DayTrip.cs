using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripPlanner.Models;

namespace TripPlanner.ViewModels
{
    public class DayTrip
    {
        public List<ActivityPoint> ActivityPoints;
        public List<Path> Paths;

        public DayTrip(List<ActivityPoint> activityPoints)
        {
            ActivityPoints = activityPoints;
        }
    }
}
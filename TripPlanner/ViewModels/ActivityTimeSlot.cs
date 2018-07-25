using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TripPlanner.Models;

namespace TripPlanner.ViewModels
{
    public class ActivityTimeSlot
    {
        public TimeSlot TimeSlot;
        public ActivityPoint Activity;
        public bool IsTraveling;

        public ActivityTimeSlot(TimeSlot timeSlot, ActivityPoint activityPoint, bool isTraveling = false)
        {
            TimeSlot = timeSlot;
            Activity = activityPoint;
            IsTraveling = isTraveling;
        }
    }
}
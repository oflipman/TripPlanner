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

        public ActivityTimeSlot(TimeSlot timeSlot, ActivityPoint activityPoint)
        {
            TimeSlot = timeSlot;
            Activity = activityPoint;
        }
    }
}
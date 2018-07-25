using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripPlanner.ViewModels
{
    public class TimeWindow
    {
        public TimeSlot TimeSlot;
        public ActivityTimeSlot ActivityBefore;
        public ActivityTimeSlot ActivityAfter;

        public TimeWindow(TimeSlot timeSlot, ActivityTimeSlot activityBefore, ActivityTimeSlot activityAfter)
        {
            TimeSlot = timeSlot;
            ActivityBefore = activityBefore;
            ActivityAfter = activityAfter;
        }
    }
}
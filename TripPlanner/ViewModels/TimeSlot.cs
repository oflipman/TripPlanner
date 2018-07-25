using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripPlanner.ViewModels
{
    public class TimeSlot
    {
        public double StartTime;
        public double EndTime;

        public TimeSlot(double startTime, double endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool IsInTimeSlot(double startTime, double endTime)
        {
            return startTime >= StartTime && endTime <= EndTime;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripPlanner.ViewModels
{
    public class TimeSlot
    {
        public int StartTime;
        public int EndTime;

        public TimeSlot(int startTime, int endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool IsInTimeSlot(int startTime, int endTime)
        {
            return startTime >= StartTime && endTime <= EndTime;
        }
    }
}
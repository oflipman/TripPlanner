using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripPlanner.ViewModels
{
    public class TripPlan
    {
        public ICollection<DayOfTrip> Days { get; set; }
    }
}
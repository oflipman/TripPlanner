using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripPlanner.ViewModels
{
    public class VacationBasicDetails
    {
        public string City { get; set; }
        public int Days { get; set; }
        public Location SleepLocation { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripPlanner.ViewModels
{
    public enum eActivityType
    {
        Culinary,
        Party,
        History,
        Museum,
        Walking,
        Beach,
        Intencity,
        Shopping
    }

    public class UserRequest
    {
        public VacationBasicDetails VacationDetails { get; set; }
        public UserInformation UserInfo { get; set; }
        public Dictionary<eActivityType, int> Preferences { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using TripPlanner.BL;
using TripPlanner.Models;
using TripPlanner.ViewModels;

namespace TripPlanner.Controllers
{
    public class TripController : ApiController
    {
        public IEnumerable<ActivityPoint> Get()
        {
            List<ActivityPoint> dayOfTrip = new List<ActivityPoint>();
            ActivityPoint activity1 = new ActivityPoint(){Id = 1,Name = "act1",Accessibility = true,AvgSpendingTimeInHours = 1.5,ClosingTime = 434,Cost = 3.3,Description = "desc1",FromAge = 0,ToAge = 3,GEPSLatitude = 232,GEPSLongtitude = 323,OpeningTime = 31,Opinions = "asds",Rank = 3,Type = "resturant",WebsiteLink = "www.google.com"};
            ActivityPoint activity2 = new ActivityPoint() { Id = 2, Name = "act2", Accessibility = true, AvgSpendingTimeInHours = 1.5, ClosingTime = 434, Cost = 3.3, Description = "desc1", FromAge = 0, ToAge = 3, GEPSLatitude = 232, GEPSLongtitude = 323, OpeningTime = 31, Opinions = "asds", Rank = 3, Type = "resturant", WebsiteLink = "www.google.com" };
            dayOfTrip.Add(activity1);
            dayOfTrip.Add(activity2);
            return dayOfTrip;
        }
        [HttpPost]
        [ResponseType(typeof(Trip))]
        public IHttpActionResult Post([FromBody]JObject value)
        {
            UserRequest request = value.ToObject<UserRequest>();
            var trip =  ActivityPointBL.GetTripPlan(request);
            return Ok(trip);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TripPlanner.Models;

namespace TripPlanner.Repositories
{
    public class ActivityPointsDbContext : DbContext
    {
        public ActivityPointsDbContext() : base("TripPanerDB")
        {

        }

        public DbSet<ActivityPoint> ActivityPoints { get; set; }
    }
}
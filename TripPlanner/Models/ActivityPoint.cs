using TripPlanner.ViewModels;

namespace TripPlanner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ActivityPoint
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [StringLength(255)]
        public string Type { get; set; }

        public double GEPSLatitude { get; set; }

        public double GEPSLongtitude { get; set; }

        public int Rank { get; set; }

        [Required]
        [StringLength(1000)]
        public string Opinions { get; set; }

        public bool Accessibility { get; set; }

        public int FromAge { get; set; }

        public int ToAge { get; set; }

        public double AvgSpendingTimeInHours { get; set; }

        [Required]
        [StringLength(1000)]
        public string WebsiteLink { get; set; }

        public double Cost { get; set; }

        public long OpeningTime { get; set; }

        public long ClosingTime { get; set; }

        public Location Location => new Location(GEPSLatitude, GEPSLongtitude);
    }
}

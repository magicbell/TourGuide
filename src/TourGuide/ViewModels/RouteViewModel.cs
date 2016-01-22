using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TourGuide.Models;

namespace TourGuide.Controllers.Api
{
    public class RouteViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255,MinimumLength =5)]
        public string Name { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }

        public IEnumerable<Point> Points { get; set; }
    }
}
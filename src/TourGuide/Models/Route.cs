using System;
using System.Collections.Generic;

namespace TourGuide.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<Point> Points { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourGuide.Models
{
    public class Route
    {
        public Route()
        {
            this.Points = new HashSet<Point>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndTime { get; set; }

        public virtual ICollection<Point> Points { get; set; }
    }
}

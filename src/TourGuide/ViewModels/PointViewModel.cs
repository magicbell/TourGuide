using System;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class PointViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Description { get; set; }
        public DateTime Arrival { get; set; }
    }
}
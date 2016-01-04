using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourGuide.Models
{
    public class Point
    {
        public Point()
        {
      //      this.Routes = new HashSet<Route>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Description { get; set; }
        public DateTime Arrival { get; set; }

      //  public virtual ICollection<Route> Routes { get; set; }
    }
}

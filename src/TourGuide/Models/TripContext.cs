using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace TourGuide.Models
{
    public class TripContext : DbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<Point> Points { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace TourGuide.Models
{
    public class TripContext : DbContext
    {
        public TripContext()
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Point> Points { get; set; }
        public DbSet<Route> Routes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = Startup.Configuration["Data:TripContextConnection"];

            optionsBuilder.UseSqlServer(connection);

            base.OnConfiguring(optionsBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Models;

namespace TourGuide.Models
{
    public class TripContext : IdentityDbContext<TripUser>
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourGuide.Models
{
    public class SeedingData
    {
        private TripContext _context;

        public SeedingData(TripContext context)
        {
            _context = context;
        }

        public void EnsureSeeding()
        {
            if (!_context.Routes.Any())
            {
                var newRoute = new Route()
                {
                    Name = "Dundalk-Dublin",
                    Created = DateTime.Now,
                    UserId = 1,
                    Description = "Trip from Dundalk to Dublin, Ireland",
                    StartDate = DateTime.Parse("01-01-2016"),
                    EndTime = DateTime.Parse("01-01-2016"),
                    Points = new List<Point>()
                    {
                        new Point() {
                            Name = "Dundalk",
                            Created = DateTime.Now,
                            Longitude = -6.405957,
                            Latitude = 53.9979451,
                            Description = "Dundalk, Co Louth, Ireland",
                            Arrival = DateTime.Parse("01-01-2016 09:00")
                        },
                        new Point()
                        {
                            Name = "Drogheda",
                            Created = DateTime.Now,
                            Longitude = -6.3560985,
                            Latitude = 53.717856,
                            Description = "Drogheda, Co Louth, Ireland",
                            Arrival = DateTime.Parse("01-01-2016 09:00")
                        },
                        new Point()
                        {
                            Name = "Dublin",
                            Created = DateTime.Now,
                            Longitude = -6.2674937,
                            Latitude = 53.344104,
                            Description = "Dublin, the capital of Ireland",
                            Arrival = DateTime.Parse("01-01-2016 09:00")
                        }
                    }
                };
                _context.Add(newRoute);
                _context.AddRange(newRoute.Points);
                _context.SaveChanges();
            }
        }
    }
}
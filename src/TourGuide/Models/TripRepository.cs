using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.Logging;

namespace TourGuide.Models
{
    public class TripRepository : ITripRepository
    {
        private TripContext _context;


        public TripRepository(TripContext context)
        {
            _context = context;

        }

        public IEnumerable<Route> GetAllRoutes()
        {
            try
            {
                return _context.Routes.ToList();
            }
            catch (Exception ex)
            {
               // _logger.LogError("Couldn't get routes from database", ex.Message);
                return null;
            }            
        }

        public IEnumerable<Route> GetAllRoutesWithPoints()
        {
            try
            {
                return _context.Routes.Include(t => t.Points).OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
               // _logger.LogError("Couldn't get routes with points from database", ex.Message);
                return null;
            }
        }
    }
}

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

        public void Add(Route newRoute)
        {
            try {
                _context.Add(newRoute);
            }
            catch(Exception)
            { }
        }

        public void AddPointToRoute(string routeName, Point point)
        {
            try
            {
                var route = GetRouteByName(routeName);
                // _context.Points.Add(point);
                route.Points.Add(point);
            }
            catch (Exception ex)
            {
                
            }
        }

        public IEnumerable<Point> GetAllPoints()
        {
            try
            {
                var result = _context.Points.ToList();
                return result;
            }
            catch(Exception)
            {
                return null;
            }
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

        //TODO
        public IEnumerable<Point> GetPoints(int routeId)
        {
            try
            {
                var result = _context.Routes.Where(t => t.Id == routeId).Select(t => t.Points).ToList();
                return (IEnumerable<Point>) result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Route GetRouteByName(string routeName)
        {
            try
            {

                var result = _context.Routes.Where(t => t.Name == routeName).Include(t => t.Points).FirstOrDefault();
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task<bool> SaveAll()
        {
            try
            {
                int x = await _context.SaveChangesAsync();
                if (x > 0) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

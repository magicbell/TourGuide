using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

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
                if (route.Points.Count > 0)
                    point.OrdNumber = route.Points.Max(t => t.OrdNumber) + 1;
                else
                    point.OrdNumber = 0;
                route.Points.Add(point);
            }
            catch (Exception ex)
            {
                
            }
        }

        public void DeletePointFromRoute(string routeName, int pointId)
        {
            try
            {
                _context.Points.Remove(_context.Points.Where(t => t.Id == pointId).First());
            }
            catch (Exception ex)
            {

                
            }
        }

        public void DeleteRoute(int routeId)
        {
            try
            {
                _context.Routes.Where(t => t.Id == routeId)
                    .Include(t => t.Points).First().Points.ToList()
                    .ForEach(d => _context.Points.Remove(d));
                _context.SaveChangesAsync();
                _context.Routes.Remove(_context.Routes.Where(t => t.Id == routeId).First());
            }
            catch (Exception ex)
            {
                
            }
        }

        public IEnumerable<Point> GetAllPoints()
        {
            try
            {
                var result = _context.Points.OrderBy(x => x.OrdNumber).ToList();
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

        public IEnumerable<Route> GetUserRoutesWithPoints(string name)
        {
            try
            {
                return _context.Routes.Where(t => t.UserName == name).Include(t => t.Points).ToList();
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SwapPoints(int id1, int id2)
        {
            try
            {
                var Point1 = _context.Points.Where(t => t.Id == id1).First();
                var Point2 = _context.Points.Where(t => t.Id == id2).First();

                var temp = Point1.OrdNumber;

                Point1.OrdNumber = Point2.OrdNumber;
                Point2.OrdNumber = temp;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

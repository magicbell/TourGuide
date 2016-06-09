using System.Collections.Generic;
using System.Threading.Tasks;

namespace TourGuide.Models
{
    public interface ITripRepository
    {
        IEnumerable<Route> GetAllRoutes();
        IEnumerable<Route> GetAllRoutesWithPoints();
        Task<bool> SaveAll();
        void Add(Route newRoute);
        IEnumerable<Point> GetPoints(int routeId);
        IEnumerable<Point> GetAllPoints();
        Route GetRouteByName(string routeName);
        void AddPointToRoute(string routeName, Point point);
        IEnumerable<Route> GetUserRoutesWithPoints(string name);
        void DeletePointFromRoute(string routeName, int pointId);
        bool SwapPoints(int id1, int id2);
        void DeleteRoute(int routeId);
    }
}
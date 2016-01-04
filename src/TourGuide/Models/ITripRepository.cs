using System.Collections.Generic;

namespace TourGuide.Models
{
    public interface ITripRepository
    {
        IEnumerable<Route> GetAllRoutes();
        IEnumerable<Route> GetAllRoutesWithPoints();
    }
}
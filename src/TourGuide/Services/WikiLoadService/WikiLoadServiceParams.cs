using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourGuide.Services
{
    public class WikiLoadServiceParams
    {
        public WikiLoadServiceParams(double radius, double latitude, double longitude)
        {
            Radius = radius;
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ThoughtWorksTrains.Models
{
    public class GetRouteDistanceResponse
    {
        public List<Route> routes { get; set; }
        public string route { get; set; }
        public double distance { get; set; }
    }
}

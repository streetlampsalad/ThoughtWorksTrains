using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Constants;

namespace ThoughtWorksTrains.Models
{
    public class GetNumberOfRoutesBetweenTownsByStopResponse
    {
        public List<Route> routes { get; set; }
        public string startTownId { get; set; }
        public string destinationTownId { get; set; }
        public int stopCount { get; set; }
        public LimitType limitType { get; set; }
        public int numberOfStops { get; set; }
    }
}

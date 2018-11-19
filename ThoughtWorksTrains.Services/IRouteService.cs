using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public interface IRouteService
    {
        double GetRouteDistance(string route, TownMap townMap);
        int GetNumberOfRoutesBetweenTownsMaxStops(string startTownId, string endTownId, TownMap townMap, int maxStops);
    }
}

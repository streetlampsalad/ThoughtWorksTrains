using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Constants;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public interface IRouteService
    {
        double GetRouteDistance(string route, TownMap townMap);
        int GetNumberOfRoutesBetweenTownsByStop(string startTownId, string endTownId, TownMap townMap, int stopCount, LimitType limitType);
        int GetNumberOfRoutesBetweenTownsByDistance(string startTownId, string endTownId, TownMap townMap, double distance, LimitType limitType);
    }
}

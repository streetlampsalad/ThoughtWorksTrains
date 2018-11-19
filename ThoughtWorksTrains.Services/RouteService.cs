using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public class RouteService : IRouteService
    {
        public double GetRouteDistance(string route, TownMap townMap)
        {
            var stops = route.Split('-');
            if(!stops.Any())
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            var distance = 0D;
            var count = 0;
            foreach(var stop in stops)
            {
                if(count == stops.Count() - 1)
                {
                    return distance;
                }

                try
                {
                    var town = townMap.Towns[stop];
                    distance += town.RouteMap[stops[count + 1]];                    
                    count++;
                }           
                catch(Exception ex)
                {
                    //Log ex
                    throw new ArgumentException("NO SUCH ROUTE");
                }
            }

            throw new ArgumentException("NO SUCH ROUTE");
        }
        
        public int GetNumberOfRoutesBetweenTownsMaxStops(string startTownId, string endTownId, TownMap townMap, int maxStops)
        {
            if(string.IsNullOrWhiteSpace(startTownId) || string.IsNullOrWhiteSpace(endTownId) || !townMap.Towns.Any() || maxStops <= 0)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            var routeCount = 0;
            var currentTowns = new List<Town>();
            try {
                currentTowns.Add(townMap.Towns[startTownId]);
            }
            catch(Exception ex)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            for(int i = 0; i <= maxStops; i++)
            {
                var connectingTowns = new List<Town>();
                foreach(var town in currentTowns)
                {
                    if(town.Id == endTownId && i != 0)
                    {
                        routeCount++;
                    }

                    foreach(var route in town.RouteMap)
                    {
                        connectingTowns.Add(townMap.Towns[route.Key]);
                    }
                }
                currentTowns = connectingTowns;
            }

            return routeCount;
        }             
    }
}

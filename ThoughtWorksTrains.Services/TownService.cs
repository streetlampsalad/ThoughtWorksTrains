using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public class TownService : ITownService
    {
        public TownMap GenerateTownMapByRoutes(List<Route> routes)
        {
            var townMap = new TownMap
            {
                Towns = new Dictionary<string, Town>()
            };

            if(!routes.Any())
            {
                throw new ArgumentException("Routes can not be empty");
            }

            foreach(var route in routes)
            {
                if(!townMap.Towns.ContainsKey(route.startTownId))
                {                    
                    townMap.Towns.Add(route.startTownId, new Town { Id = route.startTownId, RouteMap = new Dictionary<string, double>() });
                }

                if(!townMap.Towns.ContainsKey(route.destinationTownId))
                {
                    townMap.Towns.Add(route.destinationTownId, new Town { Id = route.destinationTownId, RouteMap = new Dictionary<string, double>() });
                }

                townMap.Towns[route.startTownId].RouteMap.Add(route.destinationTownId, route.distance);
            }

            return townMap;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorksTrains.Constants;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public class RouteService : IRouteService
    {        
        public double GetRouteDistance(string route, TownMap townMap)
        {
            if(string.IsNullOrWhiteSpace(route) || townMap.Towns == null)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            var stops = route.Split('-');
            if(!stops.Any())
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            var distance = 0D;
            var count = 0;
            foreach(var stop in stops)
            {
                // return distance on second to last stop because you add the distance for the last stop in the in the previous loop
                if(count == stops.Count() - 1)
                {
                    return distance;
                }

                try
                {
                    // get the current town, then get the distance to the next town from current town and add it to the total
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
        
        public int GetNumberOfRoutesBetweenTownsByStop(string startTownId, string destinationTownId, TownMap townMap, int stopCount, LimitType limitType)
        {            
            if(string.IsNullOrWhiteSpace(startTownId) || string.IsNullOrWhiteSpace(destinationTownId) || townMap.Towns == null || stopCount <= 0)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            var routeCount = 0;            
            var currentTowns = new List<Town>();
            // catch if method is passed a bad starting id
            try {
                currentTowns.Add(townMap.Towns[startTownId]);
            }
            catch(Exception ex)
            {
                // Log ex
                throw new ArgumentException("NO SUCH ROUTE");
            }

            for(int i = 0; i <= stopCount; i++)
            {
                var connectingTowns = new List<Town>();
                foreach(var town in currentTowns)
                {
                    if(town.Id == destinationTownId && i != 0)
                    {
                        // depending on the limit typed passed count if current town in the destination town
                        if(limitType == LimitType.MaxOrEqual)
                        {
                            routeCount++;
                        }
                        else if(limitType == LimitType.Exact && stopCount == i)
                        {
                            routeCount++;
                        }
                        else if(limitType == LimitType.LessThen && stopCount < i)
                        {
                            routeCount++;
                        }
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

        public int GetNumberOfRoutesBetweenTownsByDistance(string startTownId, string destinationTownId, TownMap townMap, double distance, LimitType limitType)
        {
            if(string.IsNullOrWhiteSpace(startTownId) || string.IsNullOrWhiteSpace(destinationTownId) || townMap.Towns == null || distance <= 0)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }
            
            var routeCount = 0;
            
            // create 2 lists, 1 to loop over and 1 to edit while in the loop
            var currentRoutes = new List<string> { startTownId };
            var potentialRoutes = new List<string> { startTownId };

            // might need a max loop count
            while (true)
            {                                
                foreach (var route in currentRoutes)
                {
                    var town = new Town();
                    try
                    {
                        town = townMap.Towns[route.Split('-').Last()];
                    }
                    catch(Exception ex)
                    {
                        // Log ex
                        throw new ArgumentException("NO SUCH ROUTE");
                    }

                    foreach(var potentialTown in town.RouteMap)
                    {
                        var potentialRoute = string.Join('-', route, potentialTown.Key);                        
                        var potentialRouteDistance = GetRouteDistance(potentialRoute, townMap);

                        // if the potential route is within the distance limit and hasn't already been added, add it to the list of valid routes and count if it end at the destination town
                        if(potentialRouteDistance <= distance && !currentRoutes.Contains(potentialRoute))
                        {
                            potentialRoutes.Add(potentialRoute);

                            if(potentialTown.Key == destinationTownId)
                            {
                                if(limitType == LimitType.MaxOrEqual)
                                {
                                    routeCount++;
                                }
                                else if(limitType == LimitType.Exact && distance == potentialRouteDistance)
                                {
                                    routeCount++;
                                }
                                else if(limitType == LimitType.LessThen && potentialRouteDistance < distance)
                                {
                                    routeCount++;
                                }
                            }
                        }
                    }
                }

                if(potentialRoutes.Count() == currentRoutes.Count())
                {
                    return routeCount;
                }

                // clear the current routes list and copy over the potential routes list that was modified during the last loop with the new routes and start the loop again with the new routes 
                currentRoutes = new List<string>();
                currentRoutes.AddRange(potentialRoutes);
            }            
        }

        public double GetShortestDistanceBetweenTownsById(string startTownId, string destinationTownId, TownMap townMap)
        {
            if(string.IsNullOrWhiteSpace(startTownId) || string.IsNullOrWhiteSpace(destinationTownId) || townMap.Towns == null)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }            
            
            if(!townMap.Towns.ContainsKey(destinationTownId))
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }

            var currentRoutes = new Dictionary<string, double> { { startTownId, 0 } };
            var potentialRoutes = new Dictionary<string, double> { { startTownId, 0 } };
            var destinationRoutes = new Dictionary<string, double>();

            // might need a max loop count
            while(true)
            {
                foreach(var route in currentRoutes)
                {
                    var town = new Town();
                    try
                    {
                        town = townMap.Towns[route.Key.Split('-').Last()];
                    }
                    catch(Exception ex)
                    {
                        // Log ex
                        throw new ArgumentException("NO SUCH ROUTE");
                    }

                    if(town.Id != destinationTownId)
                    {
                        foreach(var potentialTown in town.RouteMap)
                        {
                            var potentialRoute = string.Join('-', route.Key, potentialTown.Key);
                            var potentialRouteDistance = GetRouteDistance(potentialRoute, townMap);

                            if(!currentRoutes.Keys.Contains(potentialRoute))
                            {                                                  
                                if(destinationRoutes.Any())
                                {
                                    if(potentialRouteDistance < destinationRoutes.Values.Min())
                                    {
                                        potentialRoutes.Add(potentialRoute, potentialRouteDistance);
                                    }
                                }
                                else
                                {
                                    potentialRoutes.Add(potentialRoute, potentialRouteDistance);
                                }
                            }

                            if(potentialTown.Key == destinationTownId && !destinationRoutes.Keys.Contains(potentialRoute))
                            {
                                destinationRoutes.Add(potentialRoute, potentialRouteDistance);
                            }
                        }
                    }                    
                }

                if(potentialRoutes.Count() == currentRoutes.Count())
                {                    
                    if(destinationRoutes.Any())
                    {
                        return destinationRoutes.Values.Min();
                    }
                    throw new ArgumentException("NO SUCH ROUTE");
                }
                
                currentRoutes = new Dictionary<string, double>();
                foreach(var potentialRoute in potentialRoutes)
                {
                    currentRoutes.Add(potentialRoute.Key, potentialRoute.Value);
                }                
            }
        }
    }
}

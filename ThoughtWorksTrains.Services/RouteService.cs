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
        
        public int GetNumberOfRoutesBetweenTownsByStop(string startTownId, string endTownId, TownMap townMap, int stopCount, LimitType limitType)
        {            
            if(string.IsNullOrWhiteSpace(startTownId) || string.IsNullOrWhiteSpace(endTownId) || townMap.Towns == null || stopCount <= 0)
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
                    if(town.Id == endTownId && i != 0)
                    {
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

        public int GetNumberOfRoutesBetweenTownsByDistance(string startTownId, string endTownId, TownMap townMap, double distance, LimitType limitType)
        {
            if(string.IsNullOrWhiteSpace(startTownId) || string.IsNullOrWhiteSpace(endTownId) || townMap.Towns == null || distance <= 0)
            {
                throw new ArgumentException("NO SUCH ROUTE");
            }
            
            var routeCount = 0;            
            var currentRoutes = new List<string> { startTownId };
            var potentialRoutes = new List<string> { startTownId };

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
                        if(potentialRouteDistance <= distance && !currentRoutes.Contains(potentialRoute))
                        {
                            potentialRoutes.Add(potentialRoute);

                            if(potentialTown.Key == endTownId)
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

                currentRoutes = new List<string>();
                currentRoutes.AddRange(potentialRoutes);
            }            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public class RouteService : IRouteService
    {
        public double GetRouteDistance(string route, List<Town> towns)
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
                    var town = towns.FirstOrDefault(x => x.Id == stop);
                    var townRoute = town.Routes.FirstOrDefault(y => y.Id == stops[count + 1]);
                    distance += townRoute.Distance;
                    count++;
                }           
                catch(Exception ex)
                {                
                    throw new ArgumentException("NO SUCH ROUTE");
                }
            }

            throw new ArgumentException("NO SUCH ROUTE");
        }
    }
}

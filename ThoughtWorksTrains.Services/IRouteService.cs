using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public interface IRouteService
    {
        double GetRouteDistance(string route, List<Town> towns);
    }
}

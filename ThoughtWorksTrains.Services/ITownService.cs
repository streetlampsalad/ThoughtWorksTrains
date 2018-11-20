using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Models;

namespace ThoughtWorksTrains.Services
{
    public interface ITownService
    {
        TownMap GenerateTownMapByRoutes(List<Route> routes);
    }
}

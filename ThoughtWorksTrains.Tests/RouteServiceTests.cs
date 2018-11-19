using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Models;
using ThoughtWorksTrains.Services;

namespace ThoughtWorksTrains.Tests
{
    [TestFixture]
    class RouteServiceTests
    {

        private IRouteService _routeService;

        [SetUp]
        public void Setup()
        {
            _routeService = new RouteService();
        }

        private readonly List<Town> TestTowns = new List<Town> {
            new Town { Id = "A", Routes = new List<Route> { new Route { Id = "B", Distance = 5 }, new Route { Id = "D", Distance = 5 }, new Route { Id = "E", Distance = 7 } } },
            new Town { Id = "B", Routes = new List<Route> { new Route { Id = "C", Distance = 4 } } },
            new Town { Id = "C", Routes = new List<Route> { new Route { Id = "D", Distance = 8 }, new Route { Id = "E", Distance = 2 } } },
            new Town { Id = "D", Routes = new List<Route> { new Route { Id = "C", Distance = 8 }, new Route { Id = "E", Distance = 6 } } },
            new Town { Id = "E", Routes = new List<Route> { new Route { Id = "B", Distance = 3 } } }
        };

        [Test]
        public void GetRouteDistance_RouteDoesNotExist()
        {
            var result = _routeService.GetRouteDistance("A-E-D", TestTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual("NO SUCH ROUTE", result);
        }
    }
}

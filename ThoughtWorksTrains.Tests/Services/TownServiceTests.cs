using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.Models;
using ThoughtWorksTrains.Services;

namespace ThoughtWorksTrains.Tests
{
    [TestFixture]
    class TownServiceTests
    {
        #region Setup    

        private ITownService _townService;

        [SetUp]
        public void Setup()
        {
            _townService = new TownService();
        }

        private readonly List<Route> _testRoutes = new List<Route> {
            new Route { startTownId = "A", destinationTownId = "B", distance = 5 },
            new Route { startTownId = "B", destinationTownId = "C", distance = 4 },
            new Route { startTownId = "C", destinationTownId = "D", distance = 8 },
            new Route { startTownId = "D", destinationTownId = "C", distance = 8 },
            new Route { startTownId = "D", destinationTownId = "E", distance = 6 },
            new Route { startTownId = "A", destinationTownId = "D", distance = 5 },
            new Route { startTownId = "C", destinationTownId = "E", distance = 2 },
            new Route { startTownId = "E", destinationTownId = "B", distance = 3 },
            new Route { startTownId = "A", destinationTownId = "E", distance = 7 }
        };

        #endregion

        [Test]
        public void GenerateTownMapByRoutes_Success()
        {
            var result = _townService.GenerateTownMapByRoutes(_testRoutes);            
            Assert.AreEqual(result.Towns.Count, 5);
            Assert.IsTrue(result.Towns.ContainsKey("A"));
            Assert.AreEqual(result.Towns["A"].RouteMap.Count, 3);
        }

        [Test]
        public void GenerateTownMapByRoutes_NoRoutes()
        {
            Assert.Throws<ArgumentException>(() => _townService.GenerateTownMapByRoutes(new List<Route>()), "Routes can not be empty");            
        }
    }
}

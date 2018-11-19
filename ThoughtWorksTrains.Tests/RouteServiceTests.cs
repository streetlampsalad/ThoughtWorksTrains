using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly TownMap _testTowns = new TownMap {
            Towns = new Dictionary<string, Town>
            {
                { "A", new Town { Id = "A", RouteMap = new Dictionary<string, double> { { "B", 5 }, { "D", 5 }, { "E", 7 } } } },
                { "B", new Town { Id = "B", RouteMap = new Dictionary<string, double> { { "C", 4 } } } },
                { "C", new Town { Id = "C", RouteMap = new Dictionary<string, double> { { "D", 8 }, { "E", 2 } } } },
                { "D", new Town { Id = "D", RouteMap = new Dictionary<string, double> { { "C", 8 }, { "E", 6 } } } },
                { "E", new Town { Id = "E", RouteMap = new Dictionary<string, double> { { "B", 3 } } } },
            }
        };

        #region GetRouteDistance

        [Test]
        public void GetRouteDistance_RouteDoesNotExist()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A-E-D", _testTowns), "NO SUCH ROUTE");            
        }

        [Test]
        public void GetRouteDistance_InvalidInput()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A-38A-A8AAS9D8H", _testTowns), "NO SUCH ROUTE");
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAB()
        {
            var result = _routeService.GetRouteDistance("A-B", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteABC()
        {
            var result = _routeService.GetRouteDistance("A-B-C", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAD()
        {
            var result = _routeService.GetRouteDistance("A-D", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteADC()
        {
            var result = _routeService.GetRouteDistance("A-D-C", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(13, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAEBCD()
        {
            var result = _routeService.GetRouteDistance("A-E-B-C-D", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(22, result);
        }

        #endregion

        #region GetNumberOfRoutesBetweenTownsMaxStops

        [Test]
        public void GetNumberOfRoutesBetweenTownsMaxStops_InvalidInput()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsMaxStops("F", "34", _testTowns, 7), "NO SUCH ROUTE");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsMaxStops_CalculateRouteCtoCMax3()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsMaxStops("C", "C", _testTowns, 3);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);
        }

        #endregion
    }
}

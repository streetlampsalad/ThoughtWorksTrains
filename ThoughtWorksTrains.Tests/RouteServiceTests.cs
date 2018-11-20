using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorksTrains.Models;
using ThoughtWorksTrains.Services;
using ThoughtWorksTrains.Constants;

namespace ThoughtWorksTrains.Tests
{
    [TestFixture]
    class RouteServiceTests
    {

        #region Setup    

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

        #endregion

        #region GetRouteDistance        

        [Test]
        public void GetRouteDistance_CalculateRouteA()
        {
            var result = _routeService.GetRouteDistance("A", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAB()
        {
            var result = _routeService.GetRouteDistance("A-B", _testTowns);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void GetRouteDistance_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A-B", new TownMap()), "NO SUCH ROUTE");            
        }

        [Test]
        public void GetRouteDistance_EmptyRoute()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("", _testTowns), "NO SUCH ROUTE");
        }

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

        #region GetNumberOfRoutesBetweenTownsByStop

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_InvalidStartTownId()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByStop("ZZ", "C", _testTowns, 7, LimitType.MaxOrEqual), "NO SUCH ROUTE");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_InvalidEndTownId()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("C", "ZZ", _testTowns, 10, LimitType.MaxOrEqual);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteAtoBMax5()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "B", _testTowns, 5, LimitType.MaxOrEqual);
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "B", new TownMap(), 5, LimitType.MaxOrEqual), "NO SUCH ROUTE");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteCtoCMax3()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("C", "C", _testTowns, 3, LimitType.MaxOrEqual);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result);
        }        

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteAtoCExact4()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "C", _testTowns, 4, LimitType.Exact);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteAtoEExact7()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "E", _testTowns, 7, LimitType.Exact);
            Assert.IsNotNull(result);
            Assert.AreEqual(9, result);
        }

        #endregion

        #region GetNumberOfRoutesBetweenTownsByDistance

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_InvalidStartTownId()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByDistance("ZZ", "C", _testTowns, 30D, LimitType.LessThen), "NO SUCH ROUTE");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_InvalidEndTownId()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "ZZ", _testTowns, 6D, LimitType.LessThen);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_CalculateRouteAtoBLessThen6()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "B", _testTowns, 6D, LimitType.LessThen);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "B", new TownMap(), 6D, LimitType.LessThen), "NO SUCH ROUTE");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_CalculateRouteCtoCLessThen30()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("C", "C", _testTowns, 30D, LimitType.LessThen);
            Assert.IsNotNull(result);
            Assert.AreEqual(7, result);
        }        

        #endregion
    }
}

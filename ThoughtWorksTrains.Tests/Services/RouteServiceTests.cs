﻿using NUnit.Framework;
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
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A", new TownMap()), "No such route");
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAB()
        {
            var result = _routeService.GetRouteDistance("A-B", _testTowns);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void GetRouteDistance_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A-B", new TownMap()), "No such route");            
        }

        [Test]
        public void GetRouteDistance_EmptyRoute()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("", _testTowns), "No such route");
        }

        [Test]
        public void GetRouteDistance_RouteDoesNotExist()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A-E-D", _testTowns), "No such route");
        }

        [Test]
        public void GetRouteDistance_InvalidInput()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetRouteDistance("A-38A-A8AAS9D8H", _testTowns), "No such route");
        }

        [Test]
        public void GetRouteDistance_CalculateRouteABC()
        {
            var result = _routeService.GetRouteDistance("A-B-C", _testTowns);
            Assert.AreEqual(9, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAD()
        {
            var result = _routeService.GetRouteDistance("A-D", _testTowns);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteADC()
        {
            var result = _routeService.GetRouteDistance("A-D-C", _testTowns);
            Assert.AreEqual(13, result);
        }

        [Test]
        public void GetRouteDistance_CalculateRouteAEBCD()
        {
            var result = _routeService.GetRouteDistance("A-E-B-C-D", _testTowns);
            Assert.AreEqual(22, result);
        }

        #endregion

        #region GetNumberOfRoutesBetweenTownsByStop

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_InvalidStartTownId()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByStop("ZZ", "C", _testTowns, 7, LimitType.MaxOrEqual), "No such route");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_InvalidEndTownId()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("C", "ZZ", _testTowns, 10, LimitType.MaxOrEqual);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteAtoBMax5()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "B", _testTowns, 5, LimitType.MaxOrEqual);
            Assert.AreEqual(8, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "B", new TownMap(), 5, LimitType.MaxOrEqual), "No such route");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteCtoCMax3()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("C", "C", _testTowns, 3, LimitType.MaxOrEqual);
            Assert.AreEqual(2, result);
        }        

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteAtoCExact4()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "C", _testTowns, 4, LimitType.Exact);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_CalculateRouteAtoEExact7()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByStop("A", "E", _testTowns, 7, LimitType.Exact);
            Assert.AreEqual(9, result);
        }

        #endregion

        #region GetNumberOfRoutesBetweenTownsByDistance

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_InvalidStartTownId()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByDistance("ZZ", "C", _testTowns, 30D, LimitType.LessThen), "No such route");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_InvalidEndTownId()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "ZZ", _testTowns, 6D, LimitType.LessThen);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_CalculateRouteAtoBLessThen6()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "B", _testTowns, 6D, LimitType.LessThen);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "B", new TownMap(), 6D, LimitType.LessThen), "No such route");
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_CalculateRouteCtoCLessThen30()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("C", "C", _testTowns, 30D, LimitType.LessThen);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_CalculateRouteAtoCExact10()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("A", "C", _testTowns, 10D, LimitType.Exact);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_CalculateRouteCtoC30()
        {
            var result = _routeService.GetNumberOfRoutesBetweenTownsByDistance("C", "C", _testTowns, 30D, LimitType.MaxOrEqual);
            Assert.AreEqual(9, result);
        }

        #endregion

        #region GetShortestDistanceBetweenTownsById

        [Test]
        public void GetShortestDistanceBetweenTownsById_InvalidStartTownId()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetShortestDistanceBetweenTownsById("ZZ", "B", _testTowns), "No such route");
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_InvalidEndTownId()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetShortestDistanceBetweenTownsById("A", "ZZ", _testTowns), "No such route");
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_CalculateShortestRouteBetweenAandB()
        {
            var result = _routeService.GetShortestDistanceBetweenTownsById("A", "B", _testTowns);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_EmptyTown()
        {
            Assert.Throws<ArgumentException>(() => _routeService.GetShortestDistanceBetweenTownsById("A", "B", new TownMap()), "No such route");
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_CalculateShortestRouteBetweenAandC()
        {
            var result = _routeService.GetShortestDistanceBetweenTownsById("A", "C", _testTowns);
            Assert.AreEqual(9, result);
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_CalculateShortestRouteBetweenCandC()
        {
            var result = _routeService.GetShortestDistanceBetweenTownsById("C", "C", _testTowns);
            Assert.AreEqual(0, result);
        }

        #endregion
    }
}

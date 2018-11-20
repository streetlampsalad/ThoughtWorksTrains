using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ThoughtWorksTrains.API.Controllers;
using ThoughtWorksTrains.Models;
using ThoughtWorksTrains.Services;
using Microsoft.AspNetCore.Mvc;
using ThoughtWorksTrains.Constants;

namespace ThoughtWorksTrains.Tests.Controllers
{
    [TestFixture]
    class TrainsControllerTests
    {
        #region Setup

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

        private readonly TownMap _testTowns = new TownMap
        {
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
        public void GetRouteDistance_BadRequest()
        {
            var townServiceMock = new Mock<ITownService>();
            var routeServiceMock = new Mock<IRouteService>();

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);
            
            Assert.IsInstanceOf<BadRequestObjectResult>(trainsController.GetRouteDistance(new GetRouteDistanceResponse()));
        }

        [Test]
        public void GetRouteDistance_Success()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetRouteDistance(It.IsAny<string>(), It.IsAny<TownMap>())).Returns(5);

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetRouteDistanceResponse
            {
                routes = _testRoutes,
                route = "A-A-A"
            };

            var result = trainsController.GetRouteDistance(payload) as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<GetRouteDistanceResponse>(result.Value);
            Assert.AreEqual(5, (result.Value as GetRouteDistanceResponse).distance);
        }

        [Test]
        public void GetRouteDistance_NoRoute()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetRouteDistance(It.IsAny<string>(), It.IsAny<TownMap>())).Throws(new ArgumentException("No such route"));

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetRouteDistanceResponse
            {
                routes = _testRoutes,
                route = "A-A-A"
            };

            var result = trainsController.GetRouteDistance(payload) as NotFoundObjectResult;

            Assert.IsInstanceOf<NotFoundObjectResult>(result);            
            Assert.AreEqual("No such route", result.Value);
        }

        #endregion

        #region GetNumberOfRoutesBetweenTownsByStop

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_BadRequest()
        {
            var townServiceMock = new Mock<ITownService>();
            var routeServiceMock = new Mock<IRouteService>();

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            Assert.IsInstanceOf<BadRequestObjectResult>(trainsController.GetNumberOfRoutesBetweenTownsByStop(new GetNumberOfRoutesBetweenTownsByStopResponse()));
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_Success()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetNumberOfRoutesBetweenTownsByStop(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TownMap>(), It.IsAny<int>(), It.IsAny<LimitType>())).Returns(5);

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetNumberOfRoutesBetweenTownsByStopResponse
            {
                routes = _testRoutes,
                startTownId = "A",
                destinationTownId = "A",
                stopCount = 3,
                limitType = LimitType.MaxOrEqual
            };

            var result = trainsController.GetNumberOfRoutesBetweenTownsByStop(payload) as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<GetNumberOfRoutesBetweenTownsByStopResponse>(result.Value);
            Assert.AreEqual(5, (result.Value as GetNumberOfRoutesBetweenTownsByStopResponse).numberOfStops);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByStop_NoRoute()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetNumberOfRoutesBetweenTownsByStop(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TownMap>(), It.IsAny<int>(), It.IsAny<LimitType>())).Throws(new ArgumentException("No such route"));

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetNumberOfRoutesBetweenTownsByStopResponse
            {
                routes = _testRoutes,
                startTownId = "A",
                destinationTownId = "A",
                stopCount = 3,
                limitType = LimitType.MaxOrEqual
            };

            var result = trainsController.GetNumberOfRoutesBetweenTownsByStop(payload) as NotFoundObjectResult;

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("No such route", result.Value);
        }

        #endregion

        #region GetNumberOfRoutesBetweenTownsByDistance

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_BadRequest()
        {
            var townServiceMock = new Mock<ITownService>();
            var routeServiceMock = new Mock<IRouteService>();

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            Assert.IsInstanceOf<BadRequestObjectResult>(trainsController.GetNumberOfRoutesBetweenTownsByDistance(new GetNumberOfRoutesBetweenTownsByDistanceResponse()));
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_Success()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetNumberOfRoutesBetweenTownsByDistance(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TownMap>(), It.IsAny<double>(), It.IsAny<LimitType>())).Returns(5);

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetNumberOfRoutesBetweenTownsByDistanceResponse
            {
                routes = _testRoutes,
                startTownId = "A",
                destinationTownId = "A",
                distance = 30,
                limitType = LimitType.MaxOrEqual
            };

            var result = trainsController.GetNumberOfRoutesBetweenTownsByDistance(payload) as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<GetNumberOfRoutesBetweenTownsByDistanceResponse>(result.Value);
            Assert.AreEqual(5, (result.Value as GetNumberOfRoutesBetweenTownsByDistanceResponse).numberOfStops);
        }

        [Test]
        public void GetNumberOfRoutesBetweenTownsByDistance_NoRoute()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetNumberOfRoutesBetweenTownsByDistance(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TownMap>(), It.IsAny<double>(), It.IsAny<LimitType>())).Throws(new ArgumentException("No such route"));

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetNumberOfRoutesBetweenTownsByDistanceResponse
            {
                routes = _testRoutes,
                startTownId = "A",
                destinationTownId = "A",
                distance = 30,
                limitType = LimitType.MaxOrEqual
            };

            var result = trainsController.GetNumberOfRoutesBetweenTownsByDistance(payload) as NotFoundObjectResult;

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("No such route", result.Value);
        }

        #endregion

        #region GetShortestDistanceBetweenTownsById

        [Test]
        public void GetShortestDistanceBetweenTownsById_BadRequest()
        {
            var townServiceMock = new Mock<ITownService>();
            var routeServiceMock = new Mock<IRouteService>();

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            Assert.IsInstanceOf<BadRequestObjectResult>(trainsController.GetShortestDistanceBetweenTownsById(new GetShortestDistanceBetweenTownsByIdResponse()));
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_Success()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetShortestDistanceBetweenTownsById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TownMap>())).Returns(5);

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetShortestDistanceBetweenTownsByIdResponse
            {
                routes = _testRoutes,
                startTownId = "A",
                destinationTownId = "A"                
            };

            var result = trainsController.GetShortestDistanceBetweenTownsById(payload) as OkObjectResult;

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsInstanceOf<GetShortestDistanceBetweenTownsByIdResponse>(result.Value);
            Assert.AreEqual(5, (result.Value as GetShortestDistanceBetweenTownsByIdResponse).distance);
        }

        [Test]
        public void GetShortestDistanceBetweenTownsById_NoRoute()
        {
            var townServiceMock = new Mock<ITownService>();
            townServiceMock.Setup(x => x.GenerateTownMapByRoutes(It.IsAny<List<Route>>())).Returns(_testTowns);
            var routeServiceMock = new Mock<IRouteService>();
            routeServiceMock.Setup(x => x.GetShortestDistanceBetweenTownsById(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TownMap>())).Throws(new ArgumentException("No such route"));

            TrainsController trainsController = new TrainsController(townServiceMock.Object, routeServiceMock.Object);

            var payload = new GetShortestDistanceBetweenTownsByIdResponse
            {
                routes = _testRoutes,
                startTownId = "A",
                destinationTownId = "A"
            };

            var result = trainsController.GetShortestDistanceBetweenTownsById(payload) as NotFoundObjectResult;

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual("No such route", result.Value);
        }

        #endregion
    }
}

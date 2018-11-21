using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThoughtWorksTrains.Services;
using ThoughtWorksTrains.Models;
using Microsoft.AspNetCore.Http;

namespace ThoughtWorksTrains.API.Controllers
{    
    [Route("api/v1/[controller]/[action]")]
    public class TrainsController : Controller
    {
        private readonly ITownService _townService;
        private readonly IRouteService _routeService;

        public TrainsController(ITownService townService, IRouteService routeService)
        {
            _townService = townService;
            _routeService = routeService;
        }

        /// <summary>
        /// Retrieves distance from a given route
        /// </summary>
        /// <response code="200">Route distance calculated</response>
        /// <response code="400">Route has missing/invalid values</response>
        /// <response code="500">Oops! Can't calculate the route distance right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetRouteDistanceResponse), 200)]
        [ProducesResponseType(typeof(GetRouteDistanceResponse), 400)]
        [ProducesResponseType(500)]
        public IActionResult GetRouteDistance([FromBody] GetRouteDistanceResponse response)
        {
            if(response == null)
            {
                return BadRequest("Route has missing/invalid values");
            }

            if(string.IsNullOrWhiteSpace(response.route) || response.routes == null)
            {
                return BadRequest("Route has missing/invalid values");
            }

            try
            {
                var townMap = _townService.GenerateTownMapByRoutes(response.routes);
                response.distance = _routeService.GetRouteDistance(response.route, townMap);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }            

            return Ok(response);
        }

        /// <summary>
        /// Retrieves number of routes between towns by stops
        /// </summary>
        /// <response code="200">Routes counted</response>
        /// <response code="400">Town has missing/invalid values</response>
        /// <response code="500">Oops! Can't calculate the number of routes right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetNumberOfRoutesBetweenTownsByStopResponse), 200)]
        [ProducesResponseType(typeof(GetNumberOfRoutesBetweenTownsByStopResponse), 400)]
        [ProducesResponseType(500)]
        public IActionResult GetNumberOfRoutesBetweenTownsByStop([FromBody] GetNumberOfRoutesBetweenTownsByStopResponse response)
        {
            if(response == null)
            {
                return BadRequest("Route has missing/invalid values");
            }

            if(string.IsNullOrWhiteSpace(response.startTownId) 
            || string.IsNullOrWhiteSpace(response.destinationTownId) 
            || response.stopCount < 0              
            || response.routes == null)
            {
                return BadRequest("Invalid or missing arguments");
            }

            try
            {
                var townMap = _townService.GenerateTownMapByRoutes(response.routes);
                response.numberOfStops = _routeService.GetNumberOfRoutesBetweenTownsByStop(response.startTownId, response.destinationTownId, townMap, response.stopCount, response.limitType);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }            

            return Ok(response);
        }

        /// <summary>
        /// Retrieves number of routes between towns by distance
        /// </summary>
        /// <response code="200">Routes counted</response>
        /// <response code="400">Town has missing/invalid values</response>
        /// <response code="500">Oops! Can't calculate the number of routes right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetNumberOfRoutesBetweenTownsByDistanceResponse), 200)]
        [ProducesResponseType(typeof(GetNumberOfRoutesBetweenTownsByDistanceResponse), 400)]
        [ProducesResponseType(500)]
        public IActionResult GetNumberOfRoutesBetweenTownsByDistance([FromBody] GetNumberOfRoutesBetweenTownsByDistanceResponse response)
        {
            if(response == null)
            {
                return BadRequest("Route has missing/invalid values");
            }

            if(string.IsNullOrWhiteSpace(response.startTownId)
            || string.IsNullOrWhiteSpace(response.destinationTownId)
            || response.distance < 0
            || response.routes == null)
            {
                return BadRequest("Invalid or missing arguments");
            }

            try
            {
                var townMap = _townService.GenerateTownMapByRoutes(response.routes);
                response.numberOfStops = _routeService.GetNumberOfRoutesBetweenTownsByDistance(response.startTownId, response.destinationTownId, townMap, response.distance, response.limitType);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }            

            return Ok(response);
        }

        /// <summary>
        /// Retrieves shortest route between 2 towns
        /// </summary>
        /// <response code="200">Distance calculated</response>
        /// <response code="400">Town has missing/invalid values</response>
        /// <response code="500">Oops! Can't calculate the distance right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(GetShortestDistanceBetweenTownsByIdResponse), 200)]
        [ProducesResponseType(typeof(GetShortestDistanceBetweenTownsByIdResponse), 400)]
        [ProducesResponseType(500)]
        public IActionResult GetShortestDistanceBetweenTownsById([FromBody] GetShortestDistanceBetweenTownsByIdResponse response)
        {
            if(response == null)
            {
                return BadRequest("Route has missing/invalid values");
            }

            if(string.IsNullOrWhiteSpace(response.startTownId)
            || string.IsNullOrWhiteSpace(response.destinationTownId)            
            || response.routes == null)
            {
                return BadRequest("Invalid or missing arguments");
            }

            try
            {
                var townMap = _townService.GenerateTownMapByRoutes(response.routes);
                response.distance = _routeService.GetShortestDistanceBetweenTownsById(response.startTownId, response.destinationTownId, townMap);
            }
            catch(Exception ex)
            {                
                return NotFound(ex.Message);
            }

            return Ok(response);
        }
    }
}

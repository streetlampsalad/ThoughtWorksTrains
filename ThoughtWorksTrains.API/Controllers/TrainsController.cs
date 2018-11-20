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
        
        [HttpPost]        
        public IActionResult GetRouteDistance([FromBody] GetRouteDistanceResponse response)
        {
            if(string.IsNullOrWhiteSpace(response.route) || response.routes == null || !response.routes.Any())
            {
                return BadRequest("Invalid or missing arguments");
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

        [HttpPost]
        public IActionResult GetNumberOfRoutesBetweenTownsByStop([FromBody] GetNumberOfRoutesBetweenTownsByStopResponse response)
        {
            if(string.IsNullOrWhiteSpace(response.startTownId) 
            || string.IsNullOrWhiteSpace(response.destinationTownId) 
            || response.stopCount < 0              
            || response.routes == null 
            || !response.routes.Any())
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

        [HttpPost]
        public IActionResult GetNumberOfRoutesBetweenTownsByDistance([FromBody] GetNumberOfRoutesBetweenTownsByDistanceResponse response)
        {
            if(string.IsNullOrWhiteSpace(response.startTownId)
            || string.IsNullOrWhiteSpace(response.destinationTownId)
            || response.distance < 0
            || response.routes == null
            || !response.routes.Any())
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

        [HttpPost]
        public IActionResult GetShortestDistanceBetweenTownsById([FromBody] GetShortestDistanceBetweenTownsByIdResponse response)
        {
            if(string.IsNullOrWhiteSpace(response.startTownId)
            || string.IsNullOrWhiteSpace(response.destinationTownId)            
            || response.routes == null
            || !response.routes.Any())
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

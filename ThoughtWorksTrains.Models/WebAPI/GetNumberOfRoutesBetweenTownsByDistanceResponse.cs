﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ThoughtWorksTrains.Constants;

namespace ThoughtWorksTrains.Models
{
    public class GetNumberOfRoutesBetweenTownsByDistanceResponse
    {
        [Required]
        public List<Route> routes { get; set; }
        [Required]
        public string startTownId { get; set; }
        [Required]
        public string destinationTownId { get; set; }
        [Required]
        public double distance { get; set; }
        [Required]
        public LimitType limitType { get; set; }
        public int numberOfStops { get; set; }
    }
}

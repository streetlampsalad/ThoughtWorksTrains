using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThoughtWorksTrains.Models
{
    public class GetRouteDistanceResponse
    {
        [Required]
        public List<Route> routes { get; set; }
        [Required]
        public string route { get; set; }
        public double distance { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ThoughtWorksTrains.Constants;

namespace ThoughtWorksTrains.Models
{
    public class GetShortestDistanceBetweenTownsByIdResponse
    {
        [Required]
        public List<Route> routes { get; set; }
        [Required]
        public string startTownId { get; set; }
        [Required]
        public string destinationTownId { get; set; }
        public double distance { get; set; }        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ThoughtWorksTrains.Models
{
    public class Route
    {
        [Required]
        public string startTownId { get; set; }
        [Required]
        public string destinationTownId { get; set; }
        [Required]
        public double distance { get; set; }
    }
}

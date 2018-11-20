using System;
using System.Collections.Generic;
using System.Text;

namespace ThoughtWorksTrains.Models
{
    public class Route
    {
        public string startTownId { get; set; }
        public string destinationTownId { get; set; }
        public double distance { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ThoughtWorksTrains.Models
{
    public class Town
    {
        public string Id { get; set; }
        public List<Route> Routes { get; set; }
    }
}

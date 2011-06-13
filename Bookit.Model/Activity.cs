using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookit.Model
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int MinimumAge { get; set; }
    }
}

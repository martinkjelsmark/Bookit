using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookit.DTO
{
    public class ActivityDTO
    {
        public int activityId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int waitingTime { get; set; }
        public bool active { get; set; }
        public int minimumAge { get; set; }
    }
}

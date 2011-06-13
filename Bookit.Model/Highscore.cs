using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookit.Model
{
    public class Highscore
    {
        public int HighscoreId { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }

        public int Point { get; set; }
        public DateTime Timestamp { get; set; }
        
        public virtual Activity Activity { get; set; }
        public virtual User User { get; set; }
    }
}

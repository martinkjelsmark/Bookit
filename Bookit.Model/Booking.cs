using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookit.Model
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int ActivityId { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; }
        public virtual Activity Activity { get; set; }
    }
}

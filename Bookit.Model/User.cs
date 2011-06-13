using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Bookit.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        //public Image Portrait { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Highscore> Highscores { get; set; }
    }
}

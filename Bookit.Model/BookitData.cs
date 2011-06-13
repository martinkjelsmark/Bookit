using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Bookit.Model
{
    public class BookitData : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Highscore> Highscores { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}

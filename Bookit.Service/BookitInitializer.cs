using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Bookit.Model;

namespace Bookit.Service
{
    class BookitInitializer : DropCreateDatabaseIfModelChanges<BookitData>
    {
        protected override void Seed(BookitData context)
        {
            //Add test users
            var users = new List<User> {
                new User { 
                    Name = "Martin Kjelsmark",
                    Phone = "22494980",
                    Age = 1980
                },
                new User { 
                    Name = "Doris Hartung Kjelsmark",
                    Phone = "22194166",
                    Age = 1980
                }
            };
            users.ForEach(u => context.Users.Add(u));

            //Add activities
            var activities = new List<Activity> {
                new Activity() { 
                    Name = "AR Drones",
                    Description = "Flyv drones ved hjælp af iPads",
                    Active = true,
                    MinimumAge = 1981
                },
                new Activity() { 
                    Name = "Segways",
                    Description = "Prøv en tur på en segway",
                    Active = true,
                    MinimumAge = 1978
                }
            };
            activities.ForEach(a => context.Activities.Add(a));

        }
    }
}

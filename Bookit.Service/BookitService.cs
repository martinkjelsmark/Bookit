using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Bookit.Model;
using Bookit.DTO;
using log4net;

namespace  Bookit.Service
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    
    public class BookitService
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        BookitData _geekLabData = new BookitData();

        [WebGet(UriTemplate = "users?phone={phone}")]
        public List<UserDTO> GetUsers(string phone)
        {
            try
            {
                IQueryable<User> user = null;

                if (phone != null)
                {
                    logger.Debug("GET /users?Phone="+ phone +" called");
                    
                    if (phone.StartsWith("*") && phone.EndsWith("*"))
                        user = from u in _geekLabData.Users where u.Phone.Contains(phone.Replace("*", "")) select u;

                    else if (phone.StartsWith("*"))
                        user = from u in _geekLabData.Users where u.Phone.EndsWith(phone.Replace("*", "")) select u;

                    else if (phone.EndsWith("*"))
                        user = from u in _geekLabData.Users where u.Phone.StartsWith(phone.Replace("*", "")) select u;
                    
                    else
                        user = from u in _geekLabData.Users where u.Phone == phone select u;
                }
                else
                {
                    logger.Debug("GET /users called");
                    user = from u in _geekLabData.Users select u;
                }
                
                var result = user.Select(u => new UserDTO()
                {
                    name = u.Name,
                    phone = u.Phone,
                    userId = u.UserId,
                    age = u.Age
                });

                return result.ToList();
            }
            catch (Exception ex)
            {
                
                logger.Error("A problem occured with GET/users "+ ex.Message);
                return null;
            }
            
        }

        [WebGet(UriTemplate = "users/{id}/activities")]
        public List<ActivityDTO> GetActivitiesForUser(string id)
        {
            try
            {
                logger.Debug("GET /users/"+ id +"/activities called");

                int userId = Convert.ToInt32(id);

                var user = (from u in _geekLabData.Users where u.UserId == userId select u).First(); 

                if (user == null) return null;

                var activities = from a in _geekLabData.Activities where a.MinimumAge >= user.Age select a;

                var result = activities.Select(a => new ActivityDTO()
                {
                    activityId = a.ActivityId,
                    name = a.Name,
                    description = a.Description,
                    active = a.Active,
                    minimumAge = a.MinimumAge,
                    waitingTime = 23
                });

                return result.ToList();
            }
            catch (Exception ex)
            {

                logger.Error("A problem occured with GET /user/" + id +"/activities" + ex.Message);
                return null;
            }

        }

        [WebGet(UriTemplate = "activities")]
        public List<ActivityDTO> GetActivities()
        {
            try
            {
               logger.Debug("GET /activities called");

                var activities = _geekLabData.Activities.Select (a => new ActivityDTO()
                {
                    activityId = a.ActivityId,
                    name = a.Name,
                    description = a.Description,
                    active = a.Active,
                    minimumAge = a.MinimumAge,
                    waitingTime = 23
                });

                return activities.ToList();
            }
            catch (Exception ex)
            {

                logger.Error("A problem occured with GET /activities " + ex.Message);
                return null;
            }

        }

        [WebInvoke(UriTemplate = "booking", Method = "PUT")]
        public BookingDTO Update(BookingDTO bookingDTO)
        {
            try
            {
                logger.Info("PUT /booking kaldt");
                
                if (bookingDTO == null)
                    return null;

                Booking booking = new Booking();
                booking.ActivityId = bookingDTO.activityId;
                booking.UserId = bookingDTO.userId;
                booking.Timestamp = DateTime.Now;

                _geekLabData.Bookings.Add(booking);
                _geekLabData.SaveChanges();

                return bookingDTO;
            }
            catch (Exception ex)
            {

                logger.Error("A problem occured with PUT /booking " + ex.Message);
                return null;
            }
        }
    }
}

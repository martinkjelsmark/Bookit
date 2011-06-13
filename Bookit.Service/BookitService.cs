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
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class BookitService
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        BookitData _geekLabData = new BookitData();

        [WebGet(UriTemplate = "users?phone={phone}")]
        public List<UserDTO> GetUsers(string phone)
        {
            try
            {
                logger.Debug("/users called");

                //if (format.ToLower() == "json")
                //   WebOperationContext.Current.OutgoingResponse.Format = WebMessageFormat.Json;
                //else

                IQueryable<User> user;

                if (String.IsNullOrEmpty(phone))
                    user = from u in _geekLabData.Users select u;
                else
                    user = from u in _geekLabData.Users where u.Phone == phone select u;

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
                
                logger.Error("A problem occured with /users "+ ex.Message);
                return null;
            }
            
        }

        [WebGet(UriTemplate = "users/{id}/activities")]
        public List<ActivityDTO> GetActivitiesForUser(string id)
        {
            try
            {
                logger.Debug("/users/"+ id +"/activities called");

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
                    minimumAge = a.MinimumAge
                });

                return result.ToList();
            }
            catch (Exception ex)
            {

                logger.Error("A problem occured with /users " + ex.Message);
                return null;
            }

        }

        [WebGet(UriTemplate = "activities")]
        public List<ActivityDTO> GetActivities()
        {
            try
            {
               logger.Debug("/activities called");

                var activities = _geekLabData.Activities.Select (a => new ActivityDTO()
                {
                    activityId = a.ActivityId,
                    name = a.Name,
                    description = a.Description,
                    active = a.Active,
                    minimumAge = a.MinimumAge
                });

                return activities.ToList();
            }
            catch (Exception ex)
            {

                logger.Error("A problem occured with /activities " + ex.Message);
                return null;
            }

        }
    }
}

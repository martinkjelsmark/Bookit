using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Data.Entity;
using Bookit.Model;

namespace Bookit.Service
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Database.SetInitializer<BookitData>(new BookitInitializer());
            
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            // Edit the base address of Service1 by replacing the "Service1" string below
            RouteTable.Routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(BookitService)));
        }
    }
}

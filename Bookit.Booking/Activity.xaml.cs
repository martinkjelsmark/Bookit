using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bookit.DTO;
using RestSharp;

namespace Bookit.Booking
{
    /// <summary>
    /// Interaction logic for Activity.xaml
    /// </summary>
    public partial class Activity : Page
    {
        public Activity(UserDTO user)
        {
            InitializeComponent();

            try
            {
                label1.Content = "Hej " + user.name + ", vælg en aktivitet:";
                
                var client = new RestClient("http://localhost:8000/api");

                var request = new RestRequest("users/"+ user.userId +"/activities", Method.GET);

                request.AddHeader("Accept", "application/xml");

                // or automatically deserialize result
                // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                RestResponse<List<ActivityDTO>> response = client.Execute<List<ActivityDTO>>(request);

                if (response != null)
                { 
                    foreach (ActivityDTO activity in response.Data)
                    {
                        ListBoxItem li = new ListBoxItem();
                        li.Content = activity.name;

                        listBox1.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                ListBoxItem li = new ListBoxItem();
                li.Content = ex.Message;

                listBox1.Items.Add(li);
            }
        }
    }
}

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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Page
    {
        public User()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new RestClient("http://localhost:8000/api");

                var request = new RestRequest("users?phone="+ textBox1.Text, Method.GET);

                request.AddHeader("Accept", "application/xml");

                // or automatically deserialize result
                // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                RestResponse<UserDTO> response = client.Execute<UserDTO>(request);

                if (response != null)
                {
                    // View ACtivity page
                    Activity activityPage = new Activity(response.Data);
                    NavigationService.Navigate(activityPage);
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;
            }
            
        }
    }
}

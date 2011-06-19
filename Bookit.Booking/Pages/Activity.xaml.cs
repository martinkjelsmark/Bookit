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

namespace Bookit.ClientBooking.Pages
{
    /// <summary>
    /// Interaction logic for Activity.xaml
    /// </summary>
    public partial class Activity : Page
    {
        private List<ActivityDTO> _activities;
        private UserDTO _userDTO;
        
        public Activity(UserDTO user)
        {
            _userDTO = user;

            InitializeComponent();

            try
            {
                label1.Content = "Hej " + user.name + ", vælg en aktivitet:";

                var client = new RestClient("http://dkmkl-fusion-7/bookit");

                var request = new RestRequest("users/"+ user.userId +"/activities", Method.GET);

                request.AddHeader("Accept", "application/xml");

                // or automatically deserialize result
                // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                RestResponse<List<ActivityDTO>> response = client.Execute<List<ActivityDTO>>(request);

                if (response != null)
                {
                    _activities = response.Data;
                    lstCustomers.ItemsSource = _activities.ToList();
                }
            }
            catch (Exception ex)
            {
                lblError.Content = ex.Message;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ActivityDTO activity = (ActivityDTO)lstCustomers.SelectedItem;

            if (activity != null)
            {
                Picture picturePage = new Picture(activity, _userDTO);
                NavigationService.Navigate(picturePage);
            }
            
        }
    }
}

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
    public partial class UserList : Page
    {
        private List<UserDTO> _users;

        public UserList(List<UserDTO> users)
        {
            InitializeComponent();

            _users = users;
            
            if (_users != null)
                lstCustomers.ItemsSource = _users.ToList();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            UserDTO user = (UserDTO) lstCustomers.SelectedItem;

            if (user != null)
            {
                Activity activityPage = new Activity(user);
                NavigationService.Navigate(activityPage);
            }
        }
    }
}

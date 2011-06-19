using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;
using Bookit.DTO;
using RestSharp;
using System.Configuration;
using log4net;

namespace Bookit.ClientBooking.Pages
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class FindUser : Page
    {
        private string _exception;
        private int _time;
        private DispatcherTimer _countdownTimer;

        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public FindUser()
        {
            try
            {
                InitializeComponent();

                txtPhoneNumber.Focus();

                //Adds eventhandlers for keypress (Enter, F1)
                grid.KeyDown += new KeyEventHandler(KeyPress);
                txtPhoneNumber.KeyDown += new KeyEventHandler(txtPhoneNumber_KeyDown);

                //Creates and starts the timeout counter
                //If screen inactive for more than x sec - client resets
                _time = Convert.ToInt32(ConfigurationManager.AppSettings["PageTimeoutSec"]);
                _countdownTimer = new DispatcherTimer();
                _countdownTimer.Interval = new TimeSpan(0, 0, 1);
                _countdownTimer.Tick += new EventHandler(CountdownTimerStep);
                _countdownTimer.Start();
            }
            catch (Exception ex)
            {
                logger.Error("Error starting up booking client", ex);
                
                lblIcon.Content = "!";
                lblInfo.Text = ConfigurationManager.AppSettings["ErrorMessage"];
                _exception = ex.Message;
            }   
        }

        
        void txtPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            //Makes it possible to search the number by hitting Enter on the keyboard
            if (e.Key == Key.Enter)
                SearchUser(txtPhoneNumber.Text);
            
            //Clear info text when user ipnuts text 
            else if (lblInfo.Text.Length > 0)
            {
                lblInfo.Text = "";
                lblIcon.Content = "";
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchUser(txtPhoneNumber.Text);
        }

        public void KeyPress(object sender, KeyEventArgs e)
        {
            //Reset timeout counter
            _time = Convert.ToInt32(ConfigurationManager.AppSettings["PageTimeoutSec"]);

            // If an exception exist, allow it to be shown as a messagebox on keypress F1
            if (e.Key == Key.F1)
            {
                if (_exception != null)
                    MessageBox.Show(_exception);
            }
        }

        private void SearchUser(string phone)
        {
            try
            {
                //Create rest client and request
                var client = new RestClient(ConfigurationManager.AppSettings["APIURL"]);
                var request = new RestRequest("users?phone=" + txtPhoneNumber.Text, Method.GET);
                request.AddHeader("Accept", "application/xml");

                // Deserialize result to a list of userDTOs
                RestResponse<List<UserDTO>> response = client.Execute<List<UserDTO>>(request);

                if (response != null)
                {
                    //No user found - show info text
                    if (response.Data.Count == 0)
                    {
                        lblIcon.Content = "?";
                        lblInfo.Text = ConfigurationManager.AppSettings["UserNotFoundMessage"];
                        txtPhoneNumber.Select(0, txtPhoneNumber.Text.Length);
                    }
                    //Show select activity page if only 1 user is found
                    else if (response.Data.Count == 1)
                    {
                        Activity activityPage = new Activity(response.Data.First());
                        NavigationService.Navigate(activityPage);
                    }
                    //Show select user page if more than one user is found
                    else
                    {
                        UserList userListPage = new UserList(response.Data);
                        NavigationService.Navigate(userListPage);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error searching for user", ex);

                lblIcon.Content = "!";
                lblInfo.Text = ConfigurationManager.AppSettings["ErrorMessage"];
                _exception = ex.Message;
            }
        }

        //Timeout counter
        private void CountdownTimerStep(object sender, EventArgs e)
        {
            if (_time > 0 && txtPhoneNumber.Text.Length > 0)
                _time--;
            
            //Clear page
            if (_time <= 0)
            {
                lblInfo.Text = "";
                lblIcon.Content = "";
                txtPhoneNumber.Text = "";
                lblTimeoutCounter.Content = "";
                _time = Convert.ToInt32(ConfigurationManager.AppSettings["PageTimeoutSec"]);
            }

            if (_time <= 10)
                lblTimeoutCounter.Content = "Nulstiller om " + _time + " sek.";
        }

    }
}

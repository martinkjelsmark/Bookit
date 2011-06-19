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
using System.Windows.Threading;
using Bookit.DTO;
using RestSharp;

namespace Bookit.ClientBooking.Pages
{
    /// <summary>
    /// Interaction logic for Webcam.xaml
    /// </summary>
    public partial class Picture : Page
    {
        private ActivityDTO _activityDTO;
        private UserDTO _userDTO;

        public Picture(ActivityDTO activityDTO, UserDTO userDTO)
        {
            _activityDTO = activityDTO;
            _userDTO = userDTO;

            InitializeComponent();
        }

        WebCam webcam;
        private int _time;
        private DispatcherTimer _countdownTimer;

        private void pictureWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
            webcam.Start();

            _countdownTimer = new DispatcherTimer();
            _countdownTimer.Interval = new TimeSpan(0, 0, 1);
            _countdownTimer.Tick += new EventHandler(CountdownTimerStep);

        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            webcam.Stop();
        }

        //private void bntStart_Click(object sender, RoutedEventArgs e)
        //{

        //    webcam.Start();
        //}

        //private void bntStop_Click(object sender, RoutedEventArgs e)
        //{
        //    webcam.Stop();
        //}

        //private void bntContinue_Click(object sender, RoutedEventArgs e)
        //{
        //    webcam.Continue();
        //}

        private void bntCapture_Click(object sender, RoutedEventArgs e)
        {
            _time = 3;
            this.lblCounter.Content = "3";
            _countdownTimer.Start();
            
        }

        private void bntReCapture_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();
            bntReCapture.Visibility = Visibility.Hidden;
            bntCapture.Visibility = Visibility.Visible;
        }

        private void CountdownTimerStep(object sender, EventArgs e)
        {
            if (_time > 1)
            {
                _time--;
                this.lblCounter.Content = _time;
            }
            else
            {
                _countdownTimer.Stop();

                this.lblCounter.Content = "";

                webcam.Stop();
                bntReCapture.Visibility = Visibility.Visible;
                bntCapture.Visibility = Visibility.Hidden;


                BookingDTO bookingDTO = new BookingDTO();
                bookingDTO.activityId = _activityDTO.activityId;
                bookingDTO.userId = _userDTO.userId;

                //var client = new RestClient("http://dkmkl-fusion-7/bookit");
                //var request = new RestRequest("booking", Method.PUT);
                //request.AddBody(bookingDTO);
                //RestResponse<BookingDTO> response = client.Execute<BookingDTO>(request);

                var client = new RestClient("http://dkmkl-fusion-7/bookit");
                var request = new RestRequest("booking", Method.PUT);

                //request.XmlSerializer.Serialize(bookingDTO);

                // add parameters for all properties on an object
                request.AddBody(bookingDTO, "http://schemas.datacontract.org/2004/07/Bookit.DTO");
                
                // execute the request
                RestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string

                lblBookingStatus.Content = content;

                //if (response.Data != null)
                //    lblBookingStatus.Content = "Booking oprettet";
                //else
                //    lblBookingStatus.Content = "PROBLEM VED BOOKING";
            }
                
        }

        //private void bntSaveImage_Click(object sender, RoutedEventArgs e)
        //{
        //    Helper.SaveImageCapture((BitmapSource)imgCapture.Source);
        //}

        //private void bntResolution_Click(object sender, RoutedEventArgs e)
        //{
        //    webcam.ResolutionSetting();
        //}

        //private void bntSetting_Click(object sender, RoutedEventArgs e)
        //{
        //    webcam.AdvanceSetting();
        //}
    }

}

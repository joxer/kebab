using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps.Platform;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Net.NetworkInformation;

namespace ShareMyKebab
{

   

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        GeoCoordinateWatcher watcher;
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Reinitialize the GeoCoordinateWatcher

            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                watcher.MovementThreshold = 100;//distance in metres

                // Add event handlers for StatusChanged and PositionChanged events
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

                // Start data acquisition
                watcher.Start();
            }
            else
            {

                tbStatus.Text = "Your internet connection is not available";
            }
            //hide button
         //   btnStart.Visibility = Visibility.Collapsed;
        }
        #region Event Handlers

        /// <summary>
        /// Handler for the StatusChanged event. This invokes MyStatusChanged on the UI thread and
        /// passes the GeoPositionStatusChangedEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyStatusChanged(e));

        }

        /// <summary>
        /// Handler for the PositionChanged event. This invokes MyStatusChanged on the UI thread and
        /// passes the GeoPositionStatusChangedEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyPositionChanged(e));
        }

        #endregion
        /// <summary>
        /// Custom method called from the PositionChanged event handler
        /// </summary>
        /// <param name="e"></param>
        void MyPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // Update the map to show the current location
            Location ppLoc = new Location();
            ppLoc.Latitude = e.Position.Location.Latitude;
            ppLoc.Longitude = e.Position.Location.Longitude;
            mapMain.SetView(ppLoc, 18);

            //update pushpin location and show

            Pushpin pin1 = new Pushpin();
            pin1.Location = ppLoc;

            // Use the Windows Phone pushpin style from App.xaml.
            pin1.Content = "I'm eating a kebab here!";
            pin1.Background = new SolidColorBrush(Colors.White);
            pin1.PositionOrigin = PositionOrigin.BottomLeft;
            pin1.Foreground = new SolidColorBrush(Colors.Brown);

            // Add the pushpin to the map.
            mapMain.Children.Add(pin1);

            


        }

        /// <summary>
        /// Custom method called from the StatusChanged event handler
        /// </summary>
        /// <param name="e"></param>
        void MyStatusChanged(GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The location service is disabled or unsupported.
                    // Alert the user
                    tbStatus.Text = "sorry we can’t find you on this device";
                    break;
                case GeoPositionStatus.Initializing:
                    // The location service is initializing.
                    // Disable the Start Location button
                    tbStatus.Text = "Your kebab is cooking";
                    break;
                case GeoPositionStatus.NoData:
                    // The location service is working, but it cannot get location data
                    // Alert the user and enable the Stop Location button
                    tbStatus.Text = "can’t find you yet…";
                    ResetMap();

                    break;
                case GeoPositionStatus.Ready:
                    // The location service is working and is receiving location data
                    // Show the current position and enable the Stop Location button
                    tbStatus.Text = "Et-voilà your kebab!";
           
           
                   System.Threading.Thread.Sleep(1000);
                     NavigationService.Navigate(new Uri("/KebabCustom.xaml?loc=" + watcher.Position.Location.Latitude.ToString().Replace(",",".")+ "," + watcher.Position.Location.Longitude.ToString().Replace(",","."), UriKind.RelativeOrAbsolute));
        
                    

                    break;

            }
        }

        void ResetMap()
        {
            Location ppLoc = new Location();
            ppLoc.Latitude = 0;
            ppLoc.Latitude = 0;
            mapMain.SetView(ppLoc, 1);

            //update pushpin location and show
           
            mapMain.Children.RemoveAt(0);

            Pushpin pin1 = new Pushpin();
            pin1.Location = ppLoc;

            // Use the Windows Phone pushpin style from App.xaml.
            pin1.Content = "I'm eating a kebab here!";
            pin1.Background = new SolidColorBrush(Colors.White);
            pin1.PositionOrigin = PositionOrigin.BottomLeft;
            pin1.Foreground = new SolidColorBrush(Colors.Brown);
            pin1.PositionOrigin = PositionOrigin.BottomLeft;
            pin1.ApplyTemplate();

            // Add the pushpin to the map.
            mapMain.Children.Add(pin1);


           
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/customize.xaml", UriKind.RelativeOrAbsolute));
        
        }

    }
}
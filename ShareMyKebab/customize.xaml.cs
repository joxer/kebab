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
using Microsoft.Phone.Tasks;

namespace ShareMyKebab
{
    public partial class customize : PhoneApplicationPage
    {
        public customize()
        {
            InitializeComponent();
            if (!System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Contains("EnableLocation"))
                EnableLocation.IsChecked = true;
            else
            {
                EnableLocation.IsChecked = (bool?)System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["EnableLocation"] ?? true;

            }
        }

        private void EnableLocation_Checked(object sender, RoutedEventArgs e)
        {

          System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["EnableLocation"] = true;

        }

        private void EnableLocation_Unchecked(object sender, RoutedEventArgs e)
        {

            
          System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["EnableLocation"] = false;

        
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            new EmailComposeTask
            {
                Subject = "Privacy Question",
                Body = "My Application",
                To = "kell.92.k@gmail.com"
            }.Show();

        }
    }
}
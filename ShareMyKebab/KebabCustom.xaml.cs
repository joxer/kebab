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
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Shell;

namespace ShareMyKebab
{
    public partial class Page1 : PhoneApplicationPage
    {
        string url,turl;
        public Page1()
        {
            InitializeComponent();


        }

        protected override void OnNavigatedTo
(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("loc", out msg))
            {

                url = string.Concat("http://dev.virtualearth.net/REST/v1/Imagery/Map/Aerial/", msg, "/18?mapSize=300,300&pp=", msg, ";22&key=AuDYnNCuX6IO32SfDG38eldjOktHoKtvEyzOuOv1yMm7wAnmuu6h3K4ozwE5ghOx");


                Visibility darkBackgroundVisibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];


                if (darkBackgroundVisibility == Visibility.Visible)
                    //Theme is Dark
                    image1.Source = new BitmapImage(new Uri("/Image/icons/refresh_dark.png", UriKind.Absolute));
                else
                    image1.Source = new BitmapImage(new Uri("/Image/icons/refresh_white.png", UriKind.Absolute));

               


                var webClient = new WebClient();

                webClient.OpenReadAsync(new Uri("http://tinyurl.com/api-create.php?url=" + url));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);

             
                //url = "http://dev.virtualearth.net/REST/v1/Imagery/Map/Road/41.9042835235596,12.54311466/15?mapSize=200,200&pp=41.9042835235596,12.54311466;22&key=AuDYnNCuX6IO32SfDG38eldjOktHoKtvEyzOuOv1yMm7wAnmuu6h3K4ozwE5ghOx";
                image1.Source = new BitmapImage(new Uri(url, UriKind.Absolute));


                //image1.Source =  new BitmapImage(new Uri(url, UriKind.Absolute));

            }   
        }

        void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            using (var reader = new StreamReader(e.Result))
            {
                turl = reader.ReadToEnd();
                textBox1.Text = "i'm eating a kebab here! "+turl;
            }

        }

        public void ShortUrl(string url)
        {

            WebRequest request = WebRequest.Create(string.Format("http://tinyurl.com/api-create.php?url={0}", url));
            request.BeginGetResponse((ar) =>
            {
                WebResponse endGetResponse = request.EndGetResponse(ar);
                Stream responseStream = endGetResponse.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                string total = sr.ReadLine();
                // Do some stuff...
                textBox1.Text = "I'm eating a kebab here!" + total;
            }, null);

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void email_Click(object sender, EventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = "I'm eating a kebab here!";
            task.Body = "Hey I'm eating a kebab here! see it "+turl;
            task.Show();
        }

        private void fb_Click(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri(turl, UriKind.Absolute);
            shareLinkTask.Message = "Hey I'm eating a kebab here! "+turl;
            shareLinkTask.Show();
        }

        private void twitter_Click(object sender, EventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.LinkUri = new Uri(turl, UriKind.Absolute);
            shareLinkTask.Message = "Hey I'm eating a kebab here!"+turl;
            shareLinkTask.Show();
        }



    }
}
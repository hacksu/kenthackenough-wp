using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Web.Http;
using System.Threading; 
using System.Windows.Threading; 
using System.Windows;
using System.Windows.Controls;
using Windows.UI.Core;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Kent_Hack_Enough
{
    class HTTPClient
    {
        const string API_SERVER = "http://api.khe.pdilyard.com/v1.0/";
        HttpWebRequest request;
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();


        public void Connect(string server, int port){
            request = (HttpWebRequest)WebRequest.Create(API_SERVER);
        }


        public void On(string nsp, string cmd, string data)
        {
            SocketRequest obj = new SocketRequest(nsp, cmd, data);

            Timer = new Timer(TimerCallback, obj, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
        }


        private void toggleProg()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Your UI update code goes here!
                try
                {
                    MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                    if (main.progBar.Visibility == Visibility.Collapsed)
                    {
                        main.progBar.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        main.progBar.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception)
                {
                 //   throw;
                }
            });
        }

        private void refreshLiveFeed()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                     // Your UI update code goes here!
                MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content; 
                main.LiveFeedItems.Children.Clear();
                int j = settings.LiveFeedSetting.messages.Count()-1;

                for (int i = j; i >= 0; i--)
                {
                    TextBlock txtMsg = new TextBlock();
                    TextBlock txtDate = new TextBlock();
                    StackPanel stkContainer = new StackPanel();


                    stkContainer.Height = 100;
                    stkContainer.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                    stkContainer.Margin = new System.Windows.Thickness(5.0);


                    txtMsg.Text = settings.LiveFeedSetting.messages[i].text.ToString();
                    txtDate.Text = settings.LiveFeedSetting.messages[i].created.ToString();
                    txtDate.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txtDate.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

                    stkContainer.Children.Add(txtMsg);
                    stkContainer.Children.Add(txtDate);


                    main.LiveFeedItems.Children.Add(stkContainer);
                }
                }
                catch (Exception)
                {
                 //   throw;
                }
            });
        }

        #region WebCleint

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;

                var results = JsonConvert.DeserializeObject<dynamic>(e.Result);

                //e.Result.Contains("start");

                RootObject Result = JsonConvert.DeserializeObject<RootObject>(e.Result);

                settings.LiveFeedSetting = Result;
                settings.Save();

                refreshLiveFeed();

                
            }
            catch
            {
                return;
            }

            toggleProg();
        }


        private void TimerCallback(object state)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
            {
                // Your UI update code goes here!
                try
                {
                    MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                    toggleProg();
                }
                catch (Exception)
                {
                    //throw;
                }
            });

            
            WebClient webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            webClient.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();

            SocketRequest obj = (SocketRequest)state;

            webClient.DownloadStringAsync(new Uri(API_SERVER + obj.getNsp()));
        }

        #endregion
    }
}

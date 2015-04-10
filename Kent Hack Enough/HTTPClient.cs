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
        ProgressIndicator prog;


        public void Connect(string server, int port){
            request = (HttpWebRequest)WebRequest.Create(API_SERVER);
        }

        public void On()
        {
            Timer = new Timer(TimerCallback, null, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
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
                int j = settings.LiveFeedSetting.messages.Count() - 1;

                for (int i = j; i > 0; i--)
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

            toggleProg();
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            toggleProg();

            try
            {
                string data = e.Result;

                var results = JsonConvert.DeserializeObject<dynamic>(e.Result);

                RootObject Result = JsonConvert.DeserializeObject<RootObject>(e.Result);

                refreshLiveFeed();

                settings.LiveFeedSetting = Result;
                settings.Save();
            }
            catch
            {
                return;
            }
        }

        private void TimerCallback(object state)
        {
            //Dispatcher.BeginInvoke(() =>
              //  {
            //        main.prog.Visibility = System.Windows.Visibility.Visible;
              //  });

        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
        {
            // Your UI update code goes here!
            try
            {
                MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                main.progBar.Visibility = Visibility.Visible;
                MessageBox.Show("UPDATING");
            }
            catch (Exception)
            {
                
                //throw;
            }
            
        });





            WebClient webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            webClient.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            
            webClient.DownloadStringAsync(new Uri(API_SERVER + "/messages"));
        }
    }
}

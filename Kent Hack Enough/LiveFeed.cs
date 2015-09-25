using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.UI.Core;

namespace Kent_Hack_Enough
{
    public class UpdateMessages
    {
        public string _id { get; set; }
        public string text { get; set; }
        public int __v { get; set; }
        public DateTime created { get; set; }
    }

    public class RootMessages
    {
        public List<UpdateMessages> messages { get; set; }
    }

    public class LiveFeed
    {
        const string API_SERVER = "http://api.khe.pdilyard.com/v1.0/";
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();
        
        
        
        public LiveFeed() { }

        LiveFeed(string msgId, string msgTxt, int msgV, DateTime msgCreated)
        {
            RootMessages rootMsg = new RootMessages();
            UpdateMessages liveMsg = new UpdateMessages();
            liveMsg._id = msgId;
            liveMsg.text = msgTxt;
            liveMsg.__v = msgV;
            liveMsg.created = msgCreated;

            rootMsg.messages.Add(liveMsg);
        }

        public void getFeed()
        {
            object obj = new object();
            obj = settings.APIServerSetting;
            string[] tmp = null;
            bool portAdded = false;

            tmp = settings.APIServerSetting.Split('/');

            for (int i = 0; i < tmp.Length; i++)
            {
                if (!portAdded)
                {
                    obj = "http://" + tmp[2] + ":" + settings.APIPortSetting;
                    portAdded = true;
                    i = 2;
                }
                else
                {
                    obj = obj + "/" + tmp[i];
                }
            }

           

            Timer = new Timer(TimerCallback, obj, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
        }

        public TextBlock parseText(UpdateMessages msg)
        {
            TextBlock result = new TextBlock();
            Markdown md = new Markdown();

            result = md.parseMarkdown(msg.text);

            return result;
        }

        public string parseDate(DateTime dt)
        {
            DateTime dtNow = new DateTime();
            dt = dt.ToLocalTime();
            
            string result = null;

            dtNow = DateTime.Now.ToLocalTime();

            int timeDif = dtNow.Hour - dt.Hour - dtNow.Minute + dt.Minute;
            timeDif = 100 - timeDif;


            if (dt.ToLocalTime().Day == dtNow.Day)
            {
                // Same day so lets check the hour
                if (dt.Hour < dtNow.Hour)
                {
                    DateTime tmp = new DateTime();
                    tmp.AddMinutes(dt.Minute);

                    if ((dtNow.Hour - dt.Hour) == 1)
                    {
                        result = "an hour ago";
                        return result;
                    }
                    result = (dtNow.Hour - dt.Hour).ToString() + " hours ago";
                }
                // Count the minutes
                else if(((dtNow.Minute - dt.Minute) > 0) && ((dtNow.Minute - dt.Minute) < 60) && (dtNow.Hour == dt.Hour) || (dtNow.Hour == dt.Hour + 1)){
                    if((dtNow.Minute - dt.Minute) == 1){
                        result = "a minute ago";
                    }else{
                        result = (dtNow.Minute - dt.Minute).ToString() + " minutes ago";
                    }
                }
                // Looks like this post was just now!
                else
                {
                    result = "just now";
                }
            }
                // Check to see if in same month
            else if (dt.Month == dtNow.Month)
            {
                // Same month lets check the day
                if (dt.ToLocalTime().Day < dtNow.Day)
                {
                    if ((dtNow.ToLocalTime().Day - dt.Day).ToString() == "1")
                    {
                        result = "a day ago";
                        return result;
                    }
                    result = (dtNow.ToLocalTime().Day - dt.Day).ToString() + " days ago";
                }
            }
            else if (dt.Month != dtNow.Month && dt.Year == dtNow.Year)
            {
                if ((dtNow.ToLocalTime().Month - dt.Month).ToString() == "1")
                {
                    result = "a month ago";
                    return result;
                }
                result = (dtNow.ToLocalTime().Month - dt.Month).ToString() + " months ago";
            }
            // Check the year
            else if (dt.Year < dtNow.Year)
            {
                if ((dtNow.ToLocalTime().Year - dt.Year).ToString() == "1")
                {
                    result = "a year ago";
                    return result;
                }
                result = (dtNow.ToLocalTime().Year - dt.Year).ToString() + " years ago";
            }
            else
            {

            }

            return result;

        }


        private async void toggleProg()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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


        private async void refreshLiveFeed()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 try
                 {
                    // Your UI update code goes here!
                    MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                     main.UpdatesItems.Children.Clear();
                     int j = settings.LiveFeedSetting.messages.Count() - 1;

                     for (int i = j; i >= 0; i--)
                     {
                         TextBlock txtMsg = new TextBlock();
                         TextBlock txtDate = new TextBlock();
                         StackPanel stkContainer = new StackPanel();


                         stkContainer.MinHeight = 50;
                         stkContainer.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                         stkContainer.Margin = new System.Windows.Thickness(5.0);


                         txtMsg = parseText(settings.LiveFeedSetting.messages[i]);
                         txtMsg.Margin = new System.Windows.Thickness(5.0);
                         

                         txtDate.Text = parseDate(settings.LiveFeedSetting.messages[i].created);
                         txtDate.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtDate.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                         txtDate.Margin = new System.Windows.Thickness(3.0);
                         txtDate.FontSize = 13;

                         stkContainer.Children.Add(txtMsg);
                         stkContainer.Children.Add(txtDate);

                         main.UpdatesItems.Children.Add(stkContainer);
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

                RootMessages Result = JsonConvert.DeserializeObject<RootMessages>(e.Result);

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


        private async void TimerCallback(object state)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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

            webClient.DownloadStringAsync(new Uri(state.ToString() + "messages"));
        }

        #endregion

    }
}

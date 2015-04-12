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
using System.Windows.Documents;
using System.Windows.Media;
using Windows.UI.Core;

namespace Kent_Hack_Enough
{
    public class LiveFeedMessages
    {
        public string _id { get; set; }
        public string text { get; set; }
        public int __v { get; set; }
        public string created { get; set; }
    }

    public class RootMessages
    {
        public List<LiveFeedMessages> messages { get; set; }
        public void parseText()
        {

        }
    }

    public class LiveFeed
    {
        const string API_SERVER = "http://api.khe.pdilyard.com/v1.0/";
        HttpWebRequest request;
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();
        
        
        
        public LiveFeed() { }

        LiveFeed(string msgId, string msgTxt, int msgV, string msgCreated)
        {
            RootMessages rootMsg = new RootMessages();
            LiveFeedMessages liveMsg = new LiveFeedMessages();
            liveMsg._id = msgId;
            liveMsg.text = msgTxt;
            liveMsg.__v = msgV;
            liveMsg.created = msgCreated;

            rootMsg.messages.Add(liveMsg);
        }

        public void getFeed()
        {
            Timer = new Timer(TimerCallback, null, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
        }

        public TextBlock parseText(LiveFeedMessages msg)
        {
            TextBlock result = new TextBlock();
            //Run textRun = new Run();
           // textRun.Text = "The text contents of this text run.";
            //textRun.FontStyle = FontStyles.Italic;
            Markdown md = new Markdown();
            result = md.parseMarkdown(msg);





           // result.Inlines.Add(textRun);
            //string text = msg.text;

            

            return result;
        }

        public DateTime parseDate(LiveFeedMessages msg)
        {
            DateTime dt = Convert.ToDateTime(msg.created.ToString());
            DateTimeOffset dateAndOffset = new DateTimeOffset(dt, TimeZoneInfo.Local.GetUtcOffset(dt));

            return dateAndOffset.DateTime;
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

                    for (int i = j; i >= 0; i--)
                    {
                        TextBlock txtMsg = new TextBlock();
                        TextBlock txtDate = new TextBlock();
                        StackPanel stkContainer = new StackPanel();


                        stkContainer.Height = 100;
                        stkContainer.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                        stkContainer.Margin = new System.Windows.Thickness(5.0);


                        txtMsg = parseText(settings.LiveFeedSetting.messages[i]);
                        txtMsg.Margin = new System.Windows.Thickness(5.0);

                        txtDate.Text = parseDate(settings.LiveFeedSetting.messages[i]).ToString();
                        txtDate.Margin = new System.Windows.Thickness(5.0);
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


        private void TimerCallback(object state)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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

            webClient.DownloadStringAsync(new Uri(API_SERVER + "/messages"));
        }

        #endregion

    }
}

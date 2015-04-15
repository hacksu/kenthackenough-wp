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
    public class Events
    {
        public string _id { get; set; }
        public string name { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string group { get; set; }
        public bool notify { get; set; }
    }

    public class RootEvents
    {
        public List<Events> events { get; set; }
    }

    public class Event
    {
        HttpWebRequest request;
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();



        public Event() { }

        Event(string eventId, string eventName, DateTime eventStart, DateTime eventEnd, String eventGroup, bool eventNotify)
        {
            RootEvents rootEvent = new RootEvents();
            Events events = new Events();

            events._id = eventId;
            events.name = eventName;
            events.start = eventStart;
            events.end = eventEnd;
            events.group = eventGroup;
            events.notify = eventNotify;

            rootEvent.events.Add(events);
        }

        public void getEvent()
        {
            object obj = new object();
           // obj = settings.APIServerSetting;
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

            Timer = new Timer(TimerCallback2, obj, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
        }

        public TextBlock parseText(Events msg)
        {
            TextBlock result = new TextBlock();
            Markdown md = new Markdown();

            result = md.parseMarkdown(msg.name);

            return result;
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


        private void refreshEvents()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    // Your UI update code goes here!
                    MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                    main.EventsItems.Children.Clear();
                    int j = settings.EventsSetting.events.Count() - 1;

                    for (int i = j; i >= 0; i--)
                    {
                        TextBlock txtMsg = new TextBlock();
                        TextBlock txtStart = new TextBlock();
                        TextBlock txtEnd = new TextBlock();
                        StackPanel stkContainer = new StackPanel();


                        stkContainer.Height = 100;
                        stkContainer.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                        stkContainer.Margin = new System.Windows.Thickness(5.0);


                        txtMsg = parseText(settings.EventsSetting.events[i]);
                        txtMsg.Margin = new System.Windows.Thickness(5.0);

                        txtStart.Text = settings.EventsSetting.events[i].start.ToLocalTime().ToString();
                        txtStart.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        txtStart.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

                        txtEnd.Text = settings.EventsSetting.events[i].end.ToLocalTime().ToString();
                        txtEnd.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        txtEnd.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

                        stkContainer.Children.Add(txtMsg);
                        stkContainer.Children.Add(txtStart);
                        stkContainer.Children.Add(txtEnd);

                        main.EventsItems.Children.Add(stkContainer);
                    }
                }
                catch (Exception)
                {
                    //   throw;
                }
            });
        }




        #region WebCleint

        void webClientEvents_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;

                var results = JsonConvert.DeserializeObject<dynamic>(e.Result);

                RootEvents Result = JsonConvert.DeserializeObject<RootEvents>(e.Result);

                settings.EventsSetting = Result;
                settings.Save();

                refreshEvents();
            }
            catch
            {
                return;
            }

            toggleProg();
        }


        private void TimerCallback2(object state)
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


            WebClient webClientEvents = new WebClient();

            webClientEvents.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClientEvents_DownloadStringCompleted);

            webClientEvents.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
           // state.ToString()
            webClientEvents.DownloadStringAsync(new Uri(state.ToString() + "events"));
        }
        #endregion
    }
}

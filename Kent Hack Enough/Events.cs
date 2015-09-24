﻿using Microsoft.Phone.Controls;
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
        public string title { get; set; }
        public string description { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public string __v { get; set; }
        public bool notify { get; set; }
        public string group { get; set; }
    }

    public class RootEvents
    {
        public List<Events> events { get; set; }
    }

    public class Event
    {
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();



        public Event() { }

        Event(string eventId, string eventTitle, string eventDescription, DateTime eventStart, DateTime eventEnd, String eventType, string eventLocation, string eventV, bool eventNotify, string eventGroup)
        {
            RootEvents rootEvent = new RootEvents();
            Events events = new Events();

            events._id = eventId;
            events.title = eventTitle;
            events.description = eventDescription;
            events.start = eventStart;
            events.end = eventEnd;
            events.type = eventType;
            events.location = eventLocation;
            events.__v = eventV;
            events.notify = eventNotify;
            events.group = eventGroup;

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

        public TextBlock parseText(string txt)
        {
            TextBlock result = new TextBlock();
            Markdown md = new Markdown();

            result.Text = txt;

           // result = md.parseMarkdown(msg.description);

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


        private async void refreshEvents()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 try
                 {
                     // Your UI update code goes here!
                     MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                     main.EventsItems.Children.Clear();

                    // int j = settings.EventsSetting.events.Count() - 1;

                     for (int i = 0; i < settings.EventsSetting.events.Count()+1; i++)
                     {
                         TextBlock txtTitle = new TextBlock();
                         TextBlock txtDescription = new TextBlock();
                         TextBlock txtStart = new TextBlock();
                         TextBlock txtEnd = new TextBlock();
                         TextBlock txtType = new TextBlock();
                         TextBlock txtLocation = new TextBlock();
                         StackPanel stkContainer = new StackPanel();

                       //  stkContainer.Height = 100;
                         stkContainer.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                         stkContainer.Margin = new System.Windows.Thickness(5.0);


                         //   txtTitle = parseText(settings.EventsSetting.events[i]);
                         txtTitle = parseText(settings.EventsSetting.events[i].title);
                         txtTitle.Margin = new System.Windows.Thickness(5.0);

                         //   txtDescription = parseText(settings.EventsSetting.events[i]);
                         txtDescription = parseText(settings.EventsSetting.events[i].description);
                         txtDescription.Margin = new System.Windows.Thickness(5.0);

                         txtStart.Text = settings.EventsSetting.events[i].start.ToLocalTime().ToString();
                         txtStart.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtStart.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                       //  txtStart.FontSize = 13;

                         txtEnd.Text = settings.EventsSetting.events[i].end.ToLocalTime().ToString();
                         txtEnd.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtEnd.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                       //  txtEnd.FontSize = 13;

                         //   txtType = parseText(settings.EventsSetting.events[i]);
                         txtType = parseText(settings.EventsSetting.events[i].type);
                         txtType.Margin = new System.Windows.Thickness(5.0);
                         txtType.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                         txtType.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                         txtType.FontSize = 14;

                         //   txtLocation = parseText(settings.EventsSetting.events[i]);
                         txtLocation = parseText(settings.EventsSetting.events[i].location);
                         txtLocation.Margin = new System.Windows.Thickness(5.0);
                         txtLocation.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtLocation.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                         txtLocation.FontSize = 14;

                         stkContainer.Children.Add(txtTitle);
                         stkContainer.Children.Add(txtDescription);
                         stkContainer.Children.Add(txtStart);
                         stkContainer.Children.Add(txtEnd);
                         stkContainer.Children.Add(txtType);
                         stkContainer.Children.Add(txtLocation);


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


        private async void TimerCallback2(object state)
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


            WebClient webClientEvents = new WebClient();

            webClientEvents.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClientEvents_DownloadStringCompleted);

            webClientEvents.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
           // state.ToString()
            webClientEvents.DownloadStringAsync(new Uri(state.ToString() + "events"));
        }
        #endregion
    }
}

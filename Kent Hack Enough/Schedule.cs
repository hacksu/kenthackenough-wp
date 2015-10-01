using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class Schedule
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
        public List<Schedule> events { get; set; }
    }

    public class Event
    {
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();



        public Event() { }

        Event(string eventId, string eventTitle, string eventDescription, DateTime eventStart, DateTime eventEnd, String eventType, string eventLocation, string eventV, bool eventNotify, string eventGroup)
        {
            RootEvents rootEvent = new RootEvents();
            Schedule events = new Schedule();

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


        public RootEvents sortEvent(RootEvents root)
        {
            List<Schedule> orderedEVents = root.events;

            for (int j = 0; j < orderedEVents.Count(); j++)
            {
                for (int k = j + 1; k < orderedEVents.Count(); k++)
                {
                    if (orderedEVents[j].start > orderedEVents[k].start)
                    {
                        orderedEVents.Insert(j, orderedEVents[k]);
                        orderedEVents.RemoveAt(k + 1);
                        break;
                    }
                }
            }

            RootEvents newRoot = new RootEvents();
            newRoot.events = orderedEVents;

            return newRoot;
        }

        public void getEventNow()
        {
            object obj = new object();
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

            Timer = new Timer(TimerCallback2, obj, 0, 0);
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


        private async void refreshSchedule()
        {
          //  sortEvent(settings.EventsSetting);

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 try
                 {
                     // Your UI update code goes here!
                     MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                     main.ScheduleItems.Children.Clear();

                     Grid DynamicGrid = new Grid();
                     ColumnDefinition gridCol1 = new ColumnDefinition();
                     ColumnDefinition gridCol2 = new ColumnDefinition();
                     ColumnDefinition gridCol3 = new ColumnDefinition();
                     gridCol1.Width = new GridLength(100, GridUnitType.Star);
                     gridCol2.Width = new GridLength(3, GridUnitType.Pixel);
                     gridCol3.Width = new GridLength(100, GridUnitType.Star);

                     DynamicGrid.ColumnDefinitions.Add(gridCol1);
                     DynamicGrid.ColumnDefinitions.Add(gridCol2);
                     DynamicGrid.ColumnDefinitions.Add(gridCol3);


                     string curDayOfWeek = "";
                     int days = 0;


                     for (int i = 0; i < settings.EventsSetting.events.Count(); i++)
                     {
                         TextBlock txtTitle = new TextBlock();
                         TextBlock txtDescription = new TextBlock();
                         TextBlock txtStart = new TextBlock();
                         TextBlock txtEnd = new TextBlock();
                         TextBlock txtLocation = new TextBlock();
                         StackPanel stkDay = new StackPanel();
                         StackPanel stkItemsLeft = new StackPanel();
                         StackPanel stkItemsRight = new StackPanel();
                         TextBlock txtDay = new TextBlock();
                         RowDefinition gridRow = new RowDefinition();

                         
                         
                         if (curDayOfWeek != settings.EventsSetting.events[i].start.ToString("dddd"))
                         {
                             days++;
                             curDayOfWeek = settings.EventsSetting.events[i].start.ToString("dddd");
                             RowDefinition gridRow2 = new RowDefinition();
                             gridRow2.Height = new GridLength(75, GridUnitType.Pixel);
                             DynamicGrid.RowDefinitions.Add(gridRow2);

                             // Add in new day of the week

                             stkDay.Width = 300;
                             stkDay.Height = 75;
                             stkDay.Background = new SolidColorBrush(Color.FromArgb(255, 35, 31, 32));
                             Canvas.SetZIndex(stkDay, 2);
                             if (i == 0)
                             {
                                 Grid.SetRow(stkDay, i);
                             }
                             else
                             {
                                 Grid.SetRow(stkDay, days -1);
                             }

                             Grid.SetColumnSpan(stkDay, 3);
                             Grid.SetColumn(stkDay, 0);

                             txtDay.Text = curDayOfWeek.ToString();
                             txtDay.HorizontalAlignment = HorizontalAlignment.Center;
                             txtDay.FontSize = 40;
                             txtDay.TextDecorations = TextDecorations.Underline;
                             txtDay.FontWeight = FontWeights.Bold;

                             stkDay.Children.Add(txtDay);
                             DynamicGrid.Children.Add(stkDay);
                         }

                         gridRow.Height = new GridLength(125, GridUnitType.Star);
                         gridRow.MinHeight = 125;
                         DynamicGrid.RowDefinitions.Add(gridRow);


                         stkItemsLeft.HorizontalAlignment = HorizontalAlignment.Left;
                         stkItemsLeft.MinWidth = 200;
                         Grid.SetRow(stkItemsLeft, days);
                         Grid.SetColumn(stkItemsLeft, 0);


                         stkItemsRight.HorizontalAlignment = HorizontalAlignment.Left;
                         stkItemsRight.MinWidth = 200;
                         Grid.SetRow(stkItemsRight, days);
                         Grid.SetColumn(stkItemsRight, 3);


                         txtTitle.Text = settings.EventsSetting.events[i].title.ToString();
                         txtTitle.Margin = new Thickness(0, 0, 0, 0);
                         txtTitle.Padding = new Thickness(15, 0, 0, 0);
                         txtTitle.FontWeight = FontWeights.Bold;
                         txtTitle.FontSize = 20;
                         txtTitle.TextWrapping = TextWrapping.Wrap;


                         txtDescription = parseText(settings.EventsSetting.events[i].description);
                         txtDescription.Margin = new Thickness(0, 3, 0, 0);
                         txtDescription.FontSize = 15;
                         txtDescription.Padding = new Thickness(15, 0, 0, 0);
                         txtDescription.TextWrapping = TextWrapping.Wrap;


                         txtStart.Text = settings.EventsSetting.events[i].start.ToLocalTime().ToString();
                         txtStart.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

                         txtEnd.Text = settings.EventsSetting.events[i].end.ToLocalTime().ToString();
                         txtEnd.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtEnd.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;


                         txtLocation = parseText(settings.EventsSetting.events[i].location);
                         txtLocation.Padding = new Thickness(0, 0, 15, 0);
                         txtLocation.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtLocation.FontSize = 15;
                         txtLocation.TextWrapping = TextWrapping.Wrap;


                         stkItemsLeft.Children.Add(txtStart);
                         stkItemsLeft.Children.Add(txtLocation);

                         stkItemsRight.Children.Add(txtTitle);
                         stkItemsRight.Children.Add(txtDescription);


                         DynamicGrid.Children.Add(stkItemsLeft);
                         DynamicGrid.Children.Add(stkItemsRight);


                         days++;
                     }

                     main.ScheduleItems.Children.Add(DynamicGrid);
                 }
                 catch (Exception ex)
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

                for (int i = 0; i < Result.events.Count(); i++)
                {
                    Result.events[i].start = Result.events[i].start.ToLocalTime();
                }

                Result = sortEvent(Result);

                if(Result != settings.EventsSetting)
                {
                    settings.EventsSetting = Result;
                    settings.Save();

                    refreshSchedule();
                }
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

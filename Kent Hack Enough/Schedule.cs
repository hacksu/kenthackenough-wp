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
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 try
                 {
                     // Your UI update code goes here!
                     MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;
                     main.ScheduleItems.Children.Clear();
                   //  main.ScheduleDay.Children.Clear();
                   //  main.ScheduleItemsLeft.Children.Clear();
                  //   main.ScheduleItemsRight.Children.Clear();
                     // int j = settings.EventsSetting.events.Count() - 1;

                     string curDayOfWeek = "";
                     int days = 0;

                     Grid DynamicGrid = new Grid();
                     ColumnDefinition gridCol1 = new ColumnDefinition();
                     ColumnDefinition gridCol2 = new ColumnDefinition();
                     ColumnDefinition gridCol3 = new ColumnDefinition();
                     DynamicGrid.ColumnDefinitions.Add(gridCol1);
                     DynamicGrid.ColumnDefinitions.Add(gridCol2);
                     DynamicGrid.ColumnDefinitions.Add(gridCol3);
                     RowDefinition gridRow1 = new RowDefinition();
                     RowDefinition gridRow2 = new RowDefinition();
                     RowDefinition gridRow3 = new RowDefinition();
                     DynamicGrid.RowDefinitions.Add(gridRow1);
                     DynamicGrid.RowDefinitions.Add(gridRow2);
                     DynamicGrid.RowDefinitions.Add(gridRow3);




                     for (int i = 0; i < settings.EventsSetting.events.Count()+1; i++)
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
                                                 
                         
                         if(i == 0)
                         {
                             curDayOfWeek = settings.EventsSetting.events[i].start.ToString("dddd");
                         }
                         else
                         {
                             if (curDayOfWeek != settings.EventsSetting.events[i].start.ToString("dddd"))
                             {
                                 curDayOfWeek = settings.EventsSetting.events[i].start.ToString("dddd");
                                 days += 1;

                                 // Add in new day of the week

                                 stkDay.Width = 200;
                                 stkDay.Height = 75;
                                 stkDay.Background = new SolidColorBrush(Color.FromArgb(100, 242, 49, 242));

                                 txtDay.Text = curDayOfWeek.ToString();
                                 txtDay.HorizontalAlignment = HorizontalAlignment.Center;
                                 txtDay.FontSize = 50;
                                 txtDay.TextDecorations = TextDecorations.Underline;
                                 txtDay.FontWeight = FontWeights.Bold;

                                 stkDay.Children.Add(txtDay);
                                 main.ScheduleItems.Children.Add(stkDay);

                             }
                         }

                      
                         stkItemsLeft.HorizontalAlignment = HorizontalAlignment.Left;
                         stkItemsLeft.MinWidth = 200;
 
                         stkItemsRight.HorizontalAlignment = HorizontalAlignment.Left;
                         stkItemsRight.MinWidth = 200;

                         txtTitle.Text = settings.EventsSetting.events[i].title.ToString();
                         txtTitle.Margin = new Thickness(200, 25, 0, 0);
                         txtTitle.Padding = new Thickness(0, 25, 0, 0);
                         txtTitle.FontWeight = FontWeights.Bold;
                         txtTitle.FontSize = 20;
                         txtTitle.TextWrapping = TextWrapping.Wrap;

                         txtDescription = parseText(settings.EventsSetting.events[i].description);
                         txtDescription.Margin = new System.Windows.Thickness(0, 15, 0, 0);
                         txtDescription.FontWeight = FontWeights.Bold;
                         txtDescription.FontSize = 20;
                         txtDescription.Padding = new Thickness(15, 0, 0, 0);
                         txtDescription.TextWrapping = TextWrapping.Wrap;
                         Grid.SetRow(txtDescription, 2);
                         Grid.SetColumn(txtDescription, 2);


                         txtStart.Text = settings.EventsSetting.events[i].start.ToLocalTime().ToString();
                         txtStart.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtStart.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;

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

                         main.ScheduleItems.Children.Add(stkItemsLeft);
                         main.ScheduleItems.Children.Add(stkItemsRight);
                         
                         
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

                refreshSchedule();
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

using Microsoft.Phone.Controls;
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
    class Dash
    {
        private AppSettings settings = new AppSettings();

        public async void refreshDash()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    // Your UI update code goes here!
                    MainPage main = (MainPage)((PhoneApplicationFrame)Application.Current.RootVisual).Content;

                    main.stkUpdates.Children.Clear();
                    main.stkSchedule.Children.Clear();

                    int scheduleIndex = 0;
                    int updateIndex = 0;

                    for (int i = 0; i < settings.EventsSetting.events.Count(); i++)
                    {
                        if (DateTime.Now > settings.EventsSetting.events[i].start)
                        {
                            scheduleIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < settings.EventsSetting.events.Count(); i++)
                    {
                        if (DateTime.Now > settings.EventsSetting.events[i].start)
                        {
                            scheduleIndex = i;
                            break;
                        }
                    }


                    TextBlock txtTextUpdate = new TextBlock();
                    TextBlock txtCreatedUpdate = new TextBlock();

                    TextBlock txtTitleSchedule = new TextBlock();
                    TextBlock txtDescriptionSchedule = new TextBlock();
                    TextBlock txtLocationSchedule = new TextBlock();
                    TextBlock txtTimeSchedule = new TextBlock();


                    txtTextUpdate = parseText(settings.LiveFeedSetting.messages[updateIndex]);
                    txtTextUpdate.Margin = new Thickness(5.0);
                    txtTextUpdate.TextWrapping = TextWrapping.Wrap;
                    txtTextUpdate.Height = 140;

                    txtCreatedUpdate.Text = parseDate(settings.LiveFeedSetting.messages[updateIndex].created);
                    txtCreatedUpdate.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txtCreatedUpdate.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    txtCreatedUpdate.Margin = new Thickness(3.0);
                    txtCreatedUpdate.FontSize = 13;
                    Grid.SetRow(txtCreatedUpdate, 2);
                    Grid.SetColumn(txtCreatedUpdate, 0);


                    txtTitleSchedule.Text = settings.EventsSetting.events[scheduleIndex].title;
                    txtTitleSchedule.Margin = new Thickness(5.0);
                    txtTitleSchedule.TextWrapping = TextWrapping.Wrap;
                    

                    txtDescriptionSchedule.Text = settings.EventsSetting.events[scheduleIndex].description;
                    txtDescriptionSchedule.Margin = new Thickness(5.0);
                    txtDescriptionSchedule.TextWrapping = TextWrapping.Wrap;

                    txtLocationSchedule.Text = settings.EventsSetting.events[scheduleIndex].location;
                    txtLocationSchedule.Margin = new Thickness(5.0);
                    txtLocationSchedule.TextWrapping = TextWrapping.Wrap;
                    txtLocationSchedule.HorizontalAlignment = HorizontalAlignment.Right;
                    txtLocationSchedule.VerticalAlignment = VerticalAlignment.Bottom;
                    txtLocationSchedule.FontSize = 13;
                    Grid.SetRow(txtLocationSchedule, 2);
                    Grid.SetColumn(txtLocationSchedule, 1);

                    txtTimeSchedule.Text = settings.EventsSetting.events[scheduleIndex].start.ToString("t") + " - " + settings.EventsSetting.events[scheduleIndex].end.ToString("t");
                    txtTimeSchedule.Margin = new Thickness(5, -3, 0, 0);
                    txtTimeSchedule.FontSize = 13;

                    main.stkUpdates.Children.Add(txtTextUpdate);
                    main.stkMsgDate.Children.Add(txtCreatedUpdate);


                    main.stkSchedule.Children.Add(txtTitleSchedule);
                    main.stkSchedule.Children.Add(txtTimeSchedule);
                    main.stkSchedule.Children.Add(txtDescriptionSchedule);
                    main.stkLocationSchedule.Children.Add(txtLocationSchedule);


                    
                }
                catch (Exception)
                {
                    //   throw;
                }
            });
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
            string result = null;
            Dates d = new Dates();

            result = d.parseDate(dt);

            return result;
        }
    }
}

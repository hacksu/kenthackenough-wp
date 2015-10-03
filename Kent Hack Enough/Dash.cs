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


                    RichTextBox txtTextUpdate = new RichTextBox();
                    RichTextBox txtCreatedUpdate = new RichTextBox();

                    RichTextBox txtTitleSchedule = new RichTextBox();
                    RichTextBox txtDescriptionSchedule = new RichTextBox();
                    RichTextBox txtLocationSchedule = new RichTextBox();
                    RichTextBox txtTimeSchedule = new RichTextBox();


                    txtTextUpdate = parseText(settings.LiveFeedSetting.messages[updateIndex]);
                    txtTextUpdate.Margin = new Thickness(5.0);
                    txtTextUpdate.TextWrapping = TextWrapping.Wrap;

                   // txtCreatedUpdate.Text = parseDate(settings.LiveFeedSetting.messages[updateIndex].created);
                    txtCreatedUpdate.HorizontalAlignment = HorizontalAlignment.Right;
                    txtCreatedUpdate.VerticalAlignment = VerticalAlignment.Bottom;
                    txtCreatedUpdate.Margin = new Thickness(3.0);
                    txtCreatedUpdate.FontSize = 13;
                    Grid.SetRow(txtCreatedUpdate, 5);


                   // txtTitleSchedule.Text = settings.EventsSetting.events[scheduleIndex].title;
                    txtTitleSchedule.Margin = new Thickness(5.0);
                    txtTitleSchedule.TextWrapping = TextWrapping.Wrap;
                    

                  //  txtDescriptionSchedule.Text = settings.EventsSetting.events[scheduleIndex].description;
                    txtDescriptionSchedule.Margin = new Thickness(5.0);
                    txtDescriptionSchedule.TextWrapping = TextWrapping.Wrap;

                   // txtLocationSchedule.Text = settings.EventsSetting.events[scheduleIndex].location;
                    txtLocationSchedule.Margin = new Thickness(5.0);
                    txtLocationSchedule.TextWrapping = TextWrapping.Wrap;
                    txtLocationSchedule.HorizontalAlignment = HorizontalAlignment.Right;
                    txtLocationSchedule.VerticalAlignment = VerticalAlignment.Bottom;
                    txtLocationSchedule.FontSize = 13;
                    Grid.SetRow(txtLocationSchedule, 7);

                  //  txtTimeSchedule.Text = settings.EventsSetting.events[scheduleIndex].start.ToString("t") + " - " + settings.EventsSetting.events[scheduleIndex].end.ToString("t");
                    txtTimeSchedule.Margin = new Thickness(5, -3, 0, 0);
                    txtTimeSchedule.FontSize = 13;

                    main.stkUpdates.Children.Add(txtTextUpdate);
                    main.stkUpdates.Children.Add(txtCreatedUpdate);


                    main.stkSchedule.Children.Add(txtTitleSchedule);
                    main.stkSchedule.Children.Add(txtTimeSchedule);
                    main.stkSchedule.Children.Add(txtDescriptionSchedule);
                    main.stkSchedule.Children.Add(txtLocationSchedule);


                    
                }
                catch (Exception)
                {
                    //   throw;
                }
            });
        }

        public RichTextBox parseText(UpdateMessages msg)
        {
            RichTextBox result = new RichTextBox();
            Markdown md = new Markdown();

            result = md.parseMarkdown(msg.text);

            return result;
        }

        public RichTextBox parseDate(DateTime dt)
        {
            RichTextBox result = new RichTextBox();
            Dates d = new Dates();

            result = d.parseDate(dt);

            return result;
        }
    }
}

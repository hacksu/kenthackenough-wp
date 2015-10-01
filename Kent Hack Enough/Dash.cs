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


                    TextBlock txtMsgUpdate = new TextBlock();
                    TextBlock txtDateUpdate = new TextBlock();
                    TextBlock txtMsgSchedule = new TextBlock();
                    TextBlock txtDateSchedule = new TextBlock();


                    txtMsgUpdate = parseText(settings.LiveFeedSetting.messages[updateIndex]);
                    txtMsgUpdate.Margin = new System.Windows.Thickness(5.0);
                    txtMsgUpdate.TextWrapping = TextWrapping.Wrap;
                    txtMsgUpdate.Height = 140;

                    txtDateUpdate.Text = parseDate(settings.LiveFeedSetting.messages[updateIndex].created);
                    txtDateUpdate.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txtDateUpdate.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    txtDateUpdate.Margin = new System.Windows.Thickness(3.0);
                    txtDateUpdate.FontSize = 13;
                    Grid.SetRow(txtDateUpdate, 2);


                    txtMsgSchedule.Text = settings.EventsSetting.events[scheduleIndex].title;
                    txtMsgSchedule.Margin = new System.Windows.Thickness(5.0);
                    txtMsgSchedule.TextWrapping = TextWrapping.Wrap;
                    txtMsgSchedule.Height = 140;

                    txtDateSchedule.Text = parseDate(settings.EventsSetting.events[scheduleIndex].start);
                    txtDateSchedule.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                    txtDateSchedule.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    txtDateSchedule.Margin = new System.Windows.Thickness(3.0);
                    txtDateSchedule.FontSize = 13;

                    Grid.SetRow(txtDateSchedule, 2);

                    main.stkUpdates.Children.Add(txtMsgUpdate);
                    main.stkUpdates.Children.Add(txtDateUpdate);

                    main.stkSchedule.Children.Add(txtMsgSchedule);
                    main.stkSchedule.Children.Add(txtDateSchedule);
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

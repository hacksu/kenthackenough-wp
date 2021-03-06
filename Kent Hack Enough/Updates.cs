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
using System.Windows.Documents;
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

    public class Updates
    {
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();
        
        
        
        public Updates() { }

        Updates(string msgId, string msgTxt, int msgV, DateTime msgCreated)
        {
            RootMessages rootMsg = new RootMessages();
            UpdateMessages liveMsg = new UpdateMessages();
            liveMsg._id = msgId;
            liveMsg.text = msgTxt;
            liveMsg.__v = msgV;
            liveMsg.created = msgCreated;

            rootMsg.messages.Add(liveMsg);
        }

        public void getFeedNow()
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
            Timer = new Timer(TimerCallback, obj, 0, 0);
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

        public RichTextBox parseText(String msg)
        {
            RichTextBox result = new RichTextBox();
            Markdown md = new Markdown();

            if(msg == null)
            {
                return result; 
            }

            result = md.parseMarkdown(msg);

            return result;
        }

        public RichTextBox parseDate(DateTime dt)
        {
            RichTextBox result = new RichTextBox();
            Dates d = new Dates();

            result = d.parseDate(dt);

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

                     for (int i = 0; i <= settings.LiveFeedSetting.messages.Count(); i++)
                     {
                         RichTextBox txtMsg = new RichTextBox();
                         RichTextBox txtDate = new RichTextBox();
                         StackPanel stkContainer = new StackPanel();


                         stkContainer.MinHeight = 50;
                         stkContainer.Background = new SolidColorBrush(Color.FromArgb(125, 255, 0, 0));
                         stkContainer.Margin = new System.Windows.Thickness(5.0);


                         txtMsg = parseText(settings.LiveFeedSetting.messages[i].text);
                         txtMsg.Margin = new System.Windows.Thickness(5.0);
                         txtMsg.TextWrapping = TextWrapping.Wrap;
                         

                         txtDate = parseDate(settings.LiveFeedSetting.messages[i].created);
                         txtDate.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                         txtDate.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                         txtDate.Margin = new System.Windows.Thickness(0);
                         txtDate.FontSize = 13;


                         stkContainer.Children.Add(txtMsg);
                         stkContainer.Children.Add(txtDate);

                         main.UpdatesItems.Children.Add(stkContainer);




                         // Dash Page
                         main.stkUpdates.Children.Clear();

                         Paragraph paraTextUpdate = new Paragraph();
                         Paragraph paraCreatedUpdate = new Paragraph();

                         RichTextBox txtTextUpdate = new RichTextBox();
                         RichTextBox txtCreatedUpdate = new RichTextBox();
   

                         txtTextUpdate = parseText(settings.LiveFeedSetting.messages[0].text);
                         txtTextUpdate.Margin = new Thickness(5.0);
                         txtTextUpdate.TextWrapping = TextWrapping.Wrap;
                         txtTextUpdate.Height = main.stkUpdates.Height - 15;

                         txtCreatedUpdate = parseDate(settings.LiveFeedSetting.messages[0].created);
                         txtCreatedUpdate.HorizontalAlignment = HorizontalAlignment.Right;
                         txtCreatedUpdate.VerticalAlignment = VerticalAlignment.Bottom;
                         txtCreatedUpdate.Margin = new Thickness(3.0);
                         txtCreatedUpdate.FontSize = 13;
                         Grid.SetRow(txtCreatedUpdate, 5);

                         main.stkUpdates.Children.Add(txtTextUpdate);
                         main.stkUpdates.Children.Add(txtCreatedUpdate);



                     }
                 }
                 catch (Exception)
                 {
                    //   throw;
                }
             });
        }



       


        #region WebCleint



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



        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string data = e.Result;

                var results = JsonConvert.DeserializeObject<dynamic>(e.Result);

                RootMessages Result = JsonConvert.DeserializeObject<RootMessages>(e.Result);

                for (int i = 0; i < Result.messages.Count(); i++)
                {
                    Result.messages[i].created = Result.messages[i].created.ToLocalTime();
                }

                settings.LiveFeedSetting = Result;
                settings.Save();

                refreshLiveFeed();
            }
            catch
            {
                
            }

            toggleProg();
            return;
        }



        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Web.Http;
using System.Threading; 
using System.Threading.Tasks; 
using System.Windows.Threading; 
using System.Windows;
using System.Windows.Controls;

namespace Kent_Hack_Enough
{
    class HTTPClient
    {
        const string API_SERVER = "http://api.khe.pdilyard.com/v1.0/";
        HttpWebRequest request;
        System.Threading.Timer Timer;
        private AppSettings settings = new AppSettings();
        


        public void Connect(string server, int port){
            request = (HttpWebRequest)WebRequest.Create(API_SERVER);
        }

        public async Task On()
        {
            await MethodAsync();
        }

        async Task<int> MethodAsync()
        {
            // start the Method task
            var task1 = Task.Run(() => getData());

            // start the timeout task
            var task2 = Task.Delay((Convert.ToInt16(settings.RefreshIntervalSetting * 1000)));

            // either task1 or task2
            //var task = await Task.WhenAny(task1, task2);

            wait Task.Wait(task1);
            while (!task1.IsCompleted)
            {

            }

            throw new TaskCanceledException();


            
        }

        //async Task<Timer> On()
        //{


        //    Timer = new Timer(TimerCallback, null, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
        //    await Task.Run(Timer(TimerCallback, null, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);
        //}

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //Deployment.Current.Dispatcher.BeginInvoke(() =>
            //    {
                    
            //       // main.prog.Visibility = System.Windows.Visibility.Collapsed;
            //    });
            try
            {
                string data = e.Result;

                var results = JsonConvert.DeserializeObject<dynamic>(e.Result);

               RootObject Result = JsonConvert.DeserializeObject<RootObject>(e.Result);
               
               settings.LiveFeedSetting = Result;
               settings.Save();
            }
            catch
            {
                return;
            }
        }

        private void getData()
        {
            //Dispatcher.BeginInvoke(() =>
              //  {
            //        main.prog.Visibility = System.Windows.Visibility.Visible;
              //  });

            WebClient webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);

            webClient.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();
            
            webClient.DownloadStringAsync(new Uri(API_SERVER + "/messages"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Web.Http;

namespace Kent_Hack_Enough
{

    class HTTPClient
    {
        const string API_SERVER = "http://api.khe.pdilyard.com/v1.0/";
        HttpWebRequest request;
        System.Threading.Timer Timer;

        public void Connect(){
            request = (HttpWebRequest)WebRequest.Create(API_SERVER);
        }

        public void On(string query, string result, List<string> headers){

            Timer = new System.Threading.Timer(TimerCallback, null, 0, Convert.ToInt16(settings.RefreshIntervalSetting) * 1000);

            for(int i = 0; i < headers.Count; i++){
                request.Headers = headers[i].ToString();
            }
        }

   void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    prog.IsVisible = false;
                });
            try
            {
                string data = e.Result;

                Dispatcher.BeginInvoke(() =>
                {
                    string[] lines;
                    double temp;

                    lines = Regex.Split(data, "\n");

                    temp = Convert.ToDouble(lines[3]);
                    temp = Math.Round(temp, 0);

                    curLoad1.Text = lines[0] + "%";
                    curLoad5.Text = lines[1] + "%";
                    curLoad15.Text = lines[2] + "%";
                    curRAM.Text = Convert.ToString(temp) + "%";
                    curDisk.Text = lines[4] + "%";
                    curFanStatus.Text = lines[5];

                    if (lines[5] == "ON")
                    {
                        stkLightStatus.Background = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        stkLightStatus.Background = new SolidColorBrush(Colors.DarkGray);
                    }
                    curLightStatus.Text = lines[6];
                    
                });            
            }
            catch
            {
                return;
            }   
        }

        private void TimerCallback(object state)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    prog.IsVisible = true;
                });

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.UtcNow.ToString();

            if (settings.RaspberryPiAddressSetting != "e.x. 192.168.1.14")
            {
                webClient.DownloadStringAsync(new Uri("http://" + settings.RaspberryPiAddressSetting + "/mobile/status.php"));
            }
        }


        NetworkCredential credentials = new NetworkCredential(userName.Text + ":^",password.Text + "=");
        request.Credentials = credentials;

        request.BeginGetResponse(new AsyncCallback(GetSomeResponse), request);
    }
}

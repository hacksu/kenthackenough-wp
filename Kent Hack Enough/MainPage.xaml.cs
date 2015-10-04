using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Kent_Hack_Enough.Resources;
using Newtonsoft.Json;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.Phone.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Kent_Hack_Enough
{  
    public partial class MainPage : PhoneApplicationPage
    {
        private AppSettings settings = new AppSettings();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Check if a Wireless or Data connection exists
            if (checkInternet())
            {

                // Trigers timer refresh
                Updates feed = new Updates();
                feed.getFeed();
                Event events = new Event();
                events.getEvent();
            }
           
        }

        void appBarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void appBarRefresh_Click(object sender, EventArgs e)
        {
            if (checkInternet())
            {
                updateFeed();
                updateSchedule();
            }
        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkInternet())
            {
                updateFeed();
                updateSchedule();
            }
        }

        public void updateFeed()
        {
            Updates feed = new Updates();
            feed.getFeedNow();
        }

        public void updateSchedule()
        {
            Event events = new Event();
            events.getEventNow();
        }

        private bool checkInternet()
        {
            var currentList = new NetworkInterfaceList().Where(i => i.InterfaceState == ConnectState.Connected).Select(i => i.InterfaceSubtype);
            if (currentList.Contains(NetworkInterfaceSubType.WiFi))
            {
                webBrowser.Navigate(new Uri("https://khe.io/about"));
                return true;
            }

            if (currentList.Contains(NetworkInterfaceSubType.Cellular_EVDO) || currentList.Contains(NetworkInterfaceSubType.Cellular_3G) || currentList.Contains(NetworkInterfaceSubType.Cellular_HSPA) || currentList.Contains(NetworkInterfaceSubType.Cellular_LTE) || currentList.Contains(NetworkInterfaceSubType.Cellular_EDGE))
            {
                webBrowser.Navigate(new Uri("https://khe.io/about"));
                return true;
            }
            else
            {
                MessageBox.Show("Please connect to a cellular network or wireless network", "Unable to reach internet", MessageBoxButton.OK);
            }
            return false;
        }

        private void webBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(new Uri("https://khe.io/about"));
        }
    }
}
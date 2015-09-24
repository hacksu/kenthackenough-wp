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
            var currentList = new NetworkInterfaceList().Where(i => i.InterfaceState == ConnectState.Connected).Select(i => i.InterfaceSubtype);
            if (currentList.Contains(NetworkInterfaceSubType.WiFi))
                //Debug.WriteLine("WiFi");
            if (currentList.Intersect(new NetworkInterfaceSubType[]
            {
    NetworkInterfaceSubType.Cellular_EVDO,
    NetworkInterfaceSubType.Cellular_3G,
    NetworkInterfaceSubType.Cellular_HSPA,
    NetworkInterfaceSubType.Cellular_EVDV,
            }).Any())
               // Debug.WriteLine("3G");
            if (currentList.Intersect(new NetworkInterfaceSubType[]
            {
    NetworkInterfaceSubType.Cellular_GPRS,
    NetworkInterfaceSubType.Cellular_1XRTT,
    NetworkInterfaceSubType.Cellular_EDGE,
            }).Any())
               // Debug.WriteLine("2G");

            updateView();
        }

        void appBarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void appBarRefresh_Click(object sender, EventArgs e)
        {
            updateView();
        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateView();
        }

        public void updateView()
        {
            LiveFeed feed = new LiveFeed();
            feed.getFeed();
            Event events = new Event();
            events.getEvent();
        }

        // Application Bar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}


    }
}
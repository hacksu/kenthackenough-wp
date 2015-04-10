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

        // Global Variables
        const int API_PORT = 80;
        const string API_SERVER = "http://api.khe.pdilyard.com/v1.0/";


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            HTTPClient client = new HTTPClient();

            client.Connect(API_SERVER, API_PORT);
            string data = null;
            client.On("/messages", "", data);
            //client.On("/events", "", data);
        }

        void appBarSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void appBarRefresh_Click(object sender, EventArgs e)
        {
            HTTPClient client = new HTTPClient();

            client.Connect(API_SERVER, API_PORT);

           // client.On();
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
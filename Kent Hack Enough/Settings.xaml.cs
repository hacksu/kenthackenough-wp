using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace Kent_Hack_Enough
{
    public partial class Settings : PhoneApplicationPage
    {
       private AppSettings settings = new AppSettings();

        public Settings()
        {
            InitializeComponent();

            sldRefreshInterval.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sldRefreshInterval_ValueChanged);

        }


        public virtual bool? toggleSwitch
        {
            get;
            set;
        }

        /// <summary>
        /// Done button clicked event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void appBarSave_Click(object sender, EventArgs e)
        {
            
            settings.RefreshIntervalSetting = Convert.ToInt32(Math.Round(sldRefreshInterval.Value, 0));

            MainPage reloadPage = new MainPage();

            NavigationService.GoBack();
        }

        private void sldRefreshInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            blkRefreshInterval.Text = Convert.ToString(Math.Round(sldRefreshInterval.Value, 0));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            sldRefreshInterval.Value = Convert.ToDouble(settings.RefreshIntervalSetting);
        }

        private void btnClearCache_Click(object sender, RoutedEventArgs e)
        {
            settings.LiveFeedSetting = null;
        }
    }
}
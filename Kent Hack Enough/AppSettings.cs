/* 
    Copyright (c) 2012 - 2013 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://code.msdn.microsoft.com/wpapps
  
*/

using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;

namespace Kent_Hack_Enough
{
    public class AppSettings
    {

        // Our isolated storage settings
        IsolatedStorageSettings settings;

        // The isolated storage key names of our settings
        const string FirstRunKeyName = "FirstRunDefault";
        const string RefreshIntervalKeyName = "RefreshInterval";
        const string LiveFeedKeyName = "LiveFeed";
        const string EventsKeyName = "Events";
        const string APIPortKeyName = "APIPort";
        const string APIServerKeyName = "APIServer";


        // The default value of our settings
        const bool FirstRunDefault = true;
        const double RefreshIntervalDefault = 5;
        const RootMessages LiveFeedDefault = null;
        const RootEvents EventsDefault = null;
        const int APIPortDefault = 80;
        //const string APIServerDefault = "http://api.khe.pdilyard.com/v1.0/";
        const string APIServerDefault = "https://api.khe.io/v1.0/";

        // Constructor that gets the application settings.
        public AppSettings()
        {
            try
            {
                // Get the settings for this application.
                settings = IsolatedStorageSettings.ApplicationSettings;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }


        // Update a setting value for our application. If the setting does not exist, then add the setting.
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        // Get the current value of the setting, or if it is not found, set the setting to the default setting.
        public valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (valueType)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }


        // Save the settings.
        public void Save()
        {
            settings.Save();
        }

        // Property to get and set FirstRun Setting Key.
        public bool FirstRunSetting
        {
            get
            {
                return GetValueOrDefault<bool>(FirstRunKeyName, FirstRunDefault);
            }
            set
            {
                if (AddOrUpdateValue(FirstRunKeyName, value))
                {
                    Save();
                }
            }
        }

        


        // Property to get and set RefreshInterval Setting Key.
        public double RefreshIntervalSetting
        {
            get
            {
                return GetValueOrDefault<double>(RefreshIntervalKeyName, RefreshIntervalDefault);
            }
            set
            {
                if (AddOrUpdateValue(RefreshIntervalKeyName, value))
                {
                    Save();
                }
            }
        }


        // Property to get and set LiveFeed Setting Key.
        public RootMessages LiveFeedSetting
        {
            get
            {
                return GetValueOrDefault<RootMessages>(LiveFeedKeyName, LiveFeedDefault);
            }
            set
            {
                if (AddOrUpdateValue(LiveFeedKeyName, value))
                {
                    Save();
                }
            }
        }

        // Property to get and set Events Setting Key.
        public RootEvents EventsSetting
        {
            get
            {
                return GetValueOrDefault<RootEvents>(EventsKeyName, EventsDefault);
            }
            set
            {
                if (AddOrUpdateValue(EventsKeyName, value))
                {
                    Save();
                }
            }
        }


        // Property to get and set the API Port Setting Key.
        public int APIPortSetting
        {
            get
            {
                return GetValueOrDefault<int>(APIPortKeyName, APIPortDefault);
            }
            set
            {
                if (AddOrUpdateValue(APIPortKeyName, value))
                {
                    Save();
                }
            }
        }


        // Property to get and set the API Server Setting Key.
        public string APIServerSetting
        {
            get
            {
                return GetValueOrDefault<string>(APIServerKeyName, APIServerDefault);
            }
            set
            {
                if (AddOrUpdateValue(APIServerKeyName, value))
                {
                    Save();
                }
            }
        }
    }
}
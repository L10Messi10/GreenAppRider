using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GreenAppRider.Utils
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string LastEmailSettings = "last_email_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string LastUsedEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastEmailSettings, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastEmailSettings, value);
            }
        }
    }
}

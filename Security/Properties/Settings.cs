using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Properties {
    partial class Settings
    {
        [global::System.Configuration.UserScopedSettingAttribute()]
        public string ConnectionString
        {
            get { return (string)this["ConnectionString"]; }
            set { this["ConnectionString"] = value; }
        }


        public Settings()
        {
            this.PropertyChanged +=
              new System.ComponentModel.PropertyChangedEventHandler(this.Settings_PropertyChanged);
            this.SettingsLoaded +=
              new System.Configuration.SettingsLoadedEventHandler(this.Settings_SettingsLoaded);
        }


        private void Settings_PropertyChanged(System.Object sender,
         System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConnectionString")
            {
                this["DetroitConnectionString"] = this.ConnectionString;
            }
        }

        private void Settings_SettingsLoaded(System.Object sender,
                System.Configuration.SettingsLoadedEventArgs e)
        {
            // Advanced codes for post loading process...
        }

        public static void ChangeConnectionString(string connectionstring)
        {
            global::Security.Properties.Settings.Default.ConnectionString = 
                connectionstring;
        }
    }
}

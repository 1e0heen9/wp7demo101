using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace WindowsPhoneApp
{
    public class Setting<T>
    {
        public String key { get; set; }
        private T value;
        public T defaultValue { get; private set; }
        private bool hasValue;
        public Setting(String key,T defaultValue){
            this.key = key;
            this.defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                if (hasValue) { return value; }
                if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value))
                {
                    value = defaultValue;
                    IsolatedStorageSettings.ApplicationSettings[key] = value;
                }
                hasValue = true;
                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
                this.value = value;
                hasValue = true;
            }
        }

        public void ForceFresh() {
            hasValue = false;
        }

        
    }
}

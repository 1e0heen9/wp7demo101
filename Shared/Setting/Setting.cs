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
        public string key { get; private set; }
        private T value,defaultValue;
        private bool hasValue;

        public T Value
        {
            get
            {
                if (hasValue) return value;
                if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(key, out value))
                {
                    IsolatedStorageSettings.ApplicationSettings[key] = defaultValue;
                    value = defaultValue;
                    hasValue = true;
                }
                return value;
            }
            set
            {
                this.value = value;
                IsolatedStorageSettings.ApplicationSettings[key] = value;
                hasValue = true;
            }
        }

        public Setting(string key, T defaultValue) {
            this.key = key;
            this.value = defaultValue;
        }
    }
}

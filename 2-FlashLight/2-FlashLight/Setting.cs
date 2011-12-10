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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TradingSystem
{
    public class MyConfig
    {
        private static readonly Lazy<MyConfig> instanceLazy = new Lazy<MyConfig>(() => new MyConfig(), true);
        private static Dictionary<string, string> appSettings;

        private MyConfig()
        {
            
        }
        public static MyConfig Instance => instanceLazy.Value;

        public static T Get<T>(string key)
        {
            appSettings = new Dictionary<string, string>();
            appSettings.Add("ExternalSystemsURL", "https://cs-bgu-wsep.herokuapp.com/");
            var converter = TypeDescriptor.GetConverter(typeof(T));
            appSettings.TryGetValue(key, out string k);
            return (T)(converter.ConvertFromInvariantString(k));
        }
    }
}

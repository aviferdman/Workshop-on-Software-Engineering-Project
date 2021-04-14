using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business
{
    public class Logger
    {
        private IList<String> activities;
        private static readonly Lazy<Logger>
        _lazy =
        new Lazy<Logger>
            (() => new Logger());

        private Logger()
        {
            this.activities = new List<String>();
        }

        public static Logger Instance { get { return _lazy.Value; } }
        public void MonitorActivity(String activity)
        {
            this.activities.Add(activity);
        }
    }
}

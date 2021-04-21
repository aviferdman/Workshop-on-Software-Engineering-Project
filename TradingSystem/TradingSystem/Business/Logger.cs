using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business
{
    public class Logger
    {
        private IList<String> activities;
        private IList<String> errors;
        private static readonly Lazy<Logger>
        _lazy =
        new Lazy<Logger>
            (() => new Logger());

        private Logger()
        {
            this.Activities = new List<String>();
            this.Errors = new List<String>();
        }

        public static Logger Instance { get { return _lazy.Value; } }

        public IList<string> Activities { get => activities; set => activities = value; }
        public IList<string> Errors { get => errors; set => errors = value; }

        public void MonitorActivity(String activity)
        {
            this.Activities.Add(activity);
        }

        public void MonitorError(String error)
        {
            this.Errors.Add(error);
        }

        public void CleanLogs()
        {
            this.Activities = new List<String>();
            this.Errors = new List<String>();
        }
    }
}

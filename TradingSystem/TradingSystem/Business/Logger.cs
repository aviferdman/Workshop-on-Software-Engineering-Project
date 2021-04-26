using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;

namespace TradingSystem.Business
{
    public class Logger
    {
        private IList<String> activities;
        private IList<String> errors;
        private bool writeToFile;
        private static readonly Lazy<Logger>
        _lazy =
        new Lazy<Logger>
            (() => new Logger());

        private Logger()
        {
            this.Activities = new List<String>();
            this.Errors = new List<String>();
            this.writeToFile = ConfigurationManager.AppSettings.Get("EnableLoggerWriteToFile").Equals("true");
        }

        public static Logger Instance { get { return _lazy.Value; } }

        public IList<string> Activities { get => activities; set => activities = value; }
        public IList<string> Errors { get => errors; set => errors = value; }

        public void MonitorActivity(String activity)
        {
            this.Activities.Add(activity);
            Write(activity);
        }

        public void MonitorError(String error)
        {
            this.Errors.Add(error);
            Write(error);
        }

        public void CleanLogs()
        {
            this.Activities = new List<String>();
            this.Errors = new List<String>();
        }

        public void Write(string message)
        {
            if (this.writeToFile)
            {
                File.WriteAllTextAsync("LoggerMessages.txt", message);
            }
        }
    }
}

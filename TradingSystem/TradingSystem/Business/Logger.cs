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
            string enabled = ConfigurationManager.AppSettings["EnableLoggerWriteToFile"];
            this.writeToFile = enabled != null ? enabled.Equals("true") : false;
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
            message += $"{message} {DateTime.Now}";
            if (this.writeToFile)
            {
                var path = ConfigurationManager.AppSettings["LoggerFilePath"];
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(message);
                    }
                }

                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
                // File.WriteAllTextAsync("LoggerMessages.txt", message);
            }
        }
    }
}

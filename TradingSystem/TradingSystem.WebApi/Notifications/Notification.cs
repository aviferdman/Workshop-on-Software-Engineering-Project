using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.Notifications
{
    public class Notification
    {
        public string Kind { get; set; }
        public string? Content { get; set; }
    }
}

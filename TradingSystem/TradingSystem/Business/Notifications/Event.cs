using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Notifications
{
    public enum EventType
    {
        RegisterEvent,
        OpenStoreEvent,
        PurchaseEvent,
        BecomeManagerEvent,
        RemovedBeingManagerEvent
    }
    public class Event
    {
        public string EventProviderName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public Event(string _eventProviderName, string _description, DateTime _date)
        {
            EventProviderName = _eventProviderName;
            Description = _description;
            Date = _date;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Notifications;
using TradingSystem.Service;

namespace TradingSystem.Business.Notifications
{
    public class Publisher
    {
        private IDictionary<EventType, TypedPublisher> publishers;
        private IDictionary<EventType, IList<string>> waiting;
        private String _username;

        public IDictionary<EventType, IList<string>> Waiting { get => waiting; set => waiting = value; }

        public Publisher(String username)
        {
            this._username = username;
            publishers = new Dictionary<EventType, TypedPublisher>()
            {
                {EventType.BecomeManagerEvent, new TypedPublisher(nameof(EventType.BecomeManagerEvent))},
                {EventType.OpenStoreEvent, new TypedPublisher(nameof(EventType.OpenStoreEvent))},
                {EventType.PurchaseEvent, new TypedPublisher(nameof(EventType.PurchaseEvent))},
                {EventType.RemoveAppointment, new TypedPublisher(nameof(EventType.RemoveAppointment))}
            };
            Waiting = new Dictionary<EventType, IList<string>>()
            {
                {EventType.BecomeManagerEvent, new List<string>()},
                {EventType.OpenStoreEvent, new List<string>()},
                {EventType.PurchaseEvent, new List<string>()},
                {EventType.RemoveAppointment, new List<string>()}
            };
        }

        // Notify observers when event occurs
        public void EventNotification(EventType eventType, string description)
        {
            if (UserService.Instance.isLoggedIn(_username))
            {
                publishers[eventType].EventNotification(description);
            }
            else
            {
                Waiting[eventType].Add(description);
            }
        }

        public void BecomeLoggedIn()
        {
            foreach(EventType key in Waiting.Keys)
            {
                foreach(string description in Waiting[key])
                {
                    publishers[key].EventNotification(description);
                }
                Waiting[key].Clear();
            }
        }

        public TypedPublisher Get(EventType eventType)
        {
            return publishers[eventType];
        }
    }
}

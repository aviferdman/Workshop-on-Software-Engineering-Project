using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Notifications;
using TradingSystem.Notifications;

namespace TradingSystem.PublisherComponent
{
    public class PublisherManagement
    {
        private IDictionary<String, Publisher> username_publisher;
        private IDictionary<String, ICollection<NotificationSubscriber>> username_subscribers;
        private bool _testMode;
        private static readonly Lazy<PublisherManagement>
        lazy =
        new Lazy<PublisherManagement>
            (() => new PublisherManagement());

        public static PublisherManagement Instance { get { return lazy.Value; } }
        private PublisherManagement()
        {
            this._testMode = false;
            this.username_publisher = new Dictionary<String, Publisher>();
            this.username_subscribers = new Dictionary<String, ICollection<NotificationSubscriber>>();
        }
        public void EventNotification(String username, EventType eventType, string description)
        {
            SetIfNotExists(username);
            username_publisher[username].EventNotification(eventType, description);
        }

        public void BecomeLoggedIn(String username)
        {
            SetIfNotExists(username);
            username_publisher[username].BecomeLoggedIn();
        }

        public NotificationSubscriber FindSubscriber(String username, EventType eventType)
        {
            SetIfNotExists(username);
            var subscribers = username_subscribers[username];
            return subscribers.Where(s => s.SubscriberName.Equals(eventType.ToString())).FirstOrDefault();
        
        }

        public Publisher FindPublisher(String username)
        {
            SetIfNotExists(username);
            var publisher = username_publisher[username];
            return publisher;
        }

        private void SetIfNotExists(String username)
        {
            if (!username_publisher.Keys.Contains(username))
            {
                username_publisher.Add(username, new Publisher(username));

                if (!username_subscribers.Keys.Contains(username))
                {
                    username_subscribers.Add(username, new HashSet<NotificationSubscriber>());
                    InitNotifications(username);
                }
            }
        }

        private void InitNotifications(String username)
        {
            var publisher = username_publisher[username];
            var subscribers = username_subscribers[username];
            NotificationSubscriber subscriber1 = new NotificationSubscriber(nameof(EventType.RemoveAppointment), _testMode);
            subscriber1.Subscribe(publisher.Get(EventType.RemoveAppointment));
            subscribers.Add(subscriber1);
            NotificationSubscriber subscriber2 = new NotificationSubscriber(nameof(EventType.OpenStoreEvent), _testMode);
            subscriber2.Subscribe(publisher.Get(EventType.OpenStoreEvent));
            subscribers.Add(subscriber2);
            NotificationSubscriber subscriber3 = new NotificationSubscriber(nameof(EventType.PurchaseEvent), _testMode);
            subscriber3.Subscribe(publisher.Get(EventType.PurchaseEvent));
            subscribers.Add(subscriber3);
            NotificationSubscriber subscriber4 = new NotificationSubscriber(nameof(EventType.BecomeManagerEvent), _testMode);
            subscriber4.Subscribe(publisher.Get(EventType.BecomeManagerEvent));
            subscribers.Add(subscriber4);
        }

        public void Subscribe(String username, NotificationSubscriber subscriber, EventType ev){
            var publisher = username_publisher[username];
            subscriber.Subscribe(publisher.Get(ev));
            username_subscribers[username].Add(subscriber);
        }

        public void DeleteAll()
        {
            this.username_publisher = new Dictionary<String, Publisher>();
            this.username_subscribers = new Dictionary<String, ICollection<NotificationSubscriber>>();
        }

        public void SetTestMode(bool testMode = false)
        {
            this._testMode = testMode;
        }

    }
}

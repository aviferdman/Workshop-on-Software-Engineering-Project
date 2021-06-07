using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Notifications
{
    public class NotificationSubscriber : IObserver<Event>
    {
        public string SubscriberName { get; private set; }
        public IList<string> Messages { get => messages; set => messages = value; }
        public bool TestMode { get => testMode; set => testMode = value; }

        private IDisposable _unsubscriber;
        private bool testMode;
        private IList<String> messages;

        public NotificationSubscriber(string _subscriberName, bool testMode = false)
        {
            SubscriberName = _subscriberName;
            this.TestMode = testMode;
            this.Messages = new List<String>();
        }

        public virtual void Subscribe(IDistinctObservable<Event> provider, EventType ev)
        {
            // Subscribe to the Observable
            if (provider != null)
                _unsubscriber = provider.Subscribe(this, ev);
        }

        public virtual void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public virtual void OnError(Exception e)
        {
            throw new NotImplementedException();
        }

        public virtual void OnNext(Event ev)
        {
            var message = $"Hey {SubscriberName} -> you received {ev.EventProviderName} {ev.Description} @ {ev.Date} ";
            
            if (TestMode)
            {
                messages.Add(message);
            }
        }

        public void MyCallback()
        {

        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}

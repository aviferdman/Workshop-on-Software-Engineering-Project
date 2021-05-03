using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace TradingSystem.Notifications
{
    public class NotificationSubscriber : IObserver<Event>
    {
        public string SubscriberName { get; private set; }
        public IList<string> Messages { get => messages; set => messages = value; }
        public bool SaveNotificationsMode { get => saveNotificationsMode; set => saveNotificationsMode = value; }

        private IDisposable _unsubscriber;
        private bool saveNotificationsMode;
        private IList<String> messages;

        public NotificationSubscriber(string _subscriberName, bool saveNotificationsMode = false)
        {
            SubscriberName = _subscriberName;
            this.SaveNotificationsMode = saveNotificationsMode;
            this.Messages = new List<String>();
        }

        public virtual void Subscribe(IObservable<Event> provider)
        {
            // Subscribe to the Observable
            if (provider != null)
                _unsubscriber = provider.Subscribe(this);
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
            
            if (SaveNotificationsMode)
            {
                messages.Add(message);
            }
            //Send message to communication HERE:

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

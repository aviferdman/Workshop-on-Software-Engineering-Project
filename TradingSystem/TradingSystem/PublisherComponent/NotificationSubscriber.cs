using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TradingSystem.Communication;

namespace TradingSystem.Notifications
{
    public class NotificationSubscriber : IObserver<Event>
    {
        public string SubscriberName { get; private set; }
        public IList<string> Messages { get => messages; set => messages = value; }
        public ICommunicate Communicate { get => communicate; set => communicate = value; }
        public bool TestMode { get => testMode; set => testMode = value; }

        private IDisposable _unsubscriber;
        private ICommunicate communicate;
        private bool testMode;
        private IList<String> messages;

        public NotificationSubscriber(string _subscriberName, bool testMode = false)
        {
            SubscriberName = _subscriberName;
            Communicate = new Communicate();
            this.TestMode = testMode;
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
            Communicate.SendMessage(SubscriberName, $"DONE");
        }

        public virtual void OnError(Exception e)
        {
            Communicate.SendMessage(SubscriberName, $"{e.Message}");
        }

        public virtual void OnNext(Event ev)
        {
            var message = $"Hey {SubscriberName} -> you received {ev.EventProviderName} {ev.Description} @ {ev.Date} ";
            if (ConfigurationManager.AppSettings.Get("ConnectRealCommunication") != null &&
                ConfigurationManager.AppSettings.Get("ConnectRealCommunication").Equals("true"))
            {
                Communicate.SendMessage(SubscriberName, message);
            }
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

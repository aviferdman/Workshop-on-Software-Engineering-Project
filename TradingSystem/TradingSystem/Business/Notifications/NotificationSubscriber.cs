using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Communication;

namespace TradingSystem.Notifications
{
    public class NotificationSubscriber : IObserver<Event>
    {
        public string SubscriberName { get; private set; }
        private IDisposable _unsubscriber;
        private ICommunicate communicate;
        private User _user;

        public NotificationSubscriber(User user, string _subscriberName)
        {
            SubscriberName = _subscriberName;
            communicate = new Communicate();
            this._user = user;
        }

        public virtual void Subscribe(IObservable<Event> provider)
        {
            // Subscribe to the Observable
            if (provider != null)
                _unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            communicate.SendMessage(_user.Username, $"DONE");
        }

        public virtual void OnError(Exception e)
        {
            communicate.SendMessage(_user.Username, $"{e.Message}");
        }

        public virtual void OnNext(Event ev)
        {
            var message = $"Hey {_user.Username} -> you received {ev.EventProviderName} {ev.Description} @ {ev.Date} ";
            communicate.SendMessage(_user.Username, message);
        }

        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}

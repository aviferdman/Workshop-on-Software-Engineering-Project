using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using TradingSystem.Service;

namespace TradingSystem.Business.Notifications
{
    public class Publisher : IDistinctObservable<Event>
    {
        public IDictionary<EventType, List<IObserver<Event>>> Observers { get => _observers; set => _observers = value; }
        public IDictionary<EventType, List<Event>> Waiting { get => waiting; set => waiting = value; }
        public bool LoggedIn { get => loggedIn; set => loggedIn = value; }

        private IDictionary<EventType, List<Event>> waiting;
        private String _username;
        private bool loggedIn;
        // Maintain a list of observers
        private IDictionary<EventType, List<IObserver<Event>>> _observers;

        public Publisher(String username)
        {
            this._username = username;
            Observers = new Dictionary<EventType, List<IObserver<Event>>>();
            Waiting = new Dictionary<EventType, List<Event>>();
            LoggedIn = false;
        }
        private class Unsubscriber : IDisposable
        {

            private List<IObserver<Event>> _observers;
            private IObserver<Event> _observer;

            public Unsubscriber(List<IObserver<Event>> observers,
                                IObserver<Event> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

        // Define Subscribe method
        public IDisposable Subscribe(IObserver<Event> observer, EventType ev)
        {
            if (!Observers.Keys.Contains(ev))
            {
                Observers.Add(ev, new List<IObserver<Event>>() { observer } );
            }
            else if (!Observers[ev].Contains(observer))
            {
                Observers[ev].Add(observer);
            }
            return new Unsubscriber(Observers[ev], observer);
        }

        // Notify observers when event occurs
        public void EventNotification(EventType eventType, string description)
        {
            if (loggedIn)
            {
                foreach (var observer in Observers[eventType])
                {
                    observer.OnNext(new Event(eventType.ToString(), description,
                                    DateTime.Now));
                }
            }
            else
            {
                if (!Waiting.Keys.Contains(eventType))
                {
                    Waiting.Add(eventType, new List<Event>());
                }
                Waiting[eventType].Add(new Event(eventType.ToString(), description, DateTime.Now));
            }
        }

        public void BecomeLoggedIn()
        {
            loggedIn = true;
            foreach(var eventType in Waiting.Keys)
            {
                foreach (var ev in Waiting[eventType])
                {
                    foreach (var observer in Observers[eventType])
                    {
                        observer.OnNext(ev);
                    }
                }
            }
            Waiting.Clear();
        }

        public void BecomeLoggedOut()
        {
            loggedIn = false;
        }
    }
}

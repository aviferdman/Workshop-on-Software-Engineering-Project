using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Notifications;
using TradingSystem.Service;

namespace TradingSystem.Business.Notifications
{
    public class Publisher : IObservable<Event>
    {
        public List<IObserver<Event>> Observers { get => _observers; set => _observers = value; }
        public IList<Event> Waiting { get => waiting; set => waiting = value; }

        private IList<Event> waiting;
        private String _username;
        // Maintain a list of observers
        private List<IObserver<Event>> _observers;

        public Publisher(String username)
        {
            this._username = username;
            Observers = new List<IObserver<Event>>();
            Waiting = new List<Event>();
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
        public IDisposable Subscribe(IObserver<Event> observer)
        {
            if (!Observers.Contains(observer))
                Observers.Add(observer);
            return new Unsubscriber(Observers, observer);
        }
        // Notify observers when event occurs
        public void EventNotification(EventType eventType, string description)
        {
            if (UserService.Instance.isLoggedIn(_username))
            {
                foreach (var observer in Observers)
                {
                    observer.OnNext(new Event(eventType.ToString(), description,
                                    DateTime.Now));
                }
            }
            else
            {
                Waiting.Add(new Event(eventType.ToString(), description, DateTime.Now));
            }
        }

        public void BecomeLoggedIn()
        {
            foreach(var e in Waiting)
            {
                foreach (var observer in Observers)
                {
                    observer.OnNext(e);
                }
               
            }
            Waiting.Clear();
        }
    }
}

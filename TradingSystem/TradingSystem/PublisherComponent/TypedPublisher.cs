using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Notifications
{
    public class TypedPublisher : IObservable<Event>
    {
        public string ProviderName { get; private set; }
        public List<IObserver<Event>> Observers { get => _observers; set => _observers = value; }

        // Maintain a list of observers
        private List<IObserver<Event>> _observers;

        public TypedPublisher(string _providerName)
        {
            ProviderName = _providerName;
            Observers = new List<IObserver<Event>>();
        }

        // Define Unsubscriber class
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
        public void EventNotification(string description)
        {
            foreach (var observer in Observers)
            {
                observer.OnNext(new Event(ProviderName, description,
                                DateTime.Now));
            }
        }
    }
}

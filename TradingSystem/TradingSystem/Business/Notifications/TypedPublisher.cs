using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Notifications
{
    public class TypedPublisher : IObservable<Event>
    {
        public string ProviderName { get; private set; }
        // Maintain a list of observers
        private List<IObserver<Event>> _observers;

        public TypedPublisher(string _providerName)
        {
            ProviderName = _providerName;
            _observers = new List<IObserver<Event>>();
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
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        // Notify observers when event occurs
        public void EventNotification(string description)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new Event(ProviderName, description,
                                DateTime.Now));
            }
        }
    }
}

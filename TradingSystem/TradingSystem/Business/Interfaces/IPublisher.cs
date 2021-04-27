using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Notifications;

namespace TradingSystem.Business.Interfaces
{
    public interface IPublisher<T>
    {
        public IDisposable Subscribe(IObserver<T> observer);
        public void EventNotification(string description);
    }
}

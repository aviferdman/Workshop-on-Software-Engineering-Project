using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Notifications;

namespace TradingSystem.PublisherComponent
{
    public interface IDistinctObservable<T>
    {
        //
        // Summary:
        //     Notifies the provider that an observer is to receive notifications.
        //
        // Parameters:
        //   observer:
        //     The object that is to receive notifications.
        //
        // Returns:
        //     A reference to an interface that allows observers to stop receiving notifications
        //     before the provider has finished sending them.
        IDisposable Subscribe(IObserver<T> observer, EventType eventType);
    }
}

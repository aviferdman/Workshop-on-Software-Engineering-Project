using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Notifications;

namespace AcceptanceTests.AppInterface.Data
{
    class ATSubscriber : NotificationSubscriber
    {
        List<Event> eventsReceived;
        public ATSubscriber(string _subscriberName, bool testMode = false) : base(_subscriberName, testMode)
        {
            eventsReceived = new List<Event>();
        }

        public override void OnNext(Event ev)
        {
            eventsReceived.Add(ev);
        }
    }
}

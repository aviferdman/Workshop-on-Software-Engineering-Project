using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Notifications;

namespace AcceptanceTests.AppInterface.Data
{
    class ATSubscriber : NotificationSubscriber
    {
        Queue<Event> eventsReceived;
        public ATSubscriber(string _subscriberName, bool testMode = false) : base(_subscriberName, testMode)
        {
            eventsReceived = new Queue<Event>();
        }

        public Queue<Event> EventsReceived { get => eventsReceived; set => eventsReceived = value; }

        public override void OnNext(Event ev)
        {
            eventsReceived.Enqueue(ev);
        }
    }
}

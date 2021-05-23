using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.WebApi.Controllers
{
    public class LoggedInController
    {
        private static readonly Lazy<LoggedInController> instanceLazy = new Lazy<LoggedInController>(() => new LoggedInController(), true);

        private ConcurrentDictionary<String, WebSocket> userWS;

        private LoggedInController()
        {
            this.userWS = new ConcurrentDictionary<string, WebSocket>();
        }

        public static LoggedInController Instance => instanceLazy.Value;

        public void addClient(String username, WebSocket socket)
        {
            userWS.TryAdd(username, socket);
            Subscriber subPurchaseEvent = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subPurchaseEvent, EventType.PurchaseEvent);
            // TODO: fix, subscribers get all messages regardless the event type they registered to
            //Subscriber subAddAppointmentEvent = new Subscriber(username, socket, false);
            //PublisherManagement.Instance.Subscribe(username, subAddAppointmentEvent, EventType.AddAppointmentEvent);
            //Subscriber subRemoveAppointment = new Subscriber(username, socket, false);
            //PublisherManagement.Instance.Subscribe(username, subRemoveAppointment, EventType.RemoveAppointment);
        }

        public void RemoveClient(String username)
        {
            _ = userWS.TryRemove(username, out _);
            PublisherManagement.Instance.Unsbscribe(username);
        }
    }
}

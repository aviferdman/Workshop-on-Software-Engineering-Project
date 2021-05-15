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
        private ConcurrentDictionary<String, List<Subscriber>> userSubs;

        private LoggedInController()
        {
            this.userWS = new ConcurrentDictionary<string, WebSocket>();
            this.userSubs = new ConcurrentDictionary<string, List<Subscriber>>();
        }

        public static LoggedInController Instance => instanceLazy.Value;

        public void addClient(String username, WebSocket socket)
        {
            userWS.TryAdd(username, socket);
            Subscriber subOpenStore = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subOpenStore, EventType.OpenStoreEvent);
            Subscriber subPurchaseEvent = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subPurchaseEvent, EventType.PurchaseEvent);
            Subscriber subAddAppointmentEvent = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subAddAppointmentEvent, EventType.AddAppointmentEvent);
            Subscriber subRemoveAppointment = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subRemoveAppointment, EventType.RemoveAppointment);
            Subscriber subRequestPurchaseEvent = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subRequestPurchaseEvent, EventType.RequestPurchaseEvent);

            List<Subscriber> subscribers = new List<Subscriber>();
            subscribers.Add(subOpenStore);
            subscribers.Add(subPurchaseEvent);
            subscribers.Add(subAddAppointmentEvent);
            subscribers.Add(subRemoveAppointment);
            subscribers.Add(subRequestPurchaseEvent);
        }

        public void RemoveClient(String username)
        {
            //userSubs.TryGetValue(username, out List<Subscriber> subscribers);
            throw new NotImplementedException();
        }
    }
}

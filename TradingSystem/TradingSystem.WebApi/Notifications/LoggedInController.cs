using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;

using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using TradingSystem.WebApi.Notifications;

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
            Subscriber subAddAppointmentEvent = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subAddAppointmentEvent, EventType.AddAppointmentEvent);
            Subscriber subRemoveAppointment = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subRemoveAppointment, EventType.RemoveAppointment);
            Subscriber subRequestPurchase = new Subscriber(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subRequestPurchase, EventType.RequestPurchaseEvent);
            // for admins stats:
            var subRequestStats = new SubscriberStatistics(username, socket, false);
            PublisherManagement.Instance.Subscribe(username, subRequestStats, EventType.Stats);

            PublisherManagement.Instance.BecomeLoggedIn(username);
        }

        public void RemoveClient(String username)
        {
            _ = userWS.TryRemove(username, out _);
            PublisherManagement.Instance.Unsbscribe(username);
            PublisherManagement.Instance.BecomeLoggedOut(username);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingSystem.Notifications;

namespace TradingSystem.WebApi
{
    public class Subscriber : NotificationSubscriber
    {
        WebSocket socket;
        public Subscriber(string _subscriberName, WebSocket socket, bool testMode = false) : base(_subscriberName, testMode)
        {
            this.socket = socket;
        }
        public virtual void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public virtual void OnError(Exception e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(e.Message);

            socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public virtual void OnNext(Event ev)
        {
            var message = $"Hey {SubscriberName} -> you received {ev.EventProviderName} {ev.Description} @ {ev.Date} ";
            byte[] buffer = Encoding.ASCII.GetBytes(message);

            socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

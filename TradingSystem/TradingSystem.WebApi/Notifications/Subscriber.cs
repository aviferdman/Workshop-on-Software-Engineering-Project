using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

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
        public override void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public override void OnError(Exception e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(e.Message);
            socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public override void OnNext(Event ev)
        {
            // TODO: fix when publisher sends event in better way
            //string message = $"Hey {SubscriberName} -> ";
            //bool isNow = DateTime.Now - ev.Date < TimeSpan.FromSeconds(10);
            //string dateStr;
            //if (isNow)
            //{
            //    dateStr = " right now!";
            //}
            //else
            //{
            //    dateStr = $" @ {ev.Date}";
            //}

            //string eventType = ev.EventProviderName;
            //if (eventType == EventType.RequestPurchaseEvent.ToString())
            //{
            //    message = ev.Description;
            //}
            //else if (eventType == EventType.OpenStoreEvent.ToString())
            //{
            //    return;
            //}
            //else
            //{
            //    message = $"you received {ev.EventProviderName}";
            //}
            //message += dateStr;

            // TODO: remove this later
            if (ev.EventProviderName == EventType.OpenStoreEvent.ToString())
            {
                return;
            }

            string message = $"Hey {SubscriberName}, {ev.Description} @ {ev.Date} ";
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}

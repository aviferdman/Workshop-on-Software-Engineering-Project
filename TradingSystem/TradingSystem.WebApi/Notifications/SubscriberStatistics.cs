using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

using Newtonsoft.Json;

using TradingSystem.Notifications;

namespace TradingSystem.WebApi.Notifications
{
    public class SubscriberStatistics : WebApiSubsriberBase
    {
        public SubscriberStatistics(string subscriberName, WebSocket socket, bool testMode) : base(socket, subscriberName, testMode)
        {
            Socket = socket;
        }

        public WebSocket Socket { get; }

        public override void OnNext(Event ev)
        {
            SendNotificationAsync(new Notification
            {
                Kind = NotificationKind.Statistics.ToString(),
                Content = null,
            });
        }
    }
}

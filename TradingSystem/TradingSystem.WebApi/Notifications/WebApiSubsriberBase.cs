using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using TradingSystem.Notifications;

namespace TradingSystem.WebApi.Notifications
{
    public class WebApiSubsriberBase : NotificationSubscriber
    {
        public WebApiSubsriberBase(WebSocket socket, string subscriberName, bool testMode) : base(subscriberName, testMode)
        {
            Socket = socket;
        }

        public WebSocket Socket { get; }

        public async Task SendNotificationAsync(Notification notification)
        {
            await SendNotificationAsync(notification, CancellationToken.None);
        }
        public async Task SendNotificationAsync(Notification notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            string json = JsonConvert.SerializeObject(notification, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            });
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            await Socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}

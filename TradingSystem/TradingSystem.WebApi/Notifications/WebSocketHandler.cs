using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using TradingSystem.WebApi.Controllers;

namespace TradingSystem.WebApi
{
    public class WebSocketHandler
    {
        public WebSocketHandler(HttpContext context, WebSocket ws, TaskCompletionSource<object> tcs)
        {
            Context = context;
            WebSocket = ws;
            TaskCompletionSource = tcs;
        }

        public HttpContext Context { get; }
        public WebSocket WebSocket { get; }
        public TaskCompletionSource<object> TaskCompletionSource { get; }

        public async Task OnAccept()
        {
            var usernameBuffer = new byte[256];
            WebSocketReceiveResult result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(usernameBuffer), CancellationToken.None);
            string username = Encoding.UTF8.GetString(usernameBuffer, 0, result.Count);
            LoggedInController.Instance.addClient(username, WebSocket);
            while (!WebSocket.CloseStatus.HasValue)
            {
                byte[]? buffer = new byte[1024];
                _ = await WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            //in logout
            //await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            //LoggedInController.Instance.RemoveClient(BitConverter.ToString(username));
        }
    }
}

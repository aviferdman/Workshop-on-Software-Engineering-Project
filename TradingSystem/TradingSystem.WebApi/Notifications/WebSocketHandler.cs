using System;
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
        public WebSocketHandler(CancellationToken cancellationToken, HttpContext context, WebSocket ws)
        {
            CancellationToken = cancellationToken;
            Context = context;
            WebSocket = ws;
        }

        public CancellationToken CancellationToken { get; }
        public HttpContext Context { get; }
        public WebSocket WebSocket { get; }

        public async Task OnAccept()
        {
            byte[]? usernameBuffer = new byte[256];
            WebSocketReceiveResult result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(usernameBuffer), CancellationToken.None);
            string username = Encoding.UTF8.GetString(usernameBuffer, 0, result.Count);
            LoggedInController.Instance.addClient(username, WebSocket);
            while (!WebSocket.CloseStatus.HasValue)
            {
                byte[]? buffer = new byte[1024];
                try
                {
                    _ = await WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (WebSocketException e) when (e.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
                {
                    break;
                }
            }
            LoggedInController.Instance.RemoveClient(username);
        }
    }
}

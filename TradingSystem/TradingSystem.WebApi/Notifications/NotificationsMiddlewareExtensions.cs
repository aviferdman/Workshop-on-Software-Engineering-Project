using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace TradingSystem.WebApi.Notifications
{
    public static class NotificationsMiddlewareExtensions
    {
        private static async Task Middleware(HttpContext context, Func<Task> next)
        {
            if (context.Request.Path == "/login")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socketFinishedTcs = new TaskCompletionSource<object>();
                    using WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                    await new WebSocketHandler(context, ws, socketFinishedTcs).OnAccept();
                    //_ = await socketFinishedTcs.Task;
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await next();
            }
        }

        public static IApplicationBuilder UseWebSocketNotifications(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.Use(Middleware);
        }
    }
}

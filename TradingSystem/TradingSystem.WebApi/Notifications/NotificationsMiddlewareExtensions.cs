using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace TradingSystem.WebApi.Notifications
{
    public static class NotificationsMiddlewareExtensions
    {
        private class NotificationsMiddleware
        {
            public NotificationsMiddleware(RequestDelegate next, IHostApplicationLifetime applicationLifetime)
            {
                Next = next;
                ApplicationLifetime = applicationLifetime;
            }

            public RequestDelegate Next { get; }
            public IHostApplicationLifetime ApplicationLifetime { get; }

            public async Task InvokeAsync(HttpContext context)
            {
                if (context.Request.Path == "/login")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                        var cts = new CancellationTokenSource();
                        using CancellationTokenRegistration cancellationRegisteration = ApplicationLifetime.ApplicationStopping.Register(() =>
                        {
                            cts.Cancel();
                        });
                        await new WebSocketHandler(cts.Token, context, ws).OnAccept();
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await Next(context);
                }
            }
        }

        public static IApplicationBuilder UseWebSocketNotifications(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<NotificationsMiddleware>();
        }
    }
}

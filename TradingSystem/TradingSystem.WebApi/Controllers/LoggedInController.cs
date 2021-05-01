using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.Controllers
{
    public class LoggedInController
    {
        private static readonly Lazy<LoggedInController> instanceLazy = new Lazy<LoggedInController>(() => new LoggedInController(), true);

        private readonly ConcurrentDictionary<String, WebSocket> userWS;

        private LoggedInController()
        {
            this.userWS = new ConcurrentDictionary<string, WebSocket>();
        }

        public static LoggedInController Instance => instanceLazy.Value;

        public void addClient(String username, WebSocket socket)
        {
            userWS.TryAdd(username, socket);
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Communication
{
    public class Communicate : ICommunicate
    {
        private static readonly Lazy<Communicate> instanceLazy = new Lazy<Communicate>(() => new Communicate(), true);
        private ConcurrentDictionary<String, WebSocket> userWS;

        private Communicate()
        {
            userWS = new ConcurrentDictionary<string, WebSocket>();
        }

        public static Communicate Instance => instanceLazy.Value;
        

        public void addClient(String username, WebSocket socket)
        {
            userWS.TryAdd(username, socket);
        }
        public bool SendMessage(string username, string message)
        {
            if (!userWS.TryGetValue(username, out WebSocket ws))
                return false;
            WSS.Instance.sendMessage(ws, message);
            return true;
        }
 
    }
}

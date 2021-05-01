using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace TradingSystem.Communication
{
    public class WSS
    {
        private WebSocketServer wss;

        public WSS()
        {
            wss = new WebSocketServer("ws://localhost");
        }

        public void start()
        {
            wss.Start();
        }
    }
}

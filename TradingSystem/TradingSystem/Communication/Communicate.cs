using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Communication
{
    public class Communicate : ICommunicate
    {
        //private ConcurrentDictionary<String, WebSocket> userWS;
        public bool SendMessage(string username, string message)
        {
            //userWS.tryGetValue(username).send(message);
            throw new NotImplementedException();
        }

        
    }
}

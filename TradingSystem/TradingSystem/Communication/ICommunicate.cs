using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Communication
{
    public interface ICommunicate
    {
        public void SendMessage(string username, string message);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Communication
{
    public interface ICommunicate
    {
        public bool SendMessage(string username, string message);
    }
}

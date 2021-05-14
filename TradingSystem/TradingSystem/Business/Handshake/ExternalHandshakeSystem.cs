using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Handshake
{
    public interface ExternalHandshakeSystem
    {
        public Task<bool> CheckAvailablity();
    }
}

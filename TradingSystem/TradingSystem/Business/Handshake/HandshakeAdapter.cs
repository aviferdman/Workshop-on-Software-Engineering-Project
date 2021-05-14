using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Handshake
{
    public interface HandshakeAdapter
    {
        public Task<bool> CheckAvailablity();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Handshake
{
    public class HandshakeImpl : HandshakeAdapter
    {
        private ExternalHandshakeSystem _handshakeSystem;
        public HandshakeImpl(ExternalHandshakeSystem handshakeSystem = null)
        {
            //proxy
            if (handshakeSystem != null)
            {
                this._handshakeSystem = handshakeSystem;
            }
            else
            {
                this._handshakeSystem = HandshakeSystem.Instance;
            }
        }
        public async Task<bool> CheckAvailablity()
        {
            return await this._handshakeSystem.CheckAvailablity();
        }
    }
}

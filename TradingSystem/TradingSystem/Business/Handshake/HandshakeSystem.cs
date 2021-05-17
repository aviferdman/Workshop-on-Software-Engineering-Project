using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TradingSystem.Business.Handshake
{
    class HandshakeSystem : ExternalHandshakeSystem
    {
        private static readonly Lazy<HandshakeSystem>
        _lazy =
        new Lazy<HandshakeSystem>
            (() => new HandshakeSystem());

        public static HandshakeSystem Instance { get { return _lazy.Value; } }

        private HandshakeSystem()
        {

        }

        public async Task<bool> CheckAvailablity()
        {
            return true;
        }
    }
}

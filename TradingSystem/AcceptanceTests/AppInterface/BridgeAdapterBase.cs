using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface
{
    public abstract class BridgeAdapterBase
    {
        public BridgeAdapterBase(SystemContext systemContext)
        {
            SystemContext = systemContext;
        }

        public SystemContext SystemContext { get; }

        public string? Username
        {
            get => SystemContext.TokenUsername;
            protected set => SystemContext.TokenUsername = value;
        }

        /// <summary>
        /// Determines whether we can inteferface with the system,
        /// either as a guest or as a member.
        /// </summary>
        public bool IsConnected => Username != null;
    }
}

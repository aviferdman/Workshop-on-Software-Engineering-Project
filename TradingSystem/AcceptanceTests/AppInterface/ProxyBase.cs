namespace AcceptanceTests.AppInterface
{
    public abstract class ProxyBase<TBridge> where TBridge : class
    {
        public ProxyBase(TBridge? realBridge)
        {
            RealBridge = realBridge;
            SystemContext = null;
        }

        /// <summary>
        /// The actual bridge which conntects to the system
        /// </summary>
        protected TBridge? RealBridge { get; }
        public SystemContext? SystemContext { get; internal set; }

        /// <summary>
        /// Returns the TBridge interface itself so that the object
        /// which holds this proxy will be able to retrieve the interface.
        /// </summary>
        public abstract TBridge Bridge { get; }
    }
}

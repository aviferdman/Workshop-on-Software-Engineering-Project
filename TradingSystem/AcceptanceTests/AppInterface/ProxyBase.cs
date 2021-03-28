namespace AcceptanceTests.AppInterface
{
    public abstract class ProxyBase<TBridge> where TBridge : class
    {
        public ProxyBase(TBridge? realBridge)
        {
            RealBridge = realBridge;
            System = null;
        }

        protected TBridge? RealBridge { get; }
        public SystemContext? System { get; internal set; }

        public abstract TBridge Bridge { get; }
    }
}

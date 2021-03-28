using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface
{

    [Serializable]
    public class ShopOwnerMismatchException : Exception
    {
        public ShopOwnerMismatchException() { }
        public ShopOwnerMismatchException(ShopInfo shop) : base($"The shop {shop} was tried to be opened with a different owner from the owner in the system.") { }
        public ShopOwnerMismatchException(string message) : base(message) { }
        public ShopOwnerMismatchException(string message, Exception inner) : base(message, inner) { }
        protected ShopOwnerMismatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

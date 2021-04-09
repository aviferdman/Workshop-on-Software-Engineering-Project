using System;
using System.Runtime.Serialization;

namespace AcceptanceTests.AppInterface
{
    [Serializable]
    public class SharedDataMismatchException : Exception
    {
        public SharedDataMismatchException() { }
        public SharedDataMismatchException(string message) : base(message) { }
        public SharedDataMismatchException(string message, Exception inner) : base(message, inner) { }
        protected SharedDataMismatchException(
          SerializationInfo info,
          StreamingContext context
        ) : base(info, context) { }
    }
}

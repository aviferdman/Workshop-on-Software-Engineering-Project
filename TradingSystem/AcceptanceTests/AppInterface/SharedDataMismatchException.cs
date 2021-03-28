using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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

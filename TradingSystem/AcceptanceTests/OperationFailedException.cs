using System;
using System.Runtime.Serialization;

namespace AcceptanceTests
{

    [Serializable]
    public class OperationFailedException : Exception
    {
        public OperationFailedException() { }
        public OperationFailedException(string message) : base(message) { }
        public OperationFailedException(string message, Exception inner) : base(message, inner) { }
        protected OperationFailedException
        (
          SerializationInfo info,
          StreamingContext context
        ) : base(info, context) { }
    }
}

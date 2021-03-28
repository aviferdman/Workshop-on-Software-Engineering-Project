using System;
using System.Runtime.Serialization;

namespace AcceptanceTests.AppInterface
{
    /// <summary>
    /// Signifies that there's a mismatch in tests registering users to the system
    /// (at least 2 tests wanted to register the same username, but with a different password).
    /// </summary>
    [Serializable]
    public class TestUserRegisterMismatchException : SharedDataMismatchException
    {
        public TestUserRegisterMismatchException(string username) : base($"Uername {username} was already registered with a different password.") { }
        public TestUserRegisterMismatchException(string message, Exception inner) : base(message, inner) { }
        protected TestUserRegisterMismatchException(
          SerializationInfo info,
          StreamingContext context
        ) : base(info, context) { }
    }
}

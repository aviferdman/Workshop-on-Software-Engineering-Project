using AcceptanceTests.AppInterface;
using AcceptanceTests.AppInterface.Data;

using NUnit.Framework;

namespace AcceptanceTests.Tests.User
{
    public class MemberUseTestBase : UserTestBase
    {
        public static readonly UserInfo User = SharedTestsData.User_Other;

        public UserInfo UserInfo { get; }

        public MemberUseTestBase(SystemContext systemContext, UserInfo loginInfo)
            : base(systemContext)
        {
            UserInfo = loginInfo;
        }

        [SetUp]
        public virtual void Setup() { }

        [TearDown]
        public virtual void Teardown() { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests
{
    public class BuyerUserInfo : UserInfo
    {
        public BuyerUserInfo
        (
            string username,
            string password,
            string phoneNumber,
            BankAccount bankAccount,
            Address address
        ) : base(username, password, phoneNumber, address)
        {
            BankAccount = bankAccount;
        }

        public BankAccount BankAccount { get; }
    }
}

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
            CreditCard creditCard,
            Address address
        ) : base(username, password, phoneNumber, address)
        {
            if (creditCard is null)
            {
                throw new ArgumentNullException(nameof(creditCard));
            }

            CreditCard = creditCard;
        }

        public CreditCard CreditCard { get; }
    }
}

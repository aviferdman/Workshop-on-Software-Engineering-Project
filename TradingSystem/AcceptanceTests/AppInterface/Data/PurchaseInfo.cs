using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class PurchaseInfo
    {
        public PurchaseInfo(string phoneNumber, CreditCard creditCard, Address address)
        {
            if (creditCard is null)
            {
                throw new ArgumentNullException(nameof(creditCard));
            }

            PhoneNumber = phoneNumber;
            CreditCard = creditCard;
            Address = address;
        }

        public string PhoneNumber { get; set; }
        public CreditCard CreditCard { get; }
        public Address Address { get; set; }
    }
}

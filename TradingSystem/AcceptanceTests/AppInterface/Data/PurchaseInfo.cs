using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class PurchaseInfo
    {
        public PurchaseInfo(string phoneNumber, BankAccount bankAccount, Address address)
        {
            PhoneNumber = phoneNumber;
            BankAccount = bankAccount;
            Address = address;
        }

        public string PhoneNumber { get; set; }
        public BankAccount BankAccount { get; set; }
        public Address Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class BankAccount : PaymentMethod
    {
        private int accountNumber;
        private int branch;

        public int AccountNumber { get => accountNumber; set => accountNumber = value; }

        public int Branch { get => branch; set => branch = value; }

        public BankAccount(int accountNumber, int branch)
        {
            AccountNumber = accountNumber;
            Branch = branch;
        }

        public string GeneratePaymentDetails()
        {
            return String.Format("BankMethod:\\{0}\\{1}", accountNumber, Branch);
        }
    }
}

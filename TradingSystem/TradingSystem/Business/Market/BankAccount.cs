using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class BankAccount
    {
        private int accountNumber;
        private int branch;
        private double current;

        public int AccountNumber { get => accountNumber; set => accountNumber = value; }

        public int Branch { get => branch; set => branch = value; }
        public double Current { get => current; set => current = value; }

        public BankAccount(int accountNumber, int branch, double current)
        {
            AccountNumber = accountNumber;
            Branch = branch;
            Current = current;
        }

        public bool CheckEnoughtCurrent(double paymentSum)
        {
            return Current >= paymentSum;
        }

        public void Pay(double sum)
        {
            Current -= sum;
        }

        public void Refound(double sum)
        {
            Current += sum;
        }
    }
}

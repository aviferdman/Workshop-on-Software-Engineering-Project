using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class CreditCard
    {
        public CreditCard(string cardNumber, string month, string year, string holderName, string cvv, string holderId)
        {
            CardNumber = cardNumber;
            Month = month;
            Year = year;
            HolderName = holderName;
            Cvv = cvv;
            HolderId = holderId;
        }

        public string CardNumber { get; }
        public string Month { get; }
        public string Year { get; }
        public string HolderName { get; }
        public string Cvv { get; }
        public string HolderId { get; }
    }
}

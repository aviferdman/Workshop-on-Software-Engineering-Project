using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class CreditCard : PaymentMethod
    {

        public string _cardNumber { get; set; }
        public string _month { get; set; }
        public string _year { get; set; }
        public string _holderName { get; set; }
        public string _cvv { get; set; }
        public string _holderId { get; set; }

        public CreditCard(string cardNumber, string month, string year, string holderName, string cvv, string holderId)
        {
            _cardNumber = cardNumber;
            _month = month;
            _year = year;
            _holderName = holderName;
            _cvv = cvv;
            _holderId = holderId;
        }

        public string GetCardNumber()
        {
            return _cardNumber;
        }

        public string GetMonth()
        {
            return _month;
        }

        public string GetYear()
        {
            return _year;
        }

        public string GetHolderName()
        {
            return _holderName;
        }

        public string GetCVV()
        {
            return _cvv;
        }

        public string GetHolderId()
        {
            return _holderId;
        }
    }
}

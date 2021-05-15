using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class CreditCard : PaymentMethod
    {

        private string _cardNumber;
        private string _month;
        private string _year;
        private string _holderName;
        private string _cvv;
        private string _holderId;

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

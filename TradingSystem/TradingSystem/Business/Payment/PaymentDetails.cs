using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Payment
{
    public class PaymentDetails
    {
        private string _username;
        private PaymentMethod _method;
        private Guid _storeId;
        private BankAccount _recieverBankAccountId;
        private double _paymentSum;

        public PaymentDetails(string username, PaymentMethod method, Guid storeId, BankAccount recieverBankAccountId, double paymentSum)
        {
            this._username = username;
            this.Method = method;
            this.StoreId = storeId;
            this.RecieverBankAccountId = recieverBankAccountId;
            this.PaymentSum = paymentSum;
        }

        public BankAccount RecieverBankAccountId { get => _recieverBankAccountId; set => _recieverBankAccountId = value; }
        public double PaymentSum { get => _paymentSum; set => _paymentSum = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public PaymentMethod Method { get => _method; set => _method = value; }
    }
}

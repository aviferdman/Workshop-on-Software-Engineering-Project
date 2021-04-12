﻿using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Payment
{
    public class PaymentDetails
    {
        private Guid _clientId;
        private BankAccount _clientBankAccountId;
        private Guid _storeId;
        private BankAccount _recieverBankAccountId;
        private double _paymentSum;

        public PaymentDetails(Guid clientId, BankAccount clientBankAccountId, Guid storeId, BankAccount recieverBankAccountId, double paymentSum)
        {
            this.ClientId = clientId;
            this.ClientBankAccountId = clientBankAccountId;
            this.StoreId = storeId;
            this.RecieverBankAccountId = recieverBankAccountId;
            this.PaymentSum = paymentSum;
        }

        public Guid ClientId { get => _clientId; set => _clientId = value; }
        public BankAccount ClientBankAccountId { get => _clientBankAccountId; set => _clientBankAccountId = value; }
        public BankAccount RecieverBankAccountId { get => _recieverBankAccountId; set => _recieverBankAccountId = value; }
        public double PaymentSum { get => _paymentSum; set => _paymentSum = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
    }
}
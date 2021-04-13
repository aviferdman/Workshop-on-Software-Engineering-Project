using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class PurchaseStatus
    {
        private bool _preConditions;
        private TransactionStatus _transactionStatus;
        private Guid _storeId;

        public PurchaseStatus(bool preConditions, TransactionStatus transactionStatus, Guid storeId)
        {
            this.PreConditions = preConditions;
            this.TransactionStatus = transactionStatus;
            this.StoreId = storeId;
        }

        public bool PreConditions { get => _preConditions; set => _preConditions = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        public TransactionStatus TransactionStatus { get => _transactionStatus; set => _transactionStatus = value; }

        public bool GetPreConditions()
        {
            return PreConditions;
        }
    }
}

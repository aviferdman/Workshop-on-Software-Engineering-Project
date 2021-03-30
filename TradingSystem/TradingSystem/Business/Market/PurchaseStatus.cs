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
        private Dictionary<Product, int> _product_quantity;

        public PurchaseStatus(bool preConditions, TransactionStatus transactionStatus, Guid storeId, Dictionary<Product, int> product_quantity)
        {
            this.PreConditions = preConditions;
            this.TransactionStatus = transactionStatus;
            this.StoreId = storeId;
            this.Product_quantity = product_quantity;
        }

        public bool PreConditions { get => _preConditions; set => _preConditions = value; }
        public Guid StoreId { get => _storeId; set => _storeId = value; }
        internal TransactionStatus TransactionStatus { get => _transactionStatus; set => _transactionStatus = value; }
        internal Dictionary<Product, int> Product_quantity { get => _product_quantity; set => _product_quantity = value; }
    }
}

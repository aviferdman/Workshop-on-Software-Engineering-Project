using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class BuyStatus
    {
        private bool _status;
        ICollection<PurchaseStatus> _purchaseStatuses;
        public BuyStatus( bool status, ICollection<PurchaseStatus> purchaseStatuses)
        {
            Status = status;
            PurchaseStatuses = purchaseStatuses;
        }

        public bool Status { get => _status; set => _status = value; }
        public ICollection<PurchaseStatus> PurchaseStatuses { get => _purchaseStatuses; set => _purchaseStatuses = value; }
    }
}

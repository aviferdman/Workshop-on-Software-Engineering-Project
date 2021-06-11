using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingSystem.WebApi.DTO.Store.Bids
{
    public enum BidStatus_Api
    {
        Approved,
        Denied,
        CustomerNegotiate,
        OwnerNegotiate
    }

    public static class BidStatus_Api_Funcs
    {
        public static BidStatus_Api ConvertStatus(Business.Market.StorePackage.BidStatus bidStatus)
        {
            return (BidStatus_Api)bidStatus;
        }
    }
}

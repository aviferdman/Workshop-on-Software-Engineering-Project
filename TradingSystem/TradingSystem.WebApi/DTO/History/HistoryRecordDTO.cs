using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Service;
using TradingSystem.WebApi.DTO.Products;

namespace TradingSystem.WebApi.DTO
{
    public class HistoryRecordDTO
    {
        public string? Username { get; set; }
        public Guid StoreID { get; set; }
        public string? StoreName { get; set; }
        public string? PaymentId { get; set; }
        public string? DeliveryId { get; set; }
        public IEnumerable<ProductHistoryDTO>? Products { get; set; }

        public static HistoryRecordDTO FromHistoryData(HistoryData historyData)
        {
            return new HistoryRecordDTO
            {
                Username = historyData.Deliveries.Username,
                StoreID = historyData.Deliveries.StoreId,
                StoreName = historyData.Deliveries.StoreName,
                PaymentId = historyData.Payments.PaymentId,
                DeliveryId = historyData.Deliveries.PackageId,
                Products = historyData.Products.ProductId_quantity.Select(ProductHistoryDTO.FromPurchasedProduct),
            };
        }
    }
}

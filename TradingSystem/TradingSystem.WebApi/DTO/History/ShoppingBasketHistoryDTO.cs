using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Service;
using TradingSystem.WebApi.DTO.Products;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingBasketHistoryDTO
    {
        public string? Username { get; set; }
        public Guid StoreID { get; set; }
        public string? StoreName { get; set; }
        public IEnumerable<ProductHistoryDTO>? Products { get; set; }

        public static ShoppingBasketHistoryDTO FromHistoryData(HistoryData historyData)
        {
            return new ShoppingBasketHistoryDTO
            {
                Username = historyData.Deliveries.Username,
                StoreID = historyData.Deliveries.StoreId,
                StoreName = historyData.Deliveries.StoreName,
                Products = historyData.Products.ProductId_quantity.Select(ProductHistoryDTO.FromPurchasedProduct),
            };
        }
    }
}

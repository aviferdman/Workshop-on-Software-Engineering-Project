using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market.Statuses;

namespace TradingSystem.WebApi.DTO
{
    public class ProductHistoryDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public static ProductHistoryDTO FromPurchasedProduct(PurchasedProduct purchasedProduct)
        {
            return new ProductHistoryDTO
            {
                Id = purchasedProduct.id,
                Name = purchasedProduct.name,
                Quantity = purchasedProduct.quantity,
                Price = purchasedProduct.price,
            };
        }
    }
}

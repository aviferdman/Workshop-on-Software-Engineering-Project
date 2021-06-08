using System;
using System.Collections.Generic;
using System.Linq;

using TradingSystem.Business.Market;
using TradingSystem.Service;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingBasketDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public double TotalPrice { get; set; }
        public IEnumerable<ShoppingBasketProductDTO>? Products { get; set; }

        public static ShoppingBasketDTO FromStoreProductsPair(KeyValuePair<NamedGuid, Dictionary<ProductData, int>> shoppingBasket)
        {
            return new ShoppingBasketDTO
            {
                Id = shoppingBasket.Key.Id,
                Name = shoppingBasket.Key.Name,
                Products = shoppingBasket.Value.Select(product =>
                {
                    return ShoppingBasketProductDTO.FromProductData(product.Key, product.Value);
                })
            };
        }
    }
}

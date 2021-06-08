using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TradingSystem.Business.Market;
using TradingSystem.Service;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartDTO
    {
        public ShoppingCartDTO(double totalPrice, IEnumerable<ShoppingBasketDTO> shoppingBaskets)
        {
            TotalPrice = totalPrice;
            ShoppingBaskets = shoppingBaskets;
        }

        public double TotalPrice { get; }
        public IEnumerable<ShoppingBasketDTO> ShoppingBaskets { get; }

        public static ShoppingCartDTO FromDictionaries(IDictionary<NamedGuid, Dictionary<ProductData, int>> cartShoppingBaskets, IDictionary<Guid, double> prices)
        {
            double totalPrice = 0;
            IDictionary<Guid, ShoppingBasketDTO> shoppingBaskets = new Dictionary<Guid, ShoppingBasketDTO>(cartShoppingBaskets.Count);
            foreach (KeyValuePair<NamedGuid, Dictionary<ProductData, int>> cartShoppingBasket in cartShoppingBaskets)
            {
                shoppingBaskets.Add(cartShoppingBasket.Key.Id, ShoppingBasketDTO.FromStoreProductsPair(cartShoppingBasket));
            }
            foreach (KeyValuePair<Guid, double> shoppingBasketPricePair in prices)
            {
                double price = prices[shoppingBasketPricePair.Key];
                shoppingBaskets[shoppingBasketPricePair.Key].TotalPrice = price;
                totalPrice += price;
            }

            return new ShoppingCartDTO(totalPrice, shoppingBaskets.Values);
        }
    }
}

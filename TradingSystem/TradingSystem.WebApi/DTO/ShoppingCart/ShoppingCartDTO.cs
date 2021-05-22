using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradingSystem.Business.Market;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartDTO : IEnumerable<ShoppingBasketDTO>
    {
        public ShoppingCartDTO(IEnumerable<ShoppingBasketDTO> shoppingBaskets)
        {
            ShoppingBaskets = shoppingBaskets;
        }

        public IEnumerable<ShoppingBasketDTO> ShoppingBaskets { get; }

        public static ShoppingCartDTO FromDictionary(Dictionary<Guid, Dictionary<ProductData, int>> dictionary)
        {
            return new ShoppingCartDTO
            (
                dictionary.Select(shoppingBasket =>
                {
                    return new ShoppingBasketDTO
                    {
                        Id = shoppingBasket.Key,
                        Name = null, // TODO: fill
                        Products = shoppingBasket.Value.Select(product =>
                        {
                            return ShoppingBasketProductDTO.FromProductData(product.Key, product.Value);
                        })
                    };
                })
            );
        }

        public IEnumerator<ShoppingBasketDTO> GetEnumerator()
        {
            return ShoppingBaskets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)ShoppingBaskets).GetEnumerator();
        }
    }
}

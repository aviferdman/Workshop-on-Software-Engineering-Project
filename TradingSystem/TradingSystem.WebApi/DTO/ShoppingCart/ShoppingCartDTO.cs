using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TradingSystem.Business.Market;
using TradingSystem.Service;

namespace TradingSystem.WebApi.DTO
{
    public class ShoppingCartDTO : IEnumerable<ShoppingBasketDTO>
    {
        public ShoppingCartDTO(IEnumerable<ShoppingBasketDTO> shoppingBaskets)
        {
            ShoppingBaskets = shoppingBaskets;
        }

        public IEnumerable<ShoppingBasketDTO> ShoppingBaskets { get; }

        public static ShoppingCartDTO FromDictionary(Dictionary<NamedGuid, Dictionary<ProductData, int>> dictionary)
        {
            return new ShoppingCartDTO
            (
                dictionary.Select(shoppingBasket =>
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

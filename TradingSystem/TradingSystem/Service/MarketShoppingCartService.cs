using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Service
{
    public class MarketShoppingCartService
    {
        private static readonly Lazy<MarketShoppingCartService> instanceLazy = new Lazy<MarketShoppingCartService>(() => new MarketShoppingCartService(), true);

        private readonly MarketUsers marketUsers;

        private MarketShoppingCartService()
        {
            marketUsers = MarketUsers.Instance;
        }

        public static MarketShoppingCartService Instance => instanceLazy.Value;

        public Dictionary<NamedGuid, Dictionary<ProductData, int>> ViewShoppingCart(string username)
        {
            var cart = (ShoppingCart)marketUsers.viewShoppingCart(username);
            if (cart == null)
            {
                return null;
            }

            var dataCart = new Dictionary<NamedGuid, Dictionary<ProductData, int>>();
            foreach (ShoppingBasket basket in cart.ShoppingBaskets)
            {
                var products = new Dictionary<ProductData, int>();
                foreach (ProductInCart p in basket.GetDictionaryProductQuantity())
                {
                    products.Add(new ProductData(p.product), p.quantity);
                }
                dataCart.Add(new NamedGuid(basket.GetStore().GetId(), basket.GetStore().name), products);
            }
            return dataCart;
        }

        public async Task<Result<Dictionary<Guid, Dictionary<ProductData, int>>>> EditShoppingCart(string username, List<Guid> products_removed, Dictionary<Guid, int> products_added, Dictionary<Guid, int> products_quan)
        {
            Result<ShoppingCart> res = await marketUsers.editShoppingCart(username, products_removed, products_added, products_quan);
            if (res.IsErr)
            {
                return new Result<Dictionary<Guid, Dictionary<ProductData, int>>>(null, true, res.Mess);
            }

            var cart = (ShoppingCart)res.Ret;
            if (cart == null)
            {
                return null;
            }

            var dataCart = new Dictionary<Guid, Dictionary<ProductData, int>>();
            foreach (ShoppingBasket basket in cart.ShoppingBaskets)
            {
                var products = new Dictionary<ProductData, int>();
                foreach (ProductInCart p in basket.GetDictionaryProductQuantity())
                {
                    products.Add(new ProductData(p.product), p.quantity);
                }
                dataCart.Add(basket.GetStore().GetId(), products);
            }
            return new Result<Dictionary<Guid, Dictionary<ProductData, int>>>(dataCart, false, null);
        }

        public async Task<Result<bool>> PurchaseShoppingCart
        (
            string username,
            string cardNumber, 
            string month, 
            string year, 
            string holderName, 
            string cvv, 
            string holderId,
            string phone,
            string state,
            string city,
            string street,
            string apartmentNum,
            string zip
        )
        {
            var card = new CreditCard(cardNumber, month, year, holderName, cvv, holderId);
            var address = new Address(state, city, street, apartmentNum, zip);
            return await marketUsers.PurchaseShoppingCart(username, card, phone, address);
        }
    }
}

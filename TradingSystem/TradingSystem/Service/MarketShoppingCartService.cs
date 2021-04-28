using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;

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

        public Dictionary<Guid, Dictionary<ProductData, int>> ViewShoppingCart(string username)
        {
            var cart = (ShoppingCart)marketUsers.viewShoppingCart(username);
            if (cart == null)
            {
                return null;
            }

            var dataCart = new Dictionary<Guid, Dictionary<ProductData, int>>();
            foreach (IShoppingBasket basket in cart.ShoppingBaskets)
            {
                var products = new Dictionary<ProductData, int>();
                foreach (KeyValuePair<Product, int> p in basket.GetDictionaryProductQuantity())
                {
                    products.Add(new ProductData(p.Key), p.Value);
                }
                dataCart.Add(basket.GetStore().GetId(), products);
            }
            return dataCart;
        }

        public Result<Dictionary<Guid, Dictionary<ProductData, int>>> EditShoppingCart(string username, List<Guid> products_removed, Dictionary<Guid, int> products_added, Dictionary<Guid, int> products_quan)
        {
            Result<IShoppingCart> res = marketUsers.editShoppingCart(username, products_removed, products_added, products_quan);
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
            foreach (IShoppingBasket basket in cart.ShoppingBaskets)
            {
                var products = new Dictionary<ProductData, int>();
                foreach (KeyValuePair<Product, int> p in basket.GetDictionaryProductQuantity())
                {
                    products.Add(new ProductData(p.Key), p.Value);
                }
                dataCart.Add(basket.GetStore().GetId(), products);
            }
            return new Result<Dictionary<Guid, Dictionary<ProductData, int>>>(dataCart, false, null);
        }

        public Result<bool> PurchaseShoppingCart
        (
            string username,
            int accountNumber,
            int branch,
            string phone,
            string state,
            string city,
            string street,
            string apartmentNum
        )
        {
            var bankAccount = new BankAccount(accountNumber, branch);
            var address = new Address(state, city, street, apartmentNum);
            return marketUsers.PurchaseShoppingCart(username, bankAccount, phone, address);
        }
    }
}

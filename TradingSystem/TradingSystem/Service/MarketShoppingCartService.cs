using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Service
{
    public enum Answer
    {
        Accept, 
        Deny,
        Bid
    }
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

        public async Task<Result<bool>> PurchaseShoppingCart
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
            return await marketUsers.PurchaseShoppingCart(username, bankAccount, phone, address);
        }

        public Result<bool> OwnerAnswerBid(string ownerUsername, Answer answer, String username, Guid storeId, Guid productId, double newBidPrice = 0)
        {
            switch (answer)
            {
                //accept bid
                case Answer.Accept:
                    PublisherManagement.Instance.EventNotification(username, EventType.RequestPurchaseEvent, ConfigurationManager.AppSettings["RequestAcceptMessage"]);
                    return MarketStores.Instance.OwnerAcceptBid(ownerUsername, username, storeId, productId, newBidPrice);

                //request new bid
                case Answer.Bid:
                    var message = $"{username} {storeId} {productId} {newBidPrice}";
                    PublisherManagement.Instance.EventNotification(username, EventType.RequestPurchaseEvent, message);
                    return new Result<bool>(true, false, "");

                //deny new bid
                default:
                    PublisherManagement.Instance.EventNotification(username, EventType.RequestPurchaseEvent, ConfigurationManager.AppSettings["RequestDenyMessage"]);
                    return new Result<bool>(true, false, "");
            }
        }

        public Result<bool> CustomerAnswerBid(Answer answer, String username, Guid storeId, Guid productId, double newBidPrice = 0)
        {
            switch (answer)
            {
                //request new bid
                case Answer.Bid:
                    PublisherManagement.Instance.EventNotification(username, EventType.RequestPurchaseEvent, ConfigurationManager.AppSettings["RequestAcceptMessage"]);
                    return MarketStores.Instance.CustomerRequestBid(username, storeId, productId, newBidPrice);
                
                //accept / deny bid
                default:
                    PublisherManagement.Instance.EventNotification(username, EventType.RequestPurchaseEvent, ConfigurationManager.AppSettings["RequestDenyMessage"]);
                    return new Result<bool>(true, false, "");
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;
using TradingSystem.Service;

namespace TradingSystemTests.DBTests
{
    [TestClass]
    public class BidTests
    {
        int QUANTITY = 5;
        int BID_PRICE = 50;
        MarketStores market = MarketStores.Instance;
        MarketUsers marketUsers = MarketUsers.Instance;
        MarketBids marketBids = MarketBids.Instance;
        UserManagement userManagement = UserManagement.Instance;
        PublisherManagement publisherManagement;
        Store store;
        User customer;
        User owner;
        Product product;
        ProductData dataproduct;

        public BidTests()
        {
        }

        [TestInitialize]
        public async Task Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = false;

            PublisherManagement.Instance.DeleteAll();
            publisherManagement = PublisherManagement.Instance;
            publisherManagement.SetTestMode(true);
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("FounderTest1", "123", null);
            await marketUsers.AddMember("FounderTest1", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("ManagerTest1", "123", null);
            await marketUsers.AddMember("ManagerTest1", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("OwnerTest1", "123", null);
            await marketUsers.AddMember("OwnerTest1", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await market.CreateStore("testStore", "FounderTest1", card, address);
            await market.makeOwner("OwnerTest1", store.Id, "FounderTest1");
            owner = marketUsers.ActiveUsers.GetOrAdd("OwnerTest1", owner);
            customer = marketUsers.ActiveUsers.GetOrAdd("ManagerTest1", customer);
            
            dataproduct = new ProductData(new Guid(), "lala", 100, 100, 100, "cat");
            var shoppingCart = new ShoppingCart(customer);
            Result<Product> res=await market.AddProduct(dataproduct, store.Id, "FounderTest1");
            product = res.Ret;
            await marketUsers.AddProductToCart(customer.Username, product.id, QUANTITY);

        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.OwnerAcceptBid(string, string, Guid, Guid, double)"/>
        [TestMethod]
        public async Task TestStoreWithoutAvailableBid()
        {
            double originPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(product.Price * QUANTITY, originPrice);
            await marketBids.OwnerChangeBidPolicy(owner.Username, store.Id, false);
            var resultBid = await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, BID_PRICE);
            var bidId = resultBid.Ret;
            await marketBids.OwnerAcceptBid(owner.Username, store.Id, bidId);
            double bidPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(originPrice, bidPrice);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.CustomerCreateBid(string, Guid, Guid, double)"/>
        [TestMethod]
        public async Task TestRequestPurcahse()
        {
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(owner.Username, EventType.RequestPurchaseEvent);
            subscriber.TestMode = true;
            int originMessages = subscriber.Messages.Count;
            Assert.AreEqual(originMessages, subscriber.Messages.Count);
            await marketBids.OwnerChangeBidPolicy(owner.Username, store.Id, true);
            await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, BID_PRICE);
            Assert.AreEqual(originMessages + 1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.OwnerAcceptBid(string, string, Guid, Guid, double)"/>
        [TestMethod]
        public async Task TestNotAllOwnersAcceptBid()
        {
            double originPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(product.Price * QUANTITY, originPrice);
            await marketBids.OwnerChangeBidPolicy(owner.Username, store.Id, true);
            var resultBid = await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, BID_PRICE);
            var bidId = resultBid.Ret;
            await marketBids.OwnerAcceptBid(owner.Username, store.Id, bidId);
            double bidPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(originPrice, bidPrice);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.OwnerAcceptBid(string, string, Guid, Guid, double)"/>
        [TestMethod]
        public async Task TestAllOwnersAcceptBid()
        {
            double originPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(product.Price * QUANTITY, originPrice);
            await marketBids.OwnerChangeBidPolicy(owner.Username, store.Id, true);
            var resultBid = await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, BID_PRICE);
            var bidId = resultBid.Ret;
            await marketBids.OwnerAcceptBid(owner.Username, store.Id, bidId);
            await marketBids.OwnerAcceptBid(store.founder.Username, store.Id, bidId);
            double bidPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(BID_PRICE * QUANTITY, bidPrice);
        }


        /// test for function :<see cref="TradingSystem.Business.Market.MarketStores.OwnerAcceptBid(string, string, Guid, Guid, double)"/>
        [TestMethod]
        public async Task TestOwnerWithoutPermissionAcceptBid()
        {
            double originPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            var resultBid = await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, originPrice * 0.5);
            var bidId = resultBid.Ret;
            await marketBids.OwnerChangeBidPolicy("blahblahblah", store.Id, true);
            await marketBids.OwnerAcceptBid("blahblahblah", store.Id, bidId);
            double bidPrice = store.CalcPrice(customer.Username, await customer.ShoppingCart.GetShoppingBasket(store));
            Assert.AreEqual(originPrice, bidPrice);
        }


        /// test for function :<see cref="TradingSystem.Service.MarketShoppingCartService.OwnerAnswerBid(string, TradingSystem.Service.Answer, string, Guid, Guid, double)">
        [TestMethod]
        public async Task TestOwnerAnswerDenyBid()
        {
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(customer.Username, EventType.RequestPurchaseEvent);
            subscriber.TestMode = true;
            int originMessages = subscriber.Messages.Count;
            Assert.AreEqual(originMessages, subscriber.Messages.Count);
            await marketBids.OwnerChangeBidPolicy(owner.Username, store.Id, true);
            var resultBid = await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, BID_PRICE);
            var bidId = resultBid.Ret;
            await marketBids.OwnerDenyBid(owner.Username, store.Id, bidId);
            Assert.AreEqual(originMessages + 1, subscriber.Messages.Count);
        }

        /// test for function :<see cref="TradingSystem.Service.MarketShoppingCartService.OwnerAnswerBid(string, TradingSystem.Service.Answer, string, Guid, Guid, double)">
        [TestMethod]
        public async Task TestOwnerAnswerNewBid()
        {
            NotificationSubscriber subscriber = PublisherManagement.Instance.FindSubscriber(customer.Username, EventType.RequestPurchaseEvent);
            subscriber.TestMode = true;
            int originMessages = subscriber.Messages.Count;
            Assert.AreEqual(originMessages, subscriber.Messages.Count);
            await marketBids.OwnerChangeBidPolicy(owner.Username, store.Id, true);
            var resultBid = await marketBids.CustomerCreateBid(customer.Username, store.Id, product.Id, BID_PRICE);
            var bidId = resultBid.Ret;
            await marketBids.OwnerNegotiateBid(owner.Username, store.Id, bidId, 1.2 * BID_PRICE);
            Assert.AreEqual(originMessages + 1, subscriber.Messages.Count);
        }


        [TestCleanup]
        public async Task DeleteAll()
        {
            UserManagement.Instance.tearDown();
            PublisherManagement.Instance.DeleteAll();
            market.tearDown();
            marketUsers.tearDown();
        }
    }
}

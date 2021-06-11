using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.DAL;

namespace TradingSystemTests.IntegrationTests
{
    [TestClass]
    public class MarketRulesTests
    {
        private MarketStores marketStores = MarketStores.Instance;
        private MarketRules marketRules = MarketRules.Instance;

        public Store store;
        public Product product;
        public ShoppingBasket shoppingBasket;

        public MarketRulesTests()
        {
            this.store = new Store("TestStore", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            this.store.SetFounder(Founder.makeFounder(new MemberState("Founder"), store));
            marketStores.LoadedStores.TryAdd(store.GetId(), store);
            this.product = new Product(new Guid(), "ProductName", 100, 100, 100, "CategoryName");
            this.shoppingBasket = new ShoppingBasket(new ShoppingCart(new User()), store);
            shoppingBasket.addProduct(product, 10);
        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        //DISCOUNTS

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleProductDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, 0.2, productId: product.Id);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleProductDiscountDifferenrId()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, 0.2, productId: Guid.NewGuid());
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleCategoryDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, 0.2, category: "CategoryName");
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleCategoryDiscountDifferentCategory()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, 0.2, category: "DifferentCategoryName");
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleStoreDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, 0.2);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalStoreQuantityDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, RuleType.Quantity, 0.2, productId: product.Id, valueLessThan: 12, valueGreaterEQThan: 20);
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalProductQuantityDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, RuleType.Quantity, 0.2, productId: product.Id, valueGreaterEQThan: 5);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalProductQuantityDiscountDifferentId()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, RuleType.Quantity, 0.2, productId: Guid.NewGuid(), valueGreaterEQThan: 5);
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalProductWeightDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, RuleType.Weight, 0.2, productId: product.Id, valueGreaterEQThan: 100);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalCategoryQuantityDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, RuleType.Quantity, 0.2, category: "CategoryName", valueGreaterEQThan: 5);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Price More than 100, and at least 5 products from category
        public async Task CheckAndConditionalDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            Guid discount1 = await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, RuleType.Price, 0.2, valueGreaterEQThan: 100);
            Guid discount2 = await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, RuleType.Quantity, 0.2, category: "CategoryName", valueGreaterEQThan: 5);
            await marketRules.GenerateConditionalDiscountsAsync(store.GetFounder().Username, DiscountRuleRelation.And, store.GetId(), discount1, discount2, false);
            store.RemoveDiscount(store.GetFounder().Username, discount2);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Price More than 100, Xor at most 5 products from category
        public async Task CheckXorConditionalDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            Guid discount1 = await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, RuleType.Price, 0.2, valueGreaterEQThan: 100);
            Guid discount2 = await marketRules.CreateConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, RuleType.Quantity, 0.2, category: "CategoryName", valueLessThan: 5);
            //Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId
            await marketRules.GenerateConditionalDiscountsAsync(store.GetFounder().Username, DiscountRuleRelation.Xor, store.GetId(), discount1, discount2, false);
            store.RemoveDiscount(store.GetFounder().Username, discount1);
            store.RemoveDiscount(store.GetFounder().Username, discount2);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        //POLICY

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public void CheckSimpleLegalProductPolicy()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 5);
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public void CheckSimpleIllegalProductPolicy()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 20);
            Assert.AreEqual(false, store.CheckPolicy(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Complex Policy: more than 5 units and weights more than 100
        public void CheckAndProductPolicy()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 5);
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.And, RuleContext.Product, RuleType.Weight, productId: product.Id, valueGreaterEQThan: 100);
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
        }


        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Complex Policy: more than 20 units or weights less than 100
        public void CheckIllegalOrProductPolicy()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 20);
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Or, RuleContext.Product, RuleType.Weight, productId: product.Id, valueLessThan: 100);
            Assert.AreEqual(false, store.CheckPolicy(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Complex Policy: if more than 5 units then price more than 500
        public void CheckConditionOrProductPolicy()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));

            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Price, productId: product.Id, valueLessThan: 500);
            marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Condition, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 5);
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
        }

    }
}

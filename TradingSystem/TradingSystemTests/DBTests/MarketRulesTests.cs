using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Business.UserManagement;
using TradingSystem.DAL;
using TradingSystem.Service;

namespace TradingSystemTests.DBTests
{
    [TestClass]
    public class MarketRulesTests
    {
        private MarketStores marketStores = MarketStores.Instance;
        private MarketRules marketRules = MarketRules.Instance;
        private MarketUsers marketUsers = MarketUsers.Instance;
        private UserManagement userManagement = UserManagement.Instance;
        public Store store;
        public ProductData product1;
        public Product product;
        public ShoppingBasket shoppingBasket;
        [TestInitialize]
        public async Task MarketRulesTestsInit()
        {
            ProxyMarketContext.Instance.IsDebug = false;
            String guestName = marketUsers.AddGuest();
            await userManagement.SignUp("founder", "123", null);
            await marketUsers.AddMember("founder", "123", guestName);
            guestName = marketUsers.AddGuest();
            await userManagement.SignUp("manager", "123", null);
            await marketUsers.AddMember("manager", "123", guestName);
            Address address = new Address("1", "1", "1", "1", "1");
            CreditCard card = new CreditCard("1", "1", "1", "1", "1", "1");
            store = await marketStores.CreateStore("testStore", "founder", card, address);
            await marketStores.makeManager("manager", store.Id, "founder");
            product1 = new ProductData("ProductName", 100, 100, 100, "CategoryName");
            User u = marketUsers.GetUserByUserName("founder");
            ShoppingCart cart = u.ShoppingCart;
            shoppingBasket =await cart.GetShoppingBasket(store);
            Result<Product> result = await MarketStores.Instance.AddProduct(product1, store.Id, "founder");
            product = result.Ret;
            await marketUsers.AddProductToCart("founder", result.Ret.Id, 10);
        }

        
       
        //DISCOUNTS

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleProductDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, 0.2, productId: product.Id);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleProductDiscountDifferenrId()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, 0.2, productId: Guid.NewGuid());
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleCategoryDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, 0.2, category: "CategoryName");
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleCategoryDiscountDifferentCategory()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, 0.2, category: "DifferentCategoryName");
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateSimpleDiscount(Guid, RuleContext, double, string, Guid)"/>
        [TestMethod]
        public async Task CheckSimpleStoreDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddSimpleDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, 0.2);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalProductQuantityDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, RuleType.Quantity, 0.2, productId: product.Id, valueGreaterEQThan: 5);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalProductQuantityDiscountDifferentId()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, RuleType.Quantity, 0.2, productId: Guid.NewGuid(), valueGreaterEQThan: 5);
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckConditionalProductWeightDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Product, RuleType.Weight, 0.2, productId: product.Id, valueGreaterEQThan: 100);
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
            Guid discount1 = await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, RuleType.Price, 0.2, valueGreaterEQThan: 100);
            Guid discount2 = await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, RuleType.Quantity, 0.2, category: "CategoryName", valueGreaterEQThan: 5);
            // var rule1 = store.GetDiscountById(discount1).GetRule();
            // var rule2 = store.GetDiscountById(discount2).GetRule();
            // Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId
            await MarketRulesService.Instance.AddDiscountRuleAsync(store.GetFounder().Username, DiscountRuleRelation.And, store.GetId(), discount1, discount2, false);
            await MarketRulesService.Instance.RemoveDiscountAsync(store.GetFounder().Username,store.Id, discount2);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.CreateConditionalDiscount(Guid, RuleContext, RuleType, double, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Price More than 100, Xor at most 5 products from category
        public async Task CheckXorConditionalDiscount()
        {
            Assert.AreEqual(1000, store.CalcPaySum(shoppingBasket));
            Guid discount1 = await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Store, RuleType.Price, 0.2, valueGreaterEQThan: 100);
            Guid discount2 = await MarketRulesService.Instance.AddConditionalDiscountAsync(store.GetFounder().Username, store.GetId(), RuleContext.Category, RuleType.Quantity, 0.2, category: "CategoryName", valueLessThan: 5);
            //Guid storeId, Guid ruleId1, Guid ruleId2, Guid discountId
            await MarketRulesService.Instance.AddDiscountRuleAsync(store.GetFounder().Username, DiscountRuleRelation.Xor, store.GetId(), discount1, discount2, false);
            await MarketRulesService.Instance.RemoveDiscountAsync(store.GetFounder().Username, store.sid, discount1);
            await MarketRulesService.Instance.RemoveDiscountAsync(store.GetFounder().Username, store.sid, discount2);
            Assert.AreEqual(800, store.CalcPaySum(shoppingBasket));
        }

        //POLICY

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckSimpleLegalProductPolicyAsync()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            await MarketRulesService.Instance.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 5);
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        public async Task CheckSimpleIllegalProductPolicyAsync()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            await MarketRulesService.Instance.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 20);
            Assert.AreEqual(false, store.CheckPolicy(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Complex Policy: more than 5 units and weights more than 100
        public async Task CheckAndProductPolicyAsync()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            await marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 5);
            await marketRules.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.And, RuleContext.Product, RuleType.Weight, productId: product.Id, valueGreaterEQThan: 100);
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
        }


        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Complex Policy: more than 20 units or weights less than 100
        public async Task CheckIllegalOrProductPolicyAsync()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
            await MarketRulesService.Instance.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 20);
            await MarketRulesService.Instance.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Or, RuleContext.Product, RuleType.Weight, productId: product.Id, valueLessThan: 100);
            Assert.AreEqual(false, store.CheckPolicy(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.MarketRules.AddPolicyRule(Guid, PolicyRuleRelation, RuleContext, RuleType, string, Guid, string, double, double, DateTime, DateTime)"/>
        [TestMethod]
        //Complex Policy: if more than 5 units then price more than 500
        public async Task CheckConditionOrProductPolicyAsync()
        {
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));

            await MarketRulesService.Instance.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Simple, RuleContext.Product, RuleType.Price, productId: product.Id, valueLessThan: 500);
            await MarketRulesService.Instance.AddPolicyRule(store.GetFounder().Username, store.GetId(), PolicyRuleRelation.Condition, RuleContext.Product, RuleType.Quantity, productId: product.Id, valueGreaterEQThan: 5);
            Assert.AreEqual(true, store.CheckPolicy(shoppingBasket));
        }
        [TestCleanup]
        public void DeleteAll()
        {
            marketUsers.tearDown();
            userManagement.tearDown();
            store = null;
        }
    }
}

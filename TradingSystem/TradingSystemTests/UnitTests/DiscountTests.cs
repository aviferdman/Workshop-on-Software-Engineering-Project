using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StorePackage.DiscountPackage;
using TradingSystem.DAL;

namespace TradingSystemTests.UnitTests
{

    [TestClass]
    public class DiscountTests
    {
        private Mock<IDiscountCalculator> discountCalc1;
        private Mock<IDiscountCalculator> discountCalc2;
        private ShoppingBasket shoppingBasket;

        public DiscountTests()
        {
            discountCalc1 = new Mock<IDiscountCalculator>();
            discountCalc2 = new Mock<IDiscountCalculator>();
            discountCalc1.Setup(d => d.CalcDiscount(It.IsAny<ShoppingBasket>())).Returns(15);
            discountCalc1.Setup(d => d.GetFunction()).Returns(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 15));
            discountCalc2.Setup(d => d.CalcDiscount(It.IsAny<ShoppingBasket>())).Returns(20);
            discountCalc2.Setup(d => d.GetFunction()).Returns(new Func<ShoppingBasket, double>((ShoppingBasket shoppingBasket) => 20));
            User u = new User();
            Store store = new Store("teststore", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Available(IShoppingBasket)"/>
        [TestMethod]
        public void CheckAllRulesTrue()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            discount.AddRule(rule1.Object);
            discount.AddRule(rule2.Object);
            Assert.AreEqual(true, discount.Available(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Available(IShoppingBasket)"/>
        [TestMethod]
        public void CheckSomeRulesFalse()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            discount.AddRule(rule1.Object);
            discount.AddRule(rule2.Object);
            Assert.AreEqual(false, discount.Available(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.ApplyDiscounts(IShoppingBasket)"/>
        [TestMethod]
        public void CheckApplyDiscounts()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Assert.AreEqual(15, discount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscounts()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            ConditionDiscount addedDiscount = discount.And(rule2.Object);
            Assert.AreEqual(15, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscountsAddedRulesFalse()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            ConditionDiscount addedDiscount = discount.And(rule2.Object);
            Assert.AreEqual(0, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscountsMyRulesFalse()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            ConditionDiscount addedDiscount = discount.And(rule2.Object);
            Assert.AreEqual(0, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscountsBothMyRulesAndAddedFalse()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            ConditionDiscount addedDiscount = discount.And(rule2.Object);
            Assert.AreEqual(0, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscounts()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            ConditionDiscount orDiscount = discount.Or(rule2.Object);
            Assert.AreEqual(15, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscountsAddedRulesFalse()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            ConditionDiscount orDiscount = discount.Or(rule2.Object);
            Assert.AreEqual(15, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscountsMyRulesFalse()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            ConditionDiscount orDiscount = discount.Or(rule2.Object);
            Assert.AreEqual(15, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscountsBothMyAndAddedRulesFalse()
        {
            ConditionDiscount discount = new ConditionDiscount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            ConditionDiscount orDiscount = discount.Or(rule2.Object);
            Assert.AreEqual(0, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsTakeFirstDiscount()
        {
            ConditionDiscount discount1 = new ConditionDiscount(discountCalc1.Object);
            ConditionDiscount discount2 = new ConditionDiscount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            ConditionDiscount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(15, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsTakeSecondDiscount()
        {
            ConditionDiscount discount1 = new ConditionDiscount(discountCalc1.Object);
            ConditionDiscount discount2 = new ConditionDiscount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            ConditionDiscount xorDiscount = discount1.Xor(discount2, false);
            Assert.AreEqual(20, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsFirstFalseSecondTrue()
        {
            ConditionDiscount discount1 = new ConditionDiscount(discountCalc1.Object);
            ConditionDiscount discount2 = new ConditionDiscount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            ConditionDiscount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(20, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsFirstTrueSecondFalse()
        {
            ConditionDiscount discount1 = new ConditionDiscount(discountCalc1.Object);
            ConditionDiscount discount2 = new ConditionDiscount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            ConditionDiscount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(20, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsBothFalse()
        {
            ConditionDiscount discount1 = new ConditionDiscount(discountCalc1.Object);
            ConditionDiscount discount2 = new ConditionDiscount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            discount2.AddRule(rule2.Object);
            ConditionDiscount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(0, xorDiscount.ApplyDiscounts(shoppingBasket));
        }
    }
}

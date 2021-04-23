using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystemTests.UnitTests
{

    [TestClass]
    public class DiscountTests
    {
        private Mock<IDiscountCalculator> discountCalc1;
        private Mock<IDiscountCalculator> discountCalc2;
        private IShoppingBasket shoppingBasket;

        public DiscountTests()
        {
            discountCalc1 = new Mock<IDiscountCalculator>();
            discountCalc2 = new Mock<IDiscountCalculator>();
            discountCalc1.Setup(d => d.CalcDiscount(It.IsAny<IShoppingBasket>())).Returns(15);
            discountCalc1.Setup(d => d.GetFunction()).Returns(new Func<IShoppingBasket, double>((IShoppingBasket shoppingBasket) => 15));
            discountCalc2.Setup(d => d.CalcDiscount(It.IsAny<IShoppingBasket>())).Returns(20);
            discountCalc2.Setup(d => d.GetFunction()).Returns(new Func<IShoppingBasket, double>((IShoppingBasket shoppingBasket) => 20));
            User u = new User();
            IStore store = new Store("teststore", new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Available(IShoppingBasket)"/>
        [TestMethod]
        public void CheckAllRulesTrue()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Discount discount = new Discount(discountCalc1.Object);
            discount.AddRule(rule1.Object);
            discount.AddRule(rule2.Object);
            Assert.AreEqual(true, discount.Available(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Available(IShoppingBasket)"/>
        [TestMethod]
        public void CheckSomeRulesFalse()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            Discount discount = new Discount(discountCalc1.Object);
            discount.AddRule(rule1.Object);
            discount.AddRule(rule2.Object);
            Assert.AreEqual(false, discount.Available(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.ApplyDiscounts(IShoppingBasket)"/>
        [TestMethod]
        public void CheckApplyDiscounts()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Assert.AreEqual(15, discount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscounts()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            additionalRules.Add(rule2.Object);
            Discount addedDiscount = discount.And(additionalRules);
            Assert.AreEqual(15, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscountsAddedRulesFalse()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            additionalRules.Add(rule2.Object);
            Discount addedDiscount = discount.And(additionalRules);
            Assert.AreEqual(0, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscountsMyRulesFalse()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            additionalRules.Add(rule2.Object);
            Discount addedDiscount = discount.And(additionalRules);
            Assert.AreEqual(0, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.And(ICollection{IRule})"/>
        [TestMethod]
        public void CheckAndDiscountsBothMyRulesAndAddedFalse()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            additionalRules.Add(rule2.Object);
            Discount addedDiscount = discount.And(additionalRules);
            Assert.AreEqual(0, addedDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscounts()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            additionalRules.Add(rule2.Object);
            Discount orDiscount = discount.Or(additionalRules);
            Assert.AreEqual(15, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscountsAddedRulesFalse()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            additionalRules.Add(rule2.Object);
            Discount orDiscount = discount.Or(additionalRules);
            Assert.AreEqual(15, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscountsMyRulesFalse()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            additionalRules.Add(rule2.Object);
            Discount orDiscount = discount.Or(additionalRules);
            Assert.AreEqual(15, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Or(ICollection{IRule})"/>
        [TestMethod]
        public void CheckOrDiscountsBothMyAndAddedRulesFalse()
        {
            Discount discount = new Discount(discountCalc1.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount.AddRule(rule1.Object);
            ICollection<IRule> additionalRules = new HashSet<IRule>();
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            additionalRules.Add(rule2.Object);
            Discount orDiscount = discount.Or(additionalRules);
            Assert.AreEqual(0, orDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsTakeFirstDiscount()
        {
            Discount discount1 = new Discount(discountCalc1.Object);
            Discount discount2 = new Discount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            Discount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(15, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsTakeSecondDiscount()
        {
            Discount discount1 = new Discount(discountCalc1.Object);
            Discount discount2 = new Discount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            Discount xorDiscount = discount1.Xor(discount2, false);
            Assert.AreEqual(20, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsFirstFalseSecondTrue()
        {
            Discount discount1 = new Discount(discountCalc1.Object);
            Discount discount2 = new Discount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            Discount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(20, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsFirstTrueSecondFalse()
        {
            Discount discount1 = new Discount(discountCalc1.Object);
            Discount discount2 = new Discount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            discount2.AddRule(rule2.Object);
            Discount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(20, xorDiscount.ApplyDiscounts(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Discount.Xor(Discount, bool)"/>
        [TestMethod]
        public void CheckXorDiscountsBothFalse()
        {
            Discount discount1 = new Discount(discountCalc1.Object);
            Discount discount2 = new Discount(discountCalc2.Object);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount1.AddRule(rule1.Object);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            discount2.AddRule(rule2.Object);
            Discount xorDiscount = discount1.Xor(discount2, true);
            Assert.AreEqual(0, xorDiscount.ApplyDiscounts(shoppingBasket));
        }
    }
}

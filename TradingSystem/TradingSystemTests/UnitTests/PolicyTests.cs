using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Market;
using Moq;

namespace TradingSystemTests
{
    [TestClass]
    public class PolicyTests
    {
        private static readonly int QUANTITY1 = 100;
        private static readonly double WEIGHT1 = 100;
        private static readonly double PRICE1 = 100;
        private static readonly int QUANTITY2 = 200;
        private static readonly double WEIGHT2 = 200;
        private static readonly double PRICE2 = 200;
        private Policy policyTest;
        private Product product1;
        private Product product2;
        private Dictionary<Product, int> product_quantity;
        private IShoppingBasket shoppingBasket;

        public PolicyTests()
        {
            this.policyTest = new Policy();
            this.product1 = new Product(QUANTITY1, WEIGHT1, PRICE1);
            this.product2 = new Product(QUANTITY2, WEIGHT2, PRICE2);
            this.product_quantity = new Dictionary<Product, int>();
            User u = new User();
            IStore store = new Store("teststore", new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

        //START OF UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckAllRulesReturnTrue()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Policy policy = new Policy();
            policy.AddRule(rule1.Object);
            policy.AddRule(rule2.Object);
            Assert.AreEqual(true,policy.Check(shoppingBasket));

        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckSomeRulesFalse()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            Policy policy = new Policy();
            policy.AddRule(rule1.Object);
            policy.AddRule(rule2.Object);
            Assert.AreEqual(false, policy.Check(shoppingBasket));
        }

        public bool CheckTotalWeightNoMoreThan400(IShoppingBasket shoppingBasket)
        {
            double weight = shoppingBasket.GetDictionaryProductQuantity().Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            return weight <= 400;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckPassTheRules()
        {
            this.product_quantity.Add(product1, 1);
            this.product_quantity.Add(product2, 1);
            Func < IShoppingBasket, bool> f = new Func<IShoppingBasket, bool>(CheckTotalWeightNoMoreThan400);
            IRule r = new Rule(f);
            policyTest.AddRule(r);
            Assert.IsTrue(policyTest.Check(this.shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckFailTheRules()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Func<IShoppingBasket, bool> f = new Func<IShoppingBasket, bool>(CheckTotalWeightNoMoreThan400);
            IRule r = new Rule(f);
            policyTest.AddRule(r);
            Assert.IsFalse(policyTest.Check(this.shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public void CheckAndPolicies()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.And(legalPolicy2);
            Assert.IsTrue(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public void CheckAndPoliciesFirstFalseSecondTrue()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.And(legalPolicy2);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public void CheckAndPoliciesFirstTrueSecondFalse()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.And(legalPolicy2);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public void CheckAndPoliciesFirstFalseSecondFalse()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.And(legalPolicy2);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public void CheckOrPolicies()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.Or(legalPolicy2);
            Assert.IsTrue(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public void CheckOrPoliciesFirstFalseSecondTrue()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.Or(legalPolicy2);
            Assert.IsTrue(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public void CheckOrPoliciesFirstTrueSecondFalse()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.Or(legalPolicy2);
            Assert.IsTrue(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public void CheckOrPoliciesFirstFalseSecondFalse()
        {
            this.shoppingBasket.addProduct(product1, 1);
            this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<IShoppingBasket>())).Returns(false);
            ICollection<IRule> firstRules = new HashSet<IRule>();
            ICollection<IRule> secondRules = new HashSet<IRule>();
            firstRules.Add(rule1.Object);
            secondRules.Add(rule2.Object);
            Policy legalPolicy1 = new Policy(firstRules);
            Policy legalPolicy2 = new Policy(secondRules);

            Policy andPolicy = legalPolicy1.Or(legalPolicy2);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        //END OF UNIT TESTS


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
            User u = new User();
            IStore store = new Store("teststore", new BankAccount(1, 1), new Address("1", "1", "1", "1"));
            shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

    }
}

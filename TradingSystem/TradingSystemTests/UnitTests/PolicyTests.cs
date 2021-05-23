using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Market;
using Moq;
using TradingSystem.DAL;
using System.Threading.Tasks;

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
        private ShoppingBasket shoppingBasket;

        public PolicyTests()
        {
            this.policyTest = new Policy();
            this.product1 = new Product(QUANTITY1, WEIGHT1, PRICE1);
            this.product2 = new Product(QUANTITY2, WEIGHT2, PRICE2);
            this.product_quantity = new Dictionary<Product, int>();
            User u = new User();
            Store store = new Store("teststore", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            this.shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

        [TestInitialize]
        public void Initialize()
        {
            ProxyMarketContext.Instance.IsDebug = true;
        }

        //START OF UNIT TESTS

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckAllRulesReturnTrue()
        {
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
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
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy policy = new Policy();
            policy.AddRule(rule1.Object);
            policy.AddRule(rule2.Object);
            Assert.AreEqual(false, policy.Check(shoppingBasket));
        }

        public bool CheckTotalWeightNoMoreThan400(ShoppingBasket shoppingBasket)
        {
            double weight = shoppingBasket.GetDictionaryProductQuantity().Aggregate(0.0, (total, next) => total + next.product.Weight * next.quantity);
            return weight <= 400;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckPassTheRules()
        {
            this.product_quantity.Add(product1, 1);
            this.product_quantity.Add(product2, 1);
            Func < ShoppingBasket, bool> f = new Func<ShoppingBasket, bool>(CheckTotalWeightNoMoreThan400);
            IRule r = new Rule(f);
            policyTest.AddRule(r);
            Assert.IsTrue(policyTest.Check(this.shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public async Task CheckFailTheRules()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Func<ShoppingBasket, bool> f = new Func<ShoppingBasket, bool>(CheckTotalWeightNoMoreThan400);
            IRule r = new Rule(f);
            policyTest.AddRule(r);
            Assert.IsFalse(policyTest.Check(this.shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public async Task CheckAndPolicies()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy andPolicy = legalPolicy1.And(rule2.Object);
            Assert.IsTrue(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public async Task CheckAndPoliciesFirstFalseSecondTrue()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy andPolicy = legalPolicy1.And(rule2.Object);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public async Task CheckAndPoliciesFirstTrueSecondFalse()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy andPolicy = legalPolicy1.And(rule2.Object);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.And(Policy))"/>
        [TestMethod]
        public async Task CheckAndPoliciesFirstFalseSecondFalse()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy andPolicy = legalPolicy1.And(rule2.Object);
            Assert.IsFalse(andPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public async Task CheckOrPolicies()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy orPolicy = legalPolicy1.Or(rule2.Object);
            Assert.IsTrue(orPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public async Task CheckOrPoliciesFirstFalseSecondTrue()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy orPolicy = legalPolicy1.Or(rule2.Object);
            Assert.IsTrue(orPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public async Task CheckOrPoliciesFirstTrueSecondFalse()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy orPolicy = legalPolicy1.Or(rule2.Object);
            Assert.IsTrue(orPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Or(Policy))"/>
        [TestMethod]
        public async Task CheckOrPoliciesFirstFalseSecondFalse()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy orPolicy = legalPolicy1.Or(rule2.Object);
            Assert.IsFalse(orPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Condition(Policy))"/>
        [TestMethod]
        public async Task CheckConditionPolicies()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy conditionPolicy = legalPolicy1.Condition(rule2.Object);
            Assert.IsTrue(conditionPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Condition(Policy))"/>
        [TestMethod]
        public async Task CheckConditionPoliciesFirstFalseSecondTrue()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy conditionPolicy = legalPolicy1.Condition(rule2.Object);
            Assert.IsTrue(conditionPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Condition(Policy))"/>
        [TestMethod]
        public async Task CheckConditionPoliciesFirstTrueSecondFalse()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(true);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy conditionPolicy = legalPolicy1.Condition(rule2.Object);
            Assert.IsFalse(conditionPolicy.Check(shoppingBasket));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Condition(Policy))"/>
        [TestMethod]
        public async Task CheckConditionPoliciesFirstFalseSecondFalse()
        {
            await this.shoppingBasket.addProduct(product1, 1);
            await this.shoppingBasket.addProduct(product2, 2);
            Mock<IRule> rule1 = new Mock<IRule>();
            rule1.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Mock<IRule> rule2 = new Mock<IRule>();
            rule2.Setup(r => r.Check(It.IsAny<ShoppingBasket>())).Returns(false);
            Policy legalPolicy1 = new Policy(rule1.Object);

            Policy conditionPolicy = legalPolicy1.Condition(rule2.Object);
            Assert.IsTrue(conditionPolicy.Check(shoppingBasket));
        }

        //END OF UNIT TESTS


        [TestCleanup]
        public void DeleteAll()
        {
            Transaction.Instance.DeleteAllTests();
            User u = new User();
            Store store = new Store("teststore", new CreditCard("1", "1", "1", "1", "1", "1"), new Address("1", "1", "1", "1", "1"));
            shoppingBasket = new ShoppingBasket(new ShoppingCart(u), store);
        }

    }
}

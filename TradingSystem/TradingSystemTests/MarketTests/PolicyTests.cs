using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Market;

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

        public PolicyTests()
        {
            this.policyTest = new Policy();
            this.product1 = new Product(QUANTITY1, WEIGHT1, PRICE1);
            this.product2 = new Product(QUANTITY2, WEIGHT2, PRICE2);
            this.product_quantity = new Dictionary<Product, int>();
        }

        public bool CheckTotalWeightNoMoreThan400(Dictionary<Product, int> product_quantity)
        {
            double weight = product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Weight * next.Value);
            return weight <= 400;
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckPassTheRules()
        {
            this.product_quantity.Add(product1, 1);
            this.product_quantity.Add(product2, 1);
            Func < Dictionary<Product, int>, bool> f = new Func<Dictionary<Product, int>, bool>(CheckTotalWeightNoMoreThan400);
            Rule r = new Rule(f);
            policyTest.AddRule(r);
            Assert.IsTrue(policyTest.Check(this.product_quantity));
        }

        /// test for function :<see cref="TradingSystem.Business.Market.Policy.Check(Dictionary{Product, int})"/>
        [TestMethod]
        public void CheckFailTheRules()
        {
            this.product_quantity.Add(product1, 1);
            this.product_quantity.Add(product2, 2);
            Func<Dictionary<Product, int>, bool> f = new Func<Dictionary<Product, int>, bool>(CheckTotalWeightNoMoreThan400);
            Rule r = new Rule(f);
            policyTest.AddRule(r);
            Assert.IsFalse(policyTest.Check(this.product_quantity));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market.RulesPackage
{
    public class RulesCreator
    {
        public RulesCreator()
        {

        }

        public Rule CreateUserAgeRule(int ageLessThan = int.MaxValue, int ageGreaterEQThan = 0)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                UserAgeLessThan(basket.GetUsername(), ageLessThan) && !UserAgeLessThan(basket.GetUsername(), ageGreaterEQThan)
            ));
        }

        private bool UserAgeLessThan(string username, int ageLessThan)
        {
            return UserManagement.UserManagement.Instance.GetUserAge(username) < ageLessThan;
        }

        public Rule CreateProductWeightRule(Guid productId, double weightLessThan = int.MaxValue, double weightGreaterEQThan = 0)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                ProductWeightLessThan(basket, productId, weightLessThan) && !ProductWeightLessThan(basket, productId, weightGreaterEQThan)
            ));
        }

        private bool ProductWeightLessThan(ShoppingBasket basket, Guid productId, double quantityLessThan)
        {
            double counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                if (product.Id.Equals(productId))
                {
                    counter += product.Weight * quantity;
                }
            }
            return counter < quantityLessThan;
        }
        public Rule CreateProductRule(Guid productId, int quantityLessThan = int.MaxValue, int quantityGreaterEQThan = 0)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                ProductLessThan(basket, productId, quantityLessThan) && !ProductLessThan(basket, productId, quantityGreaterEQThan)
            ));
        }

        private bool ProductLessThan(ShoppingBasket basket, Guid productId, int quantityLessThan)
        {
            int counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                if (product.Id.Equals(productId))
                {
                    counter += quantity;
                }
            }
            return counter < quantityLessThan;
        }

        public Rule CreateCategoryRule(string category, int quantityLessThan = int.MaxValue, int quantityGreaterEQThan = 0)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                CategoryLessThan(basket, category, quantityLessThan) && !CategoryLessThan(basket, category, quantityGreaterEQThan)
            ));
        }

        private bool CategoryLessThan(ShoppingBasket basket, string category, int quantityLessThan)
        {
            int counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                if (product.Category.Equals(category))
                {
                    counter += quantity;
                }
            }
            return counter < quantityLessThan;
        }
        public Rule CreateStorePriceRule(double priceLessThan = int.MaxValue, double priceGreaterEQThan = 0)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                StorePriceLessThan(basket, priceLessThan) && !StorePriceLessThan(basket, priceGreaterEQThan)
            ));
        }

        private bool StorePriceLessThan(ShoppingBasket basket, double priceLessThan)
        {
            double counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                counter += product.Price * quantity;
            }
            return counter < priceLessThan;
        }
        public Rule CreateStoreRule(int quantityLessThan = int.MaxValue, int quantityGreaterEQThan = 0)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                StoreLessThan(basket, quantityLessThan) && !StoreLessThan(basket, quantityGreaterEQThan)
            ));
        }

        private bool StoreLessThan(ShoppingBasket basket, int quantityLessThan)
        {
            int counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.product;
                var quantity = p_q.quantity;
                counter += quantity;
            }
            return counter < quantityLessThan;
        }

        public Rule CreateTimeRule(DateTime BeforeDate, DateTime AfterDate)
        {
            return new Rule(new Func<ShoppingBasket, bool>((ShoppingBasket basket) =>
                DateLessThan(BeforeDate) && !DateLessThan(AfterDate)
            ));
        }

        private bool DateLessThan(DateTime beforeDate)
        {
            return beforeDate.Date < DateTime.Now.Date;
        }
    }
}

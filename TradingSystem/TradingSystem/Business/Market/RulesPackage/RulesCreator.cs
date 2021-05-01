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

        public Rule CreateUserAgeRule(string username, int ageLessThan = int.MaxValue, int ageGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                UserAgeLessThan(username, ageLessThan) && !UserAgeLessThan(username, ageGreaterEQThan)
            ));
        }

        private bool UserAgeLessThan(string username, int ageLessThan)
        {
            return UserManagement.UserManagement.Instance.GetUserAge(username) < ageLessThan;
        }

        public Rule CreateProductWeightRule(Guid productId, double weightLessThan = int.MaxValue, double weightGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                ProductWeightLessThan(basket, productId, weightLessThan) && !ProductWeightLessThan(basket, productId, weightGreaterEQThan)
            ));
        }

        private bool ProductWeightLessThan(IShoppingBasket basket, Guid productId, double quantityLessThan)
        {
            double counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.Key;
                var quantity = p_q.Value;
                if (product.Id.Equals(productId))
                {
                    counter += product.Weight * quantity;
                }
            }
            return counter < quantityLessThan;
        }
        public Rule CreateProductRule(Guid productId, int quantityLessThan = int.MaxValue, int quantityGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                ProductLessThan(basket, productId, quantityLessThan) && !ProductLessThan(basket, productId, quantityGreaterEQThan)
            ));
        }

        private bool ProductLessThan(IShoppingBasket basket, Guid productId, int quantityLessThan)
        {
            int counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.Key;
                var quantity = p_q.Value;
                if (product.Id.Equals(productId))
                {
                    counter += quantity;
                }
            }
            return counter < quantityLessThan;
        }

        public Rule CreateCategoryRule(string category, int quantityLessThan = int.MaxValue, int quantityGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                CategoryLessThan(basket, category, quantityLessThan) && !CategoryLessThan(basket, category, quantityGreaterEQThan)
            ));
        }

        private bool CategoryLessThan(IShoppingBasket basket, string category, int quantityLessThan)
        {
            int counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.Key;
                var quantity = p_q.Value;
                if (product.Category.Equals(category))
                {
                    counter += quantity;
                }
            }
            return counter < quantityLessThan;
        }
        public Rule CreateStorePriceRule(double priceLessThan = int.MaxValue, double priceGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                StorePriceLessThan(basket, priceLessThan) && !StorePriceLessThan(basket, priceGreaterEQThan)
            ));
        }

        private bool StorePriceLessThan(IShoppingBasket basket, double priceLessThan)
        {
            double counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.Key;
                var quantity = p_q.Value;
                counter += product.Price * quantity;
            }
            return counter < priceLessThan;
        }
        public Rule CreateStoreRule(int quantityLessThan = int.MaxValue, int quantityGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                StoreLessThan(basket, quantityLessThan) && !StoreLessThan(basket, quantityGreaterEQThan)
            ));
        }

        private bool StoreLessThan(IShoppingBasket basket, int quantityLessThan)
        {
            int counter = 0;
            foreach (var p_q in basket.GetDictionaryProductQuantity())
            {
                var product = p_q.Key;
                var quantity = p_q.Value;
                counter += quantity;
            }
            return counter < quantityLessThan;
        }

        public Rule CreateTimeRule(DateTime BeforeDate, DateTime AfterDate)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                DateLessThan(BeforeDate) && !DateLessThan(AfterDate)
            ));
        }

        private bool DateLessThan(DateTime beforeDate)
        {
            return beforeDate.Date < DateTime.Now.Date;
        }
    }
}

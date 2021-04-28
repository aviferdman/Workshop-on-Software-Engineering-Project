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

        public Rule CreateProductWeightRule(Guid productId, double weightLessThan = Double.MaxValue, double weightGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                ProductWeightLessThan(basket, productId, weightLessThan) && !ProductWeightLessThan(basket, productId, weightGreaterEQThan)
            ));
        }

        private bool ProductWeightLessThan(IShoppingBasket basket, Guid productId, double quantityLessThan)
        {
            double counter = 0;
            foreach (Product p in basket.GetProducts())
            {
                if (p.Id.Equals(productId))
                {
                    counter += p.Weight;
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
            foreach (Product p in basket.GetProducts())
            {
                if (p.Id.Equals(productId))
                {
                    counter += p.Quantity;
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
            foreach (Product p in basket.GetProducts())
            {
                if (p.Category.Equals(category))
                {
                    counter += p.Quantity;
                }
            }
            return counter < quantityLessThan;
        }
        public Rule CreateStorePriceRule(double priceLessThan = Double.MaxValue, double priceGreaterEQThan = 0)
        {
            return new Rule(new Func<IShoppingBasket, bool>((IShoppingBasket basket) =>
                StorePriceLessThan(basket, priceLessThan) && !StorePriceLessThan(basket, priceGreaterEQThan)
            ));
        }

        private bool StorePriceLessThan(IShoppingBasket basket, double priceLessThan)
        {
            double counter = 0;
            foreach (Product p in basket.GetProducts())
            {
                counter += p.Price;
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
            foreach (Product p in basket.GetProducts())
            {
                counter += p.Quantity;
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

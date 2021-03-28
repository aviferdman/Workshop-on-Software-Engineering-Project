using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingSystem.Business.Market
{
    class ShoppingBasket
    {
        Dictionary<Product, int> _product_quantity;

        public ShoppingBasket()
        {
            this._product_quantity = new Dictionary<Product, int>();
        }

        internal Dictionary<Product, int> Product_quantity { get => _product_quantity; set => _product_quantity = value; }

        public bool IsEmpty()
        {
            var possitiveQuantities = _product_quantity.Where(p_q => p_q.Value > 0);
            return !possitiveQuantities.Any();
        }

        public void RemoveProduct(Product product)
        {
            _product_quantity.Remove(product);
        }

        public void UpdateProduct(Product product, int quantity)
        {
            _product_quantity[product] = quantity;
        }

        public double CalcCost()
        {
            return _product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Price * next.Value);
        }
    }
}

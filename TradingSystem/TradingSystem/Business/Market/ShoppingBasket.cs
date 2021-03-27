using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.Market
{
    class ShoppingBasket
    {
        Dictionary<Product, int> _products_quantity;

        public ShoppingBasket()
        {
            this._products_quantity = new Dictionary<Product, int>();
        }

        public void AddProduct(Product product, int quantity)
        {
            _products_quantity.Add(product, quantity);
        }

        public void RemoveProduct(Product product, int quantity)
        {
            _products_quantity.Remove(product);
        }

        public void UpdateProduct(Product product, int quantity)
        {
            try
            {
                _products_quantity[product] = quantity;
            }
            catch
            //try to update product's quantity for no existing product
            {

            }
        }
    }
}

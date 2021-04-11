﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingSystem.Business.Market
{
    public class ShoppingBasket : IShoppingBasket
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

        public string addProduct(Product p, int q)
        {
            if (_product_quantity.ContainsKey(p))
                return "product is already in shopping basket";
            _product_quantity.Add(p, q);
            return "product added to shopping basket";
        }

        public bool RemoveProduct(Product product)
        {
            return _product_quantity.Remove(product);
        }

        public void UpdateProduct(Product product, int quantity)
        {
            if (!_product_quantity.ContainsKey(product))
            {
                _product_quantity.Add(product, quantity);
            }
            else
            {
                _product_quantity[product] = quantity;
            }
        }

        public double CalcCost()
        {
            return _product_quantity.Aggregate(0.0, (total, next) => total + next.Key.Price * next.Value);
        }

        public ICollection<Product> GetProducts()
        {
            return _product_quantity.Keys.Where(k => _product_quantity[k] > 0).ToList();
        }

        public int GetProductQuantity(Product product)
        {
            if (_product_quantity.ContainsKey(product))
            {
                return _product_quantity[product];
            }
            return 0;
        }

        public Dictionary<Product, int> GetDictionaryProductQuantity()
        {
            return _product_quantity;
        }

        public int CompareTo(object obj)
        {
            return obj.GetHashCode() - GetHashCode();
        }
    }
}

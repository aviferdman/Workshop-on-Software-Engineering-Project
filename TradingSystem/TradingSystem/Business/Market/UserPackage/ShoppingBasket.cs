﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Market.UserPackage;
using TradingSystem.DAL;

namespace TradingSystem.Business.Market
{
    public class ShoppingBasket 
    {
        public HashSet<ProductInCart> _product_quantity { get; set; }
        public  ShoppingCart shoppingCart { get; set; }
        public Store store { get; set; }

        public ShoppingBasket(ShoppingCart shoppingCart, Store store)
        {
            this.ShoppingCart = shoppingCart;
            this.Store = store;
            this._product_quantity = new HashSet<ProductInCart>();
        }

        public ShoppingBasket(ShoppingBasket s, ShoppingCart shoppingCart)
        {
            this.store = s.store;
            this.shoppingCart = shoppingCart;
            this._product_quantity =new  HashSet<ProductInCart>();
            foreach(ProductInCart p in s.Product_quantity)
            {
                this.Product_quantity.Add(new ProductInCart(p.product, p.quantity));
            }
        }

        public HashSet<ProductInCart> Product_quantity { get => _product_quantity; set => _product_quantity = value; }
        public ShoppingCart ShoppingCart { get => shoppingCart; set => shoppingCart = value; }
        public Store Store { get => store; set => store = value; }

        public bool IsEmpty()
        {
            var possitiveQuantities = _product_quantity.Where(p_q => p_q.quantity > 0);
            return !possitiveQuantities.Any();
        }

        public string addProduct(Product p, int q)
        {
            if (!_product_quantity.Where(p=> p.product.Equals(p)).Any())
                return "product is already in shopping basket";
            _product_quantity.Add(new ProductInCart(p, q));
            return "product added to shopping basket";
        }

        public bool RemoveProduct(Product product)
        {
            if(_product_quantity.Where(p => p.product.Equals(product)).Any())
            {
                MarketDAL.Instance.removeProductFromCart(_product_quantity.Where(p => p.product.Equals(product)).Single());
                _product_quantity.RemoveWhere(p => p.product.Equals(product));
                return true;
            }
               
            return false;
        }

        public void UpdateProduct(Product product, int quantity)
        {
            if (!_product_quantity.Where(p => p.product.Equals(p)).Any())
            {
                _product_quantity.Add(new ProductInCart(product, quantity));
            }
            else
            {
                _product_quantity.Where(p => p.product.Equals(p)).First().quantity = quantity;
            }
        }

        public bool TryUpdateProduct(Product product, int quantity)
        {
            if (!_product_quantity.Where(p => p.product.Equals(p)).Any())
            {
                return false;
            }
            else
            {
                _product_quantity.Where(p => p.product.Equals(p)).First().quantity = quantity;
                return true;
            }
        }

        public double CalcCost()
        {
            return _product_quantity.Aggregate(0.0, (total, next) => total + next.product.Price * next.quantity);
        }

        public ICollection<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            foreach(ProductInCart p in _product_quantity)
            {
                products.Add(p.product);
            }
            return products;
        }

        public int GetProductQuantity(Product product)
        {
            if (_product_quantity.Where(p => p.product.Equals(p)).Any())
            {
                return _product_quantity.Where(p => p.product.Equals(p)).First().quantity;
            }
            return 0;
        }

        public HashSet<ProductInCart> GetDictionaryProductQuantity()
        {
            return _product_quantity;
        }

        public int CompareTo(object obj)
        {
            return obj.GetHashCode() - GetHashCode();
        }

        public Store GetStore()
        {
            return store;
        }

        public ShoppingCart GetShoppingCart()
        {
            return this.ShoppingCart;
        }
    }
}

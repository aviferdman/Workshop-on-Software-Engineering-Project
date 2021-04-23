using System;
using System.Collections.Generic;

namespace TradingSystem.Business.Market
{
    public interface IShoppingBasket : IComparable
    {
        public bool IsEmpty();

        public bool RemoveProduct(Product product);

        public bool TryUpdateProduct(Product product, int quantity);

        public void UpdateProduct(Product product, int quantity);

        public double CalcCost();

        public ICollection<Product> GetProducts();

        public int GetProductQuantity(Product product);

        public Dictionary<Product, int> GetDictionaryProductQuantity();

        public string addProduct(Product p, int q);

        public IStore GetStore();

        public IShoppingCart GetShoppingCart();

    }
}
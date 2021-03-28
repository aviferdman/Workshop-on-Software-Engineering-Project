using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class Product : ProductInfo
    {
        public Product(string name, int price, int quantity, int id) :
            base(name, price, quantity)
        {
            Id = id;
        }

        public int Id { get; }

        public override bool Equals(object? obj) => obj is Product other && Equals(other);
        public bool Equals(Product other) => other != null && other.Id == Id;
        public override int GetHashCode() => Id.GetHashCode();
        public override string ToString()
        {
            return $"Product {Id} - '{Name}'";
        }
    }
}

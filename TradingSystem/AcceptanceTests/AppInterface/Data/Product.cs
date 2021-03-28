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
    }
}

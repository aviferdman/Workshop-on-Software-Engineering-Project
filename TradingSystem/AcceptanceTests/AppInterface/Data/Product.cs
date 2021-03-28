using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public class Product : ProductInfo
    {
        public Product(string name, int quantity, int id) :
            base(name, quantity)
        {
            Id = id;
        }

        public int Id { get; }
    }
}

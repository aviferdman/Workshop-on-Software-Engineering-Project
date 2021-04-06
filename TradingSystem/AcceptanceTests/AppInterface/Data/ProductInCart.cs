using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductInCart
    {
        public ProductId ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductInCart(ProductId productId, int quantity) : this()
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}

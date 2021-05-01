using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market.ShoppingCart
{
    public class ProductsForEdit
    {
        public IEnumerable<ProductInCart> ProductsAdd { get; set; }
        public IEnumerable<ProductId> ProductsRemove { get; set; }
        public IEnumerable<ProductInCart> ProductsEdit { get; set; }
    }
}

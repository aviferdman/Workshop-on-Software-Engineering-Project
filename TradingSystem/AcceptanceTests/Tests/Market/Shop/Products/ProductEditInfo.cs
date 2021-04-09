using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market.Shop.Products
{
    public class ProductEditInfo
    {
        public ProductEditInfo(ProductInfo productInfoOriginal, ProductInfo productInfoEdit)
        {
            ProductInfoOriginal = productInfoOriginal;
            ProductInfoEdit = productInfoEdit;
        }

        public ProductInfo ProductInfoOriginal { get; }
        public ProductInfo ProductInfoEdit { get; }
    }
}

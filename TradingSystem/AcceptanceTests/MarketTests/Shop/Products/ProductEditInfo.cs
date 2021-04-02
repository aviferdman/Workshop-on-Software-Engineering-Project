using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.MarketTests.Shop.Products
{
    public class ProductEditInfo
    {
        public ProductEditInfo(ProductInfo productInfoOriginal, ProductInfo productInfoEdit)
        {
            if (productInfoOriginal is null)
            {
                throw new ArgumentNullException(nameof(productInfoOriginal));
            }

            if (productInfoEdit is null)
            {
                throw new ArgumentNullException(nameof(productInfoEdit));
            }

            ProductInfoOriginal = productInfoOriginal;
            ProductInfoEdit = productInfoEdit;
        }

        public ProductInfo ProductInfoOriginal { get; }
        public ProductInfo ProductInfoEdit { get; }
    }
}

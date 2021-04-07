using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.Data
{
    public struct ProductSearchResult
    {
        public ProductSearchResult(ProductId productId, ProductInfo productInfo)
        {
            ProductId = productId;
            ProductInfo = productInfo;
        }

        public ProductId ProductId { get; }
        public ProductInfo ProductInfo { get; }
    }
}

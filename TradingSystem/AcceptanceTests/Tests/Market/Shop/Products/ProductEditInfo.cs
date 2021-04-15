
using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market.Shop.Products
{
    public class ProductEditInfo
    {
        public ProductEditInfo(ProductIdentifiable productOriginal, ProductInfo productInfoEdit)
        {
            ProductOriginal = productOriginal;
            ProductInfoEdit = productInfoEdit;
        }

        public ProductIdentifiable ProductOriginal { get; }
        public ProductInfo ProductInfoEdit { get; }
    }
}

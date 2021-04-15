
using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.Tests.Market.Shop.Products
{
    public struct ProductShopEditInfo
    {
        public ProductShopEditInfo(ProductIdentifiable productOriginal, ProductInfo productInfoEdit)
        {
            ProductOriginal = productOriginal;
            ProductInfoEdit = productInfoEdit;
        }

        public ProductIdentifiable ProductOriginal { get; }
        public ProductInfo ProductInfoEdit { get; }
    }
}
